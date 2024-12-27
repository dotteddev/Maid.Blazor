using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace Maid.Blazor.Core;
public class MaidComponent : IComponent, IHandleAfterRender, IHandleEvent
{
#region PRIVATE MEMBERS

	readonly RenderFragment _renderFragment;
	RenderHandle _renderHandle;
	bool _renderPending;
	bool _initialized;
	bool _firstRender;

	#endregion

	#region PARAMETERS

	[Parameter] public bool Hidden { get; set; } = false;
	[Parameter] public RenderFragment? ChildContent { get; set; }

#endregion


	public MaidComponent()
	{
		Console.WriteLine($"MaidComponent construction {this.GetType().ToString()}");
		_firstRender = true;
		_initialized = false;
		_renderPending = false;

		_renderFragment = (builder) =>
		{
			if (Hidden is false)
			{
				BuildRenderTree(builder);
				return;
			}
#if DEBUG
			builder.AddContent(0, "####HIDDEN####");
#endif
		};
	}

	protected Task InvokeAsync(Action workItem)
		=> _renderHandle.Dispatcher.InvokeAsync(workItem);
	protected Task InvokeAsync(Func<Task> workItem)
		=> _renderHandle.Dispatcher.InvokeAsync(workItem);
	protected Task DispatchExceptionAsync(Exception exception)
		=> _renderHandle.DispatchExceptionAsync(exception);
	protected virtual void BuildRenderTree(RenderTreeBuilder builder) { }

	protected virtual void AssignProperties(ParameterView parameters)
	{
		Console.WriteLine($"AssigningProperties {this.GetType().ToString()}");
		this.ChildContent = parameters.GetValueOrDefault("ChildContent", this.ChildContent);
		this.Hidden = parameters.GetValueOrDefault("Hidden", this.Hidden);
	}

	protected virtual Task OnNewParametersAsync(bool init) { return Task.CompletedTask; }


	protected void Render()
	{
		Console.WriteLine($"rendering {this.GetType().ToString()}");
		if (_renderPending && _renderHandle.IsRenderingOnMetadataUpdate is false) return;

		_renderPending = true;
		_renderHandle.Render(_renderFragment);
	}

	async Task AwaitAndRender(Task task)
	{
		try
		{
			await task;

		}
		catch
		{
			if (task.IsCanceled) return;

			throw; 
		}
		Render();
	}

	void IComponent.Attach(RenderHandle renderHandle)
	{
		_renderHandle = renderHandle;
	}

	Task IHandleEvent.HandleEventAsync(EventCallbackWorkItem item, object? arg)
	{
		var eventTask = item.InvokeAsync(arg);
		return AwaitAndRender(eventTask);
	}

	Task IHandleAfterRender.OnAfterRenderAsync()
	{
		//add possible AfterRender ability to consumer to be able to react to after render event.
		_renderPending = false;
		if (_firstRender) _firstRender = false;
		return Task.CompletedTask;
	}

	Task IComponent.SetParametersAsync(ParameterView parameters)
	{
		AssignProperties(parameters);
		var task = OnNewParametersAsync(_initialized);
		if (_initialized is false)
		{
			_initialized = true;
		}
		return AwaitAndRender(task);
	}
}

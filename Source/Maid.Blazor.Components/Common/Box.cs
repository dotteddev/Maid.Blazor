using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Maid.Blazor.Core;
using Maid.Blazor.SourceGeneration;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Maid.Blazor.Components;
[EnableAutoHandle]
public partial class Box : MaidComponent
{
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		builder.OpenElement(0, "div");
		builder.AddContent(1, ChildContent);
		builder.CloseElement();
	}

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maid.Blazor.Core.Logic;

public record ApplicationState
{
}

public record SharedApplicationState : ApplicationState
{
	
}

public record StateProperty<TType> 
{
	private TType _value;

	public static implicit operator StateProperty<TType>(TType value)
	{
		return new() { _value = value };
	}
	public static implicit operator TType(StateProperty<TType> value)
	{
		return value._value;
	}

	public TType GetValue()
	{
		return _value;
	}
}

public class StateActions<TStateType> where TStateType : ApplicationState
{ 
}

public record CounterState : ApplicationState
{
	public class Actions : StateActions<CounterState>
	{
		public void AddOne(CounterState state)
		{
			state.Model = state.Model.GetValue() with { };
		}
	}
	public required StateProperty<int> Count { get; set; }
	public required StateProperty<MyModel> Model { get; set; }
}
public record MyModel
{
	public int Id { get; set; }
	
	public string Name { get; set; }
}
using System;

namespace DataLayer.PureData.Managed.Data
{
	public interface IAtom<T>
	{
		T Value {get;}
	}

	public interface IAtom
	{
		object Value {get;}		
	}
}


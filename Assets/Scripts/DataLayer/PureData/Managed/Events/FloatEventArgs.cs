using System;
using DataLayer.PureData.Managed.Data;

namespace DataLayer.PureData.Managed.Events
{
	public class FloatEventArgs : EventArgs
	{
		public string Receiver { get; private set; }

		public Float Float { get; private set; }

		public FloatEventArgs (string text, float value)
		{
			Receiver = text;
			Float = new Float (value);
		}
	}
}

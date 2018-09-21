using System;

namespace DataLayer.PureData.Managed.Events
{
	public class ControlChangeEventArgs:EventArgs
	{
		public int Channel { get; private set; }

		public int Controller { get; private set; }

		public int Value { get; private set; }

		public ControlChangeEventArgs (int channel, int controller, int value)
		{
			Channel = channel;
			Controller = controller;
			Value = value;
		}
	}
}

using System;

namespace DataLayer.PureData.Managed.Events
{
	public class MidiByteEventArgs:EventArgs
	{
		public int Port { get; private set; }

		public int Value { get; private set; }

		public MidiByteEventArgs (int port, int value)
		{
			Port = port;
			Value = value;
		}
	}
}
using System;
using DataLayer.PureData.Managed.Data;

namespace DataLayer.PureData.Managed.Events
{
	public class PrintEventArgs : EventArgs
	{
		public Symbol Symbol { get; private set; }

		public PrintEventArgs (string text)
		{
			Symbol = new Symbol (text);
		}
	}
}
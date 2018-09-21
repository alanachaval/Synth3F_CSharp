using System;
using System.Collections.Generic;
using DataLayer.PureData.Managed.Data;
using DataLayer.PureData.Managed.Utils;

namespace DataLayer.PureData.Managed.Events
{
	public class ListEventArgs : EventArgs
	{
		public string Receiver { get; private set; }

		public IEnumerable<IAtom> List { get; private set; }

		public ListEventArgs (string recv, int argc, IntPtr argv)
		{
			Receiver = recv;
			List = MessageInvocation.ConvertList (argc, argv);
		}
	}

}

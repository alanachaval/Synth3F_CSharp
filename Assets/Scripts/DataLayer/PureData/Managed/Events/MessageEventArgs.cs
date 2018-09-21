using System;
using System.Collections.Generic;
using DataLayer.PureData.Managed.Data;
using DataLayer.PureData.Managed.Utils;

namespace DataLayer.PureData.Managed.Events
{
	public class MessageEventArgs : EventArgs
	{
		public string Receiver { get; private set; }

		public string Message { get; private set; }

		public IEnumerable<IAtom> List { get; private set; }

		public MessageEventArgs (string recv, string msg, int argc, IntPtr argv)
		{
			Receiver = recv;
			Message = msg;
			List = MessageInvocation.ConvertList (argc, argv);
		}
	}

}

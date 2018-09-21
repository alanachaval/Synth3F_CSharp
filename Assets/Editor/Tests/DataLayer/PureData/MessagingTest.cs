using DataLayer.PureData.Managed;
using DataLayer.PureData.Managed.Data;
using System;
using System.Collections.Generic;

namespace DomainLayer.PureData
{
    public class MessagingTest : IMessaging
    {
        public bool IsDisposed { get; set; } = false;
        public List<Send> Messages { get; set; } = new List<Send>();

        public void SendArray(string[] receiver, IAtom[] atoms)
        {
            throw new NotImplementedException();
        }

        void IDisposable.Dispose()
        {
            IsDisposed = true;
        }

        void IMessaging.Send(string receiver, params IAtom[] atoms)
        {
            Messages.Add(new Send() { Receiver = receiver, Message = null, Atoms = atoms });
        }

        void IMessaging.Send(string receiver, string message, params IAtom[] atoms)
        {
            Messages.Add(new Send() { Receiver = receiver, Message = message, Atoms = atoms });
        }

        public struct Send
        {
            public string Receiver { get; set; }
            public string Message { get; set; }
            public IAtom[] Atoms { get; set; }
        }
    }
}

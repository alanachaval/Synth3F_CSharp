using DataLayer.PureData.Managed.Data;
using System;

namespace DataLayer.PureData.Managed
{
    public interface IMessaging : IDisposable
    {
        void Send(string receiver, params IAtom[] atoms);

        void Send(string receiver, string message, params IAtom[] atoms);

        void SendArray(string[] receiver, IAtom[] atoms);
    }
}
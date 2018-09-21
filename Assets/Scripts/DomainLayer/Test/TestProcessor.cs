using DomainLayer.Common;
using Entities;

namespace DomainLayer.Test
{
    internal class TestProcessor : IProcessor
    {
        void IProcessor.Clear(Patch[] patches) { }

        void IProcessor.Connect(Connection connection) { }

        void IProcessor.Disconnect(Connection connection) { }

        void IProcessor.CreatePatch(Patch patch) { }

        void IProcessor.Delete(Patch patch) { }

        void IProcessor.LoadConnections(Connection[] connections) { }

        void IProcessor.LoadPatches(Patch[] patches) { }

        void IProcessor.Process(float[] data, int channels) { }

        void IProcessor.SetValue(Patch patch, string parameter) { }
    }
}
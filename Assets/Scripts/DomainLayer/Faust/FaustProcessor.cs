using Assets.Scripts.DataLayer.Faust;
using Constants;
using DomainLayer.Common;
using Entities;
using System.Collections.Generic;

namespace DomainLayer.Faust
{
    public class FaustProcessor : IProcessor
    {
        private FaustApi faustApi;

        public void SetFaustApi(FaustApi faustApi)
        {
            this.faustApi = faustApi;
        }

        void IProcessor.Clear(Patch[] patches)
        {
            foreach (Patch patch in patches)
            {
                faustApi.RemovePatch(patch.Id);
            }
        }

        void IProcessor.Connect(Connection connection)
        {
            faustApi.Connect(connection.SourcePatch, connection.SourceOutlet, connection.TargetPatch, connection.TargetInlet);
        }

        void IProcessor.Disconnect(Connection connection)
        {
            faustApi.Disconnect(connection.SourcePatch, connection.SourceOutlet, connection.TargetPatch, connection.TargetInlet);
        }

        void IProcessor.CreatePatch(Patch patch)
        {
            faustApi.AddPatch(patch.Code, patch.Id);
            foreach (KeyValuePair<string, float> parameter in patch.Values)
            {
                ((IProcessor)this).SetValue(patch, parameter.Key);
            }
        }

        void IProcessor.Delete(Patch patch)
        {
            foreach (Connection connection in patch.GetOutputConnections())
            {
                ((IProcessor)this).Disconnect(connection);
            }
            faustApi.RemovePatch(patch.Id);
        }

        void IProcessor.LoadConnections(Connection[] connections)
        {
            foreach (Connection connection in connections)
            {
                ((IProcessor)this).Connect(connection);
            }
        }

        void IProcessor.LoadPatches(Patch[] patches)
        {
            foreach (Patch patch in patches)
            {
                ((IProcessor)this).CreatePatch(patch);
            }
        }

        void IProcessor.Process(float[] data, int channels)
        {
            faustApi.Process(data, channels);
        }

        void IProcessor.SetValue(Patch patch, string parameter)
        {
            faustApi.SetValue(patch.Code + "_" + parameter + "_" + patch.Id.ToString(), patch.Values[parameter]);
        }
    }
}

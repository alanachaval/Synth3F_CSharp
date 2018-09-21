using Constants;
using DataLayer.PureData;
using DataLayer.PureData.Managed;
using DataLayer.PureData.Managed.Data;
using DomainLayer.Common;
using Entities;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace DomainLayer.PureData
{
    public class PureDataProcessor : IProcessor
    {
        private string mainPatchName;
        private List<int> pureDataIds;
        private int numberOfTicks;
        private IMessaging messaging;

        void IProcessor.Clear(Patch[] patches)
        {
            foreach (Patch patch in patches)
            {
                ((IProcessor)this).Delete(patch);
            }
        }

        void IProcessor.Connect(Connection connection)
        {
            int newId = pureDataIds.Count;
            int sourcePatch = GetPureDataId(connection.SourcePatch);
            int targetPatch = GetPureDataId(connection.TargetPatch);
            string fadeName = "x_fade_" + connection.Id;
            IAtom[] array = { new Float(10), new Float(10), new Symbol("x_fade"), new Symbol(fadeName) };
            pureDataIds.Add(connection.Id);
            Tracer.Log(mainPatchName + " obj 10 10 x_fade " + fadeName);
            messaging.Send(mainPatchName, "obj", array);
            array = new IAtom[] { new Float(sourcePatch), new Float(connection.SourceOutlet), new Float(newId), new Float(0) };
            Tracer.Log(mainPatchName + " connect " + sourcePatch + " " + connection.SourceOutlet + " " + newId + " 0");
            messaging.Send(mainPatchName, "connect", array);
            array = new IAtom[] { new Float(newId), new Float(0), new Float(targetPatch), new Float(connection.TargetInlet) };
            Tracer.Log(mainPatchName + " connect " + newId + " 0 " + targetPatch + " " + connection.TargetInlet);
            messaging.Send(mainPatchName, "connect", array);
            Tracer.Log(fadeName + " 1");
            messaging.Send(fadeName, new Float(1));
        }

        void IProcessor.Disconnect(Connection connection)
        {
            throw new NotImplementedException();
        }

        void IProcessor.CreatePatch(Patch patch)
        {
            int newId = pureDataIds.Count;
            string name = "x_" + patch.Code + "_" + patch.Id;
            pureDataIds.Add(patch.Id);
            IAtom[] array = { new Float(10), new Float(10), new Symbol("x_" + patch.Code), new Symbol(name) };
            Tracer.Log(mainPatchName + " obj 10 10 x_" + patch.Code + " " + name);
            messaging.Send(mainPatchName, "obj", array);
            foreach (KeyValuePair<String, float> parameter in patch.Values)
            {
                Tracer.Log(parameter.Key + parameter.Value);
                ((IProcessor)this).SetValue(patch, parameter.Key);
            }
        }

        void IProcessor.Delete(Patch patch)
        {
            IAtom[] array = { null, new Float(1) };
            List<int> toDelete = new List<int>();

            foreach (Connection connection in patch.GetOutputConnections())
            {
                array[0] = new Symbol("x_fade_" + connection.Id);
                Tracer.Log(mainPatchName + " find " + array[0].Value + " 1");
                messaging.Send(mainPatchName, "find", array);
                Tracer.Log(mainPatchName + " cut " + array[0].Value + " 1");
                messaging.Send(mainPatchName, "cut", array);
                toDelete.Add(connection.Id);
            }

            foreach (Connection connection in patch.GetInputConnections())
            {
                array[0] = new Symbol("x_fade_" + connection.Id);
                Tracer.Log(mainPatchName + " find " + array[0].Value + " 1");
                messaging.Send(mainPatchName, "find", array);
                Tracer.Log(mainPatchName + " cut " + array[0].Value + " 1");
                messaging.Send(mainPatchName, "cut", array);
                toDelete.Add(connection.Id);
            }

            array[0] = new Symbol("x_" + patch.Code + "_" + patch.Id);
            Tracer.Log(mainPatchName + " find " + array[0].Value + " 1");
            messaging.Send(mainPatchName, "find", array);
            Tracer.Log(mainPatchName + " cut " + array[0].Value + " 1");
            messaging.Send(mainPatchName, "cut", array);
            toDelete.Add(patch.Id);

            for (int i = 0; i < pureDataIds.Count; i++)
            {
                if (toDelete.Contains(pureDataIds[i]))
                {
                    pureDataIds.RemoveAt(i);
                    i--;
                }
            }
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

        private float[] emptyInput = new float[0];

        void IProcessor.Process(float[] data, int channels)
        {
            LibPD.Process(numberOfTicks, emptyInput, data);
        }

        void IProcessor.SetValue(Patch patch, string parameter)
        {
            string name = "x_" + patch.Code + "_" + patch.Id + "_" + parameter;
            Tracer.Log(name + " " + patch.Values[parameter].ToString());
            messaging.Send(name, new Float(patch.Values[parameter]));
        }

        public void Init(string mainPatchName, int numberOfTicks, IMessaging messaging)
        {
            this.mainPatchName = "pd-" + mainPatchName;
            pureDataIds = new List<int>();
            this.numberOfTicks = numberOfTicks;
            this.messaging = messaging;
        }

        private int GetPureDataId(int id)
        {
            int pureDataId = -1;
            for (int i = 0; i < pureDataIds.Count; i++)
            {
                if (id == pureDataIds[i])
                {
                    pureDataId = i;
                }
            }
            return pureDataId;
        }
    }
}

using Constants;
using System.IO;
using UnityEngine;
using System;
using System.Text;
using System.Xml.Serialization;
using Entities;
using System.Collections.Generic;

namespace DomainLayer.Common
{
    public class XMLSerializer : ISerializer
    {
        IPatchGraphManager ISerializer.Load(string filename)
        {
            PatchGraph patchGraph;
            string xml;
            IPatchGraphManager patchGraphManager = new PatchGraphManager();
            try
            {
                Tracer.Log("Application.persistentDataPath + Others.SavesFolder + filename" + Application.persistentDataPath + Others.SavesFolder + filename);
                xml = File.ReadAllText(Application.persistentDataPath + Others.SavesFolder + filename);
                using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(PatchGraph));
                    patchGraph = (PatchGraph)xmlSerializer.Deserialize(memoryStream);
                }
                SerializedPatch[] serializedPatches = patchGraph.patches;
                SerializedPatch serializedPatch;
                Dictionary<int, Patch> patches = new Dictionary<int, Patch>();
                Patch patch;
                for (int i = 0; i < serializedPatches.Length; i++)
                {
                    serializedPatch = serializedPatches[i];
                    patch = new Patch()
                    {
                        Id = serializedPatch.id,
                        Code = serializedPatch.code,
                        PosX = serializedPatch.PosX,
                        PosY = serializedPatch.PosY,
                        Values = new SortedList<string, float>()
                    };
                    foreach (Parameter parameter in serializedPatch.parameters)
                    {
                        patch.Values.Add(parameter.name, parameter.value);
                    }
                    patches.Add(patch.Id, patch);
                }
                ((PatchGraphManager)patchGraphManager).patches = patches;
                Dictionary<int, Connection> connections = new Dictionary<int, Connection>();
                foreach (Connection connection in patchGraph.connections)
                {
                    patches[connection.SourcePatch].GetOutputConnections().Add(connection);
                    patches[connection.TargetPatch].GetInputConnections().Add(connection);
                    connections.Add(connection.Id, connection);
                }
                ((PatchGraphManager)patchGraphManager).connections = connections;
                ((PatchGraphManager)patchGraphManager).maxId = patchGraph.maxId;
            }
            catch (Exception e)
            {
                Tracer.Log(e.Message + e.StackTrace);
            }
            return patchGraphManager;
        }

        void ISerializer.Save(IPatchGraphManager patchGraphManager, string filename)
        {
            PatchGraph patchGraph = new PatchGraph();
            Patch[] patches = patchGraphManager.GetPatches();
            patchGraph.patches = new SerializedPatch[patches.Length];
            SerializedPatch serializedPatch;
            Patch patch;
            int j;
            for (int i = 0; i < patches.Length; i++)
            {
                patch = patches[i];
                serializedPatch = new SerializedPatch()
                {
                    id = patch.Id,
                    code = patch.Code,
                    PosX = patch.PosX,
                    PosY = patch.PosY,
                    parameters = new Parameter[patch.Values.Count]
                };
                j = 0;
                foreach (KeyValuePair<string, float> pair in patch.Values)
                {
                    serializedPatch.parameters[j++] = new Parameter()
                    {
                        name = pair.Key,
                        value = pair.Value
                    };
                }
                patchGraph.patches[i] = serializedPatch;
            }
            patchGraph.connections = patchGraphManager.GetConnections();
            patchGraph.maxId = ((PatchGraphManager)patchGraphManager).maxId;
            string xml;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(PatchGraph));
            using (MemoryStream memoryStream = new MemoryStream())
            {
                xmlSerializer.Serialize(memoryStream, patchGraph);
                memoryStream.Position = 0;
                using (StreamReader streamReader = new StreamReader(memoryStream))
                {
                    xml = streamReader.ReadToEnd();
                }
            }
            try
            {
                Tracer.Log("Application.persistentDataPath + Others.SavesFolder + filename" + Application.persistentDataPath + Others.SavesFolder + filename);
                File.WriteAllText(Application.persistentDataPath + Others.SavesFolder + filename, xml, Encoding.UTF8);
            }
            catch (Exception e)
            {
                Tracer.Log(e.Message + e.StackTrace);
            }
        }

        public class PatchGraph
        {
            public int maxId;
            public SerializedPatch[] patches;
            public Connection[] connections;
        }

        public class SerializedPatch
        {
            public int id;
            public string code;
            public float PosX;
            public float PosY;
            public Parameter[] parameters;
        }

        public class Parameter
        {
            public string name;
            public float value;
        }
    }
}
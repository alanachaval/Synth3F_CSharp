using Entities;
using System.Collections.Generic;
using System.Linq;

namespace DomainLayer.Common
{
    public class PatchGraphManager : IPatchGraphManager
    {
        public int maxId;
        public Dictionary<int, Patch> patches;
        public Dictionary<int, Connection> connections;

        public PatchGraphManager()
        {
            maxId = 0;
            patches = new Dictionary<int, Patch>();
            connections = new Dictionary<int, Connection>();
        }

        int IPatchGraphManager.AddPatch(Patch patch)
        {
            maxId++;
            patch.Id = maxId;
            patches.Add(maxId, patch);
            return maxId;
        }

        void IPatchGraphManager.Clear()
        {
            maxId = 0;
            connections.Clear();
            patches.Clear();
        }

        Connection IPatchGraphManager.Connect(int sourcePatch, int sourceOutlet, int targetPatch, int targetInlet)
        {
            maxId++;
            Connection connection = new Connection
            {
                Id = maxId,
                SourcePatch = sourcePatch,
                SourceOutlet = sourceOutlet,
                TargetPatch = targetPatch,
                TargetInlet = targetInlet
            };
            connections.Add(maxId, connection);
            patches[sourcePatch].AddOutputConnection(connection);
            patches[targetPatch].AddInputConnection(connection);
            return connection;
        }

        Connection IPatchGraphManager.Disconnect(int connectionId)
        {
            Connection connection = connections[connectionId];
            connections.Remove(connectionId);
            patches[connection.SourcePatch].RemoveOutputConnection(connection);
            patches[connection.TargetPatch].RemoveInputConnection(connection);
            return connection;
        }

        Connection IPatchGraphManager.GetConnection(int connectionId)
        {
            return connections[connectionId];
        }

        Connection[] IPatchGraphManager.GetConnections()
        {
            return connections.Values.ToArray();
        }

        Patch IPatchGraphManager.GetPatch(int patchId)
        {
            return patches[patchId];
        }

        Patch[] IPatchGraphManager.GetPatches()
        {
            return patches.Values.ToArray();
        }

        Patch IPatchGraphManager.RemovePatch(int patchId)
        {
            Patch patch = patches[patchId];
            patches.Remove(patchId);
            foreach (Connection connection in patch.GetInputConnections())
            {
                connections.Remove(connection.Id);
                patches[connection.SourcePatch].RemoveOutputConnection(connection);
            }
            foreach (Connection connection in patch.GetOutputConnections())
            {
                connections.Remove(connection.Id);
                patches[connection.TargetPatch].RemoveInputConnection(connection);
            }
            return patch;
        }
    }
}
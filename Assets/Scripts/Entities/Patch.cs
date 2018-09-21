using System.Collections.Generic;

namespace Entities
{
    public class Patch
    {
        public int Id { get; set; }
        public string Code { get; set; }
        private IList<Connection> outputConnections = new List<Connection>(0);
        private IList<Connection> inputConnections = new List<Connection>(0);
        public float PosX { get; set; }
        public float PosY { get; set; }
        public IDictionary<string, float> Values { get; set; }

        public IList<Connection> GetInputConnections()
        {
            return inputConnections;
        }

        public IList<Connection> GetOutputConnections()
        {
            return outputConnections;
        }

        public void AddInputConnection(Connection connection)
        {
            inputConnections.Add(connection);
        }

        public bool RemoveInputConnection(Connection connection)
        {
            return inputConnections.Remove(connection);
        }

        public void AddOutputConnection(Connection connection)
        {
            outputConnections.Add(connection);
        }

        public bool RemoveOutputConnection(Connection connection)
        {
            return outputConnections.Remove(connection);
        }
    }
}

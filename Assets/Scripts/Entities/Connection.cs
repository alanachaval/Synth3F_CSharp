using System;

namespace Entities
{
    [Serializable]
    public class Connection
    {
        public int Id { get; set; }
        public int SourcePatch { get; set; }
        public int SourceOutlet { get; set; }
        public int TargetPatch { get; set; }
        public int TargetInlet { get; set; }
    }
}

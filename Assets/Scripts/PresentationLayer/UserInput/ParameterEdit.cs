using Entities;
using UnityEngine;

namespace PresentationLayer.UserInput
{
    public abstract class ParameterEdit : MonoBehaviour
    {
        [SerializeField] private PatchEdit patchEdit;
        public PatchEdit PatchEdit { get { return patchEdit; } }
        [SerializeField] private string parameter;
        public string Parameter { get { return parameter; } }
        public Patch Patch { get; set; }

        public abstract void Init(Patch patch);

        public abstract float SetValue(float value);
    }
}

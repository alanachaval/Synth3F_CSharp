using Constants;
using Entities;
using UnityEngine;
using UnityEngine.UI;

namespace PresentationLayer.UserInput
{
    public class PatchEdit : MonoBehaviour
    {
        [SerializeField] private ParameterEdit[] parameterEdits;
        public ParameterEdit[] ParameterEdits { get { return parameterEdits; } }
        [SerializeField] private Text editedParameter;
        [SerializeField] private InputField inputField;
        public Patch Patch { get; set; }
        private string editedValue;

        public void Init(Patch patch)
        {
            Patch = patch;
            foreach (ParameterEdit parameterEdit in parameterEdits)
            {
                parameterEdit.Init(patch);
            }
            editedValue = parameterEdits[0].Parameter;
            editedParameter.text = editedValue;
            inputField.text = patch.Values[editedValue].ToString().Replace(",", ".");
        }

        public void EditValue(string parameter, float value)
        {
            editedValue = parameter;
            editedParameter.text = parameter;
            inputField.text = value.ToString().Replace(",", ".");
        }

        public void InputValue(string s)
        {
            float value = float.Parse(inputField.text);
            foreach (ParameterEdit parameterEdit in parameterEdits)
            {
                if (parameterEdit.Parameter == editedValue)
                {
                    value = parameterEdit.SetValue(value);
                    inputField.text = value.ToString().Replace(",", ".");
                }
            }
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}
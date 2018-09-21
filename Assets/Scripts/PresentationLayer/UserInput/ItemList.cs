using DomainLayer.Common;
using Entities;
using UnityEngine;
using UnityEngine.UI;

namespace PresentationLayer.UserInput
{
    public class ItemList : ParameterEdit
    {
        [SerializeField] private Image[] options;
        [SerializeField] private Color activeColor;
        [SerializeField] private Color inactiveColor;

        public override void Init(Patch patch)
        {
            Patch = patch;
            SetActive(Mathf.RoundToInt(Patch.Values[Parameter]));
        }

        public override float SetValue(float value)
        {
            int intValue = Mathf.RoundToInt(value);
            if (intValue < 0)
            {
                intValue = 0;
            }
            else if (intValue > options.Length - 1)
            {
                intValue = options.Length - 1;
            }
            PatchEdit.EditValue(Parameter, intValue);
            SetActive(intValue);
            MainManager.GetInstance().SetValue(Patch, Parameter, intValue);
            return intValue;
        }

        public void ItemClick(int active)
        {
            SetValue(active);
        }

        private void SetActive(int active)
        {
            for (int i = 0; i < options.Length; i++)
            {
                if (i == active)
                {
                    options[i].color = activeColor;
                }
                else
                {
                    options[i].color = inactiveColor;
                }
            }
        }
    }
}

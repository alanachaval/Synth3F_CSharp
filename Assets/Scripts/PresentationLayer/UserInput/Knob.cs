using Constants;
using DomainLayer.Common;
using Entities;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PresentationLayer.UserInput
{
    public class Knob : ParameterEdit, IDragHandler, IPointerDownHandler
    {
        [SerializeField] private GameObject toRotate;
        [SerializeField] private float minValue;
        [SerializeField] private float maxValue;
        //0: linear, 1: exponential_left, 2: exponential_center, 3: integer_linear
        [SerializeField] private int scale;
        private float initialY;

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            float progress = (eventData.position.y - initialY) / Others.KnobTouchAmplitude;
            if (progress < 0)
            {
                progress = 0;
            }
            else if (progress > 1)
            {
                progress = 1;
            }
            float value = 0;
            if (scale == 0) //linear
            {
                value = minValue + (progress * (maxValue - minValue));
            }
            else if (scale == 1) //exponential_left
            {
                value = minValue + Mathf.Pow(maxValue + 1, progress) - 1;
            }
            else if (scale == 2) //exponential_center
            {
                float halfAmplitude = (maxValue - minValue) / 2;
                if (progress > .5f)
                {
                    value = Mathf.Pow(halfAmplitude + 1, (progress - .5f) * 2) - 1;
                }
                else
                {
                    value = -Mathf.Pow(halfAmplitude + 1, (-progress + .5f) * 2) + 1;
                }
            }
            else if (scale == 3) //integer_linear
            {
                value = minValue + (progress * (maxValue - minValue));
                value = Mathf.Round(value);
            }
            SetValue(value);
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            float value = Patch.Values[Parameter];
            if (scale == 0 || scale == 3) //linear || integer_linear
            {
                initialY = transform.position.y - ((value - minValue) / (maxValue - minValue)) * Others.KnobTouchAmplitude;
            }
            else if (scale == 1) //exponential_left
            {
                initialY = transform.position.y - (Mathf.Log(value - minValue + 1) / Mathf.Log(maxValue + 1)) * Others.KnobTouchAmplitude;
            }
            else if (scale == 2) //exponential_center
            {
                float halfAmplitude = (maxValue - minValue) / 2;
                float center = maxValue - halfAmplitude;
                float distance;
                if (value > center)
                {
                    distance = value - center;
                    initialY = transform.position.y - ((Mathf.Log(distance + 1) / Mathf.Log(halfAmplitude + 1)) + 1) * Others.KnobTouchAmplitude / 2;
                }
                else
                {
                    distance = center - value;
                    initialY = transform.position.y - (Mathf.Log(distance + 1) / Mathf.Log(halfAmplitude + 1)) * Others.KnobTouchAmplitude / 2;
                }
            }
        }

        public override void Init(Patch patch)
        {
            Patch = patch;
            SetRotation(Patch.Values[Parameter]);
        }

        public override float SetValue(float value)
        {
            if (scale == 3) //integer_linear
            {
                value = Mathf.Round(value);
            }
            if (value < minValue)
            {
                value = minValue;
            }
            else if (value > maxValue)
            {
                value = maxValue;
            }
            PatchEdit.EditValue(Parameter, value);
            SetRotation(value);
            MainManager.GetInstance().SetValue(Patch, Parameter, value);
            return value;
        }

        private void SetRotation(float value)
        {
            float rotation = 0f;

            if (scale == 0 || scale == 3)  //linear || integer_linear
            {
                rotation = -(Others.KnobRotationAngle / 2) + Others.KnobRotationAngle * ((value - minValue) / (maxValue - minValue));
            }
            else if (scale == 1) //exponential_left
            {
                rotation = -(Others.KnobRotationAngle / 2) + Others.KnobRotationAngle * (Mathf.Log(value - minValue + 1) / Mathf.Log(maxValue + 1));
            }
            else if (scale == 2) //exponential_center
            {
                float halfAmplitude = (maxValue - minValue) / 2;
                float center = maxValue - halfAmplitude;
                float distance;
                if (value > center)
                {
                    distance = (value - center);
                    rotation = (Mathf.Log(distance + 1) / Mathf.Log(halfAmplitude + 1)) * Others.KnobRotationAngle / 2;
                }
                else
                {
                    distance = (center - value);
                    rotation = -(Mathf.Log(distance + 1) / Mathf.Log(halfAmplitude + 1)) * Others.KnobRotationAngle / 2;
                }
            }
            toRotate.transform.eulerAngles = new Vector3(0, 0, -rotation);
        }
    }
}

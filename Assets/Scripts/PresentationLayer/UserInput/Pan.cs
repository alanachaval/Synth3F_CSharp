using PresentationLayer.Presenters;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PresentationLayer.UserInput
{
    public class Pan : MonoBehaviour, IBeginDragHandler, IDragHandler
    {
        [SerializeField] private GameObject patchBodyHolder;
        private Vector3 offset;
        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            offset = InputHandler.GetTouchPosition();          
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            Camera.main.transform.position -= InputHandler.GetTouchPosition() - offset;
            offset = InputHandler.GetTouchPosition();
            InputHandler.CheckCameraLimits();
        }
    }
}
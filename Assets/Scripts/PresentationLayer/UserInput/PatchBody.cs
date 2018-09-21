using Entities;
using PresentationLayer.Presenters;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PresentationLayer.UserInput
{
    public class PatchBody : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private Color color;
        public Color Color { get { return color; } }
        public Patch Patch { get; set; }
        public PatchCreator PatchCreator { get; set; }
        private Vector2 offset;

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            PatchCreator.PatchBody = this;
            offset = (Vector2)transform.position - (Vector2)InputHandler.GetTouchPosition();
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            transform.position = (Vector2)InputHandler.GetTouchPosition() + offset;
            WireDrawer.GetInstance().MovePatch(this);
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            PatchCreator.PatchBody = null;
            Patch.PosX = transform.position.x;
            Patch.PosY = transform.position.y;
        }
    }
}
using Constants;
using DomainLayer.Common;
using Entities;
using PresentationLayer.Presenters;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PresentationLayer.UserInput
{
    public class Inlet : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private int id;
        public int Id { get { return id; } }
        [SerializeField] private PatchBody patchBody;
        public PatchBody PatchBody { get { return patchBody; } }

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            WireDrawer.GetInstance().StartDraw(gameObject, patchBody.Color);
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            WireDrawer.GetInstance().Draw(InputHandler.GetTouchPosition());
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            WireDrawer.GetInstance().Release();
            RaycastHit2D raycastHit = InputHandler.RayCast(Layers.Inlets);
            if (raycastHit.collider)
            {
                Outlet outlet = raycastHit.collider.gameObject.GetComponent<Outlet>();
                Connection connection = MainManager.GetInstance().Connect(outlet.PatchBody.Patch.Id, outlet.Id, patchBody.Patch.Id, Id);
                WireDrawer.GetInstance().AddConnection(connection.Id, outlet, this);
            }
        }
    }
}
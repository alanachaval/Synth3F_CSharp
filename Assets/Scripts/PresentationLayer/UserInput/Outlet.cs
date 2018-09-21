using Constants;
using DomainLayer.Common;
using Entities;
using PresentationLayer.Presenters;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PresentationLayer.UserInput
{
    public class Outlet : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
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
                Inlet inlet = raycastHit.collider.gameObject.GetComponent<Inlet>();
                Connection connection = MainManager.GetInstance().Connect(patchBody.Patch.Id, Id, inlet.PatchBody.Patch.Id, inlet.Id);
                WireDrawer.GetInstance().AddConnection(connection.Id, this, inlet);
            }
        }
    }
}
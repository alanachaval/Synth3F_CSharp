using PresentationLayer.Presenters;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PresentationLayer.UserInput
{
    public class EagerButton : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private PatchCreator patchCreator;
        public PatchCreator PatchCreator { get { return patchCreator; } }
        [SerializeField] private string patchCode;
        public string PatchCode { get { return patchCode; } }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            patchCreator.CreatePatchBody(patchCode);
        }

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            (patchCreator.PatchBody as IBeginDragHandler).OnBeginDrag(eventData);
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            (patchCreator.PatchBody as IDragHandler).OnDrag(eventData);
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            if (patchCreator.PatchBody)
            {
                (patchCreator.PatchBody as IEndDragHandler).OnEndDrag(eventData);
            }
        }
    }
}
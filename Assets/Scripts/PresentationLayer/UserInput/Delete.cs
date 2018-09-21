using Constants;
using DomainLayer.Common;
using PresentationLayer.Presenters;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PresentationLayer.UserInput
{
    public class Delete : MonoBehaviour, IDropHandler
    {
        [SerializeField] private PatchCreator patchCreator;
        public PatchCreator PatchCreator { get { return patchCreator; } }

        void IDropHandler.OnDrop(PointerEventData eventData)
        {
            patchCreator.Delete();
        }
    }
}
using Constants;
using DomainLayer.Common;
using Entities;
using PresentationLayer.UserInput;
using System.Collections.Generic;
using UnityEngine;

namespace PresentationLayer.Presenters
{
    public class PatchCreator : MonoBehaviour
    {
        [SerializeField] private GameObject patchEditHolder;
        [SerializeField] private GameObject patchBodyHolder;
        public GameObject PatchEditHolder { get { return patchEditHolder; } }

        public List<PatchBody> PatchBodies { get; set; }
        private PatchBody[] patchBodiesPrefabs;
        private PatchEdit[] patchEditPrefabs;

        public PatchBody PatchBody { get; set; }
        private PatchEdit patchEdit;

        private void Awake()
        {
            Init();
        }
        
        public void Clear()
        {
            foreach(PatchBody patchBody in PatchBodies)
            {
                Destroy(patchBody.gameObject);
            }
            PatchBodies.Clear();
        }

        public void CreatePatchBody(string patchCode)
        {
            PatchBody patchBodyPrefab = null;
            foreach (PatchBody prefab in patchBodiesPrefabs)
            {
                if (prefab.name == patchCode)
                {
                    patchBodyPrefab = prefab;
                }
            }
            Vector3 position = InputHandler.GetTouchPosition();
            PatchBody = Instantiate(patchBodyPrefab, position, Quaternion.identity);
            PatchBody.Patch = MainManager.GetInstance().CreatePatch(patchCode);
            PatchBody.Patch.PosX = position.x;
            PatchBody.Patch.PosY = position.y;
            PatchBody.PatchCreator = this;
            PatchBody.transform.parent = patchBodyHolder.transform;
            PatchBodies.Add(PatchBody);
        }

        public void Delete()
        {
            if (PatchBody != null)
            {
                PatchBodies.Remove(PatchBody);
                MainManager.GetInstance().Delete(PatchBody.Patch);
                WireDrawer.GetInstance().RemovePatch(PatchBody);
                Destroy(PatchBody.gameObject);
                PatchBody = null;
            }
        }

        public void Init()
        {
            Object[] resources = Resources.LoadAll(Others.PatchBodiesPrefabsFolder);
            patchBodiesPrefabs = new PatchBody[resources.Length];
            for (int i = 0; i < resources.Length; i++)
            {
                patchBodiesPrefabs[i] = ((GameObject)resources[i]).GetComponent<PatchBody>();
            }
            resources = Resources.LoadAll(Others.PatchEditPrefabsFolder);
            patchEditPrefabs = new PatchEdit[resources.Length];
            for (int i = 0; i < resources.Length; i++)
            {
                patchEditPrefabs[i] = ((GameObject)resources[i]).GetComponent<PatchEdit>();
            }
            PatchBodies = new List<PatchBody>();
        }

        public void Load(Patch[] patches)
        {
            foreach (Patch patch in patches)
            {
                PatchBody patchBodyPrefab = null;
                foreach (PatchBody prefab in patchBodiesPrefabs)
                {
                    if (prefab.name == patch.Code)
                    {
                        patchBodyPrefab = prefab;
                    }
                }
                Vector3 position = new Vector3(patch.PosX, patch.PosY, 0);
                PatchBody = Instantiate(patchBodyPrefab, position, Quaternion.identity);
                PatchBody.Patch = patch;
                PatchBody.PatchCreator = this;
                PatchBody.transform.parent = patchBodyHolder.transform;
                PatchBodies.Add(PatchBody);
            }
        }

        public void OpenEditMenu(PatchBody patchBody)
        {
            if (patchEdit)
            {
                Destroy(patchEdit.gameObject);
            }
            foreach (PatchEdit prefab in patchEditPrefabs)
            {
                if (prefab.name == patchBody.Patch.Code)
                {
                    patchEdit = Instantiate(prefab, patchEditHolder.transform, false);
                    patchEdit.Init(patchBody.Patch);
                }
            }
        }
    }
}

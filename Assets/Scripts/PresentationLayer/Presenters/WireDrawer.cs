using UnityEngine;
using System.Collections.Generic;
using Entities;
using System;
using PresentationLayer.UserInput;
using Constants;

namespace PresentationLayer.Presenters
{
    public class WireDrawer : MonoBehaviour
    {
        [SerializeField] private PatchCreator patchCreator;
        [SerializeField] private Material lineMaterial;
        private Dictionary<int, LineRenderer> lines;
        private LineRenderer lineRenderer;

        private static WireDrawer instance;

        public void AddConnection(int id, Outlet outlet, Inlet inlet)
        {
            LineRenderer lineRenderer = CreateLineRenderer(outlet.gameObject, outlet.PatchBody.Color);
            lineRenderer.SetPositions(GeneratePath(lineRenderer, outlet.transform.position, inlet.transform.position));
            lines.Add(id, lineRenderer);
        }

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                lines = new Dictionary<int, LineRenderer>();
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
        }

        public void Clear()
        {
            foreach (LineRenderer lineRenderer in lines.Values)
            {
                Destroy(lineRenderer.gameObject);
            }
            lines.Clear();
        }

        public void Draw(Vector3 position)
        {
            lineRenderer.SetPositions(GeneratePath(lineRenderer, lineRenderer.GetPosition(0), position));
        }

        public static WireDrawer GetInstance()
        {
            return instance;
        }

        public void Load(Connection[] connections)
        {
            Dictionary<int, PatchBody> patchBodies = new Dictionary<int, PatchBody>();
            PatchBody originPatchBody;
            PatchBody targetPatchBody;
            Outlet[] outlets;
            Outlet originOutlet = null;
            Inlet[] inlets;
            Inlet targetInlet = null;
            foreach (PatchBody patchBody in patchCreator.PatchBodies)
            {
                patchBodies.Add(patchBody.Patch.Id, patchBody);
            }
            foreach(Connection connection in connections)
            {
                originPatchBody = patchBodies[connection.SourcePatch];
                outlets = originPatchBody.GetComponentsInChildren<Outlet>();
                foreach(Outlet outlet in outlets)
                {
                    originOutlet = outlet;
                }
                targetPatchBody = patchBodies[connection.TargetPatch];
                inlets = targetPatchBody.GetComponentsInChildren<Inlet>();
                foreach (Inlet inlet in inlets)
                {
                    targetInlet = inlet;
                }
                AddConnection(connection.Id, originOutlet, targetInlet);
            }
        }

        public void MovePatch(PatchBody patchBody)
        {
            LineRenderer lineRenderer;
            Outlet[] outlets = patchBody.GetComponentsInChildren<Outlet>();
            foreach (Connection connection in patchBody.Patch.GetOutputConnections())
            {
                foreach (Outlet outlet in outlets)
                {
                    if (connection.SourceOutlet == outlet.Id)
                    {
                        lineRenderer = lines[connection.Id];
                        lineRenderer.SetPositions(GeneratePath(lineRenderer, outlet.transform.position, lineRenderer.GetPosition(lineRenderer.positionCount - 1)));
                    }
                }
            }
            Inlet[] inlets = patchBody.GetComponentsInChildren<Inlet>();
            foreach (Connection connection in patchBody.Patch.GetInputConnections())
            {
                foreach (Inlet inlet in inlets)
                {
                    if (connection.TargetInlet == inlet.Id)
                    {
                        lineRenderer = lines[connection.Id];
                        lineRenderer.SetPositions(GeneratePath(lineRenderer, lineRenderer.GetPosition(0), inlet.transform.position));
                    }
                }
            }
        }

        public void Release()
        {
            Destroy(lineRenderer.gameObject);
            lineRenderer = null;
        }

        public void RemovePatch(PatchBody patchBody)
        {
            LineRenderer lineRenderer;
            foreach (Connection connection in patchBody.Patch.GetOutputConnections())
            {
                lineRenderer = lines[connection.Id];
                lines.Remove(connection.Id);
                Destroy(lineRenderer.gameObject);
            }
            foreach (Connection connection in patchBody.Patch.GetInputConnections())
            {
                lineRenderer = lines[connection.Id];
                lines.Remove(connection.Id);
                Destroy(lineRenderer.gameObject);
            }
        }

        public void StartDraw(GameObject start, Color color)
        {
            lineRenderer = CreateLineRenderer(start, color);
            lineRenderer.SetPositions(GeneratePath(lineRenderer, start.transform.position, start.transform.position));
        }

        private LineRenderer CreateLineRenderer(GameObject parent, Color color)
        {
            GameObject gameObject = new GameObject();
            gameObject.transform.parent = parent.transform;
            LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.widthCurve = new AnimationCurve(new Keyframe(0, Others.WireWidth));
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;
            lineRenderer.sortingLayerName = Layers.Wires;
            lineRenderer.material = lineMaterial;
            return lineRenderer;
        }

        private Vector3[] GeneratePath(LineRenderer lineRenderer, Vector3 start, Vector3 end)
        {
            Vector3[] vector3s = new Vector3[Others.InterpolationPoints];
            float step = 1f / (Others.InterpolationPoints - 1);
            float interpolation;
            start.z = Others.WireHeight;
            end.z = Others.WireHeight;
            Vector3 mid = new Vector3()
            {
                x = (start.x + end.x) / 2,
                y = (start.y + end.y) / 2 - Math.Abs(end.x - start.x) / 4,
                z = -1
            };
            for (int i = 0; i < Others.InterpolationPoints; i++)
            {
                interpolation = i * step;
                vector3s[i] = (1.0f - interpolation) * (1.0f - interpolation) * start + 2.0f * (1.0f - interpolation) * interpolation * mid + interpolation * interpolation * end;
            }
            lineRenderer.positionCount = Others.InterpolationPoints;
            lineRenderer.SetPositions(vector3s);
            return vector3s;
        }
    }
}
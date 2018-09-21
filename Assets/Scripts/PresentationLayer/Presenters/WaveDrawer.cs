using UnityEngine;
using DomainLayer.Common;
using Constants;

public class WaveDrawer : MonoBehaviour, IWaveDrawer
{
    public LineRenderer lineRendererLeft;
    public LineRenderer lineRendererRigth;
    private Vector3[] leftPositions;
    private Vector3[] rigthPositions;
    private bool changed = false;
    private float[] soundData;
    private int bufferSize;

    void IWaveDrawer.Init(int bufferSize)
    {
        this.bufferSize = bufferSize;
        leftPositions = new Vector3[bufferSize];
        lineRendererLeft.positionCount = bufferSize;
        rigthPositions = new Vector3[bufferSize];
        lineRendererRigth.positionCount = bufferSize;
        soundData = new float[bufferSize * 2];
    }

    void IWaveDrawer.LoadWave(float[] data)
    {
        data.CopyTo(soundData, 0);
        changed = true;
    }

    void Update()
    {
        if (changed)
        {
            changed = false;
            float cameraX = Camera.main.transform.position.x;
            float cameraY = Camera.main.transform.position.y;
            float cameraSize = Camera.main.orthographicSize * 2;
            float offset = cameraSize / 5;
            float heightAdjust = Camera.main.orthographicSize / Others.CameraMaxSize;
            float relation = (2 * cameraSize) / (bufferSize - 1);

            for (int i = 0; i < leftPositions.Length; i++)
            {
                leftPositions[i] = new Vector3((relation * i) - cameraSize + cameraX, (soundData[i * 2] * heightAdjust) + offset + cameraY, 0);
                rigthPositions[i] = new Vector3((relation * i) - cameraSize + cameraX, (soundData[(i * 2) + 1] * heightAdjust) - offset + cameraY, 0);
            }

            lineRendererLeft.startWidth = Others.WaveWidth * cameraSize;
            lineRendererLeft.endWidth = Others.WaveWidth * cameraSize;
            lineRendererLeft.SetPositions(leftPositions);
            lineRendererRigth.startWidth = Others.WaveWidth * cameraSize;
            lineRendererRigth.endWidth = Others.WaveWidth * cameraSize;
            lineRendererRigth.SetPositions(rigthPositions);
        }
    }
}

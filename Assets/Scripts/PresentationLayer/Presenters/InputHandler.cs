using Constants;
using PresentationLayer.UserInput;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PresentationLayer.Presenters
{
    public class InputHandler : MonoBehaviour
    {
        [SerializeField] private PatchCreator patchCreator;
        public PatchCreator PatchCreator { get { return patchCreator; } }
        private float timeSinceLastTap = 0f;

        void Awake()
        {
            AddPhysics2DRaycaster();
        }

        public static Vector3 GetTouchPosition()
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = -Camera.main.transform.position.z;
            return Camera.main.ScreenToWorldPoint(mousePos);
        }

        public static RaycastHit2D RayCast(params string[] layers)
        {
            int layerMask = LayerMask.GetMask(layers);
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = -Camera.main.transform.position.z;
            return Physics2D.Raycast(Camera.main.ScreenToWorldPoint(mousePos), Vector2.one, 0f, layerMask);
        }

        public static void CheckCameraLimits()
        {
            Camera camera = Camera.main;
            if (camera.orthographicSize + camera.transform.position.x > Others.CameraMaxPanX)
            {
                camera.transform.position = new Vector3(Others.CameraMaxPanX - camera.orthographicSize, camera.transform.position.y, camera.transform.position.z);
            }
            else if (camera.orthographicSize - camera.transform.position.x > Others.CameraMaxPanX)
            {
                camera.transform.position = new Vector3(-Others.CameraMaxPanX + camera.orthographicSize, camera.transform.position.y, camera.transform.position.z);
            }
            if (camera.orthographicSize + camera.transform.position.y > Others.CameraMaxPanY)
            {
                camera.transform.position = new Vector3(camera.transform.position.x, Others.CameraMaxPanY - camera.orthographicSize, camera.transform.position.z);
            }
            else if (camera.orthographicSize - camera.transform.position.y > Others.CameraMaxPanY)
            {
                camera.transform.position = new Vector3(camera.transform.position.x, -Others.CameraMaxPanY + camera.orthographicSize, camera.transform.position.z);
            }
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (timeSinceLastTap > float.Epsilon)
                {
                    DoubleTap();
                }
                else
                {
                    timeSinceLastTap = Others.TimeForDoubleTap;
                }
            }
            else
            {
                if (timeSinceLastTap > float.Epsilon)
                {
                    timeSinceLastTap -= Time.deltaTime;
                }
            }

#if UNITY_ANDROID && !UNITY_EDITOR
            if (Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);
                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
                float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;
                float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

                ZoomOrthographicCamera(Camera.main.ScreenToWorldPoint(Input.mousePosition), -deltaMagnitudeDiff / 100f);
            }
#else
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                ZoomOrthographicCamera(Camera.main.ScreenToWorldPoint(Input.mousePosition), 1);
            }

            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                ZoomOrthographicCamera(Camera.main.ScreenToWorldPoint(Input.mousePosition), -1);
            }
#endif
        }

        private void AddPhysics2DRaycaster()
        {
            Physics2DRaycaster physicsRaycaster = FindObjectOfType<Physics2DRaycaster>();
            if (physicsRaycaster == null)
            {
                Camera.main.gameObject.AddComponent<Physics2DRaycaster>();
            }
        }

        private void DoubleTap()
        {
            RaycastHit2D raycastHit2D = RayCast(Layers.PatchBodies);
            if (raycastHit2D.collider)
            {
                PatchBody patchBody = raycastHit2D.collider.gameObject.GetComponent<PatchBody>();
                patchCreator.OpenEditMenu(patchBody);
            }
        }

        private void ZoomOrthographicCamera(Vector3 zoomTowards, float amount)
        {
            float multiplier = (1f / Camera.main.orthographicSize * amount);
            transform.position += (zoomTowards - transform.position) * multiplier;
            Camera.main.orthographicSize -= amount;
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, Others.CameraMinSize, Others.CameraMaxSize);
            CheckCameraLimits();
        }
    }
}
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

namespace LevelEditor
{
    public class LevelEditorCameraControls : MonoBehaviour
    {

        Vector3 origin;
        Vector3 diff;

        [SerializeField] Camera _mainCam;
        [SerializeField] CinemachineVirtualCamera _mainCamVirtualCamera;

        [SerializeField] float maxCamZoom;
        [SerializeField] float minCamZoom;
        [SerializeField] float ZoomSens;
        float camZoomLevel = 15;

        bool _isDragging;

       public void OnDrag(InputAction.CallbackContext context)
       {
            if(PauseMenu.instance.isPaused) return;

            if (context.started) origin = GetMousePos;
            _isDragging = context.started || context.performed;
       }

        public void OnZoomIn(InputAction.CallbackContext context)
        {
            if (PauseMenu.instance.isPaused) return;
            Debug.Log("Zooming In?");
            if (context.started)
            {
                camZoomLevel += ZoomSens;
                camZoomLevel = Mathf.Clamp(camZoomLevel, minCamZoom, maxCamZoom);
             //   Debug.Log(camZoomLevel);
                _mainCamVirtualCamera.m_Lens.OrthographicSize = camZoomLevel;
            }
        }

        public void OnZoomOut(InputAction.CallbackContext context) 
        {
            if (PauseMenu.instance.isPaused) return;
            if (context.started)
            {
                camZoomLevel -= ZoomSens;
                camZoomLevel = Mathf.Clamp(camZoomLevel, minCamZoom, maxCamZoom);
                Debug.Log(camZoomLevel);
                _mainCamVirtualCamera.m_Lens.OrthographicSize = camZoomLevel;
            }
        }


        private void LateUpdate()
        {
            if (PauseMenu.instance.isPaused) return;
            if (!_isDragging) return;

            diff = GetMousePos - transform.position;
            transform.position = origin - diff;
        }

        private Vector3 GetMousePos => _mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

    }
}


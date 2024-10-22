using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraZoom : MonoBehaviour
{
    private Actions actions;
    private InputAction zoomAction;

    [SerializeField] private float minZoomDistanceP = 30;
    [SerializeField] private float maxZoomDistanceP = 120;

    [SerializeField] private float minZoomDistanceO = 5;
    [SerializeField] private float maxZoomDistanceO = 20;

    void Awake()
    {
        actions = new Actions();
        zoomAction = actions.camera.zoom;
    }

    void OnEnable()
    {
        zoomAction.Enable();
        zoomAction.performed += ZoomFunction;
    }

    void OnDisable()
    {
        zoomAction.Disable();
        zoomAction.performed -= ZoomFunction;
    }

    private void ZoomFunction(InputAction.CallbackContext context)
    {
        float mult = -0.01f;
        float zoomVal = zoomAction.ReadValue<float>();
        if (Camera.main.orthographic == true){
            float currentSize = Camera.main.orthographicSize;
            Camera.main.orthographicSize = Mathf.Clamp(currentSize + (zoomVal*mult), minZoomDistanceO, maxZoomDistanceO);
        }else{
            float currentFOV = Camera.main.fieldOfView;
            Camera.main.fieldOfView = Mathf.Clamp(currentFOV + (zoomVal*mult), minZoomDistanceP, maxZoomDistanceP);
        }
    }
}

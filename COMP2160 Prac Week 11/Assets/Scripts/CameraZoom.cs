using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraZoom : MonoBehaviour
{
    private Actions actions;
    private InputAction zoomAction;

    [SerializeField] private GameObject marble;
    [SerializeField] private float minZoomDistance = 30;
    [SerializeField] private float maxZoomDistance = 120;

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
        float zoomVal = zoomAction.ReadValue<float>();
        if (Camera.main.orthographic == true){
            Camera.main.orthographicSize += zoomVal*-0.01f;
        }else{
            Camera.main.fieldOfView += zoomVal*-0.01f;
        }
    }
    
    void Update(){
        if (marble != null){
            this.transform.position = marble.transform.position + new Vector3(0,10,0);
        }
    }
}

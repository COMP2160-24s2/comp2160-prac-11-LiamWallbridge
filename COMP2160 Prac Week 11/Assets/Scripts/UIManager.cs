/**
 * A singleton class to allow point-and-click movement of the marble.
 * 
 * It publishes a TargetSelected event which is invoked whenever a new target is selected.
 * 
 * Author: Malcolm Ryan
 * Version: 1.0
 * For Unity Version: 2022.3
 */

using UnityEngine;
using UnityEngine.InputSystem;

// note this has to run earlier than other classes which subscribe to the TargetSelected event
[DefaultExecutionOrder(-100)]
public class UIManager : MonoBehaviour
{
#region UI Elements
    [SerializeField] private Transform crosshair;
    [SerializeField] private Transform target;
    [SerializeField] private bool deltaEnabled;
#endregion 

#region Singleton
    static private UIManager instance;
    static public UIManager Instance
    {
        get { return instance; }
    }
#endregion 

#region Actions
    private Actions actions;
    private InputAction mouseAction;
    private InputAction deltaAction;
    private InputAction selectAction;
#endregion

#region Events
    public delegate void TargetSelectedEventHandler(Vector3 worldPosition);
    public event TargetSelectedEventHandler TargetSelected;
#endregion

#region Init & Destroy
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("There is more than one UIManager in the scene.");
        }

        instance = this;

        actions = new Actions();
        mouseAction = actions.mouse.position;
        deltaAction = actions.mouse.delta;
        selectAction = actions.mouse.select;

        Cursor.visible = false;
        target.gameObject.SetActive(false);
    }

    void OnEnable()
    {
        actions.mouse.Enable();
    }

    void OnDisable()
    {
        actions.mouse.Disable();
    }
#endregion Init

#region Update
    void Update()
    {
        MoveCrosshair();
        SelectTarget();
    }

    private void MoveCrosshair() 
    {
        if (deltaEnabled){
            //STEP 5 METHOD
            Vector2 delta = deltaAction.ReadValue<Vector2>();

            Vector3 crosshairScreenPos = Camera.main.WorldToScreenPoint(crosshair.position);
            crosshairScreenPos += new Vector3(delta.x, delta.y, 0);

            crosshairScreenPos.x = Mathf.Clamp(crosshairScreenPos.x, 0, Screen.width);
            crosshairScreenPos.y = Mathf.Clamp(crosshairScreenPos.y, 0, Screen.height);

            Vector3 newCrosshairWorldPos = Camera.main.ScreenToWorldPoint(crosshairScreenPos);
            crosshair.position = newCrosshairWorldPos;
        }
        else{
            //DEFAULT METHOD
            Vector3 mousePosition = new Vector3(mouseAction.ReadValue<Vector2>().x, mouseAction.ReadValue<Vector2>().y, 0);
            LayerMask mask = LayerMask.GetMask("Walls");
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
            {
                crosshair.transform.position = hit.point + new Vector3(0,0.1f,0);
            }
        }
    }

    private void SelectTarget()
    {
        if (selectAction.WasPerformedThisFrame())
        {
            // set the target position and invoke 
            target.gameObject.SetActive(true);
            target.position = crosshair.position;     
            TargetSelected?.Invoke(target.position);       
        }
    }

#endregion Update

}

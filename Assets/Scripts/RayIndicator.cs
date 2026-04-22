using UnityEngine;
using UnityEngine.InputSystem;

public class RayIndicator : MonoBehaviour
{
    public Transform rayOrigin;
    public LineRenderer line;
    public InputActionReference triggerAction;
    public float maxDistance = 10f;

    private GameObject currentHover;
    private GameObject currentSelected;
    private Renderer hoverRenderer;
    private Renderer selecedRenderer;

    void OnEnable() 
    {
        triggerAction.action.Enable();
    }

    void OnDisable() 
    {
        triggerAction.action.Disable();
    }
    
    void Update()
    {
        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);
        RaycastHit hit;
        Vector3 endPoint = rayOrigin.position + rayOrigin.forward * maxDistance;
        
        if (currentHover != null && currentHover != currentSelected && hoverRenderer != null)
        {
            hoverRenderer.material.color = Color.white; //change hover color
        }

        currentHoever = null;
        hoverRenderer = null;

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            endPoint = hit.point;

            if (hit.collider.GetComponentInParent<Grabbable>() != null)
            {
                currentHover = hit.collider.gameObject;
                hoverRenderer = currentHover.GetComponent<Renderer>();

                if (currentHover != currentSelected && hoverRenderer != null)
                {
                    hoverRenderer.material.color = Color.yellow;
                }
            }
        }

        line.SetPosition(0, rayOrigin.position);
        line.SetPosition(1, endPoint);

        if (triggerAction.action.WasPressedThisFrame()) 
        {
            if (currentSelected != null && selectedRenderer != null) 
            {
                selectedRenderer.material.color = Color.white;
            }
            currentSelected = currentHover;
            if (currentSelected != null)
            {
                selectedRenderer = currentSelected.GetComponenet<Rederer>();
                if (selectedRenderer != null)
                {
                    selectedRenderer.material.color = Color.yellow;
                }
            }
        }
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

public class RayIndicator : MonoBehaviour
{
    public Transform rayOrigin;
    public LineRenderer line;
    public float maxDistance = 10f;
    public RaycastHit hit;

    private GameObject currentHover;
    private GameObject currentSelected;
    private Renderer hoverRenderer;
    private Renderer selectedRenderer;

    
    void Update()
    {
        // Variable for new ray (actual ray itself)
        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);

        // Variable for the endpoint of the ray (where it lands)
        Vector3 endPoint = rayOrigin.position + rayOrigin.forward * maxDistance;

        // Resetting any previous selected/hovered objects
        if (currentHover != null && currentHover != currentSelected && hoverRenderer != null)
        {
            hoverRenderer.material.color = Color.white; //change hover color
        }

        currentHover = null;
        hoverRenderer = null;

        // Checks if ray is hitting any object
        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            endPoint = hit.point;

            //Checking if the hit object is interactable (Comment this part out)
            
            /**
            if (hit.collider.GetComponentInParent<Grabbable>() != null)
            {
                currentHover = hit.collider.gameObject;
                hoverRenderer = currentHover.GetComponent<Renderer>();

                //Setting selected object color
                if (currentHover != currentSelected && hoverRenderer != null)
                {
                    hoverRenderer.material.color = Color.yellow;
                }

            }
            */
           

        }

        //Adjusts visible line to match ray
        line.SetPosition(0, rayOrigin.position);
        line.SetPosition(1, endPoint);

        //Checks trigger button was pressed
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            // Resetting color for previously selected object
            if (currentSelected != null && selectedRenderer != null) 
            {
                selectedRenderer.material.color = Color.white;
            }

            // Setting color for current selected object
            currentSelected = currentHover;
            if (currentSelected != null)
            {
                selectedRenderer = currentSelected.GetComponent<Renderer>();
                if (selectedRenderer != null)
                {
                    selectedRenderer.material.color = Color.yellow;
                }
            }
        }
    }
}

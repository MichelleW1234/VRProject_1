using UnityEngine;
using UnityEngine.InputSystem;
using Oculus.Interaction;

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

        //Adjusts visible line to match ray
        line.SetPosition(0, rayOrigin.position);
        line.SetPosition(1, endPoint);

    }
}

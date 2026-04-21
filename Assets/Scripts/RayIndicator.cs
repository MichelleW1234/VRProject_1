using UnityEngine;
using UnityEngine.InputSystem;

public class RayIndicator : MonoBehaviour
{
    public Transform rayOrigin;
    public LineRenderer line;
    public Transform hitMarker;
    public float maxDistance = 10f;

    private SelectableObject currentHover;
    private SelectableObject currentSelected;

    void Update()
    {
        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);
        RaycastHit hit;

        Vector3 endPoint = rayOrigin.position + rayOrigin.forward * maxDistance;
        SelectableObject newHover = null;

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            endPoint = hit.point;

            // move hit marker
            if (hitMarker != null)
            {
                hitMarker.position = hit.point;
                hitMarker.gameObject.SetActive(true);
            }

            newHover = hit.collider.GetComponentInParent<SelectableObject>();
        }
        else
        {
            if (hitMarker != null)
                hitMarker.gameObject.SetActive(false);
        }

        // update line
        line.SetPosition(0, rayOrigin.position);
        line.SetPosition(1, endPoint);

        // hover highlight
        if (currentHover != null && currentHover != currentSelected)
            currentHover.SetHighlight(false);

        currentHover = newHover;

        if (currentHover != null && currentHover != currentSelected)
            currentHover.SetHighlight(true);

        // trigger input (OVR version example)
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            if (currentSelected != null)
                currentSelected.SetSelected(false);

            if (currentHover != null)
            {
                currentSelected = currentHover;
                currentSelected.SetSelected(true);
            }
        }
    }
}

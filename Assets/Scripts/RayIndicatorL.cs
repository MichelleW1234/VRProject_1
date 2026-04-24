using UnityEngine;
using UnityEngine.InputSystem;

public class RayIndicatorL : MonoBehaviour
{
    public Transform rayOrigin;
    public LineRenderer line;
    public float maxDistance = 10f;
    public RaycastHit hit;


    void Update()
    {
        // Variable for new ray (actual ray itself)
        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);

        // Variable for the endpoint of the ray (where it lands)
        Vector3 endPoint = rayOrigin.position + rayOrigin.forward * maxDistance;


        // Checks if ray is hitting any object
        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            endPoint = hit.point;

        }

        //Adjusts visible line to match ray
        line.SetPosition(0, rayOrigin.position);
        line.SetPosition(1, endPoint);

    }
}

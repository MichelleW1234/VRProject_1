using UnityEngine;
using UnityEngine.InputSystem;
using Oculus.Interaction;

public class RayIndicator : MonoBehaviour
{
    public Transform rayOrigin;
    public LineRenderer line;
    public float maxDistance = 10f;
    public RaycastHit hit;
    public bool hasHit;

    public Ray ray;
    public Vector3 endPoint;
    
    void Update()
    {
        // Variable for new ray (actual ray itself)
        ray = new Ray(rayOrigin.position, rayOrigin.forward);

        // Variable for the endpoint of the ray (where it lands)
        endPoint = rayOrigin.position + rayOrigin.forward * maxDistance;


        hasHit = Physics.Raycast(ray, out hit, maxDistance);
        if (hasHit)
        {
            endPoint = hit.point;
        }
   

        //Adjusts visible line to match ray
        line.SetPosition(0, rayOrigin.position);
        line.SetPosition(1, endPoint);

    }


}

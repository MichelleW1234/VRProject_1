using UnityEngine;

public class RayIndicatorL : MonoBehaviour
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
        if (rayOrigin == null || line == null)
        {
            return;
        }

        ray = new Ray(rayOrigin.position, rayOrigin.forward);
        endPoint = rayOrigin.position + rayOrigin.forward * maxDistance;

        hasHit = Physics.Raycast(ray, out hit, maxDistance);

        if (hasHit)
        {
            endPoint = hit.point;
        }

        line.SetPosition(0, rayOrigin.position);
        line.SetPosition(1, endPoint);
    }
}
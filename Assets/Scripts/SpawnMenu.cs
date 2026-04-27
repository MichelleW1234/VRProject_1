using UnityEngine;

public class SpawnMenu : MonoBehaviour
{

    public RayIndicator currentRay;
    public GameObject activeMenuOption;


    void Start()
    {
        activeMenuOption = GameObject.Find("chair");
    }

    void Update()
    {
            
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {

            Collider chairCollider = GameObject.Find("chairButton").GetComponent<Collider>();
            Collider cabinetCollider = GameObject.Find("cabinetButton").GetComponent<Collider>();

            if (chairCollider.ClosestPoint(currentRay.endPoint) == currentRay.endPoint)
            { //set chair to active
                
                activeMenuOption = GameObject.Find("chair");

            } else if (cabinetCollider.ClosestPoint(currentRay.endPoint) == currentRay.endPoint)
            { // set cabinet to active
                
                activeMenuOption = GameObject.Find("cabinet");

            }
            
            // close menu (outside of menu)
            gameObject.SetActive(false);

        }
        
    }
}

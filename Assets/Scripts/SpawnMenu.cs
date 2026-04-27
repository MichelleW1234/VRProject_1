using UnityEngine;

public class SpawnMenu : MonoBehaviour
{

    public RayIndicator currentRay;
    public GameObject activeMenuOption;
    public GameObject chairPrefab;
    public GameObject cabinetPrefab;
    public GameObject LockerPrefab;



    void Start()
    {

    }

    void Update()
    {
            
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            gameObject.SetActive(true);
            Debug.Log("A button is clicked");

            Collider chairCollider = GameObject.Find("ChairButton").GetComponent<Collider>();
            Collider cabinetCollider = GameObject.Find("CabinetButton").GetComponent<Collider>();
            Collider lockerCollider = GameObject.Find("LockerButton").GetComponent<Collider>();


            if (chairCollider.ClosestPoint(currentRay.endPoint) == currentRay.endPoint)
            { //set chair to active
                Debug.Log("CHAIR IS ASSIGNED TO ACTIVE MENU");
                activeMenuOption = chairPrefab;

            } 
            else if (cabinetCollider.ClosestPoint(currentRay.endPoint) == currentRay.endPoint)
            { // set cabinet to active
                Debug.Log("CABINET IS ASSIGNED TO ACTIVE MENU");

                activeMenuOption = cabinetPrefab;

            }
            else if (lockerCollider.ClosestPoint(currentRay.endPoint) == currentRay.endPoint)
            { // set cabinet to active
                Debug.Log("LOCKER IS ASSIGNED TO ACTIVE MENU");

                activeMenuOption = LockerPrefab;

            }

            // close menu (outside of menu)
            gameObject.SetActive(false);

        }
        
    }
}

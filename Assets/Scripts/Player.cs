using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Mathematics;
using Unity.XR.CoreUtils;

public class Player : MonoBehaviour
{
    public RayIndicator rayCastInfo;

    public GameObject playerPOV;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


        //Checks trigger button was pressed
        if (OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger))
        {
            
            // Resetting color for previously selected object
            if (rayCastInfo.currentSelected != null && rayCastInfo.selectedRenderer != null) 
            {
                rayCastInfo.selectedRenderer.material.color = Color.white;
            }

            // Setting color for current selected object
            rayCastInfo.currentSelected = rayCastInfo.currentHover;
            if (rayCastInfo.currentSelected != null)
            {
                rayCastInfo.selectedRenderer = rayCastInfo.currentSelected.GetComponent<Renderer>();
                if (rayCastInfo.selectedRenderer != null)
                {
                    rayCastInfo.selectedRenderer.material.color = Color.yellow;
                }
            }
        }

        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            
            GameObject newObject1 = GameObject.Find("chair");
            Debug.Log("chair is created");
            Vector3 objectInitialPosition = rayCastInfo.hit.point;
            Quaternion objectInitialRotation = quaternion.identity;
            GameObject.Instantiate(newObject1, objectInitialPosition, objectInitialRotation);

        }

        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {

            playerPOV.transform.localPosition = rayCastInfo.hit.point;

        }
        
    }


}

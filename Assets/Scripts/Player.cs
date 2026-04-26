using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Mathematics;
using Unity.XR.CoreUtils;
using Oculus.Interaction;
using System.Collections.Generic;


public class Player : MonoBehaviour
{

    public GameObject SpawnMenu;
    public RayIndicator rayCastInfo;
    public GameObject playerPOV;

    private bool isGrabbable = false;
    private float grabbing_dist;

    private GameObject currentHover;
    private GameObject currentSelected;
    private Renderer hoverRenderer;
    private Renderer selectedRenderer;
    private Dictionary<Renderer, Color> defaultColor = new Dictionary<Renderer, Color>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        HandleHighlight();
        HandleSelect();
        HandleSpawn();
        HandleTeleport();
        HandleGrabbedObject();
        
    }

    private void HandleSelect()
    {
        if (OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger))
        {
            //restore original color for previously selected object
            if (currentSelected != null)
            {
                Renderer[] oldRenderers = currentSelected.GetComponentsInChildren<Renderer>();
                foreach (Renderer r in oldRenderers)
                {
                    if (defaultColor.ContainsKey(r))
                    {
                        r.material.color = defaultColor[r];
                    }
                }
            }
            currentSelected = currentHover;

            if (currentSelected != null)
            {
                Renderer[] renderers = currentSelected.GetComponentsInChildren<Renderer>();
                foreach (Renderer r in renderers)
                {
                    if (!defaultColor.ContainsKey(r))
                    {
                        defaultColor[r] = r.material.color;
                    }

                    r.material.color = Color.yellow;
                }

                //we set it true here because if currentSelected != null, there is an grabbable object selected
                isGrabbable = true; 


                grabbing_dist = Vector3.Distance(rayCastInfo.rayOrigin.position,currentSelected.transform.position);

                //get the objects rigidbody and allow moving
                Rigidbody rb = currentSelected.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                }
            }
        }
    }

    private void HandleMenu()
    {
        
        // Figure out what player should do to trigger menu
        SpawnMenu.SetActive(true);

    }

    private void HandleSpawn()
    {
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {

            GameObject currObject = SpawnMenu.GetComponent<SpawnMenu>().activeMenuOption;
            Vector3 objectInitialPosition =  new Vector3(
                                                rayCastInfo.hit.point.x,
                                                rayCastInfo.hit.point.y + currObject.transform.position.y,
                                                rayCastInfo.hit.point.z
                                            );
            Quaternion objectInitialRotation = quaternion.identity;
            GameObject.Instantiate(currObject, objectInitialPosition, objectInitialRotation);
 
        }
    }

    private void HandleTeleport()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {

            //edited
            if (rayCastInfo.hit.name.Contains("SM-M-Floor"))
            {
                
                playerPOV.transform.localPosition = rayCastInfo.hit.point;

            } else
            {
                Debug.Log("Unable to proceed!!");

            }
            /*
            if (rayCastInfo.hit.point.y == 0)
            {
                
                playerPOV.transform.localPosition = rayCastInfo.hit.point;

            } else
            {
                Debug.Log("Unable to proceed!!");

            }
            */

        }

    }

    private void HandleHighlight()
    {
        // Reset previous hover (restore original color)
        if (currentHover != null && currentHover != currentSelected)
        {
            Renderer[] oldRenderers = currentHover.GetComponentsInChildren<Renderer>();
            foreach (Renderer r in oldRenderers)
            {
                if (defaultColor.ContainsKey(r))
                {    
                    r.material.color = defaultColor[r];
                }
            }
        }

        currentHover = null;

        // check if raycast is actually hitting object
        if (rayCastInfo == null || !rayCastInfo.hasHit || rayCastInfo.hit.collider == null)
            return;

        // check if object is grabbable
        Grabbable grabbable = rayCastInfo.hit.collider.GetComponentInParent<Grabbable>();

        if (grabbable == null)
            return;

        // set current select to the object of select
        currentHover = grabbable.gameObject;

        // Highlight new hover if it was not already selected
        if (currentHover != currentSelected)
        {
            Renderer[] renderers = currentHover.GetComponentsInChildren<Renderer>();
            //store original color for later restore
            foreach (Renderer r in renderers)
            {
                if (!defaultColor.ContainsKey(r))
                {
                    defaultColor[r] = r.material.color;
                }
                r.material.color = Color.yellow;
            }
        }
    }

    private void HandleGrabbedObject()
    {
        if (!isGrabbable || currentSelected == null)
            return;

        // While holding the button move the selected object followed by ray
        Vector3 targetPosition = rayCastInfo.rayOrigin.position + rayCastInfo.rayOrigin.forward * grabbing_dist;

        currentSelected.transform.position = targetPosition;

        // When button is released, stop grabbing
        if (OVRInput.GetUp(OVRInput.Button.SecondaryHandTrigger))
        {
            Rigidbody rb = currentSelected.GetComponent<Rigidbody>(); //get object's ridig body
            if (rb != null)
            {
                rb.isKinematic = false; //disallow moving
            }

            isGrabbable = false;
        }
    }

}

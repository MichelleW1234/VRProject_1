using Oculus.Interaction;
using System.Collections.Generic;
using System.Net;
using Unity.Mathematics;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;


public class Player : MonoBehaviour
{

    public GameObject Menu;
    public RayIndicator rayCastInfo;
    public RayIndicatorL L_rayCastInfo; //gets left controller raycast info for moving objects

    public GameObject playerPOV;

    private bool isGrabbable = false;
    private float grabbing_dist;

    private float rotateSpeed = 60f; 

    private GameObject currentHover;
    private GameObject currentSelected;
    private Dictionary<Renderer, Color> defaultColor = new Dictionary<Renderer, Color>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //----controllers ------
        //left trigger == teleport
        //left grip (on select) == scale object down
        //both grip on select == scale object up
        //right grip = select & move
        //right trigger == spawn
        //left trigger + right grip == roatate

        //----object highlight information----
        //CYAN = current pointing object
        //Yellow = current selected object

    }

    // Update is called once per frame
    void Update()
    {
        HandleOrientation();

        HandleHighlight();
        HandleRightGripSelect(); //selects object (1st selection method)
        HandleLeftGripSelect(); //selects object (2nd selection method)
        HandleSpawn(); //right trigger
        HandleTeleport(); //left trigger 

        //right grip does normal select
        //hold right grip + left trigger does rotation
        HandleMoveSelected(); //hold right grip to move selected objects
        HandleScaleSelected();//both grip scale selected object
        HandleRotateSelected();
    }

    private void HandleOrientation()
    {
        Vector2 rightThumbstick = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

        if (Mathf.Abs(rightThumbstick.x) > 0.1f)
        {
            float rotateSpeed = 40f; 

            transform.Rotate(0f, rightThumbstick.x * rotateSpeed * Time.deltaTime, 0f);
        }

    }


    private void HandleHighlight()
    {
        if (currentHover != null && currentHover != currentSelected)
        {
            RestoreColor(currentHover);
        }

        currentHover = null;
        isGrabbable = false;

        if (rayCastInfo == null || !rayCastInfo.hasHit || rayCastInfo.hit.collider == null)
            return;

        Grabbable grabbable = rayCastInfo.hit.collider.GetComponentInParent<Grabbable>();

        if (grabbable == null)
            return;

        isGrabbable = true;
        currentHover = grabbable.gameObject;

        if (currentHover != currentSelected)
        {
            HighlightObject(currentHover, Color.cyan); //indicate current pointing object as cyan
        }
    }

    //helper function for HandleHighlight()
    private void HighlightObject(GameObject obj, Color color)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();

        foreach (Renderer r in renderers)
        {
            if (!defaultColor.ContainsKey(r))
            {
                defaultColor[r] = r.material.color;
            }

            r.material.color = color;
        }
    }

    //helper function for HandleHighlight()
    private void RestoreColor(GameObject obj)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();

        foreach (Renderer r in renderers)
        {
            if (defaultColor.ContainsKey(r))
            {
                r.material.color = defaultColor[r];
            }
        }
    }


    private void HandleRightGripSelect()
    {
        if (OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger))
        {
            if (isGrabbable && currentHover != null)
            {
                SelectCurrent(); //helper to handle object select
            }
        }
    }
    private void HandleLeftGripSelect()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger))
        {
            if (isGrabbable && currentHover != null)
            {
                SelectCurrent(); //helper to handle object select
            }
        }
    }


    //helper function to handle selection and making object moveable
    private void SelectCurrent()
    {
        if (currentSelected != null)
        {
            RestoreColor(currentSelected); //helper to restore old color of previous selected
        }
        currentSelected = currentHover; //change current select to current ray hitting object

        if (currentSelected != null)
        {
            HighlightObject(currentSelected, Color.yellow); //helper to highlight current select object

            //we set it true here because if currentSelected != null, there is an grabbable object selected
            isGrabbable = true;

            grabbing_dist = Vector3.Distance(rayCastInfo.rayOrigin.position, currentSelected.transform.position);

        }
    }

    private void HandleMenu()
    {
        
        // Figure out what player should do to trigger menu
        Menu.SetActive(true);

    }

    private void HandleSpawn()
    {
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            if (rayCastInfo == null)
            {
                Debug.LogError("rayCastInfo is not assigned in Player Inspector.");
                return;
            }

            if (!rayCastInfo.hasHit || rayCastInfo.hit.collider == null)
            {
                Debug.Log("ray is not hitting anything.");
                return;
            }

            GameObject currObject = Menu.GetComponent<SpawnMenu>().activeMenuOption;

            Vector3 objectInitialPosition =  new Vector3(
                                                rayCastInfo.hit.point.x,
                                                rayCastInfo.hit.point.y + 1f,
                                                rayCastInfo.hit.point.z
                                            );
            Quaternion objectInitialRotation = currObject.transform.rotation;
            GameObject.Instantiate(currObject, objectInitialPosition, objectInitialRotation);
 
        }
    }

    private void HandleTeleport()
    {
        bool rightGrip = OVRInput.Get(OVRInput.Button.SecondaryHandTrigger);

        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) && (!rightGrip))
        {
            
            if (L_rayCastInfo == null)
            {
                Debug.LogError("L_rayCastInfo is not assigned in Player Inspector.");
                return;
            }

            if (!L_rayCastInfo.hasHit || L_rayCastInfo.hit.collider == null)
            {
                Debug.Log("Left ray is not hitting anything.");
                return;
            }
            
            GameObject hitObject = L_rayCastInfo.hit.collider.gameObject;

            Debug.Log("Left ray hit: " + hitObject.name);
            Debug.Log("Left ray hit tag: " + hitObject.tag);

            if (hitObject.CompareTag("TentFloor"))
            {
                playerPOV.transform.localPosition = L_rayCastInfo.hit.point;
            }
            else
            {
                Debug.Log("Unable to proceed!! Hit object: " + hitObject.name);
            }
        }
    }

    private void HandleMoveSelected()
    {
        if (currentSelected == null) return;

        bool leftGrip = OVRInput.Get(OVRInput.Button.PrimaryHandTrigger);
        bool rightGrip = OVRInput.Get(OVRInput.Button.SecondaryHandTrigger);
        //bool leftTrigger = OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger);


        //when only leftgrip is getting hold,
        if (!leftGrip && rightGrip)
        {

            //get the objects rigidbody and allow moving
            Rigidbody rb = currentSelected.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }
            //the selected object follows the right controller ray allowing moving object
            Vector3 targetPos = rayCastInfo.rayOrigin.position + rayCastInfo.rayOrigin.forward
                                  * grabbing_dist;
            currentSelected.transform.position = targetPos;
        }

        //when right grip is not hold, release object from selected
        if (OVRInput.GetUp(OVRInput.Button.SecondaryHandTrigger))
        {
            Rigidbody rb = currentSelected.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
            }

            RestoreColor(currentSelected);

            currentSelected = null;
            isGrabbable = false;
        }
    }

    private void HandleScaleSelected()
    {
        if (currentSelected == null) return;

        bool leftGrip = OVRInput.Get(OVRInput.Button.PrimaryHandTrigger);
        bool rightGrip = OVRInput.Get(OVRInput.Button.SecondaryHandTrigger);


        // Both grips = scale up
        if (leftGrip && rightGrip)
        {
            currentSelected.transform.localScale += Vector3.one * 0.001f;
        }
        // if only left grip = scale down
        if (leftGrip && !rightGrip)
        {
            currentSelected.transform.localScale -= Vector3.one * 0.0005f;

            //checks scale not going into negative 
            if (currentSelected.transform.localScale.x < 0.01f) 
            {
                currentSelected.transform.localScale = Vector3.one * 0.01f; 
                //if scale is too small, set to fixed scale value 
            }
        }
        
    }

    private void HandleRotateSelected()
    {
        if (currentSelected == null) return;

        bool rightGrip = OVRInput.Get(OVRInput.Button.SecondaryHandTrigger);
        bool leftTrigger = OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger);

        // Right grip + left trigger = rotate
        if (rightGrip && leftTrigger)
        {
            currentSelected.transform.Rotate(
                Vector3.up,
                rotateSpeed * Time.deltaTime,
                Space.World
            );
        }

    }
}

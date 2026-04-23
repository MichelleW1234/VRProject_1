using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Mathematics;

public class Player : MonoBehaviour
{
    public RayIndicator rayCastInfo;
    public InputActionReference placeAction;
    public Vector3 playerPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        placeAction.action.performed += PlaceFunction;
        placeAction.action.Enable();
        Debug.Log("PLAYER OnEnable is enabled");


    }

    private void OnDisable()
    {
        placeAction.action.performed -= PlaceFunction;
        placeAction.action.Disable();
        Debug.Log("PLAYER OnDisable is enabled");


    }

    private void PlaceFunction(InputAction.CallbackContext context)
    {
        
        GameObject newObject1 = GameObject.Find("Object1");
        Debug.Log("OBJECT1 is created");
        Vector3 objectInitialPosition = rayCastInfo.hit.point;
        Quaternion objectInitialRotation = quaternion.identity;
        GameObject.Instantiate(newObject1, objectInitialPosition, objectInitialRotation);

    }
}

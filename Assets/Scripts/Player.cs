using UnityEngine;
using Unity.Engine.InputSystem;
using Unity.Mathematics;

public class Player : MonoBehaviour
{
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

    }

    private void OnDisable()
    {
        placeAction.action.performed -= PlaceFunction;
        placeAction.action.Disable();

    }

    private void PlaceFunction(InputAction.CallbackContext context)
    {
        
        GameObject newObject1 = GameObject.Find("Object1");
        Vector3 objectInitialPosition = transform.forward;
        Quaternion objectInitialRotation = quaternion.identity;
        GameObject.Insantiate(newObject1, objectInitialPosition, objectInitialRotation);

    }
}

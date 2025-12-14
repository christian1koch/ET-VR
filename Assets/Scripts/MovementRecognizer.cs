using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class MovementRecognizer : MonoBehaviour
{
    public XRNode inputSource;
    public InputHelpers.Button inputButton;
    public float inputThreshold = 0.1f;
    private bool isMoving = false;
    public Transform movementSource;

    public GameObject debugCubePrefab;
    
    public float newPositionThresholdDistance = 0.01f;
    private List<Vector3> positionList = new List<Vector3>();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        InputHelpers.IsPressed(InputDevices.GetDeviceAtXRNode(inputSource), inputButton, out bool isPressed, inputThreshold);
        
        // Start the movement
        if (!isMoving && isPressed)
        {
            StartMovement();
        }
        // Ending the Movement
        else if (isMoving && isPressed)
        {
            EndMovement();
        }
        // Updating the Movement
        else if (!isMoving && isPressed)
        {
            UpdateMovement();
        }
    }

    void StartMovement()
    {
        Debug.Log("Starting movement");
        isMoving = true;
        positionList.Clear();
        positionList.Add(movementSource.position);
        if (debugCubePrefab)
        {
            Destroy(Instantiate(debugCubePrefab, movementSource.position, Quaternion.identity), 3f);
        }
    }
    void EndMovement()
    {
        Debug.Log("Ending movement");
        isMoving = false;
    }

    void UpdateMovement()
    {
        Debug.Log("Moving...");
        Vector3 lastPosition = positionList[positionList.Count - 1];
        if (Vector3.Distance(lastPosition, movementSource.position) > newPositionThresholdDistance)
        {
            positionList.Add(movementSource.position);
            if (debugCubePrefab)
            {
                Destroy(Instantiate(debugCubePrefab, movementSource.position, Quaternion.identity), 3f);
            }
        }
    }
}

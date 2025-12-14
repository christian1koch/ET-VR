using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class MovementRecognizer : MonoBehaviour
{
    public XRNode inputSource;
    public InputHelpers.Button inputButton;
    public float inputThreshold = 0.1f;
    private bool isMoving = false;
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
    }
    void EndMovement()
    {
        Debug.Log("Ending movement");
        isMoving = false;
    }

    void UpdateMovement()
    {
        Debug.Log("Moving...");
    }
}

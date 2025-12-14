using System.Collections.Generic;
using PDollarGestureRecognizer;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using System.IO;
using UnityEngine.Events;

public class MovementRecognizer : MonoBehaviour
{
    [Header("Input Settings")]
    [SerializeField, Tooltip("The XR node to track input from (e.g., LeftHand or RightHand)")]
    private XRNode inputSource;
    
    [SerializeField, Tooltip("The button to use for gesture input")]
    private InputHelpers.Button inputButton;
    
    [SerializeField, Tooltip("Threshold for button press detection")]
    private float inputThreshold = 0.1f;
    
    [SerializeField, Tooltip("Transform to track movement from (usually the controller or hand)")]
    private Transform movementSource;

    [Header("Recognition Settings")]
    [SerializeField, Tooltip("Minimum score required to recognize a gesture (0-1)")]
    private float recognitionThreshold = 0.8f;
    
    [SerializeField, Tooltip("Minimum distance between recorded positions")]
    private float newPositionThresholdDistance = 0.01f;

    [Header("Creation Mode")]
    [SerializeField, Tooltip("Enable to create and save new gestures instead of recognizing")]
    private bool creationMode;
    
    [SerializeField, Tooltip("Name for the new gesture when in creation mode")]
    private string newGestureName = "New Gesture";

    [Header("Debug")]
    [SerializeField, Tooltip("Prefab to instantiate at each recorded position for debugging")]
    private GameObject debugCubePrefab;

    [Header("Events")]
    [Tooltip("Event triggered when a gesture is recognized, passes the gesture name")]
    public UnityEvent<string> OnGestureRecognized;

    // Private fields
    private bool isMoving;
    private List<Gesture> trainingSet = new List<Gesture>();
    private List<Vector3> positionList = new List<Vector3>();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        string[] gestureFiles = Directory.GetFiles(Application.dataPath + "/Resources/GestureSet/", "*.xml");
        foreach (string file in gestureFiles)
        {
            trainingSet.Add(GestureIO.ReadGestureFromFile(file));
        }
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
        else if (isMoving && !isPressed)
        {
            EndMovement();
        }
        // Updating the Movement
        else if (isMoving && isPressed)
        {
            UpdateMovement();
        }
    }

    void StartMovement()
    {
        positionList.Clear();
        Debug.Log("Starting movement");
        isMoving = true;
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
        // Create the gesture from the position List
        Point[] pointArray = new Point[positionList.Count];

        for (int i = 0; i < positionList.Count; i++)
        {
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(positionList[i]);
            pointArray[i] = new Point(screenPoint.x, screenPoint.y, 0);
        }
        Gesture newGesture = new Gesture(pointArray);
        if (creationMode) 
        {
            newGesture.Name = newGestureName;
            trainingSet.Add(newGesture);
            Debug.Log($"Added new gesture: {newGestureName} with {pointArray.Length} points.");
            string fileName = Application.dataPath + "/Resources/GestureSet/" + newGestureName + ".xml";
            GestureIO.WriteGesture(pointArray, newGestureName, fileName);
        }
        else
        {
            Result result = PointCloudRecognizer.Classify(newGesture, trainingSet.ToArray());
            Debug.Log($"Recognized gesture: {result.GestureClass} with score {result.Score}");
            if (result.Score >= recognitionThreshold)
            {
                OnGestureRecognized?.Invoke(result.GestureClass);
            }
        }
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

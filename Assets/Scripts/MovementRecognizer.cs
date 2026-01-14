using System;
using System.Collections.Generic;
using PDollarGestureRecognizer;
using Spellcasting_System;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using System.IO;
using UnityEngine.Serialization;

public enum SketchType
{
    Square,
    Star,
    Triangle,
    Circle,
}

public class MovementRecognizer : MonoBehaviour, IRecognitionSystem
{
    [Header("Input Settings")]
    [SerializeField, Tooltip("Use hand tracking with pinch instead of controller")]
    private bool useHandTracking;
    
    [SerializeField, Tooltip("OVRHand component for right hand (only used in hand tracking mode)")]
    private OVRHand rightHand;
    
    [SerializeField, Tooltip("The XR node to track input from (e.g., LeftHand or RightHand)")]
    private XRNode inputSource;
    
    [SerializeField, Tooltip("The button to use for gesture input")]
    private InputHelpers.Button inputButton;
    
    [SerializeField, Tooltip("Threshold for button press detection")]
    private float inputThreshold = 0.1f;
    
    [SerializeField, Tooltip("Pinch strength threshold for hand tracking (0-1)")]
    private float pinchThreshold = 0.8f;
    
    [SerializeField, Tooltip("Transform to track movement from controller")]
    private Transform movementSource;
    
    [SerializeField, Tooltip("Transform to track movement from hand")]
    private Transform handMovementSource;

    [Header("Recognition Settings")]
    [SerializeField, Tooltip("Minimum score required to recognize a gesture (0-1)")]
    private float recognitionThreshold = 0.8f;
    
    [SerializeField, Tooltip("Minimum distance between recorded positions")]
    private float newPositionThresholdDistance = 0.005f; // Smaller threshold to capture more detail

    [Header("Creation Mode")]
    [SerializeField, Tooltip("Enable to create and save new gestures instead of recognizing")]
    private bool creationMode;
    
    [SerializeField, Tooltip("Name for the new gesture when in creation mode")]
    private string newGestureName = "New Gesture";

    [Header("Debug")]
    [SerializeField, Tooltip("Prefab to instantiate at each recorded position for debugging")]
    private GameObject debugCubePrefab;

    // Events
    // Event triggered when a gesture is recognized, passes the SketchType enum
    public event Action<int> OnRecognized;

    // Private fields
    private bool isMoving;
    private List<Gesture> trainingSet = new List<Gesture>();
    private List<Vector3> positionList = new List<Vector3>();
    private GameObject debugCubeInstance;
    
    public Vector3 centerPosition;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        string[] gestureFiles = Directory.GetFiles(Application.dataPath + "/Resources/GestureSet/", "*.xml");
        foreach (string file in gestureFiles)
        {
            trainingSet.Add(GestureIO.ReadGestureFromFile(file));
        }
        if (useHandTracking && handMovementSource != null)
        {
            movementSource = handMovementSource;
        }
        
        // Report tracking mode to SpellAnalytics
        if (SpellAnalytics.Instance != null)
        {
            SpellAnalytics.Instance.SetTrackingMode(useHandTracking);
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool isPressed = false;
        
        if (useHandTracking && rightHand != null)
        {
            // Hand tracking mode - check pinch strength
            float pinchStrength = rightHand.GetFingerPinchStrength(OVRHand.HandFinger.Index);
            isPressed = pinchStrength > pinchThreshold;
        }
        else
        {
            // Controller mode
            InputHelpers.IsPressed(InputDevices.GetDeviceAtXRNode(inputSource), inputButton, out isPressed, inputThreshold);
        }
        
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
        if (debugCubePrefab && debugCubeInstance == null)
        {
            debugCubeInstance = Instantiate(debugCubePrefab, movementSource.position, Quaternion.identity);
        }
    }
    void EndMovement()
    {
        Debug.Log("Ending movement");
        isMoving = false;
        
        // Destroy the debug cube when movement ends
        if (debugCubeInstance != null)
        {
            Destroy(debugCubeInstance);
            debugCubeInstance = null;
        }
        
        // Calculate the center of the gesture
        centerPosition = CalculateGestureCenter();
        Debug.Log($"Gesture center: {centerPosition}");
        
        // Create the gesture from the position List
        Point[] pointArray = new Point[positionList.Count];

        // Calculate a local coordinate system based on the gesture
        Vector3 center = centerPosition;
        
        // Use camera's orientation for the projection plane
        Camera cam = Camera.main;
        Vector3 up = cam != null ? cam.transform.up : Vector3.up;
        Vector3 right = cam != null ? cam.transform.right : Vector3.right;

        for (int i = 0; i < positionList.Count; i++)
        {
            // Project onto a plane perpendicular to the camera
            Vector3 localPos = positionList[i] - center;
            float x = Vector3.Dot(localPos, right);
            float y = Vector3.Dot(localPos, up);
            pointArray[i] = new Point(x * 1000f, y * 1000f, 0); // Scale up for better precision
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
                // Parse lowercase gesture name to SketchType enum (case-insensitive)
                if (Enum.TryParse(result.GestureClass, true, out SketchType sketchType))
                {
                    OnRecognized?.Invoke((int)sketchType);
                    Debug.Log($"Successfully recognized as {sketchType}");
                }
                else
                {
                    Debug.LogWarning($"Gesture '{result.GestureClass}' does not match any SketchType enum value");
                }
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
            centerPosition = CalculateGestureCenter();
        }
        
        // Update debug cube position to follow movement source
        if (debugCubeInstance != null)
        {
            debugCubeInstance.transform.position = movementSource.position;
        }
    }
    
    private Vector3 CalculateGestureCenter()
    {
        if (positionList.Count == 0)
            return Vector3.zero;

        Vector3 sum = Vector3.zero;
        foreach (Vector3 position in positionList)
        {
            sum += position;
        }
        
        return sum / positionList.Count;
    }
    
}

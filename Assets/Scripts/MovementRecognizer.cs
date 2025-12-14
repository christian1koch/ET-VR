using System.Collections.Generic;
using PDollarGestureRecognizer;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using System.IO;

public class MovementRecognizer : MonoBehaviour
{
    public XRNode inputSource;
    public InputHelpers.Button inputButton;
    public float inputThreshold = 0.1f;
    private bool isMoving = false;
    public Transform movementSource;

    public GameObject debugCubePrefab;
    public bool creationMode = true;
    public string newGestureName = "New Gesture";
    public float newPositionThresholdDistance = 0.01f;
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

using System;
using Oculus.Interaction.PoseDetection;
using UnityEngine;
using UnityEngine.Events;

public class RecognitionSystem : MonoBehaviour, IRecognitionSystem
{
    public ShapeRecognizer recognizedPose = null;
    public event Action<int> OnRecognized;
    public UnityEvent<string> onUnityRecognized;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SetRecognized(ShapeRecognizer recognized)
    {
        recognizedPose = recognized;
        //TODO: Find a better way to do this, something like a list we can add the list of Poses and then each one recieves a index
        OnRecognized?.Invoke(Int32.Parse(recognizedPose.ShapeName));
        onUnityRecognized?.Invoke(recognizedPose.name);
    }
}

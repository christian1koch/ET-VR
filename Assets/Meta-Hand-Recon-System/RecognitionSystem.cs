using System;
using Oculus.Interaction.PoseDetection;
using UnityEngine;

public class RecognitionSystem : MonoBehaviour
{
    public ShapeRecognizer recognizedPose = null;
    public event Action<int> OnRecognized;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SetRecognized(ShapeRecognizer recognized)
    {
        recognizedPose = recognized;
        //TODO: Find a better way to do this, something like a list we can add the list of Poses and then each one recieves a index
        OnRecognized?.Invoke(Int32.Parse(recognizedPose.ShapeName));
    }
}

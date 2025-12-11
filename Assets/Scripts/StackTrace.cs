using UnityEngine;
using System;

public class StrackTrace : MonoBehaviour
{
    void OnEnable()
    {
        Debug.Log($"{name} ENABLED\n{Environment.StackTrace}", this);
    }

    void OnDisable()
    {
        Debug.Log($"{name} DISABLED\n{Environment.StackTrace}", this);
    }
}
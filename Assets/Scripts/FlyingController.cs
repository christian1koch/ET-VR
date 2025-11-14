using System;
using UnityEngine;
using Oculus.Interaction.Locomotion;

public class FlyingController : MonoBehaviour
{
    [SerializeField] private FlyingLocomotor locomotor;
    [SerializeField] private Transform headTransform;
    [SerializeField] private float flySpeed = 5f;
    [SerializeField] private float gravity = 9.81f;
    private float verticalVelocity = 0f;
    void Update()
    {
        
        float input = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.LTouch);

        if (input > 0.1f)
        {
            // Apply upward velocity based on trigger input
            verticalVelocity = input * flySpeed;
        }
        else if (locomotor.IsGrounded) 
        {
            // Reset vertical velocity when grounded
            verticalVelocity = 0f;
        }
        else
        {
            // Apply gravity when not pressing trigger
            verticalVelocity -= gravity * Time.deltaTime;
        }

        // Always apply the vertical velocity
        Vector3 velocity = Vector3.up * verticalVelocity;

        var locomotionEvent = new LocomotionEvent(
            Guid.NewGuid().GetHashCode(),
            velocity,
            LocomotionEvent.TranslationType.Velocity
        );

        locomotor.HandleLocomotionEvent(locomotionEvent);
    }
}
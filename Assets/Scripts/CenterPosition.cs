using UnityEngine;

public class CenterPosition : MonoBehaviour
{
    [SerializeField, Tooltip("Reference to the MovementRecognizer")]
    private MovementRecognizer movementRecognizer;
    
    [SerializeField, Tooltip("Placeholder position when no gesture center is available")]
    private Vector3 placeholderPosition = new Vector3(0, 1, 2);

    // Update is called once per frame
    void Update()
    {
        if (movementRecognizer)
        {
            // Use the center position from the gesture, or placeholder if it's zero
            Vector3 targetPosition = movementRecognizer.centerPosition;
            
            if (targetPosition == Vector3.zero)
            {
                transform.position = placeholderPosition;
            }
            else
            {
                transform.position = targetPosition;
            }
        }
        else
        {
            transform.position = placeholderPosition;
        }
    }
}

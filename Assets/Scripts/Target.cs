using UnityEngine;

public class Target : MonoBehaviour
{
    private void Start()
    {
        // Register this target with the GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RegisterTarget(this);
        }
        else
        {
            Debug.LogWarning("GameManager not found! Target will not be tracked.");
        }
    }
    private void OnDestroy()
    {
        // Notify the GameManager when this target is destroyed
        if (GameManager.Instance != null)
        {
            Debug.Log("Target Destroyed!");
            GameManager.Instance.OnTargetDestroyed(this);
        }
    }
}


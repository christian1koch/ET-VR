using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    public static GameManager Instance { get; private set; }

    [Header("Events")]

    private List<Target> activeTargets = new List<Target>();
    private bool isGameActive = true;

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void RegisterTarget(Target target)
    {
        if (!activeTargets.Contains(target))
        {
            activeTargets.Add(target);
            Debug.Log($"Target registered. Total targets: {activeTargets.Count}");
        }
    }

    public void OnTargetDestroyed(Target target)
    {
        if (!isGameActive) return;

        if (activeTargets.Contains(target))
        {
            activeTargets.Remove(target);
            Debug.Log($"Target destroyed! Remaining targets: {activeTargets.Count}");

            // Check if all targets are destroyed
            if (activeTargets.Count == 0)
            {
                EndGame();
            }
        }
    }

    public void EndGame()
    {
        if (!isGameActive) return;

        isGameActive = false;
        Debug.Log("Game Over! All targets destroyed!");
        
        // Quit the application
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}


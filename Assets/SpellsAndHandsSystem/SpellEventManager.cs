using System;
using UnityEngine;

public class SpellEventManager : MonoBehaviour
{
    [SerializeField] private RecognitionSystem handRecognitionSystem;
    [SerializeField] private ControllerRecognitionSystem controllerRecognitionSystem;
    [SerializeField] private MovementRecognizer sketchRecognitionSystem;
    [SerializeField] private bool castOnRecognize = true;
    
    private IRecognitionSystem recognitionSystem;
    public event Action<int> onSpellCast;
    private int selectedSpell = 0;

    private void Awake()
    {
        // Choose which system to use (prioritize hand recognition if assigned)
        if (handRecognitionSystem != null)
        {
            recognitionSystem = handRecognitionSystem;
        }
        else if (controllerRecognitionSystem != null)
        {
            recognitionSystem = controllerRecognitionSystem;
        }
        else if (sketchRecognitionSystem != null)
        {
            recognitionSystem = sketchRecognitionSystem;
        }
        else
        {
            Debug.LogError("No recognition system assigned to SpellEventManager.");
        }
    }

    private void OnEnable()
    {
        if (recognitionSystem != null)
        {
            recognitionSystem.OnRecognized += SetSelectedSpell;
        }
    }

    private void OnDisable()
    {
        if (recognitionSystem != null)
        {
            recognitionSystem.OnRecognized -= SetSelectedSpell;
        }
    }

    void SetSelectedSpell(int spellNumber)
    {
        selectedSpell = spellNumber;
        if (castOnRecognize)
        {
            CastSpell();
        }
    }

    public void CastSpell()
    {
        onSpellCast?.Invoke(selectedSpell);
    }
    
}

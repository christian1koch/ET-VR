using System;
using UnityEngine;

public class SpellEventManager : MonoBehaviour
{
    [SerializeField] private RecognitionSystem handRecognitionSystem;
    [SerializeField] private ControllerRecognitionSystem controllerRecognitionSystem;
    
    private IRecognitionSystem recognitionSystem;
    public event Action<int> onSpellCast;
    private int selectedSpell = 0;

    private void Awake()
    {
        // Choose which system to use (prioritize hand recognition if assigned)
        recognitionSystem = handRecognitionSystem != null ? handRecognitionSystem : (IRecognitionSystem)controllerRecognitionSystem;
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
    }

    public void CastSpell()
    {
        onSpellCast?.Invoke(selectedSpell);
    }
    
}

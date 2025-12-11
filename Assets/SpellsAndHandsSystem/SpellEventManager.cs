using System;
using UnityEngine;

public class SpellEventManager : MonoBehaviour
{
    
    [SerializeField] private RecognitionSystem recognitionSystem;
    private IRecognitionSystem _recognitionSystem;
    public event Action<int> onSpellCast;
    private int selectedSpell = 0;

    private void Awake()
    {
        _recognitionSystem = recognitionSystem;
    }

    private void OnEnable()
    {
        _recognitionSystem.OnRecognized += SetSelectedSpell;
    }

    private void OnDisable()
    {
        _recognitionSystem.OnRecognized -= SetSelectedSpell;
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

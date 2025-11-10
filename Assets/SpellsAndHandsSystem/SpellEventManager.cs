using System;
using UnityEngine;

public class SpellEventManager : MonoBehaviour
{
    
    [SerializeField] private RecognitionSystem recognitionSystem;
    public event Action<int> onSpellCast;
    private int selectedSpell = 0;

    private void OnEnable()
    {
        recognitionSystem.OnRecognized += SetSelectedSpell;
    }

    void SetSelectedSpell(int spellNumber)
    {
        selectedSpell = spellNumber;
    }
    
}

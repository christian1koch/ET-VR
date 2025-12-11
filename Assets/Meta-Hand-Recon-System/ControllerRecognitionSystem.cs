using System;
using UnityEngine;

public class ControllerRecognitionSystem : MonoBehaviour, IRecognitionSystem
{
    // Spell Indexes go from 0 to 4
    [SerializeField] private int numOfSpells = 5;
    private int selectedSpell = 0;
    
    public event Action<int> OnRecognized;

    public void IncreaseSpell()
    {
        selectedSpell++;
        if (selectedSpell >= numOfSpells)
        {
            selectedSpell = 0;
        }
        OnRecognized?.Invoke(selectedSpell);
    }

    public void DecreaseSpell()
    {
        selectedSpell--;
        if (selectedSpell < 0)
        {
            selectedSpell = numOfSpells - 1;
        }
        OnRecognized?.Invoke(selectedSpell);
    }
}


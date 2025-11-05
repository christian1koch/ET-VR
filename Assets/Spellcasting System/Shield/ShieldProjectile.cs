using Spellcasting_System;
using UnityEngine;
using UnityEngine.InputSystem;
public class ShieldProjectile : MonoBehaviour, SpellBehaviour
{
    
    private Spell sourceSpell;
    private float spawnTime;
    
    public void Init(Spell spell, Transform castPoint)
    {
        sourceSpell = spell;
        spawnTime = Time.time;
    }
    
    private void Update()
    {
        if (sourceSpell == null) return;

        // Lifetime check
        if (Mouse.current.leftButton.isPressed)
        {
            return;
        }
        Destroy(gameObject);
    }
}

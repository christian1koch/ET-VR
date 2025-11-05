using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Spellcasting_System
{
    public class SpellCaster : MonoBehaviour
    {
        [Header("Setup")]
        public Transform castPoint;                // Empty transform in front of camera
        public List<Spell> availableSpells;        // Assign ScriptableObjects here

        private int currentSpellIndex = 0;
        private float nextCastTime = 0f;

        private void Update()
        {
            HandleSpellSwitch();
            HandleCast();
        }

        private void HandleSpellSwitch()
        {
            // Mouse wheel scroll to switch spells
            float scroll = Mouse.current.scroll.ReadValue().y;
            if (scroll != 0 && availableSpells.Count > 0)
            {
                currentSpellIndex = (currentSpellIndex + (scroll > 0 ? 1 : -1) + availableSpells.Count) % availableSpells.Count;
                Debug.Log("Selected spell: " + availableSpells[currentSpellIndex].spellName);
            }
        }

        private void HandleCast()
        {
            if (availableSpells.Count == 0) return;
            if (!Mouse.current.leftButton.wasPressedThisFrame) return;

            Spell spell = availableSpells[currentSpellIndex];
            if (Time.time < nextCastTime)
                return; // still cooling down
           
            var projectile = spell.Cast(castPoint);
            var spellProj = projectile.GetComponent<SpellBehaviour>();
            spellProj?.Init(spell, castPoint);
            nextCastTime = Time.time + spell.cooldown;
        }
    }
}
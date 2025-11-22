using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

namespace Spellcasting_System
{
    public class SpellCaster : MonoBehaviour
    {
        [Header("Setup")]
        public LookAtTarget lookAt;                // Empty transform in front of camera
        public List<Spell> availableSpells;        // Assign ScriptableObjects here
        
        private float nextCastTime = 0f;
        [SerializeField] private SpellEventManager spellEventManager;

        private void OnEnable()
        {
            spellEventManager.onSpellCast += HandleCast;
        }

        private void OnDisable()
        {
            spellEventManager.onSpellCast -= HandleCast;
        }
        private void HandleCast(int spellNum)
        {
            var castPoint = lookAt.objToSpawn.transform;
            Spell spell = availableSpells[spellNum];
            if (Time.time < nextCastTime)
                return; // still cooling down
           
            var projectile = spell.Cast(castPoint.transform);
            var spellProj = projectile.GetComponent<SpellBehaviour>();
            spellProj?.Init(spell, castPoint);
            nextCastTime = Time.time + spell.cooldown;
        }
    }
}
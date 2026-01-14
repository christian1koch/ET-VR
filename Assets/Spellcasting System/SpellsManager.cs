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
           
            var projectile = CastSpell(spell, castPoint, (SketchType)spellNum);
            if (projectile != null)
            {
                var spellProj = projectile.GetComponent<SpellBehaviour>();
                spellProj?.Init(spell, castPoint, (SketchType)spellNum);
            }
            nextCastTime = Time.time + spell.cooldown;
        }

        private GameObject CastSpell(Spell spell, Transform castPoint, SketchType spellType)
        {
            if (spell.projectilePrefab == null) return null;
            
            // Track spell fired
            if (SpellAnalytics.Instance != null)
            {
                SpellAnalytics.Instance.RecordSpellFired(spellType);
            }
            
            // Spawn projectile
            return Object.Instantiate(spell.projectilePrefab, castPoint.position, 
                castPoint.rotation * spell.projectilePrefab.transform.localRotation);
        }
    }
}
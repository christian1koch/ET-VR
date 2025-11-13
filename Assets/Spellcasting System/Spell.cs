using Oculus.Interaction.Input;
using UnityEngine;

namespace Spellcasting_System
{
    [CreateAssetMenu(menuName = "Spells/Spell")]
    public class Spell : ScriptableObject
    {
        [Header("Basic Info")]
        public string spellName = "New Spell";
        public GameObject projectilePrefab;

        [Header("Stats")]
        public float damage = 25f;
        public float lifetime = 5f;
        public float speed = 25f;
        public float cooldown = 1f;
        public float manaCost = 10f;

        // Called by SpellCaster when cast
        public virtual GameObject Cast(Transform castPoint)
        {
            if (projectilePrefab == null) return null;
            // Spawn projectile
            var projectile = Instantiate(projectilePrefab, castPoint.position, castPoint.rotation * projectilePrefab.transform.localRotation);
            return projectile;
        }
    }
}
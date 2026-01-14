using UnityEngine;

namespace Spellcasting_System
{
    public class SpellProjectile : MonoBehaviour, SpellBehaviour
    {
        private Spell sourceSpell;
        private float spawnTime;
        private bool _hasHit = false;
        private SketchType _spellType;

        // Initialize with data from the ScriptableObject
        public void Init(Spell spell, Transform castPoint, SketchType spellType)
        {
            sourceSpell = spell;
            spawnTime = Time.time;
            _spellType = spellType;
            // Apply velocity
            var rb = GetComponent<Rigidbody>();
                rb.linearVelocity = castPoint.forward * sourceSpell.speed;
                // rotate 90 degrees in the y axis cause the model is -90 degrees
                Debug.Log("Forward");
                Debug.Log(castPoint.forward);

                Debug.Log("Rotation");
                Debug.Log(castPoint.rotation);
        }


        private void Update()
        {
            if (sourceSpell == null) return;

            // Lifetime check
            if (Time.time - spawnTime >= sourceSpell.lifetime)
                Destroy(gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (sourceSpell == null || _hasHit) return;

            // Check if the collision object has the matching tag
            if (!string.IsNullOrEmpty(sourceSpell.targetTag) && 
                collision.gameObject.CompareTag(sourceSpell.targetTag))
            {
                _hasHit = true;
                
                // Track hit
                if (SpellAnalytics.Instance != null)
                {
                    SpellAnalytics.Instance.RecordSpellHit(_spellType);
                }
                
                Destroy(collision.gameObject); // Destroy the target
            }

            Destroy(gameObject); // Destroy the projectile
        }
    }
}
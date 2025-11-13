using UnityEngine;

namespace Spellcasting_System
{
    public class SpellProjectile : MonoBehaviour, SpellBehaviour
    {
        private Spell sourceSpell;
        private float spawnTime;

        // Initialize with data from the ScriptableObject
        public void Init(Spell spell, Transform castPoint)
        {
            sourceSpell = spell;
            spawnTime = Time.time;
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
            if (sourceSpell == null) return;
            
            Destroy(gameObject);
        }
    }
}
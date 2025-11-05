using UnityEngine;

namespace Spellcasting_System.Ice_Lance
{
    public class IceLanceProjectile : MonoBehaviour, SpellBehaviour
    {
        private Spell sourceSpell;
        private float spawnTime;
        private Quaternion spawnRotation;
        private bool hasColided = false;
        // Initialize with data from the ScriptableObject
        public void Init(Spell spell, Transform castPoint)
        {
            sourceSpell = spell;
            spawnTime = Time.time;
                
            // Apply velocity
            var rb = GetComponent<Rigidbody>();
                rb.linearVelocity = castPoint.forward * sourceSpell.speed;
                
            spawnRotation = castPoint.rotation * spell.projectilePrefab.transform.localRotation;
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
            if (hasColided) return;
            hasColided = true;
            Rigidbody rb = GetComponent<Rigidbody>();
            
            ContactPoint contact = collision.contacts[0];

            // Record the hit point and direction of travel

            // Stop all physics
           
            // Stop all physics
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;

            // Move to the impact pointfloat embedOffset = 0.1f; // tweak based on your model length
            transform.position = contact.point;
            transform.rotation = spawnRotation;
            // Rotate so the forward direction matches the travel direction
            
            

        }
    }
}
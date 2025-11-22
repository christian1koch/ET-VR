using Oculus.Interaction;
using UnityEngine;

namespace DefaultNamespace
{
    public class LookAtTarget : MonoBehaviour
    {
        public Quaternion lastRotation;
        public Vector3 lastPosition;
        public GameObject objToSpawn;
        [SerializeField]
        private Transform _toRotate;

        [SerializeField]
        private Transform _target;

        protected virtual void Start()
        {
            this.AssertField(_toRotate, nameof(_toRotate));
            this.AssertField(_target, nameof(_target));
            // Instantiate an empty Object at root
            objToSpawn = new GameObject("ObjectToSpawn");

        }

        protected virtual void Update()
        {
            // We create a new object because this object is influenced by another script changing it transform
            // So the new object has ONLY the rotation and position we want and then that's what gets sent to the spell system
            // TODO: Clean this script because it's doing 2 things at once
            Vector3 dirToTarget = (_target.position - _toRotate.position).normalized;
            _toRotate.LookAt(_toRotate.position - dirToTarget, Vector3.up);
            lastRotation = _toRotate.rotation;
            lastPosition = _toRotate.position;
            objToSpawn.transform.rotation = _toRotate.rotation;
            objToSpawn.transform.position = _toRotate.position;
        }
    }
}
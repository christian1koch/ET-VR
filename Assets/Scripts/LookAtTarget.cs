using Oculus.Interaction;
using UnityEngine;

namespace DefaultNamespace
{
    public class LookAtTarget : MonoBehaviour
    {
        [SerializeField]
        private Transform _toRotate;

        [SerializeField]
        private Transform _target;

        protected virtual void Start()
        {
            this.AssertField(_toRotate, nameof(_toRotate));
            this.AssertField(_target, nameof(_target));
        }

        protected virtual void LateUpdate()
        {
            Vector3 dirToTarget = (_target.position - _toRotate.position).normalized;
            _toRotate.LookAt(_toRotate.position - dirToTarget, Vector3.up);
        }
    }
}
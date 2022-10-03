using UnityEngine;

namespace FMODUnity
{
    public abstract class EventHandler : MonoBehaviour
    {
        public string CollisionTag = "";

        protected virtual void Start()
        {
            HandleGameEvent(EmitterGameEvent.ObjectStart);
        }

        protected virtual void OnDestroy()
        {
            HandleGameEvent(EmitterGameEvent.ObjectDestroy);
        }
 

        private void OnEnable()
        {
            HandleGameEvent(EmitterGameEvent.ObjectEnable);
        }

        private void OnDisable()
        {
            HandleGameEvent(EmitterGameEvent.ObjectDisable);
        }

        #if UNITY_PHYSICS_EXIST
        private void OnTriggerEnter(Collider other)
        {
            if (string.IsNullOrEmpty(CollisionTag) || other.CompareTag(CollisionTag) || (other.attachedRigidbody && other.attachedRigidbody.CompareTag(CollisionTag)))
            {
                HandleGameEvent(EmitterGameEvent.TriggerEnter, other);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (string.IsNullOrEmpty(CollisionTag) || other.CompareTag(CollisionTag) || (other.attachedRigidbody && other.attachedRigidbody.CompareTag(CollisionTag)))
            {
                HandleGameEvent(EmitterGameEvent.TriggerExit);
            }
        }
        #endif

        #if UNITY_PHYSICS2D_EXIST
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (string.IsNullOrEmpty(CollisionTag) || other.CompareTag(CollisionTag))
            {
                HandleGameEvent(EmitterGameEvent.TriggerEnter2D);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (string.IsNullOrEmpty(CollisionTag) || other.CompareTag(CollisionTag))
            {
                HandleGameEvent(EmitterGameEvent.TriggerExit2D);
            }
        }
        #endif

        private void OnCollisionEnter(Collision collision)
        {
            HandleGameEvent(EmitterGameEvent.CollisionEnter, collision);
        }

        private void OnCollisionExit()
        {
            HandleGameEvent(EmitterGameEvent.CollisionExit);
        }

        private void OnCollisionEnter2D()
        {
            HandleGameEvent(EmitterGameEvent.CollisionEnter2D);
        }

        private void OnCollisionExit2D()
        {
            HandleGameEvent(EmitterGameEvent.CollisionExit2D);
        }

        private void OnMouseEnter()
        {
            HandleGameEvent(EmitterGameEvent.MouseEnter);
        }

        private void OnMouseExit()
        {
            HandleGameEvent(EmitterGameEvent.MouseExit);
        }

        private void OnMouseDown()
        {
            HandleGameEvent(EmitterGameEvent.MouseDown);
        }

        private void OnMouseUp()
        {
            HandleGameEvent(EmitterGameEvent.MouseUp);
        }

        protected abstract void HandleGameEvent(EmitterGameEvent gameEvent);
        protected abstract void HandleGameEvent(EmitterGameEvent gameEvent, Collision collision);
        protected abstract void HandleGameEvent(EmitterGameEvent gameEvent, Collider collider);
    }
}
using System.Collections;
using UnityEngine;

namespace Guns.Gun
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Bullet : MonoBehaviour
    {
        private int ObjectsPenetrated;

        public Rigidbody2D Rigidbody { get; private set; }
        [field: SerializeField] public Vector2 SpawnLocation { get; private set; }
        [field: SerializeField] public Vector2 SpawnVelocity { get; private set; }

        public delegate void CollisionEvent(Bullet bullet);
        public delegate void TriggerEvent(Bullet bullet, Collider2D collider, int objectsPenetrated);

        public event CollisionEvent OnCollision;
        public event TriggerEvent OnTrigger;

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
        }

        public void Spawn(Vector2 force, float despawnDelaySeconds)
        {
            ObjectsPenetrated = 0;
            SpawnLocation = transform.position;

            Rigidbody.AddForce(force);
            SpawnVelocity = force * Time.fixedDeltaTime / Rigidbody.mass;
            StartCoroutine(DelayedDisable(despawnDelaySeconds));
        }

        private IEnumerator DelayedDisable(float time)
        {
            yield return null;
            yield return new WaitForSeconds(time);
            OnCollisionEnter2D();
            OnTriggerEnter2D(null);
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            OnTrigger?.Invoke(this, collider, ObjectsPenetrated);
            ObjectsPenetrated++;
        }

        private void OnCollisionEnter2D()
        {
            OnCollision?.Invoke(this);
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            Rigidbody.linearVelocity = Vector2.zero;
            Rigidbody.angularVelocity = 0f;
            OnCollision = null;
            OnTrigger = null;
        }
    }
}
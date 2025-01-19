using Unity.Collections;
using UnityEngine;
using UnityEngine.Pool;

namespace Guns.Gun
{
    [CreateAssetMenu(fileName = "Gun", menuName = "Guns/Gun", order = 0)]
    public class GunScriptableObject : ScriptableObject
    {
        // ReuConfigs
        public ShootConfigScriptableObject ShootConfig;

        public GunType Type;
        public string Name;
        public GameObject ModelPrefab;
        public Vector2 SpawnPoint;
        public Vector3 SpawnRotation;

        private MonoBehaviour ActiveMonoBehaviour;
        private GameObject Model;
        private float LastShootTime;
        private float CurrentAmmo;
        private ObjectPool<Bullet> BulletPool;

        public void Spawn(Transform parent, MonoBehaviour activeMonoBehaviour)
        {
            ActiveMonoBehaviour = activeMonoBehaviour;
            LastShootTime = 0;
            CurrentAmmo = ShootConfig.BaseAmmo;

            Model = Instantiate(ModelPrefab);
            Model.transform.SetParent(parent, false);
            Model.transform.localPosition = SpawnPoint;
            Model.transform.localRotation = Quaternion.Euler(SpawnRotation);

            BulletPool = new ObjectPool<Bullet>(_CreateBullet);
        }

        public void Shoot()
        {
            if (Time.time > ShootConfig.SecondsBetweenShots + LastShootTime)
            {
                LastShootTime = Time.time;
                if (CurrentAmmo == 0)
                {
                    Debug.Log("Out of ammo m8");
                    return;
                }

                Vector2 shootDirection = Vector2.right;

                _DoBulletShoot(shootDirection);

                CurrentAmmo--;
            }
        }

        private void _DoBulletShoot(Vector2 shootDirection)
        {
            Bullet bullet = BulletPool.Get();
            bullet.gameObject.SetActive(true);
            bullet.OnCollision += _HandleBulletCollision;
            bullet.OnTrigger += _HandleColliderTrigger;
            bullet.transform.position = SpawnPoint;
            bullet.Spawn(shootDirection * ShootConfig.BulletForce, ShootConfig.BulletDespawnDelaySeconds);

        }

        // Handle bullet collisions with Enemy layered colliders. These colliders are passed through
        // unless number of objectsPenetrated exceeds ShootConfig.MaxObjectsBulletPenetrate
        private void _HandleColliderTrigger(Bullet bullet, Collider2D collider, int objectsPenetrated)
        {
            if (collider == null || ShootConfig.MaxObjectsBulletPenetrate <= objectsPenetrated)
            {
                _DisableBullet(bullet);
            }

            if (collider != null)
            {
                Debug.Log("Hit target: " + collider.gameObject.name);
            }
        }

        // Handle bullet collisions with non-Enemy layered colliders.
        private void _HandleBulletCollision(Bullet bullet)
        {
            Debug.Log("Collided with terrain.");
            _DisableBullet(bullet);
        }

        private void _DisableBullet(Bullet bullet)
        {
            bullet.gameObject.SetActive(false);
            BulletPool.Release(bullet);
        }

        private Bullet _CreateBullet()
        {
            return Instantiate(ShootConfig.BulletPrefab);
        }
    }
}

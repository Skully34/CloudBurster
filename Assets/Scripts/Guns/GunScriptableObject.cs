using System;
using System.Collections;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Pool;

namespace Guns.Gun
{
    [CreateAssetMenu(fileName = "Gun", menuName = "Guns/Gun", order = 0)]
    public class GunScriptableObject : ScriptableObject, System.ICloneable
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
            Model.transform.localRotation = Quaternion.Euler(SpawnRotation);

            BulletPool = new ObjectPool<Bullet>(_CreateBullet, null, null, OnDestroyPoolObject, true, 10, 30);
        }

        void OnDestroyPoolObject(Bullet system)
        {
            Destroy(system.gameObject);
        }

        public void Shoot(Vector2 shootDirection)
        {

            if (Time.time > ShootConfig.SecondsBetweenShots + LastShootTime)
            {
                LastShootTime = Time.time;
                if (CurrentAmmo == 0)
                {
                    Debug.Log("Out of ammo m8");
                    return;
                }

                _DoBulletShoot(shootDirection);

                Vector2 spreadStart = rotate(shootDirection, -ShootConfig.BulletSpread);
                float spreadFactor = 2 * ShootConfig.BulletSpread / ShootConfig.ExtraBulletsPerShot;
                for (int i = 0; i < ShootConfig.ExtraBulletsPerShot; i++)
                {
                    _DoBulletShoot(rotate(spreadStart, spreadFactor * (i + 1)));
                }

                CurrentAmmo--;
            }
        }

        /// <summary>
        /// Despawns the active gameobjects and cleans up pools.
        /// </summary>
        public void Despawn()
        {
            Model.SetActive(false);
            Destroy(Model);
            if (BulletPool != null)
            {
                ActiveMonoBehaviour.StartCoroutine(_DestroyBulletPool(BulletPool));
            }
        }

        /// <summary>
        /// Wait for all bullets to be inactive to clear, otherwise active bullets won't be cleaned
        /// up when Clear() is called.
        /// </summary>
        private IEnumerator _DestroyBulletPool(ObjectPool<Bullet> bulletPool)
        {
            yield return new WaitUntil(() => bulletPool.CountActive == 0);
            BulletPool.Clear();
        }


        private static Vector2 rotate(Vector2 v, float delta)
        {
            return new Vector2(
                v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
                v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
            );
        }

        private void _DoBulletShoot(Vector2 shootDirection)
        {
            Bullet bullet = BulletPool.Get();
            bullet.gameObject.SetActive(true);
            bullet.OnCollision += _HandleBulletCollision;
            bullet.OnTrigger += _HandleColliderTrigger;
            bullet.transform.position = Model.transform.position;
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
                // Debug.Log("Hit target: " + collider.gameObject.name);
                collider.gameObject.GetComponent<BaseHealthHit>().Hit(ShootConfig.BulletDamage, ShootConfig.FreezeAmount);
            }
        }

        // Handle bullet collisions with non-Enemy layered colliders.
        private void _HandleBulletCollision(Bullet bullet)
        {
            // Debug.Log("Collided with terrain.");
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
        public object Clone()
        {
            GunScriptableObject config = CreateInstance<GunScriptableObject>();

            config.Type = Type;
            config.Name = Name;
            config.name = name;
            config.ShootConfig = ShootConfig.Clone() as ShootConfigScriptableObject;

            config.ModelPrefab = ModelPrefab;
            config.SpawnPoint = SpawnPoint;
            config.SpawnRotation = SpawnRotation;

            return config;
        }
    }
}

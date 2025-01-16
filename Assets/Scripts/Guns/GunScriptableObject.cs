using Unity.Collections;
using UnityEngine;

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
        public Vector3 SpawnPoint;
        public Vector3 SpawnRotation;

        private MonoBehaviour ActiveMonoBehaviour;
        private ParticleSystem ShootSystem;
        private GameObject Model;
        private float LastShootTime;
        private float CurrentAmmo;

        public void Spawn(Transform parent, MonoBehaviour activeMonoBehaviour)
        {
            ActiveMonoBehaviour = activeMonoBehaviour;
            LastShootTime = 0;
            CurrentAmmo = ShootConfig.BaseAmmo;

            Model = Instantiate(ModelPrefab);
            Model.transform.SetParent(parent, false);
            Model.transform.localPosition = SpawnPoint;
            Model.transform.localRotation = Quaternion.Euler(SpawnRotation);

            ShootSystem = Model.GetComponentInChildren<ParticleSystem>();
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

                ShootSystem.Play();

                Vector2 shootDirection = Vector2.right;

                _DoHitscanShoot(shootDirection);

                CurrentAmmo--;
            }
        }

        private void _DoHitscanShoot(Vector2 shootDirection)
        {
            RaycastHit2D hit = Physics2D.Raycast(
                    new Vector2(ShootSystem.transform.position.x, ShootSystem.transform.position.y),
                    shootDirection,
                    float.MaxValue,
                    ShootConfig.HitMask
                );

            if (hit)
            {
                Debug.Log("Hit target: " + hit.transform.name);
            }
        }
    }
}

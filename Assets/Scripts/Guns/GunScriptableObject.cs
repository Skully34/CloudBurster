using Unity.Collections;
using UnityEngine;

namespace Guns.Gun
{
    [CreateAssetMenu(fileName = "Gun", menuName = "Guns/Gun", order = 0)]
    public class GunScriptableObject : ScriptableObject
    {
        // Configs
        public ShootConfigScriptableObject ShootConfig;

        public GunType Type;
        public string Name;
        public GameObject ModelPrefab;
        public Vector3 SpawnPoint;
        public Vector3 SpawnRotation;

        private MonoBehaviour ActiveMonoBehaviour;
        private GameObject Model;
        private float LastShootTime;
        private ParticleSystem ShootSystem;

        public void Spawn(Transform parent, MonoBehaviour activeMonoBehaviour)
        {
            ActiveMonoBehaviour = activeMonoBehaviour;
            LastShootTime = 0;

            Model = Instantiate(ModelPrefab);
            Model.transform.SetParent(parent, false);
            Model.transform.localPosition = SpawnPoint;
            Model.transform.localRotation = Quaternion.Euler(SpawnRotation);

            ShootSystem = Model.GetComponentInChildren<ParticleSystem>();
        }

        public void Shoot()
        {
            if (Time.time > ShootConfig.FireRate + LastShootTime)
            {
                LastShootTime = Time.time;

                ShootSystem.Play();

                Vector3 shootDirection = ShootSystem.transform.forward;

                _DoHitscanShoot(shootDirection);
            }
        }

        private void _DoHitscanShoot(Vector3 shootDirection)
        {
            if (Physics.Raycast(
                    ShootSystem.transform.position,
                    shootDirection,
                    out RaycastHit hit,
                    float.MaxValue,
                    ShootConfig.HitMask
                ))
            {
                Debug.Log(hit.transform.name);
            }
        }
    }
}

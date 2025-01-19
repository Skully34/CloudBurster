using Unity.Collections;
using UnityEngine;

namespace Guns.Gun
{
    [CreateAssetMenu(fileName = "ShootConfig", menuName = "Guns/ShootConfig", order = 1)]
    public class ShootConfigScriptableObject : ScriptableObject
    {
        public LayerMask HitMask;
        public Vector3 Spread = Vector3.zero;
        public float SecondsBetweenShots = 0.25f;
        public int BaseAmmo = 30;
        public int BulletsPerShot = 1;

        public Bullet BulletPrefab;

        // Can't be too high otherwise colliders won't be detected MAX 3000f
        public float BulletForce = 3000f;
        public int MaxObjectsBulletPenetrate = 1;
        public float BulletDespawnDelaySeconds = 2f;

        public float BulletDamage = 0f;
        public int FreezeAmount = 0;
    }
}
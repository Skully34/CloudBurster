using UnityEngine;
using System.Reflection;
using System;
using UnityEngine.Audio;

namespace Guns.Gun
{
    [CreateAssetMenu(fileName = "ShootConfig", menuName = "Guns/ShootConfig", order = 1)]
    public class ShootConfigScriptableObject : ScriptableObject
    {
        public LayerMask HitMask;
        public Vector3 Spread = Vector3.zero;
        public float SecondsBetweenShots = 0.25f;
        public int BaseAmmo = 30;

        // Set this to an even number plis I don't want to do math
        public int ExtraBulletsPerShot = 0;
        public float BulletSpread = Mathf.PI / 6;

        
        public Bullet BulletPrefab;

        // Can't be too high otherwise colliders won't be detected MAX 3000f
        public float BulletForce = 3000f;
        public int MaxObjectsBulletPenetrate = 1;
        public float BulletDespawnDelaySeconds = 2f;

        public float BulletDamage = 0f;
        public int FreezeAmount = 0;

        public object Clone()
        {
            ShootConfigScriptableObject config = CreateInstance<ShootConfigScriptableObject>();

            CopyValues(this, config);

            return config;
        }

        private static void CopyValues<T>(T Base, T Copy)
        {
            Type type = Base.GetType();
            foreach (FieldInfo field in type.GetFields())
            {
                field.SetValue(Copy, field.GetValue(Base));
            }
        }
    }
}
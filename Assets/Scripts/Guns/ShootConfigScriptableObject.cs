using Unity.Collections;
using UnityEngine;

namespace Guns.Gun
{
    [CreateAssetMenu(fileName = "ShootConfig", menuName = "Guns/ShootConfig", order = 1)]
    public class ShootConfigScriptableObject : ScriptableObject
    {
        public LayerMask HitMask;
        public Vector3 Spread = Vector3.zero;
        public float FireRate = 0.25f;
    }
}
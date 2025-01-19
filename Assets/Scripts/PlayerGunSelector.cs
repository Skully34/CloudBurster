using Guns.Gun;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerGunSelector : MonoBehaviour
{
    [SerializeField] GunScriptableObject Gun;
    [SerializeField] AimGun Aim;

    void Start()
    {
        Gun.Spawn(this.transform, this);
        Aim.gun = Gun;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Gun.Shoot(Vector2.right);
        }
    }
}

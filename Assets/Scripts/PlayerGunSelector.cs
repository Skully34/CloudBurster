using Guns.Gun;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerGunSelector : MonoBehaviour
{
    [SerializeField] GunScriptableObject Gun;
    [SerializeField] AimGun aim;

    void Start()
    {
        Gun.Spawn(this.transform, this);
        aim.gun = Gun;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Gun.Shoot();
        }
    }
}

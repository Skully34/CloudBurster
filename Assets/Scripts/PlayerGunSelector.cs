using Guns.Gun;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerGunSelector : MonoBehaviour
{
    [SerializeField] GunScriptableObject Gun;

    void Start()
    {
        Gun.Spawn(this.transform, this);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Gun.Shoot();
        }
    }
}

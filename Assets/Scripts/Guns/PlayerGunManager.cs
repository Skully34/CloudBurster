using System.Collections.Generic;
using Guns.Gun;
using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public class PlayerGunManager : MonoBehaviour
{
    private GunScriptableObject ActiveGun;

    [SerializeField]
    private List<GunScriptableObject> BaseGuns;
    [SerializeField] bool AimWithArrows;

    [SerializeField] InputActionReference Aim;
    [SerializeField] InputActionReference Fire;
    [SerializeField] InputActionReference Move;
    private Vector2 CurrentOrientation = Vector2.right;

    void Start()
    {
        _SetupGun(GunType.None);
    }

    public void PickupGun(GunType type)
    {
        _DespawnActiveGun();
        _SetupGun(type);
    }


    private void _SetupGun(GunType gunType)
    {
        GunScriptableObject gun = BaseGuns.Find(g => g.Type == gunType);

        if (gun == null)
        {
            Debug.LogError($"Selected gun type doesn't exist {gunType}");
        }

        ActiveGun = gun.Clone() as GunScriptableObject;
        ActiveGun.Spawn(this.transform, this);
    }
    private void _DespawnActiveGun()
    {
        if (ActiveGun != null)
        {
            ActiveGun.Despawn();
        }

        Destroy(ActiveGun);
    }

    private void Update()
    {
        if (AimWithArrows)
        {
            if (Aim.action.triggered)
            {
                ActiveGun.Shoot(Aim.action.ReadValue<Vector2>());
            }
        }

        if (!AimWithArrows)
        {
            if (Move.action.ReadValue<Vector2>().magnitude != 0)
            {
                CurrentOrientation = Move.action.ReadValue<Vector2>();

            }

            if (Fire.action.triggered)
            {
                ActiveGun.Shoot(CurrentOrientation);
            }
        }

    }

}

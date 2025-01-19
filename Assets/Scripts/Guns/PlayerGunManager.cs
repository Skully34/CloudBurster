using Guns.Gun;
using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public class PlayerGunManager : MonoBehaviour
{
    [SerializeField] GunScriptableObject Gun;
    [SerializeField] bool AimWithArrows;

    [SerializeField] InputActionReference Aim;
    [SerializeField] InputActionReference Fire;
    [SerializeField] InputActionReference Move;
    private Vector2 CurrentOrientation = Vector2.right;

    void Start()
    {
        Gun.Spawn(this.transform, this);
    }

    private void Update()
    {
        if (AimWithArrows)
        {
            if (Aim.action.triggered)
            {
                Gun.Shoot(Aim.action.ReadValue<Vector2>());
            }
        }

        if (!AimWithArrows)
        {
            if (Fire.action.triggered)
            {
                if (Move.action.ReadValue<Vector2>().magnitude != 0)
                {
                    CurrentOrientation = Move.action.ReadValue<Vector2>();
                }

                Gun.Shoot(CurrentOrientation);
            }
        }
    }

}

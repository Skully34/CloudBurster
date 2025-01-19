using Guns.Gun;
using UnityEngine;
using UnityEngine.InputSystem;

public class AimGun : MonoBehaviour
{
    public GunScriptableObject gun;
    [SerializeField] bool aimWithArrows;

    [SerializeField] InputActionReference aim;
    [SerializeField] InputActionReference fire;
    [SerializeField] InputActionReference move;

    private void Update()
    {
        if (aimWithArrows)
        {
            if (aim.action.triggered)
            {
                gun.shootDirection = aim.action.ReadValue<Vector2>();
                gun.Shoot();
            }
        }

        if (!aimWithArrows)
        {
            if (fire.action.triggered)
            {
                gun.shootDirection = move.action.ReadValue<Vector2>();
                gun.Shoot();
            }
        }
    }

}

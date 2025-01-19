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
    Vector2 saveDirection;

    private void Update()
    {
        if (aimWithArrows)
        {
            if (aim.action.triggered)
            {
                gun.Shoot(aim.action.ReadValue<Vector2>());
            }
        }

        if (!aimWithArrows)
        {
            if (fire.action.triggered)
            {


                if (move.action.ReadValue<Vector2>().magnitude != 0)
                {
                    saveDirection = move.action.ReadValue<Vector2>();
                }

                gun.SpawnPoint = new Vector2(gameObject.transform.localPosition.x + saveDirection.x, gameObject.transform.localPosition.y + (saveDirection.y * 1.3f));

                gun.Shoot(saveDirection);
            }
        }
    }

}

using Guns.Gun;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float dashSpeed;
    [SerializeField] float jumpHeight;
    [SerializeField] float jumpSpeed;
    [SerializeField] float dashDistance;

    CapsuleCollider2D playerCollision;
    BoxCollider2D wallAntistick;
    Rigidbody2D rb;

    [SerializeField] InputActionReference move;
    [SerializeField] InputActionReference jump;
    [SerializeField] InputActionReference aim;
    [SerializeField] InputActionReference dash;

    bool dashing = false;
    [SerializeField] int dashChargeLimit = 2;
    int dashChargeActual;
    [SerializeField] float dashCDTime = 3;
    float dashCDTimeActual;
    bool burst = false;
    CircleCollider2D dashCollision;
    public bool grounded = false;


    //  ContactFilter2D contactFilter;
    // Collider2D[] overlap;
    Collider2D recentCollision;
    bool colliding = false;
    Vector2 startPosition;
    [SerializeField] public GunType GunType;
    private PlayerGunManager playerGunManager;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollision = GetComponent<CapsuleCollider2D>();
        wallAntistick = GetComponent<BoxCollider2D>();
        dashCollision = GetComponent<CircleCollider2D>();
        dashChargeActual = dashChargeLimit;
        playerGunManager = GetComponent<PlayerGunManager>();

    }

    void Update()
    {   
        // Test code start
        GunType nextGun = GunType;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            nextGun = GunType.Hail;
        } else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            nextGun = GunType.Ash;
        } else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            nextGun = GunType.Lightning;
        }

        if (nextGun != GunType)
        {
            playerGunManager.PickupGun(nextGun);
        }
        // Test code end


        if (dash.action.triggered) { Dash(); }

        if (!dashing)
        {
            if (jump.action.triggered && grounded) { Jump(); }
            if (move.action.inProgress) { MoveC(); }
        }

        if (dashing)
        {
            if (!colliding && (Vector2.Distance(transform.position, startPosition) > dashDistance))
            {
                DashEnd();

            }
        }

        if (dashChargeActual < dashChargeLimit && !dashing)
        {
            dashCDTimeActual -= Time.deltaTime;
            DashRecharge();
        }
    }

    private void Jump()
    {
        rb.linearVelocityY = jumpSpeed;

    }

    private void DashRecharge()
    {
        if (dashCDTimeActual <= 0)
        {
            dashChargeActual = dashChargeLimit;
        }
    }

    private void MoveC()
    {

        rb.linearVelocityX = move.action.ReadValue<Vector2>().x * speed;

    }
    private void Dash()
    {
        if (!dashing && dashChargeActual > 0)
        {

            startPosition = new Vector2(transform.position.x, transform.position.y);
            burst = false;

            dashing = true;
            dashChargeActual--;
            rb.gravityScale = 0;
            rb.linearVelocity = move.action.ReadValue<Vector2>() * dashSpeed;
            playerCollision.enabled = false;
            wallAntistick.enabled = false;
            playerCollision.size /= 2;
            wallAntistick.size /= 2;



        }
    }
    private void DashEnd()
    {

        dashing = false;
        rb.gravityScale = 1;
        playerCollision.size *= 2;
        wallAntistick.size *= 2;
        playerCollision.enabled = true;
        wallAntistick.enabled = true;
        dashCDTimeActual = dashCDTime;

        if (burst == true)
        {
            rb.linearVelocity *= 1.4f;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        colliding = true;
        recentCollision = collision;
        CheckWeaponSwap();
    }
    private void OnTriggerExit2D(Collider2D Ecollision)
    {

        if (recentCollision == Ecollision)
        {
            colliding = false;
        }
    }

    private void CheckWeaponSwap()
    {

        if (recentCollision.gameObject.TryGetComponent(out CloudScript Cloud))
        {

            burst = true;
            switch (Cloud.CloudType)
            {
                case Cloudtype.Cloud:
                    {
                        return;
                    }
                case Cloudtype.Ash:
                    {
                        GunType = GunType.Ash;
                        SwitchWeapon();
                        return;
                    }
                case Cloudtype.Hail:
                    {
                        GunType = GunType.Hail;
                        SwitchWeapon();
                        return;
                    }
                case Cloudtype.Storm:
                    {
                        GunType = GunType.Lightning;
                        SwitchWeapon();
                        return;
                    }
                default:
                    { return; }

            }


        }

    }

    private void SwitchWeapon()
    {
        Debug.Log("Seven");
    }
}

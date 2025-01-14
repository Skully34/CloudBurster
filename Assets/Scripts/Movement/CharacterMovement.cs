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


    //  ContactFilter2D contactFilter;
    // Collider2D[] overlap;
    Collider2D recentCollision;
    bool colliding = false;
    [SerializeField] public string WeaponType;
    Vector2 startPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollision = GetComponent<CapsuleCollider2D>();
        dashCollision = GetComponent<CircleCollider2D>();
        dashChargeActual = dashChargeLimit;

    }

        void Update()
        {

            if (dash.action.triggered) { Debug.Log("Zero"); Dash(); }

            if (!dashing)
            {
                if (jump.action.triggered) { Jump(); }
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
        Debug.Log("Update?");
    }

    private void DashRecharge()
    {
        if(dashCDTimeActual <= 0)
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
            Debug.Log("One");
            startPosition = new Vector2 (transform.position.x, transform.position.y);
            burst = false;

            dashing = true;
            dashChargeActual--;
            rb.gravityScale = 0;
            rb.linearVelocity = move.action.ReadValue<Vector2>() * dashSpeed;
            playerCollision.enabled = false;
            playerCollision.size /= 2;



        }
    }
    private void DashEnd()
    {

        dashing = false;
        rb.gravityScale = 1;
        playerCollision.size *=  2;
        playerCollision.enabled = true;
        dashCDTimeActual = dashCDTime;

        if (burst == true)
        {
            rb.linearVelocity *= 1.4f;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Three");
        colliding = true;
        recentCollision = collision;
        CheckWeaponSwap();
    }
    private void OnTriggerExit2D(Collider2D Ecollision)
    {
        Debug.Log("Four");
        if (recentCollision == Ecollision)
        {
            colliding = false;
        }
    }

    private void CheckWeaponSwap()
    {
        
        if (recentCollision.gameObject.TryGetComponent(out CloudScript Cloud))
            {
                Debug.Log("wep");
                burst = true;
                switch (Cloud.CloudType)
                {
                    case Cloudtype.Cloud:
                    {
                        return;
                    }
                    case Cloudtype.Ash:
                    {
                            WeaponType = "ash";
                            SwitchWeapon();
                            return;
                    }
                    case Cloudtype.Hail:
                        {
                            WeaponType = "hail";
                            SwitchWeapon();
                            return;
                        }
                    case Cloudtype.Storm:
                        {
                            WeaponType = "storm";
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

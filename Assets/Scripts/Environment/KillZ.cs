using UnityEngine;

public class KillZ : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<BaseHealthHit>(out BaseHealthHit health))
        {
            health.DeathEvent();
        }
    }

}

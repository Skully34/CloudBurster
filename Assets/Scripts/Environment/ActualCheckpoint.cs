using UnityEngine;

public class ActualCheckpoint : MonoBehaviour
{
    [SerializeField] public int ID;
    [SerializeField] CheckpointSystem control;

    private void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<BaseHealthHit>(out BaseHealthHit health))
        {
            if (health.MC)
            {
                control.SetCheckpoint(ID);
            }
        }
    }
}

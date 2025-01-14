using UnityEngine;

public enum Cloudtype
{
    Cloud,
    Ash,
    Storm,
    Hail
}
public class CloudScript : MonoBehaviour
{
    [SerializeField] public Cloudtype CloudType;

    
}

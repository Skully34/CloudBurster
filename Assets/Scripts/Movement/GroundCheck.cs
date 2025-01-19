using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] CharacterMovement character;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        character.grounded = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        character.grounded = false;
    }
}

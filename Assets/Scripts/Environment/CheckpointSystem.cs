using UnityEngine;

public class CheckpointSystem : MonoBehaviour
{
    [SerializeField] ActualCheckpoint[] checkpoints;

    [SerializeField] CheckpointID checkpointID;

    [SerializeField] GameObject mainCharacter;

    private void Start()
    {
        RespawnPlayer();
    }

    public void SetCheckpoint(int ID)
    {
        checkpointID.CheckpointIdentity = ID;
    }

    public void RespawnPlayer()
    {
        foreach (var checkpoint in checkpoints)
        {
            if(checkpoint.ID == checkpointID.CheckpointIdentity)
            {
                Instantiate(mainCharacter, checkpoint.transform.position, new Quaternion(0,0,0,0));
            }
        }
    }

    public void WinGame()
    {
        SetCheckpoint(0);
    }

    public void LeaveGame()
    {
        SetCheckpoint(0);
    }

}

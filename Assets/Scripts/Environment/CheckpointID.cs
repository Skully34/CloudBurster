using UnityEngine;

[CreateAssetMenu]
public class CheckpointID : ScriptableObject
{
	[SerializeField]
	private int _checkpointID;

	public int CheckpointIdentity
	{
		get { return _checkpointID; }
		set { _checkpointID = value; }
	}

}

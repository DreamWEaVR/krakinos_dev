using UnityEngine;
using System.Collections;

public class OffsetController : MonoBehaviour {

	public Transform markerLocalStart;
	public Transform markerLocalEnd;
	public Transform markerOffsetStart;
	public Transform markerOffsetEnd;

	public AreaRotation player;
	// Use this for initialization
//	void Start () {
//		
//	}

	void Update () {
		//adjust their positions
		//figure out the local positions and offset
		Vector3 offset = markerLocalEnd.localPosition - markerLocalStart.localPosition;

		//position the offset markers
		markerOffsetStart.position = player.gameObject.transform.position;

		//rotate the offset to match the player
		offset = player.getAngle() * offset;
		markerOffsetEnd.localPosition = markerOffsetStart.localPosition + (offset * player.getScale ());

	}
}

using UnityEngine;
using System.Collections;

public class ViewVisualizer : MonoBehaviour {

	public VRLineRenderer[] targetLines;
	public VRLineRenderer[] FOVLines;

	public FieldOfView fov;

	// Use this for initialization
	void Start () {
		FOVLines [0].transform.position = Vector3.zero;
		FOVLines [1].transform.position = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 viewAngleA = fov.DirFromAngle (-fov.viewAngle / 2, false);
		Vector3 viewAngleB = fov.DirFromAngle (fov.viewAngle / 2, false);


		FOVLines [0].SetPositions (new Vector3[]{fov.transform.position, fov.transform.position + viewAngleA * fov.viewRadius});
		FOVLines [1].SetPositions (new Vector3[]{fov.transform.position, fov.transform.position + viewAngleB * fov.viewRadius});


		for (int i = 0; i < targetLines.Length; i++) {
			targetLines [i].gameObject.SetActive (false);
		}
		//draw lines to targets
		for(int i=0; i<fov.visibleTargets.Count; i++){
			targetLines [i].gameObject.SetActive (true);
			targetLines [i].transform.position = Vector3.zero;

			targetLines [i].SetPositions (new Vector3[]{fov.transform.position, fov.visibleTargets[i].position});
		}
	}
}

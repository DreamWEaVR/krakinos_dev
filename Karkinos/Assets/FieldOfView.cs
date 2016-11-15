using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FieldOfView : MonoBehaviour {

	public float viewRadius;

	public LayerMask targetMask;
	public LayerMask unwalkableMask;

	public List<Transform> visibleTargets = new List<Transform>();

	[Range(0,360)]
	public float viewAngle;

	void Start(){
		StartCoroutine ("FindTargetsWithDelay", .2f);
	}

	public Vector3 DirFromAngle(float angleDegrees, bool angleIsGlobal){
		if(!angleIsGlobal){
			angleDegrees += transform.eulerAngles.y;
		}
		return new Vector3 (Mathf.Sin (angleDegrees * Mathf.Deg2Rad), 0, Mathf.Cos (angleDegrees * Mathf.Deg2Rad));
	}

	IEnumerator FindTargetsWithDelay(float delay){
		while (true) {
			yield return new WaitForSeconds (delay);
			FindVisibleTargets ();
		}
	}

	void FindVisibleTargets(){
		Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

		visibleTargets.Clear ();

		for (int i = 0; i < targetsInViewRadius.Length; i++) {
			Transform target = targetsInViewRadius [i].transform;
			Vector3 dirToTarget = (target.position - transform.position).normalized;
			if (Vector3.Angle (transform.forward, dirToTarget) < viewAngle / 2) {
				float distToTarget = Vector3.Distance (transform.position, target.position);
				if(!Physics.Raycast(transform.position, dirToTarget, distToTarget, unwalkableMask)){
					visibleTargets.Add (target);
				}
			}
		}
	}

	ViewCastInfo ViewCast(float globalAngle){
		Vector3 dir = DirFromAngle (globalAngle, true);
		RaycastHit hit;
		if (Physics.Raycast (transform.position, dir, out hit, viewRadius, unwalkableMask)) {
			return new ViewCastInfo (true, hit.point, hit.distance, globalAngle);
		} else {
			return new ViewCastInfo (false, transform.position + dir * viewRadius, viewRadius, globalAngle);
		}
	}

	public struct ViewCastInfo{
		public bool hit;
		public Vector3 point;
		public float length;
		public float angle;

		public ViewCastInfo(bool _hit, Vector3 _point, float _length, float _angle){
			hit = _hit;
			point = _point;
			length = _length;
			angle = _angle;
		}
	}
}

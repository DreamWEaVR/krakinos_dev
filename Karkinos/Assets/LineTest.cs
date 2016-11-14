using UnityEngine;
using System.Collections;

public class LineTest : MonoBehaviour {
	public Vector3[] randomPathPoints;
	public Vector3[] smoothPoints;

	[Range(0,6)]
	public float smoothAmount=0;

	[Range(0.0f,1.0f)]
	public float curveTightness=0;

	[Range(0,15)]
	public int curveIterations = 1;

	public float pointCount=15;
	public Vector3 size = new Vector3 (5, 5, 5);
	public Vector3 maxDis = new Vector3 (1, 1, 1);

	public float maxSegmentCurveDistance=.4f;

	public VRLineRenderer lineRender;

	public bool smoothOn = false;
	public bool tightenCurves = true;

	public Vector3 lineDirection;
	public float directionRandomness=.45f;



	// Use this for initialization
	void Start () {
		lineDirection = new Vector3 (Random.Range (-maxDis.x, maxDis.x), Random.Range (-maxDis.y, maxDis.y), Random.Range (-maxDis.y, maxDis.y));
		randomPath ();
		//then draw it;
		drawPath (randomPathPoints, smoothAmount);
	}

	void randomPath(){
		randomPathPoints = new Vector3[(int)pointCount];

		Vector3 lastPoint = Vector3.zero;
		for(int i=0; i<pointCount; i++){
			 
			if (Random.Range (0.0f, 1.0f) > directionRandomness) {
				//change direction
				lineDirection = new Vector3 (Random.Range(-maxDis.x,maxDis.x), Random.Range(-maxDis.y,maxDis.y), Random.Range(-maxDis.y,maxDis.y));
			} 
			Vector3 pnt = lineDirection + lastPoint;

			randomPathPoints [i] = pnt;
			lastPoint = pnt;
		}
	}

	void drawPath(Vector3[] points, float smooth){
		
		if (smoothOn) {
//			Vector3[] segmentedPoints = PathSmooth.TightenCorners (points, curveTightness);
			smoothPoints = PathSmooth.RoundCorners(points, curveTightness, curveIterations);
//
//			//first add segments to path before curving it
//			if (tightenCurves) {
////				Vector3[] segmentedPoints = PathSmooth.SubdivideLine (points, maxSegmentCurveDistance);
////				smoothPoints = PathSmooth.MakeSmoothCurve (segmentedPoints, smooth);
//
//
//
//			} else {
////				smoothPoints = PathSmooth.MakeSmoothCurve (points, smooth);
//				smoothPoints = PathSmooth.RoundCorners(points, curveTightness, 3);
//			}
			lineRender.SetPositions (smoothPoints);
		} else {
			lineRender.SetPositions (points);
		}

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("space")) {
			//redraw
			drawPath (randomPathPoints, smoothAmount);
		}
		if (Input.GetKeyDown (KeyCode.P)) {
			//redraw
			randomPath();
			drawPath (randomPathPoints, smoothAmount);
		}
		if (Input.GetKeyDown (KeyCode.S)) {
			smoothOn = !smoothOn;
			//redraw
			drawPath (randomPathPoints, smoothAmount);
		}
//		if (Input.GetKeyDown (KeyCode.D)) {
//			tightenCurves = !tightenCurves;
//			//redraw
//			drawPath (randomPathPoints, smoothAmount);
//		}
	}
}

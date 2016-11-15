using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathSmooth {

	public static Vector3[] MakeSmoothCurve(Vector3[] arrayToCurve,float smoothness){
		List<Vector3> points;
		List<Vector3> curvedPoints;
		int pointsLength = 0;
		int curvedLength = 0;

		if(smoothness < 1.0f) smoothness = 1.0f;

		pointsLength = arrayToCurve.Length;

		curvedLength = (pointsLength*Mathf.RoundToInt(smoothness))-1;
		curvedPoints = new List<Vector3>(curvedLength);

		float t = 0.0f;
		for(int pointInTimeOnCurve = 0;pointInTimeOnCurve < curvedLength+1;pointInTimeOnCurve++){
			t = Mathf.InverseLerp(0,curvedLength,pointInTimeOnCurve);

			points = new List<Vector3>(arrayToCurve);

			for(int j = pointsLength-1; j > 0; j--){
				for (int i = 0; i < j; i++){
					points[i] = (1-t)*points[i] + t*points[i+1];
				}
			}

			curvedPoints.Add(points[0]);
		}

		return(curvedPoints.ToArray());
	}

	public static Vector3[] RoundCorners(Vector3[] arrayToRound, float tightness, int iterations){
		if (iterations < 1) {
			return arrayToRound;
		}

		Vector3 direction;

		if (tightness > 0 && tightness < 1) {
			arrayToRound = TightenCorners (arrayToRound, tightness);
		}

		List<Vector3> newPoints = new List<Vector3> ();
		direction = arrayToRound [0] - arrayToRound [1];
		//find all the corners and round them
		newPoints.Add(arrayToRound [0]);
		for (int i = 1; i < arrayToRound.Length - 1; i++) {
			Vector3 last = arrayToRound [i-1];
			Vector3 current = arrayToRound [i];
			Vector3 next = arrayToRound [i + 1];

			Vector3 nextDirection = current - next;
			if(nextDirection.normalized != direction.normalized){
				//round it
				newPoints.Add (Vector3.Lerp (last, current, .5f));
				newPoints.Add (Vector3.Lerp (current, next, .5f));
				direction = nextDirection;
			}else{
				//pass it through
				newPoints.Add (current);
			}


			//newPoints.Add (Vector3.Lerp (start, end, tightness));
		}
		newPoints.Add (arrayToRound[arrayToRound.Length-1]);

		Vector3[] newPointsArray = newPoints.ToArray ();

		if (iterations > 1) {
			newPointsArray = RoundCorners (newPointsArray, 0, iterations-1);
		}

		return(newPointsArray);
	}

	//change to tighten corners
	public static Vector3[] SubdivideLine(Vector3[] arrayToDivide,float maxDistance){

		List<Vector3> newPoints = new List<Vector3> ();

		//loop through each point get the distance
		for (int i = 0; i < arrayToDivide.Length-1; i++) {
			Vector3 start = arrayToDivide [i];
			Vector3 end = arrayToDivide [i + 1];

			float tightness = .1f;
			newPoints.Add (start);
			newPoints.Add (Vector3.Lerp (start, end, tightness));
			newPoints.Add (Vector3.Lerp (start, end, 1.0f-tightness));

		}
		newPoints.Add (arrayToDivide[arrayToDivide.Length-1]);
		//if it's greater than max divide it by the max to get the divisions

		//add each one to the new array
		return(newPoints.ToArray());
	}

	public static Vector3[] TightenCorners(Vector3[] arrayToDivide,float tightness){
		List<Vector3> newPoints = new List<Vector3> ();

        //loop through each point get the distance
        newPoints.Add(arrayToDivide[0]);

        for (int i = 1; i < arrayToDivide.Length-1; i++) {
            Vector3 last = arrayToDivide[i - 1];

            Vector3 current = arrayToDivide [i];
			Vector3 next = arrayToDivide [i + 1];

            newPoints.Add(Vector3.Lerp(last, current, 1.0f - tightness));
            newPoints.Add (current);
			
			newPoints.Add (Vector3.Lerp (current, next, tightness));

		}
		newPoints.Add (arrayToDivide[arrayToDivide.Length-1]);

		return(newPoints.ToArray());
	}

}

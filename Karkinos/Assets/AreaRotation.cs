using UnityEngine;
using System.Collections;

public class AreaRotation : MonoBehaviour {

	public Transform pivotObject;

	public Vector3 initialPosition;
	public Quaternion initialRotation;

	//rotation axis 1 is for on 0 is for off... or could be used to inverting axis or flipping?
	public Vector3 RotationAxis = new Vector3(1,1,1);

	public bool ScaleEnabled = true;
	public float minScale = .5f;
	public float maxScale = 4f;

	// Use this for initialization
	void Start () {
		initialPosition = transform.localPosition;
		initialRotation = transform.localRotation;

	}
	
	// Update is called once per frame
	void Update () {

		Vector3 pivotAngles = new Vector3(hXSliderValue * RotationAxis.x, hYSliderValue * RotationAxis.y, hZSliderValue * RotationAxis.z);
		Quaternion qAngles = Quaternion.Euler (pivotAngles);
		//will also need to store the start rotation and add this new rotation to it

		HandleRotation (qAngles);



	}

	public Quaternion getAngle(){
		return transform.localRotation;
	}
	public float getScale(){
		return transform.localScale.x;
	}

	private void HandleRotation(Quaternion angles){
		pivotObject.localRotation = angles;

		//calculate the offset of the pivot
		//pivot must be direct child
		Vector3 dir = new Vector3() - pivotObject.localPosition;

		angles = Quaternion.Inverse (angles);

		//rotate it the offset
		dir = angles * dir;

		if (ScaleEnabled) {
			float targScale = Mathf.Clamp (hScaleSliderValue, minScale, maxScale);
			dir = dir * targScale;
			transform.localScale = new Vector3 (targScale, targScale, targScale);
		}

		//set this transforms position
		transform.localPosition = initialPosition + dir + pivotObject.localPosition;

		//set the rotation, at this point angle is the inverse of the child rotation
		transform.localRotation = angles;
	}


	//Debuggin
	public float hXSliderValue = 0.0F;
	public float hYSliderValue = 0.0F;
	public float hZSliderValue = 0.0F;

	public float hScaleSliderValue = 1f;

	void OnGUI() {
		hXSliderValue = GUI.HorizontalSlider(new Rect(25, 25, 200, 30), hXSliderValue, 0.0F, 360.0F);
		hYSliderValue = GUI.HorizontalSlider(new Rect(25, 50, 200, 30), hYSliderValue, 0.0F, 360.0F);
		hZSliderValue = GUI.HorizontalSlider(new Rect(25, 75, 200, 30), hZSliderValue, 0.0F, 360.0F);

		hScaleSliderValue = GUI.HorizontalSlider(new Rect(25, 100, 200, 30), hScaleSliderValue, 0.2F, 9.0F);
	}
}

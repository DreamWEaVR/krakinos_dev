using UnityEngine;
using System.Collections;
using Valve.VR;

public class WorldControlManager : MonoBehaviour {
    public GameObject worldRoot;
    public ViveGrip_GripPoint leftGripPoint;
    public ViveGrip_GripPoint rightGripPoint;

    [Tooltip("The button used for gripping.")]
    public ViveGrip_ControllerHandler.ViveInput grab = ViveGrip_ControllerHandler.ViveInput.Grip;
    // Use this for initialization
    void Start () {
        //get a gripPoint
        //
        if (leftGripPoint != null)
        {
            leftGripPoint.airGrabbingTarget = gameObject;
        }
        if (rightGripPoint != null)
        {
            rightGripPoint.airGrabbingTarget = gameObject;
        }
    }

    public bool panningActive = false;
	// Update is called once per frame
	void Update () {

        if (leftGripPoint != null && leftGripPoint.controller!=null && leftGripPoint.controller.Pressed("trigger"))
        {
            if (!leftGripPoint.TouchingSomething())
            {
                //panningActive = true;
                Debug.Log("left pressed");
                //Rigidbody desiredBody = gripPoint.TouchedObject().GetComponent<Rigidbody>();
                //desiredBody.gameObject.GetComponent<ViveGrip_Grabbable>().GrabFrom(transform.position);
                //joint = ViveGrip_JointFactory.JointToConnect(jointObject, desiredBody, transform.rotation);
                grabWorld(leftGripPoint);
            }else
            {
                Debug.Log("left pressed, BUT ALREADY TOUCHING SOMETHING");
            }
        }
        if (rightGripPoint != null && rightGripPoint.controller != null &&  rightGripPoint.controller.Pressed("trigger"))
        {
            if (!rightGripPoint.TouchingSomething())
            {
                //panningActive = true;
                Debug.Log("right pressed");
                grabWorld(rightGripPoint);
            }
        }

    }
    public void grabWorld(ViveGrip_GripPoint grip)
    {
        Debug.Log("grabbing the world");
        //attach the world
        //grip.grabber.OverrideGrabWith(grip, GetComponent<Rigidbody>());
        //grip.GrabObject(gameObject);
    }

    //scripts for draggint he world up and down
    private ViveGrip_ControllerHandler controller;
    void ViveGripInteractionStart(ViveGrip_GripPoint gripPoint)
    {
        //if (gripPoint.HoldingSomething())
        //{
        //    controller = gripPoint.controller;
        //}
        //controller = gripPoint.controller;

    }
    //void ViveGripInteractionStop()
    //{
    //    //controller = null;
    //    Debug.Log("ViveGripInteractionStop");
    //}
    //void ViveGripGrabStart(ViveGrip_GripPoint gripPoint) {
    //    Debug.Log("NOTHING World ViveGripGrabStart");
    //}
    void ViveGripGrabStop(ViveGrip_GripPoint gripPoint) {
        Debug.Log("World ViveGripGrabStop SET KINEMATIC TRUE");
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }
    void ViveGripAirGrabStart(ViveGrip_GripPoint gripPoint)
    {
        Debug.Log("World ViveGripAIRGrabStart");
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
    }
    //void ViveGripAirGrabStop(ViveGrip_GripPoint gripPoint)
    //{
    //    Debug.Log("World ViveGripAIRGrabStop");
    //}
}

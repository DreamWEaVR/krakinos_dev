using UnityEngine;
using System.Collections;
using VRTK;

public class PlayerMove : MonoBehaviour {

    public VRTK_ControllerEvents leftController;
    public VRTK_ControllerEvents rightController;


    public Vector3 localDragStartPosition;

   

    public Quaternion playerDragStartRotation;
    public Vector3 playerDragStartPosition;

    public Quaternion pivotDragStartRotation;
    public Vector3 pivotDragStartGPosition;

    public float startScaleDis = 1;
    public Vector3 startScale;

    public float dragAmplifier = 1;

    public Transform pivotObject;
    public float lastYAngle;

    public VRTK_ControllerEvents dragController;

    bool dragging;
    bool rotating;

    int hasReported=0;


    public Vector3 RotationAxis = new Vector3(1, 1, 1);

    public bool ScaleEnabled = true;
    public float minScale = .5f;
    public float maxScale = 4f;

    public float scaleVal = 1;


    void Start () {
        leftController.TriggerPressed += new ControllerInteractionEventHandler(DoTriggerPressed);
        leftController.TriggerReleased += new ControllerInteractionEventHandler(DoTriggerReleased);

        rightController.TriggerPressed += new ControllerInteractionEventHandler(DoTriggerPressed);
        rightController.TriggerReleased += new ControllerInteractionEventHandler(DoTriggerReleased);
    }

    void FixedUpdate()
    {
        if (dragging)
        {
           // Vector3 dragOffset = localDragStartPosition - dragController.gameObject.transform.localPosition;
          //  gameObject.transform.position = playerDragStartPosition + dragOffset;
        }
        if (rotating)
        {

            positionPivotObject();

            //get the amount of change in rotation on the controller
            Quaternion deltaRotation = pivotObject.localRotation * Quaternion.Inverse(pivotDragStartRotation);
           

            transform.localRotation = Quaternion.Inverse(deltaRotation) * playerDragStartRotation;
            //

            //Vector3 deltaPosition = pivotObject.position - pivotDragStartGPosition;
            

            if (ScaleEnabled)
            {
                float currentDis = Vector3.Distance(rightController.gameObject.transform.localPosition, leftController.gameObject.transform.localPosition);
                scaleVal = currentDis / startScaleDis;
                float targScale = Mathf.Clamp(scaleVal, minScale, maxScale);
                transform.localScale = startScale / targScale;
            }

            Vector3 offset = pivotDragStartGPosition - pivotObject.position;
            transform.position += offset;





            if (hasReported < 2)
            {
                Debug.Log("start angle" + pivotDragStartRotation);
                Debug.Log("controller rotation" + pivotObject.localRotation);
                Debug.Log("deltaRotation" + deltaRotation);
                hasReported++;
            }

           // HandleRotation(deltaRotation * playerDragStartRotation );
        }
    }
    

    private void HandleRotation(Quaternion angles)
    {
        //pivotObject.localRotation = angles;
        
        //calculate the offset of the pivot
        //pivot must be direct child

        //allow draggin while rotating
        //Vector3 deltaPosition = pivotDragStartGPosition - pivotObject.localPosition;
        //Vector3 dir = new Vector3() - (pivotObject.localPosition);

        ////Vector3 dir = new Vector3() - rotationCenterObject.localPosition;

        //Quaternion iAngles = Quaternion.Inverse(angles);

        ////rotate it the offset
        //dir = iAngles * dir;

        //if (ScaleEnabled)
        //{
        //    float targScale = Mathf.Clamp(scaleVal, minScale, maxScale);
        //    dir = dir * targScale;
        //    transform.localScale = startScale / targScale;// new Vector3(targScale, targScale, targScale);
        //}

        ////set this transforms position
        //transform.localPosition = playerDragStartPosition + dir;// + (rotationCenterObject.localPosition);

        ////set the rotation, at this point angle is the inverse of the child rotation
        //transform.localRotation = angles;
    }

    private void startDragging(VRTK_ControllerEvents sender)
    {
        dragController = sender;
        dragging = true;
        //localDragStartPosition = dragController.gameObject.transform.localPosition;
       // playerDragStartPosition = gameObject.transform.localPosition;
       // playerDragStartRotation = gameObject.transform.localRotation;
    }
    private void startRotating()
    {
        rotating = true;
        dragging = false;
        //reset the rotation base to 0
        //pivotObject.transform.rotation = new Quaternion();

        //store the player position and rotation
        playerDragStartPosition = gameObject.transform.localPosition;
        playerDragStartRotation = gameObject.transform.localRotation;

        positionPivotObject();

        //store the initial rotation and position of the 
        pivotDragStartRotation = pivotObject.localRotation;
        pivotDragStartGPosition = pivotObject.position;

        startScale = transform.localScale;
        startScaleDis = Vector3.Distance(rightController.gameObject.transform.localPosition, leftController.gameObject.transform.localPosition);
        //lastYAngle = rotationCenterObject.transform.eulerAngles.y;
        //localDragStartPosition = dragController.gameObject.transform.localPosition;
        //playerWorldStartPosition = gameObject.transform.position;

        //localDragStartRotation = dragController.gameObject.transform.localRotation;
        //playerDragStartRotation = gameObject.transform.rotation;
    }

    private void positionPivotObject()
    {
        Vector3 pivotCenter = (rightController.gameObject.transform.localPosition + leftController.gameObject.transform.localPosition) * .5f;
        pivotObject.localPosition = pivotCenter;
        //pivotObject.LookAt(rightController.gameObject.transform, rightController.gameObject.transform.up);
        pivotObject.LookAt(rightController.gameObject.transform, pivotObject.up);



        pivotObject.localEulerAngles = new Vector3(
            pivotObject.localEulerAngles.x * RotationAxis.x,
            pivotObject.localEulerAngles.y * RotationAxis.y,
            pivotObject.localEulerAngles.z * RotationAxis.z
            );
    }

    private void StopDragging(VRTK_ControllerEvents sender)
    {
        if (sender == dragController)
        {
            dragging = false;
        }
        
    }
    private void StopRotating()
    {
        rotating = false;
        hasReported = 0;
    }

    private void DoTriggerPressed(object sender, ControllerInteractionEventArgs e)
    {
        Debug.Log("TRIGGER pressed");

        if (dragging)
        {
            startRotating();
          
        }else
        {
            startDragging((VRTK_ControllerEvents)sender);
        }
 
    }

    private void DoTriggerReleased(object sender, ControllerInteractionEventArgs e)
    {
        Debug.Log("TRIGGER released");

        if (rotating)
        {
            StopRotating();
        }
        else if (dragging)
        {
            StopDragging((VRTK_ControllerEvents)sender);
        }
    }
}

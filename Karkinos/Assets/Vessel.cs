using UnityEngine;
using System.Collections;

public class Vessel : MonoBehaviour {

    // Use this for initialization
    //void Start () {
    public bool isDragging = false;
    public Transform dragTarget;
    //}
    public GridRunner runnerScript;
    private Vector3 velocity = Vector3.zero;

    public Node lastValidNode;
    public GridRunner originalRunner;

    void Start()
    {
        runnerScript = GetComponent<GridRunner>();
    }
	
	// Update is called once per frame
	void FixedUpdate() {
        //snap to grid
        if (isDragging && dragTarget!=null)
        {
            //Vector3 origPos = transform.localPosition;
            //transform.position = dragTarget.position;

            //snap to controller
            //Vector3 targPosition = transform.localPosition;
            //Vector3 localTargPosition = transform.localPosition;
            Vector3 localTargPosition = transform.parent.InverseTransformPoint(dragTarget.position);

            //clamp it
            //localTargPosition = runnerScript.grid.getClosestNodePosition(localTargPosition);

            Node targNode = runnerScript.grid.getClosestNode(localTargPosition);
            if (targNode.walkable == true) {
                lastValidNode = targNode;
                originalRunner.MakeNewPath(lastValidNode);
            }

            Vector3 localTargNodePosition = runnerScript.grid.getTransformPosition(lastValidNode.position);
            //snap it to the grid
            //transform.localPosition = Vector3.Lerp(origPos, localTargPosition, Time.deltaTime);
            //transform.localPosition = localTargPosition;
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, localTargNodePosition, ref velocity, .1f);
        }
	}

    public void setAlpha(float val)
    {
        //transform.localScale = transform.localScale * 1.2f;
        Color col = GetComponent<Renderer>().material.GetColor("_Color");
        col.a = val;
        //col.r = 1;
        GetComponent<Renderer>().material.SetColor("_Color", col);
    }
}

using UnityEngine;
using System.Collections;

public class GridRunner : MonoBehaviour {
	public GridSystem grid;
	public Node gridNode; //current node
	public Node gridTargetNode; //the next node i'm moving to

	public GridPath currentPath;

	float speed = 1f;
	// Use this for initialization
	void Start () {
		

        //PickANewPath();
    }

    public void randomize()
    {
        speed = Random.Range(.4f, 3);
        gridNode = grid.getRandomNode(true);
        gridTargetNode = gridNode;
        //move to start position on grid
        transform.localPosition = grid.getTransformPosition(gridNode.position);
    }

    float startTime=0;
	float journeyLength=0;
	public bool idle=true;
	// Update is called once per frame
	void FixedUpdate () {
		//lerp to gridTargetNode
		if (!idle) {
			float distCovered = (Time.time - startTime) * speed;
			float fracJourney = distCovered / journeyLength;

//			Debug.Log("distCovered:" + distCovered + " fracJourney:"+fracJourney);

			if(fracJourney>=1){
//				Debug.Log("step complete, next node");
				nextNode();
			}else{
				if(float.IsNaN(fracJourney)) return;
				transform.localPosition = Vector3.Lerp (grid.getTransformPosition (gridNode.position), grid.getTransformPosition (gridTargetNode.position), fracJourney);
			}
		}

        //once there set a new path
        //StartCoroutine(PathComplete());

        //if (controller)
        //{
        //    controller.Vibrate(50, 0.1f);
        //}
    }

    void nextNode(){
		gridNode = currentPath.path[currentPath.currentIndex];
		currentPath.currentIndex++;

		if(currentPath.currentIndex >= currentPath.path.Count){
//			Debug.Log("path complete wait for new path");
			gridTargetNode = gridNode;
			//StartCoroutine(PathComplete());
            idle = true;
            return;
		}
		gridTargetNode = currentPath.path[currentPath.currentIndex];

		startTime = Time.time;
		journeyLength = Vector3.Distance(grid.getTransformPosition(gridNode.position), grid.getTransformPosition(gridTargetNode.position));
	}

	void OnMouseDown() {
        //PickANewPath();
    }
    public void MakeNewPath(Node toNode)
    {
        idle = true;
        currentPath = new GridPath();
        currentPath.find(grid, gridNode, toNode);
        Vector3[] linePath = currentPath.getNodeVectorPositions();
        //grid.pathRenderer.SetVertexCount(linePath.Length);
        //grid.pathRenderer.SetPositions(linePath);

        grid.vrLine.SetPositions(linePath);
    }
    public void TravelPath()
    {
        idle = false;
    }
    void PickANewPath(){
		idle = false;
		Node endPathNode = grid.getRandomNode(true);
		
		currentPath = new GridPath ();
		currentPath.find (grid, gridNode, endPathNode);
		grid.lastGridPath = currentPath;
        //		gridNode = gridTargetNode;
        //		transform.position = grid.getTransformPosition(gridNode.position);

        //		grid.lastGridPath = currentPath;
        //
        //		StartCoroutine (FollowPath ());

        //send the path to the line renderer
        
    }

	IEnumerator PathComplete() {
		idle = true;
		yield return new WaitForSeconds(.25f);
        //PickANewPath();
    }

    //SteamVR interaction junks

    //private ViveGrip_ControllerHandler controller;
    //void ViveGripInteractionStart(ViveGrip_GripPoint gripPoint)
    //{
    //    //if (gripPoint.HoldingSomething())
    //    //{
    //    //    controller = gripPoint.controller;
    //    //}
    //    controller = gripPoint.controller;
    //    if (idle)
    //    {
    //        PickANewPath();
    //    }
    //}
    //void ViveGripInteractionStop()
    //{
    //    controller = null;
    //}
}

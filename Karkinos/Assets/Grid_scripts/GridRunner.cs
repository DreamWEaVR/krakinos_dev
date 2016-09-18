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
		speed = Random.Range (.4f, 3);
		gridNode = grid.getRandomNode(true);
		gridTargetNode = gridNode;
		//move to start position on grid
		transform.localPosition = grid.getTransformPosition(gridNode.position);

        PickANewPath();
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


    }

    void nextNode(){
		gridNode = currentPath.path[currentPath.currentIndex];
		currentPath.currentIndex++;

		if(currentPath.currentIndex >= currentPath.path.Count){
//			Debug.Log("path complete wait for new path");
			gridTargetNode = gridNode;
			StartCoroutine(PathComplete());
			return;
		}
		gridTargetNode = currentPath.path[currentPath.currentIndex];

		startTime = Time.time;
		journeyLength = Vector3.Distance(grid.getTransformPosition(gridNode.position), grid.getTransformPosition(gridTargetNode.position));
	}

	void OnMouseDown() {
        //PickANewPath();
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
        Vector3[] linePath = currentPath.getNodeVectorPositions();
        //grid.pathRenderer.siz
        grid.pathRenderer.SetVertexCount(linePath.Length);
       grid.pathRenderer.SetPositions(linePath);
	}

	IEnumerator PathComplete() {
		idle = true;
		yield return new WaitForSeconds(2f);
        PickANewPath();
    }

//	IEnumerator FollowPath() {
//		//move to next node
//		gridNode = currentPath.path[currentPath.currentIndex];
//		transform.position = grid.getTransformPosition(gridNode.position);
//
//		currentPath.currentIndex++;
//		//if out of nodes, wait for a second then 
//		yield return new WaitForSeconds(.2f);
//
//		if (currentPath.currentIndex < currentPath.path.Count) {
//			StartCoroutine (FollowPath ());
//		} else {
//			Debug.Log("completed path");
//			yield return new WaitForSeconds(2f);
//			PickANewPath();
//		}
////		for (float f = 1f; f >= 0; f -= 0.1f) {
////			Color c = renderer.material.color;
////			c.a = f;
////			renderer.material.color = c;
////			yield return null;
////		}
//	}
}

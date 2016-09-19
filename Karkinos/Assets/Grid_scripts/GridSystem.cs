using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//public enum enMenuScreen {Main, Options, Extras};
public struct IntVector3  {
	public int x, y, z;
	public IntVector3(int _x, int _y, int _z){
		x = _x;
		y = _y;
		z = _z;
	}
}

public class GridSystem : MonoBehaviour {
	public Vector3 gridSize = new Vector3(10,10,10); 
	public Vector3 segmentSize = new Vector3(0.5f,0.5f,0.5f);
	// Use this for initialization
	public GridPath lastGridPath;

	public List<GridRunner> runners;

	public LayerMask unwalkableMask;

	public int[,,] grid;

    public LineRenderer pathRenderer;

	public Node[,,] nodes;
	public List<Node> walkableNodes; // when walkables change

	public bool drawGrid;

    public float worldScale;

	void Awake(){
		buildGrid ();
	}
	void Start () {
//		buildGrid ();
	}
	/*INIT*/
	void buildGrid(){
		grid = new int[(int)gridSize.x, (int)gridSize.y, (int)gridSize.z];

		nodes = new Node[(int)gridSize.x, (int)gridSize.y, (int)gridSize.z];
		//populate nodes?
		for (int x = 0; x < gridSize.x; x++) {
			for (int y = 0; y < gridSize.y; y++) {
				for (int z = 0; z < gridSize.z; z++) {
					IntVector3 pos = new IntVector3 (x, y, z);
                    //Vector3 localPoint = getTransformPosition(pos);
                    //Vector3 worldPoint = transform.TransformPoint(localPoint);

                    Vector3 worldPoint = localToWorld(getTransformPosition(pos));

                    bool walkable = !( Physics.CheckSphere(worldPoint, segmentSize.x *.5f * transform.lossyScale.x, unwalkableMask) );
					nodes[x, y, z] = new Node(walkable, pos);
				}
			}
		}

		updateWalkables ();
		Debug.Log ("grid built");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Vector3 localToWorld(Vector3 localPoint)
    {
        return transform.TransformPoint(localPoint);
    }

	/*UTILS*/
	public int MaxSize{
		get {
			return (int)(gridSize.x * gridSize.y * gridSize.z);
		}
	}
	public Vector3 getTransformPosition(IntVector3 nodePos){
		return new Vector3((float)nodePos.x * segmentSize.x, (float)nodePos.y * segmentSize.y, (float)nodePos.z * segmentSize.z) + transform.localPosition;
	}
	public Node getNodeForPosition(IntVector3 pos){
		return nodes[pos.x, pos.y, pos.z];
	}
	//set a position in the grid to a value
//	public void setGridNode(Vector3 _node, int _value){
//		grid[(int)_node.x, (int)_node.y, (int)_node.z] = _value;
//	}

	public List<Node> getNeighbors(Node n){
		//up down left right forward backward
		List<Node> neighbors = new List<Node>();
		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1 ; y++) {
				for (int z = -1; z <= 1; z++) {
                    if (Mathf.Abs(x) + Mathf.Abs(y) + Mathf.Abs(z) == 3) continue; //ignore the corner neightbors
					if(x==0 && y==0 && z==0) continue;
					int gX = n.position.x + x;
					int gY = n.position.y + y;
					int gZ = n.position.z + z;
					if (gX >= 0 && gY >= 0 && gZ >= 0 && gX < gridSize.x && gY < gridSize.y && gZ < gridSize.z) {
						neighbors.Add(nodes[gX,gY,gZ]);
					}
				}
			}
		}
		return neighbors;
	}

	public Node getRandomNode(bool forceWalkable){
		if (walkableNodes == null) {
			Debug.Log ("getRandomNode !walkableNodes ");
			return null;
		}
		if (forceWalkable) {
			return walkableNodes [(int)Random.Range (0, walkableNodes.Count - 1)];
		}
		return nodes [(int)Random.Range (0, gridSize.x - 1), (int)Random.Range (0, gridSize.y - 1), (int)Random.Range (0, gridSize.z - 1)];
	}

	void updateWalkables(){
		walkableNodes = new List<Node>();
		foreach (Node n in nodes) {
			if (n.walkable)
				walkableNodes.Add (n);
		}
	}

	private void OnDrawGizmos () {
		//show the grid
		if (drawGrid) {
			if (nodes!=null) {
				foreach (Node n in nodes) {
                    Vector3 localPoint = getTransformPosition(n.position);
                    Vector3 worldPoint = transform.TransformPoint(localPoint);
                    if (n.walkable) {
						Gizmos.color = Color.blue;
                        //getTransformPosition
                        Gizmos.DrawSphere(worldPoint, 0.005f);
                        //Gizmos.DrawSphere (new Vector3 (n.position.x * segmentSize.x, n.position.y * segmentSize.y, n.position.z * segmentSize.z) + transform.position, 0.05f);
                    }
                    else
                    {
                        Gizmos.color = Color.green;
                        Gizmos.DrawSphere(worldPoint, 0.005f);
                    }
				}
			}
		}
		//float cur = 0;
		//if (Application.isPlaying) {
		//	foreach (GridRunner r in runners) {
  //              if (r.idle) return;
		//		IntVector3 lastPos = r.currentPath.startNode.position;
		//		float ratio = cur / (float)runners.Count;
		//		Gizmos.color = new Color (.8f, ratio * .7f, 1f - ratio, 1f);

		//		for (int n = 0; n < r.currentPath.path.Count; n++) {
		//			Gizmos.DrawLine (getTransformPosition (lastPos), getTransformPosition (r.currentPath.path [n].position));
		//			lastPos = r.currentPath.path [n].position;
		//		}
		//		cur++;
		//	}
		//}

	}
}

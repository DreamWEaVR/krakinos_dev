using UnityEngine;
using System.Collections;

public class Node : IHeapItem<Node>{

	public bool walkable;
	public IntVector3 position;

	public int gCost;
	public int hCost;

	public Node parent;

	int heapIndex;

	public Node(bool _walkable, IntVector3 _position){
		walkable = _walkable;
		position = _position;
	}

	public int fCost{
		get{
			return gCost + hCost;
		}
	}

	public int HeapIndex{
		get{
			return heapIndex;
		}
		set{
			heapIndex = value;
		}
	}

	public int CompareTo(Node compareNode){
		int compare = fCost.CompareTo (compareNode.fCost);
		if(compare == 0){
			compare = hCost.CompareTo (compareNode.hCost);
		}
		return -compare;
	}
}

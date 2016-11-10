using UnityEngine;
using System.Collections;

public class VRLineRenderer : MonoBehaviour {

    public GameObject lineSegment;
    public Vector3[] Positions;
    public GameObject[] Segments; 
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    private void clear()
    {
        for(int i =0; i<Segments.Length; i++)
        {
            Destroy(Segments[i]); //maybe don't clear out segments, just hide
        }
    }
    public void SetPositions(Vector3[] positions)
    {
        Positions = positions;
        clear();
        Segments = new GameObject[Positions.Length];
        //render them
        for (int i = 0; i < Positions.Length-1; i++)
        {
            GameObject seg = Instantiate(lineSegment);
            seg.transform.parent = lineSegment.transform.parent;
            float dis = Vector3.Distance(Positions[i], Positions[i + 1]);
            seg.transform.localScale = new Vector3(1f, 1f, dis*.5f);
            seg.transform.localPosition = Positions[i];
            seg.transform.LookAt(transform.TransformPoint(Positions[i + 1]));
            Segments[i] = seg;
        }
    }

    /*
     * grid.pathRenderer.SetVertexCount(linePath.Length);
        grid.pathRenderer.SetPositions(linePath);
     * */
}

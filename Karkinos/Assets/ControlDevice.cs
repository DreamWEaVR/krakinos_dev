using UnityEngine;
using System.Collections;
using VRTK;

public class ControlDevice : MonoBehaviour {

    public VRTK_ControllerEvents controller;
    public Transform cursor;

    public GameObject hoveredObject;
    public GameObject selectedObject;

    public GameObject selectionClone;
    // Use this for initialization
    void Start () {
        controller.TriggerPressed += new ControllerInteractionEventHandler(DoTriggerPressed);
        controller.TriggerReleased += new ControllerInteractionEventHandler(DoTriggerReleased);
        Debug.Log("Start");
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.position = controller.transform.position;
        transform.rotation = controller.transform.rotation;
    }

    private void DoTriggerPressed(object sender, ControllerInteractionEventArgs e)
    {
        //startDragging((VRTK_ControllerEvents)sender);
        //hit test
        //cursor.transform.hit
        if (hoveredObject!=null)
        {
            selectedObject = hoveredObject;
            selectionClone = (GameObject)Instantiate(hoveredObject);
            selectionClone.GetComponent<Vessel>().dragTarget = cursor;
            selectionClone.GetComponent<Vessel>().lastValidNode = selectedObject.GetComponent<GridRunner>().gridNode;
            selectionClone.GetComponent<Vessel>().originalRunner = hoveredObject.GetComponent<GridRunner>();
            selectionClone.transform.parent = hoveredObject.transform.parent;
            selectionClone.transform.position = hoveredObject.transform.position;
            selectionClone.transform.localScale = hoveredObject.transform.localScale;

            selectionClone.GetComponent<Vessel>().isDragging = true;

            selectionClone.layer = 0;
            selectionClone.GetComponent<Vessel>().setAlpha(.5f);
        }
    }

    private void DoTriggerReleased(object sender, ControllerInteractionEventArgs e)
    {
        if (selectedObject) {
            selectedObject.GetComponent<GridRunner>().TravelPath();
            selectedObject = null;
            Destroy(selectionClone);
         }
    }

    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("OnTriggerEnter");
        hoveredObject = collider.gameObject;
    }
    void OnTriggerExit(Collider collider)
    {
        hoveredObject = null;
        Debug.Log("OnTriggerExit");
    }
}

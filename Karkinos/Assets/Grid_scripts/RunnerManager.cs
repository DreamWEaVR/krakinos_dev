using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RunnerManager : MonoBehaviour {
    public int runnerCount = 10;
    public GridRunner baseRunner; //to duplicate
    public List<GridRunner> runners;
	// Use this for initialization
	void Start () {
        runners = new List<GridRunner>();
        for(int i =0; i<runnerCount; i++)
        {
            GridRunner runner = (GridRunner)Instantiate(baseRunner, baseRunner.transform.position, Quaternion.identity);
            
            runner.transform.parent = baseRunner.transform.parent;
            runner.transform.localScale = baseRunner.transform.localScale;

            runner.randomize();

            runners.Add(runner);
        }
    }
	
}

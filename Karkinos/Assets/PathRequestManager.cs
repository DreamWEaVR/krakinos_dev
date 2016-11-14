using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PathRequestManager : MonoBehaviour {


    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    PathRequest currentPathRequest;

    static PathRequestManager instance;

    public GridSystem grid;
    public GridPath gridPath;
    bool isProcessingPath;

    void Awake()
    {
        instance = this;
        grid = GetComponent<GridSystem>();
        gridPath = GetComponent<GridPath>();
    }

    public static void RequestPath(Node startNode, Node endNode, Action<GridPath, bool> callback)
    {
        Debug.Log("RequestPath");
        PathRequest newRequest = new PathRequest(startNode, endNode, callback);
        instance.pathRequestQueue.Enqueue(newRequest);
        instance.TryProcessNext();
    }
    public static PathRequestManager GetInstance()
    {
        return instance;
    }


    void TryProcessNext()
    {
        Debug.Log("TryProcessNext");
        if (!isProcessingPath && pathRequestQueue.Count > 0)
        {
            currentPathRequest = pathRequestQueue.Dequeue();
            isProcessingPath = true;
            gridPath.StartFindPath(grid, currentPathRequest.pathStart, currentPathRequest.pathEnd);
        }
    }

    public void FinishedProcessingPath(GridPath path, bool success)
    {
        Debug.Log("FinishedProcessingPath");
        currentPathRequest.callback(path, success);
        isProcessingPath = false;
        //TryProcessNext();
    }

	struct PathRequest
    {
        public Node pathStart;
        public Node pathEnd;
        public Action<GridPath, bool> callback;
        
        public PathRequest(Node _start, Node _end, Action<GridPath, bool> _callback)
        {
            pathStart = _start;
            pathEnd = _end;
            callback = _callback;
        }
    }
}

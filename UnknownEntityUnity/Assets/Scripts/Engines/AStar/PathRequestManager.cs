using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathRequestManager : MonoBehaviour
{
    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    PathRequest currentPathRequest;
    static PathRequestManager instance;
    Pathfinding pathfinding;
    bool isProcessingPath;

    void Awake() {
        instance = this;
        pathfinding = GetComponent<Pathfinding>();
    }

    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, float unitIntel, Action<Vector3[], bool> callBack) {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, unitIntel, callBack);
        instance.pathRequestQueue.Enqueue(newRequest);
        instance.TryProcessNext();
    }

    void TryProcessNext() {
        if (!isProcessingPath && pathRequestQueue.Count > 0) {
            currentPathRequest = pathRequestQueue.Dequeue();
            isProcessingPath = true;
            //Debug.Log("Succesfully processing path request.");
            pathfinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd, currentPathRequest.unitIntel);
        }
    }

    public void FinishedProcessingPath(Vector3[] path, bool success) {
        currentPathRequest.callBack(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }

    struct PathRequest {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public float unitIntel;
        public Action<Vector3[], bool> callBack;

        public PathRequest(Vector3 _start, Vector3 _end, float _intel, Action<Vector3[], bool> _callBack) {
            pathStart = _start;
            pathEnd = _end;
            unitIntel = _intel;
            callBack = _callBack;
        }
    }
}

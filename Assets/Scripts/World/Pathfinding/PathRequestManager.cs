using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PathRequestManager : MonoBehaviour {
	
	Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
	PathRequest currentPathRequest;
	
	static PathRequestManager instance;
	Pathfinding pathfinding;
	
	bool isProcessingPath;
	
	void Awake() {
		instance = this;
		pathfinding = GetComponent<Pathfinding>();
	}
	
	public static void RequestPath(int accesableGroundType, Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback) {
		PathRequest newRequest = new PathRequest(accesableGroundType, pathStart, pathEnd,callback);
		instance.pathRequestQueue.Enqueue(newRequest);
		instance.TryProcessNext();
	}
	
	void TryProcessNext() {
		if (!isProcessingPath && pathRequestQueue.Count > 0) {
			currentPathRequest = pathRequestQueue.Dequeue();
			isProcessingPath = true;
			pathfinding.StartFindPath(currentPathRequest.accesableGroundType, currentPathRequest.pathStart, currentPathRequest.pathEnd);
		}
	}
	
	public void FinishedProcessingPath(Vector3[] path, bool success) {
		currentPathRequest.callback(path,success);
		isProcessingPath = false;
		TryProcessNext();
	}
	
	struct PathRequest {
        public int accesableGroundType;
        public Vector3 pathStart;
		public Vector3 pathEnd;
		public Action<Vector3[], bool> callback;
		
		public PathRequest(int _accesableGroundType, Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback) {
            accesableGroundType = _accesableGroundType;
            pathStart = _start;
			pathEnd = _end;
			callback = _callback;
		}
		
	}
}

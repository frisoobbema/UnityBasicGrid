using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    public int accesableGroundType = 0;

    private float speed = 5;
    private bool selected;

    private Vector3[] path;
    private int targetIndex;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(1) && selected) {

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 1000)) {

                targetIndex = 0;
                path = null;
                PathRequestManager.RequestPath(accesableGroundType, transform.position, hit.point, OnPathFound);
            }

        }

        if (GetComponent<Renderer>().isVisible && Input.GetMouseButton(0)) {

            Vector3 cameraPos = Camera.main.WorldToScreenPoint(transform.position);
            cameraPos.y = MainCamera.InvertMouseY(cameraPos.y);
            selected = MainCamera.selection.Contains(cameraPos);
        }

        if (selected) {

            GetComponent<Renderer>().material.color = Color.red;
        }
        else {

            GetComponent<Renderer>().material.color = Color.white;
        }

    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccesful)
    {
        if (pathSuccesful)
        {
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }

    }

    IEnumerator FollowPath()
    {
        if (path.Length != 0)
        {
            Vector3 currentWaypoint = path[0];

            while (true)
            {
                if (transform.position == currentWaypoint)
                {
                    targetIndex++;

                    if (targetIndex >= path.Length)
                    {
                        targetIndex = 0;
                        path = null;
                        yield break;
                    }
                    currentWaypoint = path[targetIndex];
                }

                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
                yield return null;
            }
        }
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {

            
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}

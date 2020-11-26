using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathUnit : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;
    float speed = 5;
    Vector3[] path;
    int targetIndex;
    void Start()
    {
        PathHandler.RequestPath(transform.position, target.position, OnPathFound);
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
    IEnumerator FollowPath(){
        Vector3 currentWaypont = path[0];

        while (true)
        {
            if (transform.position == currentWaypont)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    yield break;
                }
                currentWaypont = path[targetIndex];
            }
            transform.position = Vector3.MoveTowards(transform.position, currentWaypont, speed*Time.deltaTime);
            yield return null;
        }
       }

    // Update is called once per frame
    void Update()
    {
        
    }
}

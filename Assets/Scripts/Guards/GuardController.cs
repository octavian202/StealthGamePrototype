using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardController : MonoBehaviour
{
	public Transform[] points;
	private int destPoint = 0;
    public Camera cam;
    public NavMeshAgent agent;


    void Start()
    {
    	agent.autoBraking = false;

    	GoToNextPoint();
    }

    void GoToNextPoint()
    {
    	if(points.Length == 0)
    		return;

    	agent.destination = points[destPoint].position;
    	destPoint = (destPoint + 1) % points.Length;
    }

    void Update()
    {
        /*if(Input.GetMouseButtonDown(0))
        {
        	Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        	RaycastHit hit;

        	if(Physics.Raycast(ray, out hit))
        		agent.SetDestination(hit.point);
        }*/

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GoToNextPoint();

        //if (patrol == true)
        //{

    //    }
    }
}

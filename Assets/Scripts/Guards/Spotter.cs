using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spotter : MonoBehaviour
{
    public Transform spotTarget;
    public Transform spotOrigin;
    public Renderer render;
    private Color oldColor = Color.green;
    private bool beingSpotted = false;
    

    void Start()
    {
        render.material.color = oldColor;
    }

    private void Update()
    {
        if (beingSpotted == true)
            if (verifyObstacle() == true)
                render.material.color = Color.red;
        else
            render.material.color = Color.green;
    }

    public bool verifyObstacle()
    {
        RaycastHit hit;
        Vector3 raycastDir = spotTarget.position - spotOrigin.position;

        Physics.Raycast(spotOrigin.position, raycastDir, out hit);

        Target target = hit.transform.GetComponent<Target>();

        Debug.Log(hit.transform.name);

        if (target != null)
            return true;
        else
            return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        beingSpotted = true;
    }

    private void OnTriggerExit(Collider other)
    {
        beingSpotted = false;
        render.material.color = Color.green;

    }
}

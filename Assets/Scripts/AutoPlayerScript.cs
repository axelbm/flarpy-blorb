using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPlayerScript : MonoBehaviour
{
    public ControllerScript controllerScript;
    public BirdScript birdScript;

    // Update is called once per frame
    void Update()
    {
        if (birdScript.nextPipe) {
            float heightTarget =
                birdScript.nextPipe.transform.position.y -
                birdScript.nextPipe.GetComponent<PipeMoveScript>().gateSize / 2 +
                birdScript.GetComponent<CircleCollider2D>().radius +
                2;

            if (birdScript.myRigidbody.velocity.y < 0.1 && birdScript.transform.position.y < heightTarget)
                controllerScript.Jump();
        }
    }
}

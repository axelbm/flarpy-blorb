using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeMoveScript : MonoBehaviour
{
    public float moveSpeed = 5;
    public float deadZone = -45;
    public bool running = true;

    public float gateSize = 16;

    public GameObject topPipe;
    public GameObject bottomPipe;
    public GameObject gate;

    Transform GetChildByName(string name)
    {
        Transform[] children = transform.GetComponentsInChildren<Transform>();

        foreach (Transform child in children)
        {
            if (child.name == name)
            {
                return child;
            }
        }

        return null;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetGateSize(gateSize);
    }

    public void SetGateSize(float size)
    {
        gate.transform.localScale = new Vector3(
            gate.transform.localScale.x,
            gateSize,
            gate.transform.localScale.z
        );

        float topPipeHeight = Math.Abs(topPipe.transform.GetComponent<SpriteRenderer>().sprite.bounds.size.y * topPipe.transform.localScale.y);
        topPipe.transform.position = new Vector3(
            topPipe.transform.position.x,
            transform.position.y + topPipeHeight / 2 + gateSize / 2,
            topPipe.transform.position.z
        );

        float bottomPipeHeight = Math.Abs(bottomPipe.transform.GetComponent<SpriteRenderer>().sprite.bounds.size.y * bottomPipe.transform.localScale.y);
        bottomPipe.transform.position = new Vector3(
            bottomPipe.transform.position.x,
            transform.position.y - bottomPipeHeight / 2 - gateSize / 2,
            bottomPipe.transform.position.z
        );
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + Vector3.left * moveSpeed * Time.deltaTime;

        if (transform.position.x < deadZone)
        {
            Destroy(gameObject);
        }
    }
}

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
    public float detectionSize = 10;

    public GameObject topPipe;
    public GameObject bottomPipe;
    public GameObject gate;

    // Start is called before the first frame update
    void Start()
    {
        SetGateSize(gateSize);
        SetDetectionSize(detectionSize);
    }

    public void SetGateSize(float size)
    {
        gateSize = size;

        gate.transform.localScale = new Vector3(
            gate.transform.localScale.x,
            size,
            gate.transform.localScale.z
        );

        float topPipeHeight = Math.Abs(topPipe.transform.GetComponent<SpriteRenderer>().sprite.bounds.size.y * topPipe.transform.localScale.y);
        topPipe.transform.position = new Vector3(
            topPipe.transform.position.x,
            transform.position.y + topPipeHeight / 2 + size / 2,
            topPipe.transform.position.z
        );

        float bottomPipeHeight = Math.Abs(bottomPipe.transform.GetComponent<SpriteRenderer>().sprite.bounds.size.y * bottomPipe.transform.localScale.y);
        bottomPipe.transform.position = new Vector3(
            bottomPipe.transform.position.x,
            transform.position.y - bottomPipeHeight / 2 - size / 2,
            bottomPipe.transform.position.z
        );
    }

    public void SetDetectionSize(float size)
    {
        detectionSize = size;

        BoxCollider2D collider = GetComponent<BoxCollider2D>();

        float totalHeight =
            Math.Abs(topPipe.transform.GetComponent<SpriteRenderer>().sprite.bounds.size.y * topPipe.transform.localScale.y) +
            Math.Abs(bottomPipe.transform.GetComponent<SpriteRenderer>().sprite.bounds.size.y * bottomPipe.transform.localScale.y) +
            gateSize;

        float targetSize = size - 5;

        collider.size = new Vector2(targetSize, totalHeight);
        collider.offset = new Vector2(-targetSize / 2, 0);
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

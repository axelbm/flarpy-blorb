using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSpawnScript : MonoBehaviour
{
    public GameObject pipePrefab;
    private float timer = 0;
    public float gateSize = 16;
    public float pipeOffset = 10;
    public float pipeSpeed = 5;
    public float pipeDistance = 40;
    public float spawnAtStart = 0.2f;

    private float spawnRate
    {
        get
        {
            return pipeDistance / pipeSpeed;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        int pipeToSpawn = (int)Math.Ceiling(spawnAtStart);

        for (int i = 0; i < pipeToSpawn; i++)
        {
            float offset = (spawnAtStart - i) * pipeDistance;
            spawnPipe(-offset);
        }

        timer = spawnAtStart % 1 * spawnRate;

        if (timer == 0)
            spawnPipe();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < spawnRate)
        {
            timer = timer + Time.deltaTime;
        }
        else
        {
            spawnPipe();
            timer = 0;
        }

    }

    void spawnPipe(float offset = 0)
    {
        Vector3 spawnPosition =
            transform.position + Vector3.up * UnityEngine.Random.Range(-pipeOffset, pipeOffset) +
            Vector3.right * offset;

        GameObject pipe = Instantiate(pipePrefab, spawnPosition, transform.rotation);
        pipe.transform.parent = transform;
        pipe.GetComponent<PipeMoveScript>().gateSize = gateSize;
        pipe.GetComponent<PipeMoveScript>().moveSpeed = pipeSpeed;
        pipe.GetComponent<PipeMoveScript>().detectionSize = pipeDistance;
    }
}

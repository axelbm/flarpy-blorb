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

    void spawnPipe()
    {
        Vector3 spawnPosition = transform.position + Vector3.up * Random.Range(-pipeOffset, pipeOffset);

        GameObject pipe = Instantiate(pipePrefab, spawnPosition, transform.rotation);
        pipe.GetComponent<PipeMoveScript>().gateSize = gateSize;
        pipe.GetComponent<PipeMoveScript>().moveSpeed = pipeSpeed;
        pipe.GetComponent<PipeMoveScript>().detectionSize = pipeDistance;
    }
}

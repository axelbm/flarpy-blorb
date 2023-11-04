using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdScript : MonoBehaviour
{
    public Rigidbody2D myRigidbody;
    public float flapStrength;
    public LogicScript logicScript;
    public float screenEdge;

    public AudioSource flapSound;
    public AudioSource scoreSound;
    public AudioSource hitSound;

    public GameObject wingUp;
    public GameObject wingDown;
    public GameObject nextPipe;

    public ParticleSystem flapParticles;


    // Start is called before the first frame update
    void Start()
    {
        logicScript = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
        // gameObject.name = "Bob Birdington";

        flapSound.volume = logicScript.soundEffectVolume;
        scoreSound.volume = logicScript.soundEffectVolume;
        hitSound.volume = logicScript.soundEffectVolume;
    }

    // Update is called once per frame
    void Update()
    {
        // rotate with velocity
        float angle = Mathf.Atan2(myRigidbody.velocity.y, 50) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (logicScript.IsGameRunning() == false)
            return;

        IsOutOfBounds();

        bool isFlapping = myRigidbody.velocity.y > 0;
        wingUp.SetActive(!isFlapping);
        wingDown.SetActive(isFlapping);

        wingUp.transform.rotation = Quaternion.AngleAxis(angle * 2, Vector3.forward);
        wingDown.transform.rotation = Quaternion.AngleAxis(angle * 2, Vector3.forward);
    }

    public void Flap()
    {
        myRigidbody.velocity = Vector2.up * flapStrength;
        flapSound.Play();

        flapParticles.Play();
    }


    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Gate"))
        {
            logicScript.AddScore();
            scoreSound.Play();
        }

        if (collider.gameObject.CompareTag("Pipe Group"))
        {
            nextPipe = collider.gameObject;

            Debug.Log("next pipe: " + nextPipe.name + " " + nextPipe.transform.position.x);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Pipe"))
        {
            hitSound.Play();
            logicScript.GameOver();
        }
    }

    void IsOutOfBounds()
    {
        if (transform.position.y > screenEdge || transform.position.y < -screenEdge)
        {
            logicScript.GameOver();
        }
    }
}

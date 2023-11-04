using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdScript : MonoBehaviour
{
    public Rigidbody2D myRigidbody;
    public float flapStrength;
    public LogicScript logicScript;
    public float screenEdge;

    public float soundEffectVolume = 0.5f;
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
        flapSound.volume = soundEffectVolume;
        scoreSound.volume = soundEffectVolume;
        hitSound.volume = soundEffectVolume;
    }

    // Update is called once per frame
    void Update()
    {
        // rotate with velocity
        float angle = Mathf.Atan2(myRigidbody.velocity.y, 50) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (logicScript && logicScript.IsGameRunning() == false)
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
        if (collider.gameObject.CompareTag("Pipe Group"))
        {
            nextPipe = collider.gameObject;
        }


        if (logicScript)
        {
            if (collider.gameObject.CompareTag("Gate"))
            {
                logicScript.AddScore();
                scoreSound.Play();
            }

            if (collider.gameObject.CompareTag("Pipe"))
            {
                hitSound.Play();
                logicScript.GameOver();
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
    }

    void IsOutOfBounds()
    {
        if (logicScript)
        {
            if (transform.position.y > screenEdge || transform.position.y < -screenEdge)
            {
                logicScript.GameOver();
            }
        }
    }
}

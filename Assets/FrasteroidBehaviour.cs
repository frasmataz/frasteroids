using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrasteroidBehaviour : MonoBehaviour
{
    public Rigidbody2D rigidBody;
    public AudioSource audioSource;
    public AudioClip[] idleAudioClips;
    public AudioClip[] hitAudioClips;
    public AudioClip impactClip;
    public float initSpeed;
    public float maxSpinSpeed;
    public float minSize;
    public float minIdleAudioTime;
    public float maxIdleAudioTime;

    private LogicBehaviour logicBehaviour;
    private float idleAudioTimer;

    // Start is called before the first frame update
    void Start()
    {
        logicBehaviour = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicBehaviour>();

        float direction = Random.Range(-180, 180);
        rigidBody.velocity += new Vector2(
            (initSpeed * Mathf.Exp(gameObject.transform.localScale.magnitude)) * Mathf.Sin(direction), 
            (initSpeed * Mathf.Exp(gameObject.transform.localScale.magnitude)) * Mathf.Cos(direction)
        );

        rigidBody.angularVelocity = Random.Range(-maxSpinSpeed, maxSpinSpeed);

        SetIdleAudioTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if (idleAudioTimer < Time.fixedTime)
        {
            audioSource.volume = gameObject.transform.localScale.magnitude * 0.5f + 0.5f;
            audioSource.pitch = 1 / ((gameObject.transform.localScale.magnitude + 0.2f) * 2) + 0.5f;
            audioSource.PlayOneShot(idleAudioClips[(int)Random.Range(0, idleAudioClips.Length)]);
            audioSource.Play();
            SetIdleAudioTimer();
        }
    }

    private void SetIdleAudioTimer()
    {
        idleAudioTimer = Time.fixedTime + Random.Range(minIdleAudioTime, maxIdleAudioTime);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Bullet")
        {
            logicBehaviour.IncreaseScore(100);
            SplitAndDie();
            Destroy(collider.gameObject);
        }
    }

    void SplitAndDie()
    {
        AudioSource.PlayClipAtPoint(impactClip, Vector3.zero, 100.0f);
        AudioSource.PlayClipAtPoint(hitAudioClips[(int)Random.Range(0, hitAudioClips.Length)], gameObject.transform.position);

        if (gameObject.transform.localScale.magnitude > minSize)
        {
            GameObject child1 = Instantiate(gameObject);
            GameObject child2 = Instantiate(gameObject);

            child1.transform.localScale = gameObject.transform.localScale / 2;
            child2.transform.localScale = gameObject.transform.localScale / 2;

            child1.GetComponent<Rigidbody2D>().velocity = rigidBody.velocity;
            child2.GetComponent<Rigidbody2D>().velocity = rigidBody.velocity;
        }

        Destroy(gameObject);
    }
}

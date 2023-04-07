using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pewBehaviour : MonoBehaviour
{
    public Rigidbody2D rigidBody;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody.velocity = transform.up * speed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

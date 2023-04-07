using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShipBehaviour : MonoBehaviour
{
    public Rigidbody2D shipRigidBody;
    public SpriteRenderer spriteRenderer;

    public GameObject pew;

    public float maxVelocity;
    public float acceleration;
    public float rotationSpeed;
    public float shotDelay;
    public float damageExp;
    public float damageMultiplier;

    public Sprite noBoost;

    private float shotCooldown;
    private float health = 100;

    private LogicBehaviour logicBehaviour;

    // Start is called before the first frame update
    void Start()
    {
        logicBehaviour = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicBehaviour>();
        logicBehaviour.UpdateHealthUI(health);
    }

    // Update is called once per frame
    void Update()
    {
        GetMovementInput();

        if (Input.GetKey(KeyCode.Space))
        {
            Shoot();
        }

        shotCooldown -= Time.deltaTime;
    }

    private void GetMovementInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            shipRigidBody.AddForce(Vector2.up * acceleration);
        }

        if (Input.GetKey(KeyCode.S))
        {
            shipRigidBody.AddForce(Vector2.down * acceleration);
        }

        if (Input.GetKey(KeyCode.A))
        {
            shipRigidBody.AddForce(Vector2.left * acceleration);
        }

        if (Input.GetKey(KeyCode.D))
        {
            shipRigidBody.AddForce(Vector2.right * acceleration);
        }

        // convert mouse position into world coordinates
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // get direction you want to point at
        Vector2 direction = (mouseWorldPosition - (Vector2)transform.position).normalized;

        // set vector of transform directly
        transform.up = direction;
    }

    private void Shoot()
    {
        if (shotCooldown <= 0)
        {
            shotCooldown = shotDelay;
            Instantiate(pew, gameObject.transform.position, gameObject.transform.rotation);
        }
    }

    private void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.tag == "Asteroid")
        {
            TakeDamage( Mathf.Log((collider.gameObject.GetComponent<Rigidbody2D>().mass * damageExp) + 1) * damageMultiplier );
        }
    }

    private void TakeDamage(float damage)
    {
        health -= damage;
        logicBehaviour.UpdateHealthUI(health);

        if (health <= 0)
        {
            logicBehaviour.GameOver();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class DestroyOffScreenEdge : MonoBehaviour
{
    public Rigidbody2D rigidBody;
    public float xOffset;
    public float yOffset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cameraPosition = Camera.main.WorldToViewportPoint(transform.position);

        if (
            (cameraPosition.x < (0.00 + xOffset) && rigidBody.velocity.x < 0.0) ||
            ((1.0 - xOffset) < cameraPosition.x && rigidBody.velocity.x > 0.0) ||
            (cameraPosition.y < (0.00 + yOffset) && rigidBody.velocity.y < 0.0) ||
            ((1.0 - yOffset) < cameraPosition.y && rigidBody.velocity.y > 0.0)
        )
        {
            Destroy(gameObject);
        }
    }
}

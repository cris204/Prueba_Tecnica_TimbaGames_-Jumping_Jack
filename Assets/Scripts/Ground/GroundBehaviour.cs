using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundBehaviour : MonoBehaviour {

    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private float horizontalSpeed;
    private Vector2 speedVector;

	void Awake () {

        rb.GetComponent<Rigidbody2D>();
        speedVector = Vector2.right * horizontalSpeed;

    }


    void FixedUpdate() {
        rb.velocity = speedVector*Time.deltaTime;	
	}
}

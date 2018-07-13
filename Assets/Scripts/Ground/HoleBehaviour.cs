using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleBehaviour : MonoBehaviour {

    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private float horizontalSpeed;
    private Vector2 speedVector;
    public BoxCollider2D groundCollider;
    public BoxCollider2D stunCollider;

    void Awake () {

        rb=GetComponent<Rigidbody2D>();
        speedVector = Vector2.right * horizontalSpeed;

    }


    void FixedUpdate() {
        rb.velocity = speedVector*Time.deltaTime;	
	}

    private void OnTriggerEnter2D (Collider2D other)
    {
        if (other.CompareTag("Stun_Area"))
        {
            stunCollider = other.GetComponent<BoxCollider2D>();
        }
        if (other.CompareTag("Ground"))
        {
            groundCollider = other.GetComponent<BoxCollider2D>();
        }
        if (other.CompareTag("Player"))
        {
            stunCollider.enabled = false;
            groundCollider.enabled = false;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Lateral_Border"))
        {
            HolePool.Instance.DisableHole(this.gameObject);
            groundCollider = null;
        }
        if (other.CompareTag("Player"))
        {
            groundCollider.enabled = true;
        }
    }
}

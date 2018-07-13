using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleBehaviour : MonoBehaviour {

    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private float horizontalSpeed;
    private Vector2 speedVector;
    public GameObject groundCollider;
    public GameObject stunCollider;
    public static bool collisionWithPlayer;
    public static bool onlyOneTime;
    public  bool collisionWithPlayer1;


    void Awake () {

        rb=GetComponent<Rigidbody2D>();
       
        collisionWithPlayer = true;
    }

    private void OnEnable()
    {
        speedVector = Vector2.right * horizontalSpeed;
    }


    private void Update()
    {
        collisionWithPlayer1 = collisionWithPlayer;
    }


    void FixedUpdate() {
        rb.velocity = speedVector*Time.deltaTime;	
	}

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            collisionWithPlayer = true;
            if (stunCollider != null && groundCollider != null)
            {
                stunCollider.layer = 14;
                groundCollider.layer = 14;
            }
        }

        if (collisionWithPlayer)
        {
            Debug.Log("veces");
            if (other.CompareTag("Stun_Area"))
            {
                stunCollider = other.gameObject;
            }
            if (other.CompareTag("Ground"))
            {
                groundCollider = other.gameObject;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Lateral_Border"))
        {
            HolePool.Instance.DisableHole(this.gameObject);
            groundCollider = null;
            stunCollider = null;

        }

        if (other.CompareTag("Player"))
        {
            stunCollider.layer = 8;
            groundCollider.layer = 8;
            if (stunCollider != null && groundCollider != null)
            {
                groundCollider = null;
                stunCollider = null;

            }
            collisionWithPlayer = false;
        }
    }

    #region Gets and Sets
        
    public float HorizontalSpeed
    {
        get
        {
            return horizontalSpeed;
        }
        set
        {
            horizontalSpeed = value;
        }
    }

    #endregion
}

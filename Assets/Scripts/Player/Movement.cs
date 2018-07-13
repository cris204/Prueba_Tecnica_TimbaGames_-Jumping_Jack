using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    #region Variables
    public float timeSpeed;
    [Header("General")]
    [SerializeField]
    private Rigidbody2D rb;


    [Header("Horizonatl Move")]
    private Vector3 speedVector;
    float h;
    [SerializeField]
    private float speed;
    private bool canHorizontalMove;

    [Header("Inputs")]
    [SerializeField]
    private string horizontalInput;

    [SerializeField]
    private string jumpInput;



    [Header("Jump")]
    RaycastHit2D hit;
    [SerializeField]
    private float jumpSpeed;
    [SerializeField]
    private float distance;
    private bool canJump;
    [SerializeField]
    private LayerMask layer;    

    #endregion

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        canHorizontalMove = true;
        canJump = true;
    }
    // Update is called once per frame
    void Update () {
        h = Input.GetAxis(horizontalInput);
        timeSpeed = Time.timeScale;

	}

    private void FixedUpdate()
    {
        hit = Physics2D.Raycast(transform.position, Vector2.down, distance,layer);
        Move();
        Jump();
    }

    void Move()
    {
        speedVector.x = speed * h * Time.deltaTime;
        speedVector.y = 0;
        if ( h != 0 && canHorizontalMove)
        {
            rb.velocity = speedVector;
        }
    }

    void Jump()
    {
        if (hit.collider != null)
        {
            canJump = true;
            canHorizontalMove = true;
            Time.timeScale = 1;
            Debug.Log(hit.transform.name);
        }
        else
        {
            canJump = false;
        }

        Debug.DrawRay(transform.position, Vector2.down * distance);

        if (Input.GetButtonDown(jumpInput) && canJump)
        {
            Time.timeScale = 0.5f;
            canJump = false;
            canHorizontalMove = false;
            speedVector.x = 0;
            speedVector.y = jumpSpeed * Time.deltaTime;
            rb.velocity = speedVector;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hole"))
        {
            speedVector.x = 0;
            speedVector.y = -jumpSpeed * Time.deltaTime;
            rb.velocity = speedVector;
        }
    }
    
    #region Coroutines
    IEnumerator WaitToJump()
    {
        yield return new WaitForSeconds(1f);
    }


    #endregion
}
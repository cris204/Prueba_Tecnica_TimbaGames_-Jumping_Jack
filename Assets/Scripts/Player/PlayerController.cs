using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    #region Variables
    public float timeSpeed;
    [Header("General")]
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private bool fallingDown;
    [SerializeField]
    private CollidersInteractions colliderInteractions;


    [Header("Horizonatl Move")]
    private Vector3 speedVector;
    float h;
    [SerializeField]
    private float speed;
    [SerializeField]
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
    [SerializeField]
    private bool canJump;
    [SerializeField]
    private bool isJumping;
    [SerializeField]
    private LayerMask layer;    

    #endregion

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        colliderInteractions = GetComponent<CollidersInteractions>();
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
        hit = Physics2D.Raycast(transform.position, Vector2.down, distance, layer);

        Move();
        Jump();
    }

    void Move()
    {

        if ( h != 0 && canHorizontalMove && !fallingDown)
        {
            speedVector.x = speed * h * Time.deltaTime;
            speedVector.y = 0;
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
            isJumping = false;
        }
        else
        {
            canJump = false;
            canHorizontalMove = false;
        }

        Debug.DrawRay(transform.position, Vector2.down * distance);

        if (Input.GetButtonDown(jumpInput) && canJump)
        {
            isJumping = true;
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
            if (!colliderInteractions.JumpToPlatform)
            {
                Time.timeScale = 0.5f;
                canHorizontalMove = false;
                canJump = false;

                if (!isJumping)
                {
                    fallingDown = true;
                    rb.velocity = Vector2.zero;
                }
                else
                {
                   GameManager.Instance.GetNewHole();
                }


            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Hole"))
        {
            fallingDown = false;
        }
    }

    #region Coroutines
    IEnumerator WaitToJump()
    {
        yield return new WaitForSeconds(1f);
    }


    #endregion
}
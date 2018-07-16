using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    #region Variables

    private static PlayerController instance;
    public static PlayerController Instance
    {
        get
        {
            return instance;
        }
    }

    public float timeSpeed;
    [Header("General")]
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private bool stuned;
    [SerializeField]
    private bool startGame;
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private SpriteRenderer playerSprite;

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



    [Header("Collision")]
    public bool isOnAir;
    private BoxCollider2D playerCollider;

    [Header("Borders")]
    private Vector2 destination;
    

    #endregion

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        playerSprite = GetComponent<SpriteRenderer>();
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        startGame = true;
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

        if ( h != 0 && canHorizontalMove && !isOnAir && !stuned)
        {

            speedVector.x = speed * h * Time.deltaTime;
            speedVector.y = 0;
            rb.velocity = speedVector;
            if (speedVector.x > 0)
            {
                playerSprite.flipX = false;
            }
            else
            {
                playerSprite.flipX = true;
            }
            anim.SetFloat("Xspeed", Mathf.Abs( speedVector.x));
        }
    }

    void Jump()
    {
        if (hit.collider != null)
        {
            canJump = true;
            canHorizontalMove = true;
            isJumping = false;
            anim.SetBool("jump", false);

        }
        else
        {
            canJump = false;
            canHorizontalMove = false;
        }

        Debug.DrawRay(transform.position, Vector2.down * distance);

        if (Input.GetButtonDown(jumpInput) && canJump && !stuned)
        {
            isJumping = true;
            anim.SetBool("jump", true);
            GameManager.Instance.ChangeTimeScale(0.5f);
            canJump = false;
            canHorizontalMove = false;
            speedVector.x = 0;
            speedVector.y = jumpSpeed * Time.deltaTime;
            rb.velocity = speedVector;
        }
    }

    void StunByFallDown()
    {
        GameManager.Instance.ChangeBGColorNormalStun(Color.white);
        stuned = true;
        rb.velocity = Vector3.zero;

        if (isOnAir)
        {

            anim.SetBool("fall_Down", true);
            StartCoroutine(StunedTime(1));
        }
        else
        {
            anim.SetBool("stuned", true);
            StartCoroutine(StunedTime(3));
        }


    }

    void StunByEnemy()
    {
        GameManager.Instance.ChangeBGColorStunByEnemy(Color.cyan);
        stuned = true;
        rb.velocity = Vector3.zero;

        if (isOnAir)
        {

            anim.SetBool("fall_Down", true);
            StartCoroutine(StunedTime(1));
        }
        else
        {
            anim.SetBool("stuned", true);
            StartCoroutine(StunedTime(3));
        }


    }

    void NormalStun()
    {
        GameManager.Instance.ChangeBGColorNormalStun(Color.white);
        stuned = true;
        rb.velocity = Vector3.zero;

        if (isOnAir)
        {

            anim.SetBool("fall_Down", true);
            StartCoroutine(StunedTime(1));
        }
        else
        {
            anim.SetBool("stuned", true);
            StartCoroutine(StunedTime(3));
        }


    }


    void SlowTime(bool slow)
    {
        if (slow)
        {
            GameManager.Instance.ChangeTimeScale(0.5f);
        }
        else
        {
            GameManager.Instance.ChangeTimeScale(1);
        }
    }

    #region OnTrigger

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Left_Border") || other.CompareTag("Right_Border"))
        {
            destination.x = other.transform.localPosition.x;
            destination.y = transform.position.y;
            transform.localPosition = destination;

        }

        if (other.CompareTag("Up_Border"))
        {
            GameManager.Instance.LevelUp();
            rb.velocity = Vector3.zero;
        }

        if (other.CompareTag("Hole"))
        {
            gameObject.layer = 11;
            if (!isOnAir)
            {
                canHorizontalMove = false;
                canJump = false;
                isOnAir = true;
                if (!isJumping)
                {
                    NormalStun();
                    
                    rb.velocity = Vector2.zero;
                }
                else
                {
                    GameManager.Instance.GetNewHole();
                    GameManager.Instance.ScoreUpdate(5);
                }
            }
            SlowTime(isOnAir);
        }

        if (other.CompareTag("Danger_Zone"))
        {
            if (!GameManager.Instance.FinishLevel && !startGame)
            {
                GameManager.Instance.LessLifes();    
            }
            startGame = false;
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            StunByEnemy();

        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Hole"))
        {
            gameObject.layer = 11;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Hole"))
        {
            playerCollider.isTrigger = false;
            gameObject.layer = 10;
            GameManager.Instance.ChangeTimeScale(1f);
        }
    }

    #endregion

    #region OnCollision
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Stun_Area"))
        {
            NormalStun();

        }

        if (other.gameObject.CompareTag("Ground"))
        {
            isOnAir = false;
            SlowTime(isOnAir);
        }
    }
    #endregion

    #region Coroutines
    IEnumerator StunedTime(float time)
    {
        yield return new WaitForSeconds(0.35f);
        anim.SetBool("fall_Down", false);
        anim.SetBool("stuned", true);
        yield return new WaitForSeconds(time);
        anim.SetBool("stuned", false);
        stuned = false;

    }


    #endregion
}
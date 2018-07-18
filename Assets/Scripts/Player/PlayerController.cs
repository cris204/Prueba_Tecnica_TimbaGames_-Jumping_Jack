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

    [Header("General")]
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private bool stuned;
    [SerializeField]
    private bool startLevel;
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
    [SerializeField]
    private bool pressJump;

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

    [Header("Sound")]
    [SerializeField]
    private AudioClip[] clipsPlayer;
    private AudioSource audioPlayer;


    [Header("Collision")]
    [SerializeField]
    private bool isOnAir;
    private BoxCollider2D playerCollider;

    [Header("Borders")]
    private Vector2 destination;
    

    #endregion

    void Awake()
    {
        audioPlayer = GetComponent<AudioSource>();
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

    void Start()
    {
        startLevel = true;
        canHorizontalMove = true;
        canJump = true;

    }

    void Update()
    {
        if (Input.GetButtonDown(jumpInput)){
            pressJump = true;
        }

        h = Input.GetAxis(horizontalInput);
        anim.SetFloat("Xspeed", Mathf.Abs(h));

    }

    private void FixedUpdate()
    {
        hit = Physics2D.Raycast(transform.position, Vector2.down, distance, layer);

        Move();
        Jump();
    }

    void Move()
    {

        if ( h != 0 && canHorizontalMove && !isOnAir && !stuned && !GameManager.Instance.FinishLevel)
        {

            speedVector.x = speed * h * Time.deltaTime;
            speedVector.y = 0;
            rb.velocity = speedVector;
            AssignAudio(0);
            if (speedVector.x > 0)
            {
                playerSprite.flipX = false;
            }
            else
            {
                playerSprite.flipX = true;
            }
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


        if (pressJump && canJump && !stuned && !GameManager.Instance.FinishLevel)
        {
            AssignAudio(1);
            isJumping = true;
            anim.SetBool("jump", true);
            GameManager.Instance.ChangeTimeScale(0.5f);
            canJump = false;
            canHorizontalMove = false;
            speedVector.x = 0;
            speedVector.y = jumpSpeed * Time.deltaTime;
            rb.velocity = speedVector;
        }
        pressJump = false;
    }

    void StunByFallDown()
    {
        stuned = true;
        rb.velocity = Vector3.zero;
        StopAllCoroutines();
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
        StopAllCoroutines();
        GameManager.Instance.ChangeBGColorStunByEnemy(Color.cyan);
        stuned = true;
        rb.velocity = Vector3.zero;

        anim.SetBool("stuned", true);
        StartCoroutine(StunedTime(0.5f));


    }

    void NormalStun()
    {
        StopAllCoroutines();
        GameManager.Instance.ChangeBGColorNormalStun(Color.white);
        stuned = true;
        rb.velocity = Vector3.zero;
        anim.SetBool("stuned", true);
        StartCoroutine(StunedTime(3));


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

    public void RestartAnimations()
    {
        gameObject.layer = 10;
        speedVector = Vector3.zero;
        stuned = false;
        StopCoroutine("StunedTime");
        anim.SetBool("jump", false);
        anim.SetBool("fall_Down", false);
        anim.SetBool("stuned", false);
        anim.SetFloat("Xspeed", Mathf.Abs(speedVector.x));

    }

    public void AssignAudio(int index,bool loop=false)
    {
        audioPlayer.clip=clipsPlayer[index];
        if (!audioPlayer.isPlaying)
        {
            audioPlayer.loop = loop;
            audioPlayer.Play();
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
            stuned = false;
            
            AssignAudio(6);
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
                    StunByFallDown();
                    AssignAudio(4, false);
                    rb.velocity = Vector2.zero;
                }
                else
                {
                    GameManager.Instance.GetNewHole();
                    GameManager.Instance.ScoreUpdate();
                }
            }
            SlowTime(isOnAir);
        }

        if (other.CompareTag("Danger_Zone"))
        {
            if (!GameManager.Instance.FinishLevel && !startLevel)
            {
                GameManager.Instance.LessLifes();    
            }
            if (GameManager.Instance.FinishGame)
            {
                StopAllCoroutines();
                audioPlayer.Stop();
                PlayerController.Instance.AssignAudio(5);
            }
            startLevel = false;
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            StunByEnemy();
            AssignAudio(2);
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
        if (!GameManager.Instance.FinishLevel)
        {
            yield return new WaitForSeconds(0.35f);
            AssignAudio(3, true);
            anim.SetBool("fall_Down", false);
            anim.SetBool("stuned", true);
            yield return new WaitForSeconds(time);
            anim.SetBool("stuned", false);
            audioPlayer.Stop();
            yield return new WaitForSeconds(0.15f);
            stuned = false;
        }
        else
        {
            anim.SetBool("stuned", false);
            stuned = false;
            yield return null;
        }
    }

    #endregion

    #region Get and Set

    public bool Stuned
    {
        get
        {
            return stuned;
        }

        set
        {
            stuned = value;
        }
    }

    public bool StartLevel
    {
        get
        {
            return startLevel;
        }

        set
        {
            startLevel = value;
        }
    }


    #endregion
}
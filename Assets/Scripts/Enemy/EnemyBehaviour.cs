using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {

    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private float horizontalSpeed;
    private Vector2 speedVector;
    //  public static bool collisionWithPlayer;
    public int floor;


    void Awake()
    {

        rb = GetComponent<Rigidbody2D>();

        //  collisionWithPlayer = true;
    }

    private void Start()
    {
       // floor = GameManager.Instance.FloorEnemy;
    }
    private void OnEnable()
    {
        speedVector = Vector2.right * horizontalSpeed;
    }

    void FixedUpdate()
    {
        if (!GameManager.Instance.StunedPlayer)
        {
            rb.velocity = speedVector * Time.deltaTime;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (horizontalSpeed > 0)
        {
            if (other.CompareTag("Right_Border"))
            {
                GameManager.Instance.GetEnemy(floor);
            }
        }
        else
        {
            if (other.CompareTag("Left_Border"))
            {
                GameManager.Instance.GetEnemy(floor);
            }
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (horizontalSpeed > 0)
        {
            if (other.CompareTag("Right_Border"))
            {
                EnemyPool.Instance.DisableEnemy(this.gameObject);

            }
        }
        else
        {
            if (other.CompareTag("Left_Border"))
            {
                EnemyPool.Instance.DisableEnemy(this.gameObject);

            }
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

    public int Floor
    {
        get
        {
            return floor;
        }

        set
        {
            floor = value;
        }
    }

    #endregion
}

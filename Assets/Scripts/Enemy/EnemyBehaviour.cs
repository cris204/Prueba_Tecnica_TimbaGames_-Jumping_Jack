using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {

    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private float horizontalSpeed;
    private Vector2 speedVector;
    private int floor;
    public int id;


    void Awake()
    {

        rb = GetComponent<Rigidbody2D>();

    }
    private void OnEnable()
    {
        speedVector = Vector2.right * HorizontalSpeed;
    }

    private void Update()
    {
        if (GameManager.Instance.FinishGame)
        {
            EnemyPool.Instance.DisableEnemy(this.gameObject);
        }
    }

    void FixedUpdate()
    {
        if (!GameManager.Instance.SlowTime)
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
       /* if (GameManager.Instance.FinishLevel)
        {
            EnemyPool.Instance.DisableEnemy(this.gameObject);
        }*/

        if (other.CompareTag("Left_Border"))
        {
            GameManager.Instance.GetEnemy(Floor, id);
        }

    }


    private void OnTriggerExit2D(Collider2D other)
    {

        if (other.CompareTag("Left_Border"))
        {
            EnemyPool.Instance.DisableEnemy(this.gameObject);

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

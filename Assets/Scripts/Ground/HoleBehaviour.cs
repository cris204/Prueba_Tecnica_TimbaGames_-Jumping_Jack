using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleBehaviour : MonoBehaviour {

    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private float horizontalSpeed;
    private Vector2 speedVector;
    private int floor;
    [SerializeField]
    private bool initialHole;
    [SerializeField]
    private bool onHole;

    void Awake () {

        rb=GetComponent<Rigidbody2D>();
    }


    private void OnEnable()
    {
        speedVector = Vector2.right * horizontalSpeed;
    }

    private void Update()
    {
        if (GameManager.Instance.FinishLevel)
        {
            HolePool.Instance.DisableHole(this.gameObject);
        }
    }

    void FixedUpdate() {

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
        if (GameManager.Instance.FinishLevel)
        {
            HolePool.Instance.DisableHole(this.gameObject);
        }

        if (horizontalSpeed > 0)
        {
            if (other.CompareTag("Right_Border"))
            {
                GameManager.Instance.GetHole(horizontalSpeed,floor);
            }
        }
        else
        {
            if (other.CompareTag("Left_Border"))
            {
                GameManager.Instance.GetHole(horizontalSpeed, floor);
            }
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {


        if (horizontalSpeed > 0)
        {
            if (other.CompareTag("Right_Border"))
            {
                HolePool.Instance.DisableHole(this.gameObject);

            }
        }
        else
        {
            if (other.CompareTag("Left_Border"))
            {
                HolePool.Instance.DisableHole(this.gameObject);

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

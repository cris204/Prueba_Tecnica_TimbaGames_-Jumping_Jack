using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidersInteractions : MonoBehaviour
{

    [Header("General")]
    private BoxCollider2D playerCollider;
    public bool jumpingToPlatform;
    [Header("Borders")]
    private Vector2 destination;
    [SerializeField]
    private bool isOnAir;


    private void Awake()
    {
        playerCollider = GetComponent<BoxCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Lateral_Border"))
        {
            destination.x = other.transform.localPosition.x;
            destination.y = transform.position.y;
            transform.localPosition = destination;

        }
        if (other.CompareTag("Hole"))
        {

            jumpingToPlatform = true;

        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Stun_Area"))
        {
            Debug.Log("Stun");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Hole"))
        {
            jumpingToPlatform = false;
            playerCollider.isTrigger = false;
        }

    }
    #region GetsAndSets
    [SerializeField]
    public bool JumpToPlatform
    {
        get
        {
            return jumpingToPlatform;
        }
        set
        {
            jumpingToPlatform = value;
        }
    }



    #endregion
}
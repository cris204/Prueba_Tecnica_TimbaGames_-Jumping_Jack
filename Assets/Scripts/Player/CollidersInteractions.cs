using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidersInteractions : MonoBehaviour
{

    [Header("General")]
    private BoxCollider2D playerCollider;
    public bool jumpingToPlatform;
    public bool fallDownPlatform;

    [Header("Borders")]
    private Vector2 destination;


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

        if (other.CompareTag("Hole_Up")){
            playerCollider.isTrigger = true;
            fallDownPlatform = true;
        }

        if (other.CompareTag("Hole_Down"))
        {

                playerCollider.isTrigger = true;
                jumpingToPlatform = true;

            //StartCoroutine(WaitToActiveCollider());
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Hole_Up")){
            playerCollider.isTrigger = false;
        }

        if (other.CompareTag("Hole_Down"))
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

    public bool FallDownPlatform
    {
        get
        {
            return fallDownPlatform;
        }
        set
        {
            fallDownPlatform = value;
        }
    }

    #endregion
}
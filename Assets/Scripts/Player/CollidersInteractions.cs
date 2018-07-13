using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidersInteractions : MonoBehaviour
{

    [Header("General")]
    public BoxCollider2D playerCollider;

    [Header("Borders")]
    private Vector2 destination;

    private void Awake()
    {
        playerCollider = GetComponent<BoxCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("LateralBorder"))
        {
            destination.x = other.transform.localPosition.x;
            destination.y = transform.position.y;
            transform.localPosition = destination;

        }

        if (other.CompareTag("Hole"))
        {
            playerCollider.isTrigger = true;
            //StartCoroutine(WaitToActiveCollider());
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Hole"))
        {
            playerCollider.isTrigger = false;
        }
    }
    #region corroutine

    IEnumerator WaitToActiveCollider()
    {
        yield return new WaitForSeconds(0.1f);
        playerCollider.enabled = true;
    }

    #endregion
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidersInteractions : MonoBehaviour {

    private Vector2 destination;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("LateralBorder"))
        {
            destination.x = other.transform.localPosition.x;
            destination.y = transform.position.y;
            transform.localPosition = destination;
            
        }

    }
}

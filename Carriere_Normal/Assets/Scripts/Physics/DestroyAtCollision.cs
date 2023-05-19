using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAtCollision : MonoBehaviour
{
///////////////////////// PARAMETERS ///////////////////////////////////

    public string tagTarget;                               // The object will be detroy if it collides an object with this tag

///////////////////////// START FUNCTION ///////////////////////////////////

    void Start(){}

///////////////////////// UPDATE FUNCTION ///////////////////////////////////

    void Update(){}

////////////////////////////////////////////////////////////

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag(tagTarget)){                   // If the object we collided has the good tag
            Destroy(gameObject);                            // We destroy the object
        }
    }
}

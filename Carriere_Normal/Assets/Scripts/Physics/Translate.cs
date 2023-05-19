using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translate : MonoBehaviour
{
    public float speed;
    public Vector3 direction;

    void Update(){
        transform.Translate(direction * Time.deltaTime * speed);
    }
}

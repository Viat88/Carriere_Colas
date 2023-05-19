using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport_player : MonoBehaviour
{

    private GameObject player;

///////////////////////// START FUNCTIONS ///////////////////////////////////

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

////////////////////////////////////////////////////////////

    /*
        Teleport the player to this platform when clicked
    */
    public void OnMouseDown(){
        player.transform.position = transform.position;
    }

}

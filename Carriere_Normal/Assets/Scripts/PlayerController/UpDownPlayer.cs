using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDownPlayer : MonoBehaviour
{
    
    private float down_height;
    public float up_height;
    private bool isUp = false;


///////////////////////// START FUNCTIONS ///////////////////////////////////

    void Start()
    {
        down_height = transform.localPosition.y;
    }

////////////////////////////////////////////////////////////

    /*
        Up or Down player if space bar is pressed
    */
    void OnGUI()
    {
        if (Event.current.Equals(Event.KeyboardEvent("Space")))
        {
            Vector3 playerPos = transform.localPosition;

            if (isUp){
                playerPos.y = down_height;
            }
            else{
                playerPos.y = up_height;
            }

            isUp = !isUp;

            transform.localPosition = playerPos;
        }
    }

}
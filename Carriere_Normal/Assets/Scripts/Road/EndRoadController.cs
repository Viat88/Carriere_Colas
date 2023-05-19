using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndRoadController : MonoBehaviour
{
///////////////////////// PARAMETERS ///////////////////////////////////
    

///////////////////////// START FUNCTION ///////////////////////////////////

    void Start(){
        RoadManager.current.OnNewCurrentIndex += ChangeEndRoadPosition;
    }

///////////////////////// UPDATE FUNCTION ///////////////////////////////////

    void Update(){}

////////////////////////////////////////////////////////////

    /*
        Change the Position and Rotation of the End Road object
    */
    private void ChangeEndRoadPosition(int index){

        transform.position = RoadManager.current.GetPoint(index);
        transform.rotation = RoadManager.current.GetCurrentPointRotation();
    }
}

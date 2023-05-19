using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class Finisseur_controler : MonoBehaviour
{
///////////////////////// PARAMETERS ///////////////////////////////////

    [Header ("PATH")]
    public EndOfPathInstruction end;            // Tells if we want to loop the path or not
    public PathCreator pathCreator;             // Object creating the path

    private float distanceTravelled;            // The distance travelled by the vehicule

    [Header ("TRANSLATION SPEED")]
    public float speed;                         // Vehicule speed

///////////////////////// START FUNCTION ///////////////////////////////////

    void Start(){
        RoadManager.current.OnNewCurrentIndex += ChangeCurrentPointRotation;
        ChangeCurrentPointRotation(0);
    }

///////////////////////// UPDATE FUNCTION ///////////////////////////////////

    void FixedUpdate()
    {
        MoveVehicule();                           // We move the vehicule
    }

////////////////////////////////////////////////////////////

    /*
        Move the vehicule along the current path
    */
    private void MoveVehicule(){

        distanceTravelled += speed*Time.deltaTime;                                              // We update the distance travelled

        transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, end);       // We move the vehicule 
        transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, end);    // We rotate the vehicule to follow path rotation

        RoadManager.current.distance_travelled = distanceTravelled;                             // We update distance travelled in RoadManager
    }

////////////////////////////////////////////////////////////

    /*
        Change the rotation of the current last point un RoadManager
        INPUT: 
        - index (int): the index of the current last point
    */
    private void ChangeCurrentPointRotation(int index){

        float distance = RoadManager.current.GetCurrentIndex()*RoadManager.current.GetDistanceBetweenPoints();      // We get the distance in the path of the current last point
        RoadManager.current.SetCurrentPointRotation(pathCreator.path.GetRotationAtDistance(distance, end));         // We change the rotation of the current last point
    }



}

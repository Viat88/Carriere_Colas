using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class Follow_path : MonoBehaviour
{

///////////////////////// PARAMETERS ///////////////////////////////////

    [Header ("PATH")]
    public EndOfPathInstruction end;            // Tells if we want to loop the path or not
    public List<PathCreator> pathCreatorList;   // List of objects creating the path
    private PathCreator currentPath;            // The current path we are following
    private int currentPathIndex = 0;           // The index of currentPath in pathCreatorList

    public List<float> timeToWaitAfterPathList; // List of the time the vehicule has to wait after ending the currentpath
    private float timeWeWaited;                 // Allow us to know if we have wait enough or not
    
    public List<int> list_paths_reverse;        // The list of indexes of paths where vehicule has to do it in reverse 
    private bool in_reverse;                    // Tells if this path is in reverse

    private float distanceTravelled;            // The distance travelled by the vehicule
    private float path_length;                  // Length of the path

    [Header ("TRANSLATION SPEED")]
    public List<float> max_speed_list;          // List of vehicule maximum speed
    private float current_max_speed;            // Maximum speed of the vehicule
    private float current_speed = 0f;           // The current soeed of the vehicule
    public float rate_acc_speed = 0.1f;         // Part of the length dedicated for acceleration and deceleration

    [Header ("WHEELS")]
    public List<GameObject> allWheels;          // allWheels of the vehicule
    public List<GameObject> wheelsToTurn;       // Wheels to turn (to change direction) (not all wheels can turn on the vehicule)
    public float wheel_turn_coeff = 0.01f;      // Coefficient to increase rotation of wheels when vehicule turns
    private float last_rotation = 0f;           // Last rotation of turning whels (it avoids too high values that can pop sometimes)
    public float wheel_rotate_coeff = 1f;       // Coefficient to increase rotation when we rotate wheels

///////////////////////// START FUNCTION ///////////////////////////////////

    void Start(){
        currentPath = pathCreatorList[currentPathIndex];                        // We take the first path
        path_length = currentPath.path.length;                                  // We get its length
        current_max_speed = max_speed_list[currentPathIndex];                   // We get the first max speed
    }

///////////////////////// UPDATE FUNCTION ///////////////////////////////////

    void FixedUpdate()
    {
        current_speed = GetSpeed();                             // We update the vehicule speed
        TurnWheels();

        /* If the path isn't finished */
        if (!HasReachedThePathEnd()){
            MoveVehicule();                           // We move the vehicule
            RotateWheels();                           // And rotate its allWheels
        }

        /* Else, if the path is finished */
        else{

            /* If we haven't waited enough */
            if (HaveWeGotToWait()){
                timeWeWaited += Time.deltaTime;       // We update the time we've waited
            }

            /* Else, if we've waited enough */
            else{
                NextPath();                           // We go to the next path
            }
        }
    }

////////////////////////////////////////////////////////////

    /*
        Move the vehicule along the current path
    */
    private void MoveVehicule(){

        distanceTravelled += current_speed* Time.deltaTime;                                     // We update the distance travelled
        transform.position = currentPath.path.GetPointAtDistance(distanceTravelled, end);       // We move the vehicule 
        transform.rotation = currentPath.path.GetRotationAtDistance(distanceTravelled, end);    // We rotate the vehicule to follow path rotation
        RotateVehicule();
    }

////////////////////////////////////////////////////////////

    /* 
        Change direction of the vehicule if we go in reverse or go back to normal path
    */
    private void RotateVehicule(){

        if(in_reverse){
            transform.Rotate(0,180,0);
        }
    }

////////////////////////////////////////////////////////////

    /*
        We rotate its allWheels
    */
    void RotateWheels(){

        wheel_rotate_coeff = Mathf.Abs(wheel_rotate_coeff);

        if (in_reverse){
            wheel_rotate_coeff = -Mathf.Abs(wheel_rotate_coeff);   
        }

        foreach(GameObject wheel in allWheels){
            wheel.transform.Rotate(wheel_rotate_coeff*Time.deltaTime*current_speed*100,0,0);                      // We update the rotation of each wheel depending of the current speed
        }
    }

////////////////////////////////////////////////////////////

    /*
        We turn its wheels (to change vehicule direction)
    */
    void TurnWheels(){

        float y_rotation = GetWheelRotation();                                        // We get the rotation of the wheels

        foreach(GameObject wheel in wheelsToTurn){
            wheel.transform.localEulerAngles = new Vector3(wheel.transform.localEulerAngles.x, y_rotation, wheel.transform.localEulerAngles.z);                      // We update the rotation of each wheel depending of the current speed
        }

        last_rotation = y_rotation;                                                    // We save last rotation
    }   

////////////////////////////////////////////////////////////

    /*
        Depending on where we are on the path, we update the speed: acceleration -> stabilisation -> deceleration
    */
    private float GetSpeed(){
        
        float speed = current_max_speed;                                            // Initially speed is the max speed
        float acc_end_distance = rate_acc_speed * path_length;              // Distance after we have to be at max speed
        float dec_start_distance = (1-rate_acc_speed) * path_length;        // Distance before we have to be at max speed

        /* If we are in the acceleration part */
        if (distanceTravelled < acc_end_distance){
            speed = Mathf.Lerp(0.01f, current_max_speed, distanceTravelled/acc_end_distance);
        }

        /* If we are in the deceleration part */
        if(distanceTravelled > dec_start_distance){
            speed = Mathf.Lerp(current_max_speed, 0.01f, (distanceTravelled - dec_start_distance)/(path_length - dec_start_distance));
        }

        /* We return the speed */
        return speed;
    }

////////////////////////////////////////////////////////////

    /*
        Give the rotation of the wheel when the vehicule turn
    */
    private float GetWheelRotation(){
        return (GetNextRotation() - transform.localEulerAngles.y) * wheel_turn_coeff;
    }

////////////////////////////////////////////////////////////

    /*
        Give the rotation the vehicule will have at the next position
    */
    private float GetNextRotation(){
        return currentPath.path.GetRotationAtDistance(distanceTravelled + current_speed* Time.deltaTime, end).eulerAngles.y;    // We return the next rotation
    }


////////////////////////////////////////////////////////////

    /*
        Take the next path and reset all linked parameters 
    */
    private void NextPath(){

        currentPathIndex = (currentPathIndex + 1)% pathCreatorList.Count;      // We update the index (modulo to cyle the list)
        currentPath = pathCreatorList[currentPathIndex];                       // We update the current path
        path_length = currentPath.path.length;                                 // We get its length
        current_max_speed = max_speed_list[currentPathIndex];                  // We get the new max speed
        distanceTravelled = 0;                                                 // We reset the distance travelled on the path
        CheckIfInReverse();                                                    // We check if it has to be done in reverse
    }

////////////////////////////////////////////////////////////

    /*
        Have we got to wait again?
    */
    private bool HaveWeGotToWait(){

        if (currentPathIndex < timeToWaitAfterPathList.Count){
            return timeWeWaited < timeToWaitAfterPathList[currentPathIndex];       // False if we have waited enough 
        }

        else{
            return false;
        }
    }

////////////////////////////////////////////////////////////

    /*
        Has the vehicule reached the end of the path?
    */
    private bool HasReachedThePathEnd(){
        return distanceTravelled >= path_length;            // True if it has reached the end of the path
    }

////////////////////////////////////////////////////////////

    /*
        Tells if the current path has to be done in reverse
    */
    private void CheckIfInReverse(){

        in_reverse = false;                                 // By default, it hasn't to be done in reverse

        foreach (int i in list_paths_reverse){              // If its index appears in the list of reversed paths
            if (i == currentPathIndex){
                in_reverse = true;
                break;
            }
        }
    }
}

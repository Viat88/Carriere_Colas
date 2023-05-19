using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class WorkerPathController : MonoBehaviour
{
///////////////////////// PARAMETERS ///////////////////////////////////

    [Header ("PATH")]
    public EndOfPathInstruction end;            // Tells if we want to loop the path or not
    public List<PathCreator> pathCreatorList;   // List of objects creating the path
    private PathCreator currentPath;            // The current path we are following
    private int currentPathIndex = 0;           // The index of currentPath in pathCreatorList
    private float distanceTravelled;            // The distance travelled by the vehicule
    private float path_length;                  // Length of the path

    public bool canWalk = true;      
    public float speed = 1.0f;            

    [Header ("ANIMATION")]
    public Animator animator;   

    

///////////////////////// START FUNCTION ///////////////////////////////////

    void Start(){
        currentPath = pathCreatorList[currentPathIndex];                        // We take the first path
        path_length = currentPath.path.length;                                  // We get its length
    }

///////////////////////// UPDATE FUNCTION ///////////////////////////////////

    void FixedUpdate(){
        
        if (canWalk){
            MoveCharacter();
        }
    }

////////////////////////////////////////////////////////////

    private void CanWalk(bool b){
        canWalk = b;
        animator.SetBool("CanWalk", b);
    }

////////////////////////////////////////////////////////////

    private void StartWalking(){
        CanWalk(true);
    }

////////////////////////////////////////////////////////////

    /*
        Move the character along the current path
    */
    private void MoveCharacter(){

        distanceTravelled += speed* Time.deltaTime;                                             // We update the distance travelled
        transform.position = currentPath.path.GetPointAtDistance(distanceTravelled, end);       // We move the character 
        transform.rotation = currentPath.path.GetRotationAtDistance(distanceTravelled, end);    // We rotate the character to follow path rotation
        HasReachedThePathEnd();
    }

////////////////////////////////////////////////////////////
    
    /*
        Has the character reached the end of the path?
    */
    private void HasReachedThePathEnd(){
        CanWalk(!(distanceTravelled >= path_length));            // True if it has reached the end of the path
        if (distanceTravelled >= path_length){
            NextPath();
        }
    }

////////////////////////////////////////////////////////////

    /*
        Take the next path and reset all linked parameters 
    */
    private void NextPath(){

        currentPathIndex = (currentPathIndex + 1)% pathCreatorList.Count;      // We update the index (modulo to cyle the list)
        currentPath = pathCreatorList[currentPathIndex];                       // We update the current path
        path_length = currentPath.path.length;                                 // We get its length
        distanceTravelled = 0;                                                 // We reset the distance travelled on the path
    }


}

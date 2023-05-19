using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using System.IO;
using System.Globalization;

public class SavePathPoints : MonoBehaviour
{
///////////////////////// PARAMETERS ///////////////////////////////////

    [Header ("PATH")]
    public EndOfPathInstruction end;                              // Tells if we want to loop the path or not
    public PathCreator pathCreator;                               // Object creating the path
    public float path_high;                                       // High of the path

    [Header ("Points")]
    public int nbPoints;                                          // Nb points we will write 

///////////////////////// START FUNCTION ///////////////////////////////////

    void Awake(){}

    void Start(){
        RoadManager.current.distanceBetweenPoints = pathCreator.path.length /nbPoints;                  // We get distance between points
        GetPoints();                                                                                    // We modify the text file
    }

///////////////////////// UPDATE FUNCTION ///////////////////////////////////

    void Update(){}

////////////////////////////////////////////////////////////

    private void GetPoints(){

        for (int i=0; i<nbPoints; i++){
            RoadManager.current.points_list.Add(GetPoint(i));
        }
    }

////////////////////////////////////////////////////////////

    private Vector3 GetPoint(int index){

        Vector3 pointPosition = pathCreator.path.GetPointAtDistance(RoadManager.current.distanceBetweenPoints * index, end);       // We get path point position
        pointPosition.y = path_high;
        return pointPosition;
    }

}

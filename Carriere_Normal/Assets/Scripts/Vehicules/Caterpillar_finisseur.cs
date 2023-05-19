using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class Caterpillar_finisseur : MonoBehaviour
{
///////////////////////// PARAMETERS ///////////////////////////////////

    [Header ("PATH")]
    public EndOfPathInstruction end;                // Tells if we want to loop the path or not
    public PathCreator path;                        // Object creating the path

    [Header ("LINKS")]
    public List<GameObject> link_list;              // The index of the link inside caterpillar
    private List<float> links_distance_travelled;   // The list of all distance travelled by links of the caterpillar
    public float speed = 1;                         // Speed of links


///////////////////////// START FUNCTION ///////////////////////////////////

    void Start(){
        GetStartPosition();
    }

///////////////////////// UPDATE FUNCTION ///////////////////////////////////

    void FixedUpdate(){

        MoveAllLinks();
    }

////////////////////////////////////////////////////////////

    /*
        Give start position of all links (called in START)
    */
    private void GetStartPosition(){

        int nb_links = link_list.Count;                             // Number of links inside the caterpillar
        links_distance_travelled = new List<float>();       // Setting the list links_distance_travelled
        float distance_step = path.path.length / nb_links;         // Distance between each link

        for (int i=0; i<nb_links; i++){                              // We browse each link
            links_distance_travelled.Add(i * distance_step);        // We define the initial position depending on its index
        }
    }

////////////////////////////////////////////////////////////

    /*
        Move all links along the caterpillar path
    */
    private void MoveAllLinks(){

        for (int i=0; i<links_distance_travelled.Count; i++){
            MoveLink(i);
        }
    }

////////////////////////////////////////////////////////////
    /*
        Move the link along the caterpillar path
        Input: 
            - index (int): the index of the link
    */
    private void MoveLink(int index){
        
        GameObject link = link_list[index];                                                  // The link we have to move
        float distanceTravelled = links_distance_travelled[index] + speed*Time.deltaTime;  // We get the distance travelled
        links_distance_travelled[index] = distanceTravelled;                             // We update the list of distance travelled

        link.transform.position = path.path.GetPointAtDistance(distanceTravelled, end);       // We move the vehicule 
        link.transform.rotation = path.path.GetRotationAtDistance(distanceTravelled, end);    // We rotate the vehicule to follow path rotation
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repandeur : MonoBehaviour
{
///////////////////////// PARAMETERS ///////////////////////////////////

    public GameObject objectToSpawn;
    public List<Transform> spawnSources;
    public int nb_rock_per_frame = 1;


///////////////////////// START FUNCTION ///////////////////////////////////

    void Start(){}

///////////////////////// UPDATE FUNCTION ///////////////////////////////////

    void FixedUpdate()
    {
      SpawnRocks();  
    }

////////////////////////////////////////////////////////////

    private void SpawnRocks(){

        foreach (Transform source in spawnSources){
            for (int i=0; i<nb_rock_per_frame; i++){
                Instantiate (objectToSpawn, source);
            }
        }
    }
}

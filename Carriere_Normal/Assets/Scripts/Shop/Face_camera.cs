using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face_camera : MonoBehaviour
{
    private Camera main_camera;

///////////////////////// START FUNCTIONS ///////////////////////////////////
    void Start()
    {
        main_camera = Camera.main;
    }

    void Update()
    {
        turnObjectTowardCamera();
    }

////////////////////////////////////////////////////////////

    private void turnObjectTowardCamera(){
        transform.rotation = Quaternion.LookRotation(getRelativePos());
    }

    private Vector3 getRelativePos(){
        float x_target = main_camera.transform.position.x;
        float z_target = main_camera.transform.position.z;

        return new Vector3 (x_target - transform.position.x, 0, z_target - transform.position.z);
    }

}

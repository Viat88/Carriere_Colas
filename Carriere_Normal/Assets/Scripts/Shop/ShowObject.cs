using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowObject : MonoBehaviour
{

    public List<GameObject> objectToShow_Hide;

///////////////////////// START FUNCTIONS ///////////////////////////////////

    void Start(){}

    void Update(){}

////////////////////////////////////////////////////////////

    private void ShowOrHideObject(){

        foreach (GameObject obj in objectToShow_Hide){
            obj.SetActive(!obj.activeSelf);
        }
    }

////////////////////////////////////////////////////////////

    private void OnMouseDown(){
        ShowOrHideObject();
    }
}

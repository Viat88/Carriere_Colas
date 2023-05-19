using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RoadManager : MonoBehaviour
{
///////////////////////// PARAMETERS ///////////////////////////////////

    public static RoadManager current;

    [HideInInspector]
    public List<Vector3> points_list;                            // List of points

    [HideInInspector]
    public float distance_travelled;                             // Distance travelled by the finisseur

    [HideInInspector]
    public float distanceBetweenPoints;                          // The distance between two successive points

    [HideInInspector]
    public Quaternion current_point_rotation;                    // The rotation of the road at the current point (so the last of the road already created)

///////////////////////// Listeners ///////////////////////////////////    

    /*
        Listener for the index of the current point
    */
    public event Action<int> OnNewCurrentIndex;
    public void NewCurrentIndex(int n){
        OnNewCurrentIndex?.Invoke(n);
    }

    [HideInInspector]
    public int currentIndex = 0;

    [SerializeField]
    public int IsNewCurrentIndex{
        get => currentIndex;
        set
        {
            currentIndex = value;
            NewCurrentIndex(currentIndex); //Fire the event
        }
    }

///////////////////////// START FUNCTION ///////////////////////////////////

    void Awake(){

        if (current == null)
        {
            current = this;
        }
        else
        {
            Destroy(obj: this);
        }

        points_list = new List<Vector3>();
    }

    void Start(){
        
    }

///////////////////////// UPDATE FUNCTION ///////////////////////////////////

    void Update(){
        ChangeCurrentIndex();
    }

////////////////////////////////////////////////////////////

    public int GetCurrentIndex(){
        return currentIndex;
    }

////////////////////////////////////////////////////////////

    public Vector3 GetPoint(int index){
        return points_list[index];
    }
    
////////////////////////////////////////////////////////////

    public Quaternion GetCurrentPointRotation(){
        return current_point_rotation;
    }

////////////////////////////////////////////////////////////

    public void SetCurrentPointRotation(Quaternion newRotation){
        current_point_rotation = newRotation;
    }

////////////////////////////////////////////////////////////

    public float GetDistanceBetweenPoints(){
        return distanceBetweenPoints;
    }


////////////////////////////////////////////////////////////

    /*
        Change the index of the current last point
    */
    private void ChangeCurrentIndex(){

        int factor = (int) (distance_travelled / distanceBetweenPoints);            // We calculate how much point we have already travelled

        // If the current index isn't righ, we change it
        if (currentIndex < factor){
            IsNewCurrentIndex = factor;
        }
    }
}

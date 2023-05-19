using UnityEngine;
using System.Collections;
using System.Globalization;

public class PathManager : MonoBehaviour {

///////////////////////// PARAMETERS ///////////////////////////////////

	public CarPath path;

	public GameObject prefab;

	public bool doLoadPointPath = false;

	public bool doBuildRoad = false;

	public bool doChangeLanes = false;

	public int smoothPathIter = 0;

	public bool doShowPath = false;

    public string pathToLoad = "none";

	public RoadBuilder roadBuilder;
	public RoadBuilder semanticSegRoadBuilder;

	public LaneChangeTrainer laneChTrainer;

///////////////////////// START FUNCTION ///////////////////////////////////

	void Awake(){}

	void Start(){
		RoadManager.current.OnNewCurrentIndex += InitNewRoad;
	}

///////////////////////// UPDATE FUNCTION ///////////////////////////////////

	void Update(){}

////////////////////////////////////////////////////////////

	public void InitNewRoad(int index)
	{
		
		if(doLoadPointPath)
		{
			MakePointPath(index);
		}

		if(smoothPathIter > 0)
			SmoothPath();

		//Should we build a road mesh along the path?
		if(doBuildRoad && roadBuilder != null)
			roadBuilder.InitRoad(path); 

		if(doBuildRoad && semanticSegRoadBuilder != null)
			semanticSegRoadBuilder.InitRoad(path);

		if(doChangeLanes && laneChTrainer != null)
			laneChTrainer.ModifyPath(ref path);

		if(doShowPath && path != null)
		{
			for(int iN = 0; iN < path.nodes.Count; iN++)
			{
				Vector3 np = path.nodes[iN].pos;
				GameObject go = Instantiate(prefab, np, Quaternion.identity) as GameObject;
				go.tag = "pathNode";
				go.transform.parent = this.transform;
			}
		}
	}

	public void DestroyRoad()
	{
		GameObject[] prev = GameObject.FindGameObjectsWithTag("pathNode");

		foreach(GameObject g in prev)
			Destroy(g);

		if(roadBuilder != null)
			roadBuilder.DestroyRoad();
	}

	void SmoothPath()
	{
		while(smoothPathIter > 0)
		{
			path.SmoothPath();
			smoothPathIter--;
		}
	}

	void MakePointPath(int index)
	{
		path = new CarPath();

		Vector3 np = Vector3.zero;

		for (int i=0; i<=index; i++){

			Vector3 point = RoadManager.current.GetPoint(i);

			np.x = point[0];
			np.y = point[1];
			np.z = point[2];
			PathNode p = new PathNode();
			p.pos = np;
			path.nodes.Add(p);
			path.centerNodes.Add(p);

		}			
	}

	public bool SegmentCrossesPath(Vector3 posA, float rad)
	{
		foreach(PathNode pn in path.nodes)
		{
			float d = (posA - pn.pos).magnitude;

			if(d < rad)
				return true;
		}

		return false;
	}

	public void SetPath(CarPath p)
	{
		path = p;

		GameObject[] prev = GameObject.FindGameObjectsWithTag("pathNode");

		Debug.Log(string.Format("Cleaning up {0} old nodes. {1} new ones.", prev.Length, p.nodes.Count));

		DestroyRoad();

		foreach(PathNode pn in path.nodes)
		{
			GameObject go = Instantiate(prefab, pn.pos, Quaternion.identity) as GameObject;
			go.tag = "pathNode";
		}
	}
}

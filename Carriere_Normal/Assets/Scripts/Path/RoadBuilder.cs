using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadBuilder : MonoBehaviour {

///////////////////////// PARAMETERS ///////////////////////////////////

	public float roadWidth = 1.0f;											// The width of the road
	public float roadHeightOffset = 0.0f;									// Offset of the road on y axis
	public float roadOffsetW = 0.0f;										// Offset of the road on weigth

	public GameObject roadPrefabMesh;										// Prefab of the road

	Texture2D customRoadTexure;
	GameObject createdRoad;

///////////////////////// START FUNCTION ///////////////////////////////////

	void Start()
	{}

////////////////////////////////////////////////////////////

	public void DestroyRoad()
	{
		GameObject[] prev = GameObject.FindGameObjectsWithTag("road_mesh");

		foreach(GameObject g in prev)
			Destroy(g);

	}


////////////////////////////////////////////////////////////

	public void InitRoad(CarPath path)
	{
		DestroyRoad();

        if(path == null)
        {
            Debug.LogWarning("No path in RoadBuilder::InitRoad");
            return;
        }
		
		GameObject go = GameObject.Instantiate(roadPrefabMesh);
		MeshRenderer mr = go.GetComponent<MeshRenderer>();
		MeshFilter mf = go.GetComponent<MeshFilter>();
		Mesh mesh = new Mesh();
		mf.mesh = mesh;
		createdRoad = go;

		if(customRoadTexure != null)
		{
			mr.material.mainTexture = customRoadTexure;
		}
		

		go.tag = "road_mesh";

		int numQuads = path.nodes.Count - 1;
		int numVerts = (numQuads + 1) * 2;
		int numTris = numQuads * 2;

		Vector3[] vertices = new Vector3[numVerts];

		int numTriIndecies = numTris * 3;
		int[] tri = new int[numTriIndecies];

		int numNormals = numVerts;
		Vector3[] normals = new Vector3[numNormals];

		int numUvs = numVerts;
		Vector2[] uv = new Vector2[numUvs];

		for(int iN = 0; iN < numNormals; iN++)
			normals[iN] = Vector3.up;

		int iNode = 0;

		Vector3 posA = Vector3.zero;
		Vector3 posB = Vector3.zero;

		Vector3 vLength = Vector3.one;
		Vector3 vWidth = Vector3.one;

		for(int iVert = 0; iVert < numVerts; iVert += 2)
		{
			if(iNode + 1 < path.nodes.Count)
			{
				PathNode nodeA = path.nodes[iNode];
				PathNode nodeB = path.nodes[iNode + 1];
				posA = nodeA.pos;
				posB = nodeB.pos;

				vLength = posB - posA;
				vWidth = Vector3.Cross(vLength, Vector3.up);

				posA.y += roadHeightOffset;
			}
			else
			{
				PathNode nodeA = path.nodes[iNode];
				posA = nodeA.pos;
				posA.y += roadHeightOffset;
			}

			Vector3 leftPos = posA + vWidth.normalized * roadWidth + vWidth.normalized * roadOffsetW;
			Vector3 rightPos = posA - vWidth.normalized * roadWidth + vWidth.normalized * roadOffsetW;

			PathNode centerNode = new PathNode();
			centerNode.pos = (leftPos + rightPos)/2;
			path.centerNodes.Add(centerNode);

			vertices[iVert] = leftPos;
			vertices[iVert + 1] = rightPos;

			uv[iVert] = new Vector2(0.2f * iNode, 0.0f);
			uv[iVert + 1] = new Vector2(0.2f * iNode, 1.0f);

			iNode++;
		}

		int iVertOffset = 0;
		int iTriOffset = 0;

		for(int iQuad = 0; iQuad < numQuads; iQuad++)
		{
			tri[0 + iTriOffset] = 0 + iVertOffset;
			tri[1 + iTriOffset] = 2 + iVertOffset;
			tri[2 + iTriOffset] = 1 + iVertOffset;

			tri[3 + iTriOffset] = 2 + iVertOffset;
			tri[4 + iTriOffset] = 3 + iVertOffset;
			tri[5 + iTriOffset] = 1 + iVertOffset;

			iVertOffset += 2;
			iTriOffset += 6;
		}


		mesh.vertices = vertices;
		mesh.triangles = tri;
		mesh.normals = normals;
		mesh.uv = uv;

		mesh.RecalculateBounds();
		// todo: return CarPath
	}
}

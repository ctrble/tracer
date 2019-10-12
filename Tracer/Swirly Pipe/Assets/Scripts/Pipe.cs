using UnityEngine;
using System.Collections;

public class Pipe : MonoBehaviour {

	//defining pipe curves and angles
	private float curveAngle;

	public float minCurveRadius;
	public float maxCurveRadius;
	public int minCurveSegmentCount;
	public int maxCurveSegmentCount;

	private float curveRadius;
	private int curveSegmentCount;

	public float pipeRadius;
	public int pipeSegmentCount;

	public float ringDistance;

	private float relativeRotation;

	public PipeItemGenerator[] generators;

	//lets make a mesh
	private Mesh mesh;
	private Vector3[] vertices;
	private int[] triangles;
	private Vector2[] uv;

	private void Awake () {

		GetComponent<MeshFilter>().mesh = mesh = new Mesh();
		mesh.name = "Pipe";
	}

	public void Generate (bool withItems = true) {
	
		curveRadius = Random.Range (minCurveRadius, maxCurveRadius);
		curveSegmentCount = Random.Range (minCurveSegmentCount, maxCurveSegmentCount + 1);
		mesh.Clear ();
		SetVertices ();
		SetUV ();
		SetTriangles ();
		mesh.RecalculateNormals ();
		for (int i = 0; i < transform.childCount; i++) {
			Destroy (transform.GetChild (i).gameObject);
		}
		if (withItems) {
			generators [Random.Range (0, generators.Length)].GenerateItems (this);
		}
	}

	private void SetVertices () {
		//find 4 vertices per quad
		//create the first quad ring
		//continue making the rest of the quads
	
		vertices = new Vector3[pipeSegmentCount * curveSegmentCount * 4]; //each quad gets it's own 4 vertices

		float uStep = ringDistance / curveRadius;
		curveAngle = uStep * curveSegmentCount * (360f / (2f * Mathf.PI));
		CreateFirstQuadRing (uStep);
		int iDelta = pipeSegmentCount * 4;
		for (int u = 2, i = iDelta; u <= curveSegmentCount; u++, i += iDelta) {
			CreateQuadRing (u * uStep, i);
		}
		mesh.vertices = vertices;
	}

	private void SetUV () {
	
		uv = new Vector2[vertices.Length];
		for (int i = 0; i < vertices.Length; i += 4) {
			uv [i] = Vector2.zero;
			uv [i + 1] = Vector2.right;
			uv [i + 2] = Vector2.up;
			uv [i + 3] = Vector2.one;
		}
		mesh.uv = uv;
	}

	private void CreateFirstQuadRing (float u) {
		//makes the first set of quads
	
		float vStep = (2f * Mathf.PI) / pipeSegmentCount;

		Vector3 vertexA = GetPointOnTorus (0f, 0f);
		Vector3 vertexB = GetPointOnTorus (u, 0f);
		for (int v = 1, i = 0; v <= pipeSegmentCount; v++, i +=4) {
			
			//assign vertices to quads
			vertices [i] = vertexA;
			vertices [i + 1] = vertexA = GetPointOnTorus (0f, v * vStep);
			vertices [i + 2] = vertexB;
			vertices [i + 3] = vertexB = GetPointOnTorus (u, v * vStep);
		}
	}

	private void CreateQuadRing (float u, int i) {
		//continues along the ring making more quads
	
		float vStep = (2f * Mathf.PI) / pipeSegmentCount;
		int ringOffset = pipeSegmentCount * 4;

		Vector3 vertex = GetPointOnTorus (u, 0f);
		for (int v = 1; v <= pipeSegmentCount; v++, i +=4) {
			
			//find the other two remaining vertices, able to remember the sides from the previous quad
			vertices [i] = vertices [i - ringOffset + 2];
			vertices [i + 1] = vertices [i - ringOffset + 3];
			vertices [i + 2] = vertex;
			vertices [i + 3] = vertex = GetPointOnTorus (u, v * vStep);
		}
	}

	private void SetTriangles () {
		//put two triangles on each quad

		triangles = new int[pipeSegmentCount * curveSegmentCount * 6];
		for (int t = 0, i = 0; t < triangles.Length; t += 6, i += 4) {
			
			//assign 2 triangles to each quad
			triangles [t] = i;
			triangles [t + 1] = triangles [t + 4] = i + 2;
			triangles [t + 2] = triangles [t + 3] = i + 1;
			triangles [t + 5] = i + 3;
		}
		mesh.triangles = triangles;
	}

	private Vector3 GetPointOnTorus (float u, float v) {
		//places vertices along the surface of the torus
		//u is the angle along the curve
		//v is the angle along the pipe
	
		Vector3 p;
		float r = (curveRadius + pipeRadius * Mathf.Cos (v));
		p.x = r * Mathf.Sin (u);
		p.y = r * Mathf.Cos (u);
		p.z = pipeRadius * Mathf.Sin (v);
		return p;
	}

	public void AlignWith (Pipe pipe) {
		//match up with the next pipe

		relativeRotation = Random.Range (0, curveSegmentCount) * 360F / pipeSegmentCount;

		transform.SetParent (pipe.transform, false);
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.Euler (0f, 0f, -pipe.curveAngle);
		transform.Translate (0f, pipe.curveRadius, 0f);
		transform.Rotate (relativeRotation, 0f, 0f);
		transform.Translate (0f, -curveRadius, 0f);
		transform.SetParent (pipe.transform.parent);
		transform.localScale = Vector3.one;
	}

	public float CurveRadius {
		//curve radius is private and shouldn't be changed, but now we can look at it

		get {
			return curveRadius;
		}
	}

	public float CurveAngle {
		//curve angle is private and shouldn't be changed, but now we can look at it

		get {
			return curveAngle;
		}
	}

	public float RelativeRotation {
		//relative rotation is private and shouldn't be changed, but now we can look at it

		get {
			return relativeRotation;
		}
	}

	public int CurveSegmentCount {
		//curve segment is private and shouldn't be changed, but now we can look at it

		get { 
			return curveSegmentCount;
		}
	}
}

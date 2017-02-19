using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fractal : MonoBehaviour {
	public int maxDepth;
	public Mesh mesh;
	public Material material;
	public float childScale;
	int depth = 0;

	static Vector3[] childDirections = {
		Vector3.up,
		Vector3.right,
		Vector3.left,
		Vector3.forward,
		Vector3.back
	};

	static Quaternion[] childOrientations = {
		Quaternion.identity,
		Quaternion.Euler (0f, 0f, -90f),
		Quaternion.Euler (0f, 0f, 90f),
		Quaternion.Euler (90f,0f,0f),
		Quaternion.Euler (-90f,0f,0f)
	};

	Material[] materials;

	private void InitializeMaterials () {
		materials = new Material[maxDepth + 1];
		for (int i = 0; i <= maxDepth; i++) {
			materials[i] = new Material(material);
			materials[i].color =
				Color.Lerp(Color.white, Color.yellow, (float)i/ maxDepth);
		}
	}


		
	// Use this for initialization
	void Start () {
		if (materials == null) {
			InitializeMaterials();
		}
		gameObject.AddComponent<MeshFilter> ().mesh = mesh;
		gameObject.AddComponent<MeshRenderer> ().material = materials[depth];
		GetComponent<MeshRenderer> ().material.color = Color.Lerp (Color.blue, Color.red, (float)depth / maxDepth);
		if (depth < maxDepth) {
			StartCoroutine ("CreateChildren");
		}

	}
	void Initialize(Fractal parent, Vector3 direction, Quaternion orientation){
		mesh = parent.mesh;
		materials = parent.materials;
		maxDepth = parent.maxDepth;
		depth = parent.depth + 1;
		childScale = parent.childScale;
		transform.parent = parent.transform;
		transform.localScale = Vector3.one * childScale;
		transform.localPosition = direction * (0.5f + 0.5f * childScale);
		transform.localRotation = orientation;
	}

	private IEnumerator CreateChildren () {
		for (int i = 0 ; i < childDirections.Length; i++) {
			yield return new WaitForSeconds (Random.Range(0.1f,0.5f));
			new GameObject ("Fractal Child").
			AddComponent<Fractal> ().Initialize (this, childDirections [i], childOrientations [i]);
		}
	}
}

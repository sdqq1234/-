using UnityEngine;
using System.Collections;

public class RotationTool : MonoBehaviour {

    public float rotationSpeed = 1;
    public bool isRotationX = false;
    public bool isRotationY = false;
    public bool isRotationZ = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (isRotationX) {
            gameObject.transform.Rotate(Vector3.right,rotationSpeed);
        }
        if (isRotationY) {
            gameObject.transform.Rotate(Vector3.up, rotationSpeed);
        }
        if (isRotationZ) {
            gameObject.transform.Rotate(Vector3.forward, rotationSpeed);
        }
	}
}

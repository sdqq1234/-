#pragma strict
var speed:float=10.0;
function Start () {
	var object:GameObject=this.gameObject;

	Debug.Log("Start called on GameObject"+object.name);
	//iTween.RotateBy(gameObject, iTween.Hash("y", 1, "time", 2, "easeType", "easeInOutBack","loopType", "pingPong"));
}

function Update () {
	//Debug.Log("Update called at time "+Time.time);
	transform.Rotate(Vector3.up*speed*Time.deltaTime); 
}
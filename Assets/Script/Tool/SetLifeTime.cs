using UnityEngine;
using System.Collections;

public class SetLifeTime : MonoBehaviour {

    public float life = 1f;//生存时间默认为1秒
    private float curLife = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (curLife > life)
        {
            GameObject.Destroy(this.gameObject);
        }
        else {
            curLife += Time.deltaTime;
        }
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GrazeCenter : MonoBehaviour {

    public List<GameObject> GrazeBulletList = new List<GameObject>();//擦弹列表
	// Use this for initialization
	void Start () {
	
	}

    void OnTriggerEnter2D(Collider2D Collider)
    {
        //BulletBase_Touhou bullet = Collider.gameObject.GetComponent<BulletBase_Touhou>();
        //if (bullet != null && !bullet.Grazed && bullet.LayerName == CommandString.EnemyBulletLayer)
        //{
        //    bullet.Grazed = true;
        //    GrazeBulletList.Add(Collider.gameObject);
        //}
        
    }

	// Update is called once per frame
	void Update () {
	
	}
}

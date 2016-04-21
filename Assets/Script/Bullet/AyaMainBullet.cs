using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class AyaMainBullet : BulletBase_Touhou {

	// Use this for initialization
	void Start () {
        //base.Start();
        rigidbody2D.velocity = speed;
	}

	// Update is called once per frame
	void Update () {
        base.Update();
	}

    //void OnTriggerEnter2D(Collider2D otherCollider)
    //{
    //    Debug.Log(otherCollider.name);
    //    PlaneBase plane = otherCollider.gameObject.GetComponent<PlaneBase>();
    //    if (plane != null && renderer.sortingLayerName == "Enemy")
    //    {
    //        Debug.Log("22222");
    //        plane.bHit();
    //    }
    //    Destroy(this.gameObject);
    //}

    
}

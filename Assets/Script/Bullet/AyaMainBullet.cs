using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class AyaMainBullet : BulletBase_Touhou {

	// Use this for initialization
	void Start () {
        rigidbody2D.velocity = Speed;
	}
    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        PlaneBase plane = otherCollider.gameObject.GetComponent<PlaneBase>();
        if (plane != null)
        {
            plane.bHit(Power);
        }
        Destroy(this.gameObject);
    }
	// Update is called once per frame
	void Update () {
        base.Update();
	}
}

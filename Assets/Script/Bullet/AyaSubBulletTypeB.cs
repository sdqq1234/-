using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class AyaSubBulletTypeB : BulletBase_Touhou { //自机的追踪弹

	// Use this for initialization
	void Start () {
        Power = 2;
        rigidbody2D.velocity = Speed;
	}
	
	// Update is called once per frame
	void Update () {
        if (StageManager.CurStage.enemylist.Count > 0) {
            if (StageManager.CurStage.enemylist[0].gameObject.activeSelf)
            {
                Vector3 Dir = (StageManager.CurStage.enemylist[0].transform.position - transform.position).normalized * SpeedScale;
                rigidbody2D.velocity = Vector2.Lerp(Dir, (StageManager.CurStage.enemylist[0].transform.position - transform.position).normalized, Time.deltaTime * SpeedScale);
                RotationToTarget(StageManager.CurStage.enemylist[0].transform.position);
            }
                
        }
        base.Update();
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

    public override void RotationToTarget(Vector3 target)
    {
        float angle = (Mathf.PI + Mathf.Atan2((transform.position.y - target.y), (transform.position.x - target.x))) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angle);
    }
}

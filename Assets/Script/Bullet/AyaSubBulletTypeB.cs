using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class AyaSubBulletTypeB : BulletBase_Touhou { //自机的追踪弹

    private float AngleSpeed = 10;//追踪的角速度
	// Use this for initialization
	void Start () {
        //base.Start();
        //is_DirSame2Speed = true;
        Power = 2;
        rigidbody2D.velocity = speed;
	}
	
	// Update is called once per frame
	void Update () {
        if (StageManager.CurStage.enemylist.Count > 0) {
            if (StageManager.CurStage.enemylist[0].gameObject.activeSelf)
            {
                //Dir_CurSpeed = (StageManager.enemylist[0].transform.position - transform.position).normalized;
                //Dir_CurSpeed = Vector2.Lerp(Dir_CurSpeed, (StageManager.CurStage.enemylist[0].transform.position - transform.position).normalized, Time.deltaTime * AngleSpeed);
            }
                
        }
        base.Update();

       
        //Vector2 target = Dir_CurSpeed * Speed_Cur + cur_Positon;
        //gameObject.transform.position = Vector2.Lerp(cur_Positon, target, Time.deltaTime);
	}
    //void FixedUpdate()
    //{
    //    rigidbody2D.velocity = speed;
    //}
}

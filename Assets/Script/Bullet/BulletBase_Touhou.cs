using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(BoxCollider2D))]
//[RequireComponent(typeof(Rigidbody2D))]
//东方系列弹幕基类
public class BulletBase_Touhou : ObjectBase 
{
    /// <summary>
    /// 擦弹记录
    /// </summary>
    public bool Grazed { get; set; }

    public float Power = 1;//子弹威力
    //public ShooterBase ownerShooter;//发出子弹的发射器
    public Vector2 speed = Vector2.one;//当前子弹刚体活的的速度
    protected bool isOutDestroy = true;//打出屏幕是否销毁
    public GameObject vanishEffect;//消失特效

    void Start() {
        //rigidbody2D.velocity = speed;
    }

    public void Update()
    {
        base.Update();
        if (isOutDestroy) {
            // Destroy when outside the screen
            if (renderer != null && renderer.isVisible == false)
            {
                Destroy(this.gameObject);
            }
        }
    }
    /// <summary>
    /// 计算当前位置
    /// </summary>
    //void UpdatePos()
    //{
    //    Vector2 cur_Positon = gameObject.transform.position;
    //    //Vector2 target = Dir_CurSpeed * Speed_Cur + cur_Positon;
    //    float target_x = Speed_CurValue * Dir_CurSpeed.x + Acceleration_Value * Dir_Acceleration.x * Mathf.Pow(Time.deltaTime, 2) / 2;
    //    float target_y = Speed_CurValue * Dir_CurSpeed.y + Acceleration_Value * Dir_Acceleration.y * Mathf.Pow(Time.deltaTime, 2) / 2;
    //    Vector2 target = new Vector2(target_x + cur_Positon.x, target_y + cur_Positon.y);
    //    gameObject.transform.position = Vector2.Lerp(cur_Positon, target, Time.deltaTime);
    //}

    /// <summary>
    /// 播放消失特效
    /// </summary>
    public void ShowVanishEffect() {
        if (vanishEffect != null) {
            GameObject effect = Instantiate(vanishEffect) as GameObject;
            effect.transform.parent = this.transform.parent;
            effect.transform.localScale = Vector3.one;
            effect.transform.position = transform.position;
        }
    }

    void OnTriggerEnter2D(Collider2D otherCollider) {

        if (renderer.sortingLayerName == "EnemyBullet")
        {
            //设置是否被擦弹过
            GrazeCenter gc = otherCollider.GetComponent<GrazeCenter>();
            if (gc != null)
            {
                Grazed = true;
                StageManager.CurStage.myPlane.InitGrazeItem();
            }
        }
        else {
            //否则则是自己打出的子弹
            PlaneBase plane = otherCollider.gameObject.GetComponent<PlaneBase>();
            if (plane != null)
            {
                plane.bHit(Power);
            }
            Destroy(this.gameObject);
        }
        
    }

    void FixedUpdate()
    {
        
    }

    public Vector3 getDirToTarget(Vector3 target) {
        float angle = (Mathf.PI + Mathf.Atan2((transform.position.y - target.y), (transform.position.x - target.x))) * Mathf.Rad2Deg - 90;
        Vector3 dir = new Vector3(0, 0, angle);
        return dir;
    }

    //获取目标与当前方向的夹角
    public float getAngleToTarget(Vector3 target) {
        float angle = (Mathf.PI + Mathf.Atan2((transform.position.y - target.y), (transform.position.x - target.x))) * Mathf.Rad2Deg - 90;
        return angle;
    }

    /// <summary>
    /// 根据目标坐标转向目标方向
    /// </summary>
    /// <param name="target">目标位置</param>
    public void RotationToTarget(Vector3 target) {
        float angle = (Mathf.PI + Mathf.Atan2((transform.position.y - target.y), (transform.position.x - target.x))) * Mathf.Rad2Deg - 90;
        transform.eulerAngles = new Vector3(0, 0, angle);
        //this.transform.
    }

    /// <summary>
    /// 根据方向转向
    /// </summary>
    public void RotationWithDirction(Vector3 dir) {
        float angle = (Mathf.PI + Mathf.Atan2(-dir.y, -dir.x)) * Mathf.Rad2Deg - 90;
        transform.eulerAngles = new Vector3(0, 0, angle);
    }
}

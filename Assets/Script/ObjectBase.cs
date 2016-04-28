using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SpriteRenderer))]//所有物体都需要2d渲染组件
public class ObjectBase : MonoBehaviour
{
    #region 变量及属性
    protected SpriteRenderer render //当前2d渲染变量
    {
        get
        {
            return gameObject.GetComponent<SpriteRenderer>();
        }
    }

    protected Color color{ //当前物体颜色
        get{
            return render.color;
        }
        set {
            render.color = value;
        }
    }

    public string LayerName { //层级名称
        get 
        {
            return render.sortingLayerName;
        }
        set {
            render.sortingLayerName = value;
        }
    }

    public int LayerOrder {//层级里的层数
        get {
            return render.sortingOrder;
        }
        set {
            render.sortingOrder = value;
        }
    }

    private float curEulerAngle;
    //private float curEulerAngle
    //{
    //    get {
    //        return gameObject.transform.localEulerAngles.z;
    //    }
    //}

    //protected GameObject hitObject = null;//碰到的其他gameobject列表
    protected List<GameObject> hitObjectList = new List<GameObject>();//碰到的其他gameobject列表
    protected bool hitEnable = false;
    //protected float RandomRange = 100;//随机范围
    protected float LifeTime = 0;//已经存活的时间
    public float SpeedScale = 0;//当前移动速度值
    //public float MaxSpeed = 20;//最大速度
    //public bool isRotationSelf = false;//是否自身旋转
    //public float RotationSelfSpeed;//自身旋转角速度
    //public bool is_DirSame2Speed = false;//自身方向和速度一样
    //public float Speed_CurValue = 0;//当前速度值，不带方向
    //public Vector2 Dir_CurSpeed = Vector2.right;//当前速度方向
    //public float Acceleration_Value = 0;//加速度值，不带方向
    //public Vector2 Dir_Acceleration = Vector2.zero;//加速度方向
    //protected float WidthRatio;//横比
    //protected float HightRatio;//纵比
    //protected bool isFog = false;//雾化效果
    //protected bool isRemove = false;//消除效果
    //protected bool isHighLight = false;//高光效果
    //protected bool isSmear = false;//拖影效果
    //public bool isOutScreen = false;//出屏即消
    //protected bool isGod = false;//是否无敌
    //protected bool isRandom_Dir = false;//方向随机
    //private Vector2 Random_Dir;//随机方向
    //protected bool isRandom_Speed = false;//速度随机
    //private float Random_Speed;//速度随机量
    //protected bool isRandom_SpeedDir = false;//速度方向随机
    //private Vector2 Random_SpeedDir;//速度方向随机量
    //protected bool isRandom_Acceleration = false;//加速度随机
    //private float Random_Acceleration;//加速度随机量
    //protected bool isRandom_AccelerationDir = false;//加速度方向随机
    //private float Random_AccelerationDir;//加速度随机量

    //public string BaseName;//基本名称
    #endregion

    //基础初始化
    //public virtual void Start() {
    //    curEulerAngle = gameObject.transform.localEulerAngles.z;
    //    InitRandomValue();
    //}

    //public void SetBaseName(string name) {
    //    BaseName = name;
    //}

    //void OnTriggerEnter2D(Collider2D Collider)
    //{
    //    hitObjectList.Add(Collider.gameObject);
    //}

    //计算速度和方向
    //private void UpdateSpeedAndDir()
    //{
    //    //mBulletData.BulletSpeed = mBulletData.BulletSpeed + mBulletData.BulletAcceleration * BulletTime; //速度等于初速度+加速度*子弹运行的时间
    //    //Speed_Cur = Speed_Cur + Acceleration * Time.deltaTime;
    //    //Debug.Log(Speed_Cur + Acceleration * Time.deltaTime);
    //    if (Speed_CurValue >= MaxSpeed)
    //    {
    //        Speed_CurValue = MaxSpeed;
    //    }

    //    if (Dir_Acceleration != Vector2.zero) {
    //        float dir_x = Dir_CurSpeed.x;
    //        float dir_y = Dir_CurSpeed.y;
    //        dir_x = dir_x + Dir_Acceleration.x * Time.deltaTime;
    //        dir_y = dir_y + Dir_Acceleration.y * Time.deltaTime;
    //        Dir_CurSpeed = new Vector2(dir_x, dir_y);
    //    }

    //    if (is_DirSame2Speed) {
    //        float angle = getAngle(Dir_CurSpeed);
    //        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, curEulerAngle + angle));
    //        //Vector3
    //    }
        
    //}

    //判定是否在操作屏幕内
    public bool CheckInScreen() {
        if (transform.position.x < GlobalData.screenLeftPoint.x || transform.position.x > GlobalData.screenRightPoint.x
            || transform.position.y < GlobalData.screenBottomPoint.y || transform.position.y > GlobalData.screenTopPoint.y) {
                return false;
        }
        return true;
    }
    

    //获取物体与y轴正方向的夹角
    protected float getAngle(Vector2 dir)
    {
        //Vector2 mDir = dir.normalized;
        return (Mathf.PI + Mathf.Atan2(dir.y, dir.x)) * Mathf.Rad2Deg +90;
    }

    /// <summary>
    /// 计算自己与指定目标的方向 结果为与x正方向的夹角
    /// </summary>
    /// <param name="TargetPosition">指向的目标点</param>
    /// <returns>返回方向弧度值,范围[0,2π]</returns>
    public float GetAngleByTarget(Vector2 TargetOriginalPosition)
    {
        return Mathf.PI + Mathf.Atan2((transform.position.y - TargetOriginalPosition.y), (transform.position.x - TargetOriginalPosition.x));
    }

    public Vector2 getDirByTarget(Vector2 TargetPosition) {
        Vector2 dir = new Vector2(TargetPosition.x - transform.position.x, TargetPosition.y - transform.position.y).normalized;
        return dir;
    }
    //初始化随机类型的数据
    //protected void InitRandomValue() {
    //    if (isRandom_Dir) { //生成的弹幕初始朝向随机
    //        float toAngle = getAngle(new Vector2(Random.Range(-RandomRange, RandomRange), Random.Range(-RandomRange, RandomRange)));
    //        gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, toAngle));
    //    }
    //    if (isRandom_Speed) {//速度随机
    //        Speed_CurValue = Random.Range(0,MaxSpeed);
    //    }
    //    if (isRandom_SpeedDir) { //速度方向随机
    //        Dir_CurSpeed = new Vector2(Random.Range(-RandomRange, RandomRange), Random.Range(-RandomRange, RandomRange));
    //    }
    //    if (isRandom_Acceleration) {//加速度值随机
    //       Acceleration_Value = Random.Range(0,Random_Acceleration);
    //    }
    //    if (isRandom_AccelerationDir) {//加速度方向随机
    //        Dir_Acceleration = new Vector2(Random.Range(-Random_AccelerationDir, Random_AccelerationDir), Random.Range(-Random_AccelerationDir, Random_AccelerationDir));
    //    }
    //}

    /// <summary>
    /// 计算当前位置
    /// </summary>
    void UpdatePos() {
        //Vector2 cur_Positon = gameObject.transform.position;
        ////Vector2 target = Dir_CurSpeed * Speed_Cur + cur_Positon;
        //float target_x = Speed_CurValue * Dir_CurSpeed.x + Acceleration_Value * Dir_Acceleration.x * Mathf.Pow(Time.deltaTime,2)/2;
        //float target_y = Speed_CurValue * Dir_CurSpeed.y + Acceleration_Value * Dir_Acceleration.y * Mathf.Pow(Time.deltaTime, 2) / 2;
        //Vector2 target = new Vector2(target_x + cur_Positon.x, target_y + cur_Positon.y);
        //gameObject.transform.position = Vector2.Lerp(cur_Positon, target, Time.deltaTime);
    }

    
    /// <summary>
    /// 更新基础数值
    /// </summary>
    public virtual void Update()
    {
        LifeTime += Time.deltaTime;
        //UpdateSpeedAndDir();
        UpdatePos();
    }

    void OnBecameInvisible() { //判断是否在屏幕内
        //isOutScreen = true;
    }
}

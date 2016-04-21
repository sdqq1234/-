using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//[RequireComponent(typeof(Collider2D))] //拥有一个碰撞框
//[RequireComponent(typeof(AudioSource))] //拥有一个音频播放器
public abstract class ShooterBase : MonoBehaviour {

    /// <summary>
    /// 射出的子弹
    /// </summary>
    //public GameObject BulletPrefab;
    //protected GameObject master
    //{ //发射器的所有人 默认为父节点的gameobject
    //    get {
    //        return transform.parent.gameObject;
    //    }
    //    set {
    //        master = value;
    //    }
    //}
    /// <summary>
    /// 可以存活的时间 -1为一直存活直到拥有此发射器的飞机死亡或者此发射器飞出屏幕
    /// </summary>
    protected float liveTime = -1;
    /// <summary>
    /// 血量值  如果发射器可被攻击则有血量
    /// </summary>
    protected float bloodValue = -1;
    //public List<GameObject> obj_OwnBulletTypeList;//拥有的子弹gameobject

    //
    /// <summary>
    /// 能射的子弹数
    /// </summary>
    public int CanShootBulletCount;
    //protected BulletBase_Touhou ownBullet{//拥有的子弹类型
        //get{
        //    return obj_OwnBullet.GetComponent<BulletBase_Touhou>();
        //}
    //}
    /// <summary>
    /// 射击时播放的声音
    /// </summary>
    public AudioClip shootSound;
    //protected int shootDirCount = 1;//每次发射子弹的条数
    //public int BrustsCount = 1;//每次连发数
    //protected float BrustSpeedSpace = 10;//连发的每个子弹的速度间隔
    //protected Vector2[] pos_Bullets; //每条发射子弹相对于发射器的位置
    //protected Vector2[] dir_Bullets;//每条子弹的方向
    //protected Vector2 dir_BulletAcc;//每条子弹的目标加速度方向
    //protected AudioSource shootAudio {//音频播放器
    //    get { 
    //        return gameObject.GetComponent<AudioSource>();
    //    }
    //}

    //protected CircleCollider2D collider { //碰撞框
    //    get {
    //        return gameObject.GetComponent<CircleCollider2D>();
    //    }
    //}

    /// <summary>
    /// 所发射子弹的层
    /// </summary>
    private string bulletLayerName;
    /// <summary>
    /// 需要量产的子弹名称列表
    /// </summary>
    protected List<string> bulletNameList = new List<string>();
    /// <summary>
    /// 每次发射射击间隔,默认每两秒发射一次
    /// </summary>
    public float shootSpace = 2f;
    /// <summary>
    /// 下次射击间隔
    /// </summary>
    protected float nextShootSpace = 0;
    /// <summary>
    /// 每次射击持续的时间
    /// </summary>
    public float shootDuration = 2;
    /// <summary>
    /// 下次持续时间
    /// </summary>
    protected float nextShootDuration = 0;

    /// <summary>
    /// 射击频率，每隔多少秒射一次，数字越小越快
    /// </summary>
    public float shootRate = 0.5f;
    /// <summary>
    /// 下次射击的时间
    /// </summary>
    protected float nextShoot = 0.0F;

    /// <summary>
    /// 射击音效最快的播放间隔
    /// </summary>
    protected float minShootAudioRate = 0.06f;
    /// <summary>
    /// 下次播放时间
    /// </summary>
    protected float nextShootAudio = 0;

    /// <summary>
    /// 射出的子弹是否自身方向和速度方向一致
    /// </summary>
    public bool Bullet_dirSameSpeed = true;
    /// <summary>
    /// 是否自狙机
    /// </summary>
    public bool isLockPlayer = true;
    protected Vector2 bulletTarget;
    /// <summary>
    /// 默认敌人子弹速度
    /// </summary>
    public float shootBulletSpeed = 3;

	// Use this for initialization
    //public override void Start()
    //{
    //    //base.Start();
    //    Init();
    //}

    //初始化
    void Init() {
        //transform.localScale = Vector3.one;
        //shootAudio.panLevel = 0;
        //shootAudio.volume = 0.2f;
        //bulletName = obj_OwnBullet.name;
    }

    //protected void setBulletTarget(Vector2 target) {
    //    bulletTarget = target;
    //}
    ////被打中伤血
    //protected void bHit()
    //{
    //    if (hitEnable && hitObject != null)
    //    {
    //        BulletBase_Touhou bullet = hitObject.GetComponent<BulletBase_Touhou>();
    //        bloodValue -= bullet.Power;
    //        if (bloodValue <= 0)
    //        {
    //            Dead();
    //        }
    //    }
    //}

    //public void setBulletSortingLayer(string layerName) {
    //    bulletLayerName = layerName;
    //}

    ////死亡方法，播放死亡特效和掉出道具
    //protected void Dead() {
    //    Debug.Log("shooter"+ gameObject.name + "死了");
    //}

    //设置射出去的子弹速度
    //public void SetShootBulletSpeed(float speed) {
    //    shootBulletSpeed = speed;
    //}

    ///// <summary>
    ///// 设置连发属性
    ///// </summary>
    ///// <param name="brustsCount">连发数</param>
    ///// <param name="speedSpace">连发速度间隔</param>
    //public void SetBrustsShoot(int brustsCount,float speedSpace) {
    //    BrustsCount = brustsCount;
    //    BrustSpeedSpace = speedSpace;
    //}
    /// <summary>
    /// 根据设定的时间间隔发射弹幕
    /// </summary>
    protected void ShootBulletByTime()
    {
        if (Time.time > nextShoot)
        {
            nextShoot = Time.time + shootRate;
            if (CanShootBulletCount == 0)
            { //子弹打完了
                return;
            }
            if (CanShootBulletCount <= -1)
            { //机体无限子弹
                InitBullet();
            }
            else if (CanShootBulletCount > 0)
            {
                InitBullet();
                CanShootBulletCount--;
            }
            if (shootSound != null)
            {
                AudioManager.AddBulletSound(shootSound);
            }

        }
        //AudioPlay();
    }


    /// <summary>
    /// 初始化子弹
    /// </summary>
    public abstract void InitBullet();
    //{
        //GameObject shot = Instantiate(BulletPrefab) as GameObject;
        //shot.transform.position = this.transform.position;

        //DemoPlayerShotScript shotScript = shot.GetComponent<DemoPlayerShotScript>();
        //shotScript.speed = new Vector2(0, 20);
        //for (int i = 0; i < dir_Bullets.Length; i++) {
        //    for (int j = 0; j < BrustsCount; j++) {
        //        //GameObject bullet = GameObject.Instantiate(Resources.Load(CommandString.BulletPrefabPath + bulletName)) as GameObject;
        //        GameObject bullet = GameObject.Instantiate(Resources.Load(CommandString.BulletPrefabPath + bulletNameList[i])) as GameObject;
        //        BulletBase_Touhou bulletBase = bullet.GetComponent<BulletBase_Touhou>();
        //        bulletBase.renderer.sortingLayerName = bulletLayerName;
        //        bulletBase.is_DirSame2Speed = Bullet_dirSameSpeed;
        //        bulletBase.ownerShooter = this;
        //        if (bulletTarget != Vector2.zero)
        //        {
        //            bulletBase.setFollow(bulletTarget);
        //        }
        //        bullet.transform.parent = UIShootRoot.tra_ShootRoot;
        //        bulletBase.Dir_CurSpeed = dir_Bullets[i];
        //        bulletBase.Speed_CurValue = shootBulletSpeed + j * BrustSpeedSpace;
        //        //Debug.Log("bulletBase.Dir_CurSpeed = " + bulletBase.Dir_CurSpeed);
        //        bullet.transform.position = (Vector2)transform.position + pos_Bullets[i];
        //        bullet.transform.localScale = Vector3.one;

        //        //生产发子弹的特效
        //        GameObject effect = GameObject.Instantiate(Resources.Load(CommandString.BulletPrefabPath + "ShootBulletEffect")) as GameObject;
        //        effect.transform.parent = UIShootRoot.tra_ShootRoot;
        //        effect.transform.position = (Vector2)transform.position + pos_Bullets[i];
        //        effect.transform.localScale = Vector3.one * 2;
        //    }
            
        //} 
    //}

    ////放到子类里去调用
    //public virtual void AudioPlay() {
    //    if (shootRate > minShootAudioRate)
    //    {
    //        minShootAudioRate = shootRate;
    //    }
    //    if (Time.time > nextShootAudio)
    //    {
    //        nextShootAudio = Time.time + minShootAudioRate;
    //        //AudioManager.AddEnemyBulletSound(shootSound);
    //        //audio.PlayOneShot(shootSound);
    //        //StageManager.StageSoundPlayer.PlayOneShot(shootSound);
    //    }
    //}

    /// <summary>
    /// 射击抽象方法
    /// </summary>
    public abstract void Shoot();
    //{
        //if (master != null)
        //{
        //    if (shootSpace > 0)//如果有射击时间间隔
        //    {
        //        if (Time.time > nextShootSpace)
        //        {
        //            //nextShootSpace = Time.time + shootSpace;
        //            //进来以后让持续射击时间开始增加
        //            nextShootDuration += Time.deltaTime;
        //            if (nextShootDuration < shootDuration)
        //            {
        //                ShootBullet();
        //            }
        //            else {
        //                nextShootDuration = 0;
        //                nextShootSpace = Time.time + shootSpace;
        //            }
        //        }    
        //    }
        //    else
        //    {
        //        ShootBullet();
        //    }
        //}
        //else {
        //    Debug.LogError("shooter master = null");
        //}
    //}

    //射出子弹
    //private void ShootBulletByTime() {
    //    if (Time.time > nextShoot)
    //    {
    //        nextShoot = Time.time + shootRate;
    //        if (CanShootBulletCount == 0)
    //        { //子弹打完了
    //            return;
    //        }
    //        if (CanShootBulletCount <= -1)
    //        { //机体无限子弹
    //            InitBullet();
    //        }
    //        else if (CanShootBulletCount > 0)
    //        {
    //            InitBullet();
    //            CanShootBulletCount--;
    //        }
    //        if (shootSound != null) {
    //            AudioManager.AddBulletSound(shootSound);
    //        }
            
    //    }
    //    //AudioPlay();
    //}
    
	// Update is called once per frame
    //public override void Update () {
    //    base.Update();
    //}
}

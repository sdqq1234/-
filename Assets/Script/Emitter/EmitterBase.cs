using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//[RequireComponent(typeof(CircleCollider2D))] //拥有一个碰撞框
//[RequireComponent(typeof(AudioSource))] //拥有一个音频播放器
public class EmitterBase : ObjectBase
{

    //public GameObject BulletPrefab;
    protected GameObject master
    { //发射器的所有人 默认为父节点的gameobject
        get {
            return transform.parent.gameObject;
        }
        set {
            master = value;
        }
    }
    public float live = -1;//可以存活的时间
    //public float bloodValue = 100;//血量值
    //public List<GameObject> obj_OwnBulletTypeList;//拥有的子弹gameobject

    //能射的子弹数
    public int CanShootBulletCount;
    //protected BulletBase_Touhou ownBullet{//拥有的子弹类型
        //get{
        //    return obj_OwnBullet.GetComponent<BulletBase_Touhou>();
        //}
    //}

    public AudioClip shootSound;//射击时播放的声音
    //protected int shootDirCount = 1;//每次发射子弹的条数
    //public int BrustsCount = 1;//每次连发数
    protected float BrustSpeedSpace = 10;//连发的每个子弹的速度间隔
    //protected Vector2[] pos_Bullets; //每条发射子弹相对于发射器的位置
    //protected Vector2[] dir_Bullets;//每条子弹的方向
    //protected Vector2 dir_BulletAcc;//每条子弹的目标加速度方向
    protected AudioSource shootAudio {//音频播放器
        get { 
            return gameObject.GetComponent<AudioSource>();
        }
    }

    //protected CircleCollider2D collider { //碰撞框
    //    get {
    //        return gameObject.GetComponent<CircleCollider2D>();
    //    }
    //}

    protected string bulletLayerName;//所发射子弹的层
    protected List<string> bulletNameList = new List<string>();//需要量产的子弹名称列表
    public float shootSpace = 2f; //每次发射射击间隔,默认每两秒发射一次
    protected float nextShootSpace = 0;//下次射击间隔

    public float shootDuration = 2;//每次射击持续的时间
    protected float nextShootDuration = 0;//下次持续时间

    public float shootRate = 0.5f;//射击频率，每隔多少秒射一次，数字越小越快
    protected float nextShoot = 0.0F;//下次射击的时间

    protected float minShootAudioRate = 0.06f;//射击音效最快的播放间隔
    protected float nextShootAudio = 0;//下次播放时间

    protected bool Bullet_dirSameSpeed;//射出的子弹是否自身方向和速度方向一致
    protected Vector2 bulletTarget;
    public float shootBulletSpeed = 2;//默认敌人子弹速度

	// Use this for initialization
    public virtual void Start()
    {
        Init();
    }

    //初始化
    void Init() {
        transform.localScale = Vector3.one;
        //shootAudio.panLevel = 0;
        //shootAudio.volume = 0.2f;
        //bulletName = obj_OwnBullet.name;
    }

    protected void setBulletTarget(Vector2 target) {
        bulletTarget = target;
    }
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

    public void setBulletSortingLayer(string layerName) {
        bulletLayerName = layerName;
    }

    //死亡方法，播放死亡特效和掉出道具
    protected void Dead() {
        Debug.Log("shooter"+ gameObject.name + "死了");
    }

    //设置射出去的子弹速度
    public void SetShootBulletSpeed(float speed) {
        shootBulletSpeed = speed;
    }

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
    /// 初始化子弹
    /// </summary>
    protected virtual void InitBullet() {
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
    }

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
    /// 射击方法
    /// </summary>
    /// <param name="bulletSpeed">射出去的子弹速度</param>
    protected virtual void Shoot() {
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
    }

    //射出子弹频率
    private void ShootBulletRate() {
        if (Time.time > nextShoot)
        {
            nextShoot = Time.time + shootRate;
            if (CanShootBulletCount == 0)
            { //子弹打完了
                return;
            }
            if (CanShootBulletCount <= -1) { //机体无限子弹
                InitBullet();
            }
            else if (CanShootBulletCount > 0) {
                InitBullet();
                CanShootBulletCount--;
            }
            if (shootSound != null) {
                AudioManager.AddBulletSound(shootSound);
            }
            
        }
        //AudioPlay();
    }
    
	// Update is called once per frame
    public override void Update()
    {
        base.Update();
    }
}

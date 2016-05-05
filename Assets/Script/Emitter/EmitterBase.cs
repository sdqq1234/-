using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EmitterBase : ObjectBase
{

    //protected GameObject master
    //{ //发射器的所有人 默认为父节点的gameobject
    //    get {
    //        return transform.parent.gameObject;
    //    }
    //    set {
    //        master = value;
    //    }
    //}
    public float live = -1;//可以存活的时间

    //能射的子弹数
    public int CanShootBulletCount;

    public AudioClip shootSound;//射击时播放的声音
    protected float BrustSpeedSpace = 10;//连发的每个子弹的速度间隔
    protected AudioSource shootAudio {//音频播放器
        get { 
            return gameObject.GetComponent<AudioSource>();
        }
    }

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
    public bool isShootByRhythm = false;//是否按照节奏射击
    //protected float shootBulletSpeed = 2;//默认敌人子弹速度

	// Use this for initialization
    public virtual void Start()
    {
    }


    protected void setBulletTarget(Vector2 target) {
        bulletTarget = target;
    }
    
    public void setBulletSortingLayer(string layerName) {
        bulletLayerName = layerName;
    }

    /// <summary>
    /// 初始化子弹
    /// </summary>
    protected virtual void InitBullet() {

    }

    /// <summary>
    /// 射击方法
    /// </summary>
    public virtual void Shoot() {
        
    }

    /// <summary>
    /// 立刻射击
    /// </summary>
    public virtual void ShootImmediately()
    { 

    }
    /// <summary>
    /// 射出子弹频率
    /// </summary>
    protected virtual void ShootBulletRate() {
    }
    
	// Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (!isShootByRhythm)
        {
            Shoot();
        }
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AyaMainShooter : MonoBehaviour {

    public GameObject projectilePrefab;

    public AudioClip shootSound;//射击时播放的声音

    protected AudioSource shootAudio
    {//音频播放器
        get
        {
            return gameObject.GetComponent<AudioSource>();
        }
    }

    //protected CircleCollider2D collider { //碰撞框
    //    get {
    //        return gameObject.GetComponent<CircleCollider2D>();
    //    }
    //}

    private string bulletLayerName;//所发射子弹的层
    //protected List<string> bulletNameList = new List<string>();//需要量产的子弹名称列表
    //public float shootSpace = 2f; //每次发射射击间隔,默认每两秒发射一次
    //private float nextShootSpace = 0;//下次射击间隔

    //public float shootDuration = 2;//每次射击持续的时间
    //private float nextShootDuration = 0;//下次持续时间

    public float shootRate = 0.5f;//射击频率，每隔多少秒射一次，数字越小越快
    private float nextShoot = 0.0F;//下次射击的时间

    protected float minShootAudioRate = 0.06f;//射击音效最快的播放间隔
    private float nextShootAudio = 0;//下次播放时间

    //protected bool Bullet_dirSameSpeed;//射出的子弹是否自身方向和速度方向一致
    public Vector2 shootBulletSpeed = new Vector2(0,20);//默认子弹速度为向上20像素每帧

    // Use this for initialization
    public void Start()
    {
        Init();
    }

    //初始化
    void Init()
    {
        transform.localScale = Vector3.one;
        shootAudio.panLevel = 0;
        shootAudio.volume = 0.2f;
        //bulletName = obj_OwnBullet.name;
    }

    //void FixedUpdate()
    //{
    //    rigidbody2D.velocity = shootBulletSpeed;
    //}

    //给生成的子弹赋值
    private void InitBullet()
    {
        GameObject shot = Instantiate(projectilePrefab) as GameObject;
        
        shot.transform.parent = UIShootRoot.tra_ShootRoot;
        shot.transform.position = this.transform.position;
        shot.transform.localScale = Vector3.one;

        BulletBase_Touhou shotScript = shot.GetComponent<BulletBase_Touhou>();
        shotScript.speed = shootBulletSpeed;
    }
   

    /// <summary>
    /// 射击方法
    /// </summary>
    /// <param name="bulletSpeed">射出去的子弹速度</param>
    public void Shoot()
    {
        if (Time.time > nextShoot)
        {
            nextShoot = Time.time + shootRate;
            InitBullet();
            if (shootSound != null)
            {
                AudioManager.AddBulletSound(shootSound);
            }

        }
    }
}

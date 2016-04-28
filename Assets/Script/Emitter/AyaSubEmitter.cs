using UnityEngine;
using System.Collections;

public class AyaSubEmitter : MonoBehaviour
{
    public GameObject projectilePrefab;

    public AudioClip shootSound;//射击时播放的声音

    protected AudioSource shootAudio
    {//音频播放器
        get
        {
            return gameObject.GetComponent<AudioSource>();
        }
    }

    public float shootRate = 0.5f;//射击频率，每隔多少秒射一次，数字越小越快
    private float nextShoot = 0.0F;//下次射击的时间

    public Vector2 shootBulletSpeed = new Vector2(0, 10);//默认子弹速度为向上20像素每帧
    public float ShootBulletSpeedScale = 10;//射出的子弹速度的缩放值 用来控制x和y方向的缩放

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
    }

    //给生成的子弹赋值
    private void InitBullet()
    {
        GameObject shot = Instantiate(projectilePrefab) as GameObject;
        shot.transform.position = this.transform.position;
        shot.transform.parent = UIEmitterRoot.tra_ShootRoot;
        shot.transform.localScale = Vector3.one;

        BulletBase_Touhou shotScript = shot.GetComponent<BulletBase_Touhou>();
        shotScript.speed = shootBulletSpeed;
        shotScript.SpeedScale = ShootBulletSpeedScale;
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

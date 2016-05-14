using UnityEngine;
using System.Collections;

//拥有自动狙击类型子弹发射器类型的敌人
public class TargetShooter : EmitterBase
{
    //public GameObject BulletPrefab;//子弹预设
    float ShootRadius = 0;//发射圈半径

    //// Use this for initialization
    public override void Start()
    {
        base.Start();
        string targetBulletName = "bullet4_0";
        bulletNameList.Add(targetBulletName);
        //shootRate = AudioManager.EveryBeatsLenght;
        setBulletSortingLayer(CommandString.EnemyBulletLayer);
        hitEnable = false;
        //dir_Bullets = TargetBulletDir;
        //pos_Bullets = TargetBulletPos;
        Bullet_dirSameSpeed = true;

    }

    protected override void InitBullet()
    {
        base.InitBullet();
        GameObject bullet = Instantiate(BulletPrefab) as GameObject;
        bullet.transform.position = this.transform.position;
        BulletBase_Touhou bulletBase = bullet.GetComponent<BulletBase_Touhou>();
        
        bulletBase.renderer.sortingLayerName = bulletLayerName;
        bulletBase.RotationToTarget(MyPlane.MyPos);
        bulletBase.rigidbody2D.velocity =getDirByTarget(MyPlane.MyPos)*SpeedScale;
        bullet.transform.parent = UIEmitterRoot.tra_ShootRoot;
        bullet.transform.position = (Vector2)transform.position;
        bullet.transform.localScale = Vector3.one;


        //生产发子弹的特效
        GameObject effect = GameObject.Instantiate(Resources.Load(CommandString.BulletPrefabPath + "ShootBulletEffect")) as GameObject;
        effect.transform.parent = UIEmitterRoot.tra_ShootRoot;
        effect.transform.position = bullet.transform.position;
        effect.transform.localScale = Vector3.one * 2;
    }

    public override void Shoot()
    {
        base.Shoot();
        //if (master != null)
        //{
            if (shootSpace > 0)//如果有射击时间间隔
            {
                if (Time.time > nextShootSpace)
                {
                    //nextShootSpace = Time.time + shootSpace;
                    //进来以后让持续射击时间开始增加
                    nextShootDuration += Time.deltaTime;
                    if (nextShootDuration < shootDuration)
                    {
                        ShootBulletRate();
                    }
                    else
                    {
                        nextShootDuration = 0;
                        nextShootSpace = Time.time + shootSpace;
                    }
                }
            }
            else
            {
                ShootBulletRate();
            }
        //}
        //else
        //{
        //    Debug.LogError("shooter master = null");
        //}
    }

    //射出子弹频率
    protected override void ShootBulletRate()
    {
        if (Time.time > nextShoot)
        {
            nextShoot = Time.time + shootRate;
            //InitBullet();
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

        }
        //AudioPlay();
    }

    public override void Update()
    {
        base.Update();
        if (!isShootByRhythm)
        {
            Shoot();
        }
    }
}

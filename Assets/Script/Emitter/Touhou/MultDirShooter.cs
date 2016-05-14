using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//多方向弹幕
public class MultDirShooter : EmitterBase
{
    //public GameObject BulletPrefab;//子弹预设
    float ShootRadius = 0.5f;//发射圈半径
    int dirCount = 3;//最少3个方向

    //// Use this for initialization
    public void Start()
    {
        //base.Start();
        switch (GlobalData.RankLevel)
        {
            case GlobalData.GameRank.Easy:
                break;
            case GlobalData.GameRank.Normal:
                dirCount = 5;
                break;
            case GlobalData.GameRank.Hard:
                dirCount = 7;
                break;
            case GlobalData.GameRank.Lunatic:
                dirCount = 9;
                break;
        }
        setBulletSortingLayer(CommandString.EnemyBulletLayer);
        //hitEnable = false;
        //Bullet_dirSameSpeed = true;
    }
    protected override void InitBullet()
    {
        base.InitBullet();
        for (int i = 0; i < dirCount; i++)
        {
            float clipAngle =2* Mathf.PI / dirCount;
            float offAngle = GetAngleByTarget(MyPlane.MyPos);
            float x = Mathf.Cos(i * clipAngle + offAngle);
            float y = Mathf.Sin(i * clipAngle + offAngle);
            
            Vector2 dir = new Vector2(x, y).normalized;
            Vector2 pos = dir * ShootRadius + (Vector2)transform.position;
            InitOneBullet(pos, dir);
        }
        
    }

    /// <summary>
    /// 发射一个子弹
    /// </summary>
    /// <param name="pos">出现位置</param>
    /// <param name="dir">方向</param>
    void InitOneBullet(Vector2 pos,Vector2 dir) {
        GameObject bullet = Instantiate(BulletPrefab) as GameObject;
        bullet.transform.position = pos;
        bullet.transform.parent = UIEmitterRoot.tra_ShootRoot;
        bullet.transform.localScale = Vector3.one;

        BulletBase_Touhou bulletBase = bullet.GetComponent<BulletBase_Touhou>();
        bulletBase.renderer.sortingLayerName = bulletLayerName;
        bulletBase.RotationWithDirction(dir);
        bulletBase.rigidbody2D.velocity = dir * SpeedScale;

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

    public override void ShootImmediately()
    {
        base.ShootImmediately();
        InitBullet();
    }

    //射出子弹频率
    protected override void ShootBulletRate()
    {
        if (Time.time > nextShoot)
        {
            nextShoot = Time.time + shootRate;
            InitBullet();
            

        }
        //AudioPlay();
    }

    public override void Update()
    {
        base.Update();
        if (!isShootByRhythm) {
            Shoot();
        }
    }
}

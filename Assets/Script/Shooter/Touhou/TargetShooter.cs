using UnityEngine;
using System.Collections;

//自动狙击类型子弹发射器 单方向
public class TargetShooter : ShooterBase
{
    
    //Vector2[] TargetBulletDir = { new Vector2(0f, 1) };//子弹射击出现的方向
    //Vector2[] TargetBulletPos = { Vector2.zero };//子弹射击出现的方向
    /// <summary>
    /// 需要射出去的子弹
    /// </summary>
    public GameObject BulletPrefab;

    //// Use this for initialization
    public void Start()
    {
        this.transform.localScale = Vector3.one;
        //base.Start();
        //string targetBulletName = "bullet4_0";
        //bulletNameList.Add(targetBulletName);
        ////shootRate = AudioManager.EveryBeatsLenght;
        //setBulletSortingLayer(CommandString.EnemyBulletLayer);
        //hitEnable = false;
        //dir_Bullets = TargetBulletDir;
        //pos_Bullets = TargetBulletPos;
        //Bullet_dirSameSpeed = true;

    }

    public override void Shoot()
    {
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
                        ShootBulletByTime();
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
                ShootBulletByTime();
            }
        //}
        //else
        //{
        //    Debug.LogError("shooter master = null");
        //}

        //throw new System.NotImplementedException();
    }

    //private void ShootBulletByTime()
    //{
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
    //        if (shootSound != null)
    //        {
    //            AudioManager.AddBulletSound(shootSound);
    //        }

    //    }
    //    //AudioPlay();
    //}
    void Update()
    {
        Shoot();
        //base.Update();
        //for (int i = 0; i < pos_Bullets.Length; i++)
        //{
        //    if (MyPlane.MyPos != null)
        //    {
        //        dir_Bullets[i] = ((Vector2)MyPlane.MyPos.transform.position - (Vector2)transform.position).normalized;
        //    }
        //    else
        //    {
        //        dir_Bullets[i] = -Vector2.up;
        //    }

        //}
    }


    /// <summary>
    /// 只有一个方向
    /// </summary>
    public override void InitBullet()
    {
        //生产发子弹的特效
        GameObject effect = GameObject.Instantiate(Resources.Load(CommandString.BulletPrefabPath + "ShootBulletEffect")) as GameObject;
        effect.transform.parent = UIShootRoot.tra_ShootRoot;
        effect.transform.position = (Vector2)transform.position;
        effect.transform.localScale = Vector3.one * 2;

        //产生子弹并赋值
        GameObject bullet = Instantiate(BulletPrefab) as GameObject;
        bullet.transform.parent = UIShootRoot.tra_ShootRoot;
        bullet.transform.position = this.transform.position;
        bullet.transform.localScale = Vector3.one;
        BulletBase_Touhou bullet_touhou = bullet.GetComponent<BulletBase_Touhou>();
        Vector3 target = new Vector3(MyPlane.MyPos.transform.position.x,MyPlane.MyPos.transform.position.y,0); //获取转向方位
        bullet_touhou.RotationToTarget(target);
        bullet_touhou.speed = ((Vector2)MyPlane.MyPos.transform.position - (Vector2)transform.position).normalized * shootBulletSpeed;
        

        //shotScript.speed = new Vector2(0, 20);
        //for (int i = 0; i < dir_Bullets.Length; i++)
        //{
        //    for (int j = 0; j < BrustsCount; j++)
        //    {
        //        //GameObject bullet = GameObject.Instantiate(Resources.Load(CommandString.BulletPrefabPath + bulletName)) as GameObject;
        //        GameObject bullet_Touhou = GameObject.Instantiate(Resources.Load(CommandString.BulletPrefabPath + bulletNameList[i])) as GameObject;
        //        BulletBase_Touhou bulletBase = bullet_Touhou.GetComponent<BulletBase_Touhou>();
        //        bulletBase.renderer.sortingLayerName = bulletLayerName;
        //        //bulletBase.is_DirSame2Speed = Bullet_dirSameSpeed;
        //        //bulletBase.ownerShooter = this;
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
        //throw new System.NotImplementedException();
    }
}

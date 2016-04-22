using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//多方向弹幕
public class MultDirShooter : ShooterBase
{
    public GameObject BulletPrefab;//子弹模板
    //Vector2[] MultBulletDir = { new Vector2(0f, 1) };//子弹射击出现的方向
    List<Vector3> MultBulletPos = new List<Vector3>();//子弹射击出现的位置
    List<Vector3> MultBulletDir = new List<Vector3>();
    /// <summary>
    /// 方向数
    /// </summary>
    int dirCount = 7;
    /// <summary>
    /// 发射圈半径
    /// </summary>
    public float ShootRadius = 0.5f;
    /// <summary>
    /// 是否会自动转向我方飞机方向
    /// </summary>
    //public bool isTurnToTarget = false;

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
        for (int i = 0; i < dirCount; i++)
        {
            float clipAngle = 2 * Mathf.PI / dirCount;
            float x = Mathf.Cos(i * clipAngle);
            float y = Mathf.Sin(i * clipAngle);
            Vector2 dir = new Vector2(x, y);
            Vector2 pos = new Vector2(this.transform.position.x + x * ShootRadius, this.transform.position.y + y * ShootRadius);

            MultBulletDir.Add(dir);
            MultBulletPos.Add(pos);
        }
        Bullet_dirSameSpeed = true;
    }
	
    ////// Update is called once per frame

    void Update() {
        Shoot();
    }

    

    public override void Shoot()
    {
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
    }

    public override void InitBullet()
    {
        for (int i = 0; i < MultBulletDir.Count; i++)
        {

            //生产发子弹的特效
            GameObject effect = GameObject.Instantiate(Resources.Load(CommandString.BulletPrefabPath + "ShootBulletEffect")) as GameObject;
            effect.transform.parent = UIShootRoot.tra_ShootRoot;
            effect.transform.position = MultBulletPos[i];  //子弹发射的位置产生发射特效
            effect.transform.localScale = Vector3.one * 2;

            //初始化一个子弹
            GameObject bullet = Instantiate(BulletPrefab) as GameObject;
            bullet.transform.parent = UIShootRoot.tra_ShootRoot;
            bullet.transform.position = MultBulletPos[i];
            bullet.transform.localScale = Vector3.one;

            //给子弹赋值属性
            BulletBase_Touhou bullet_touhou = bullet.GetComponent<BulletBase_Touhou>();
            bullet_touhou.RotationWithDirction(MultBulletDir[i]);
            bullet_touhou.speed = (MultBulletPos[i] - transform.position).normalized * shootBulletSpeed;
        } 
    }
}

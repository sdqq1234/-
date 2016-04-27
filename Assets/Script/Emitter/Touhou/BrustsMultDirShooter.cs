using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//连发单条多方向弹幕
public class BrustsMultDirShooter : EmitterBase
{

    //// Use this for initialization
    //List<Vector2> MultBulletPos = new List<Vector2>();//子弹射击出现的位置
    //List<Vector2> MultBulletDir = new List<Vector2>();
    //int dirCount = 3;//最少3个方向
    //float ShootRadius = 3;//发射圈半径
    //public bool isTurnToTarget = true;//是否会自动转向我方飞机方向

    //// Use this for initialization
    //public override void Start()
    //{
    //    base.Start();
    //    switch (GlobalData.RankLevel)
    //    {
    //        case GlobalData.GameRank.Easy:
    //            break;
    //        case GlobalData.GameRank.Normal:
    //            BrustsCount = 3;                
    //            dirCount = 5;
    //            break;
    //        case GlobalData.GameRank.Hard:
    //            dirCount = 7;
    //            BrustsCount = 5;
    //            break;
    //        case GlobalData.GameRank.Lunatic:
    //            BrustsCount = 7;
    //            dirCount = 9;
    //            break;
    //    }
    //    for (int i = 0; i < dirCount; i++)
    //    {
    //        float clipAngle = 2 * Mathf.PI / dirCount;
    //        float x = Mathf.Cos(i * clipAngle);
    //        float y = Mathf.Sin(i * clipAngle);
    //        Vector2 dir = new Vector2(x, y);
    //        Vector2 pos = new Vector2(x * ShootRadius * 10, y * ShootRadius * 10);

    //        MultBulletDir.Add(dir);
    //        MultBulletPos.Add(pos);
    //    }
    //    //bulletName = "bullet4_0";
    //    //bulletName = obj_OwnBullet.name;
    //    BrustSpeedSpace = 20;
    //    shootBulletSpeed = 150;
    //    //shootRate = AudioManager.EveryBeatsLenght;
    //    setBulletSortingLayer(CommandString.EnemyBulletLayer);
    //    hitEnable = false;
    //    dir_Bullets = MultBulletDir.ToArray();

    //    pos_Bullets = MultBulletPos.ToArray();
    //    Bullet_dirSameSpeed = true;
    //}

    ////// Update is called once per frame
    //public override void Update()
    //{
    //    base.Update();
    //    if (isTurnToTarget)
    //    {
    //        List<Vector2> dir_temp = new List<Vector2>();
    //        List<Vector2> pos_temp = new List<Vector2>();
    //        for (int i = 0; i < dirCount; i++)
    //        {
    //            float clipAngle = 2 * Mathf.PI / dirCount;
    //            float offAngle = GetAngleByTarget((Vector2)MyPlane.MyPos.transform.position);
    //            float x = Mathf.Cos(i * clipAngle + offAngle);
    //            float y = Mathf.Sin(i * clipAngle + offAngle);
    //            Vector2 dir = new Vector2(x, y);
    //            Vector2 pos = new Vector2(x * ShootRadius * 10, y * ShootRadius * 10);

    //            dir_temp.Add(dir);
    //            pos_temp.Add(pos);
    //        }
    //        MultBulletDir = dir_temp;
    //        MultBulletPos = pos_temp;
    //        dir_Bullets = MultBulletDir.ToArray();

    //        pos_Bullets = MultBulletPos.ToArray();
    //    }
    //}
}

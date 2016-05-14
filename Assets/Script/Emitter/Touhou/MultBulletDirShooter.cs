using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//多子弹类型，多连发，多方向弹幕发射器
public class MultBulletDirShooter : EmitterBase
{

    
    float ShootRadius = 0.5f;//发射圈半径
    int dirCount = 15;//最少15个方向
    int oneGroupBulletCount = 5;//每一组包含5个方向的弹幕

    //// Use this for initialization
    public void Start()
    {
        //base.Start();
        switch (GlobalData.RankLevel)
        {
            case GlobalData.GameRank.Easy:
                break;
            case GlobalData.GameRank.Normal:
                dirCount = 21;
                oneGroupBulletCount = 7;
                break;
            case GlobalData.GameRank.Hard:
                dirCount = 27;
                oneGroupBulletCount = 9;
                break;
            case GlobalData.GameRank.Lunatic:
                dirCount = 33;
                oneGroupBulletCount = 11;
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
            float clipAngle = 2 * Mathf.PI / dirCount;
            float offAngle = GetAngleByTarget(MyPlane.MyPos);
            float x = Mathf.Cos(i * clipAngle + offAngle);
            float y = Mathf.Sin(i * clipAngle + offAngle);

            Vector2 dir = new Vector2(x, y).normalized;
            Vector2 pos = dir * ShootRadius + (Vector2)transform.position;
            int Dir_index = i % (dirCount / oneGroupBulletCount); //每隔oneGroupBulletCount个角度的方向上子弹用列表里第二个子弹
            int bullet_index = 0;
            if (Dir_index > 0)
                bullet_index = 0;
            else
                bullet_index = 1;
            InitOneBullet(pos, dir, bullet_index);
        }

    }

    /// <summary>
    /// 初始化一个子弹的位置的出现效果
    /// </summary>
    /// <param name="bullet"></param>
    /// <param name="position"></param>
    /// <param name="direct"></param>
    void InitOneBulletPosAndEffect(GameObject bullet,Vector2 position,Vector2 direct) {
        bullet.transform.position = position;
        bullet.transform.parent = UIEmitterRoot.tra_ShootRoot;
        bullet.transform.localScale = Vector3.one;

        BulletBase_Touhou bulletBase = bullet.GetComponent<BulletBase_Touhou>();
        bulletBase.renderer.sortingLayerName = bulletLayerName;
        bulletBase.RotationWithDirction(direct);
        bulletBase.rigidbody2D.velocity = direct * SpeedScale;

        //生产发子弹的特效
        GameObject effect = GameObject.Instantiate(Resources.Load(CommandString.BulletPrefabPath + "ShootBulletEffect")) as GameObject;
        effect.transform.parent = UIEmitterRoot.tra_ShootRoot;
        effect.transform.position = bullet.transform.position;
        effect.transform.localScale = Vector3.one * 2;
    }

    /// <summary>
    /// 发射一个子弹
    /// </summary>
    /// <param name="pos">出现位置</param>
    /// <param name="dir">方向</param>
    void InitOneBullet(Vector2 pos, Vector2 dir)
    {
        GameObject bullet = Instantiate(BulletPrefab) as GameObject;
        InitOneBulletPosAndEffect(bullet,pos,dir);
    }

    /// <summary>
    /// 发射一个子弹
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="dir"></param>
    /// <param name="indexInList"></param>
    void InitOneBullet(Vector2 pos, Vector2 dir,int indexInList)
    {
        GameObject bullet = Instantiate(BulletPrefabList[indexInList]) as GameObject;
        InitOneBulletPosAndEffect(bullet, pos, dir);
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
        if (!isShootByRhythm)
        {
            Shoot();
        }
    }

    //// Use this for initialization
    //List<Vector2> MultBulletPos = new List<Vector2>();//子弹射击出现的位置
    //List<Vector2> MultBulletDir = new List<Vector2>();
    ////List<BulletBase_Touhou> OneDirBulletList = new List<BulletBase_Touhou>();//单个大方向多种子弹的列表,子弹种类决定这个大方向上分几个小方向
    ////float OneDirRange = 2*Mathf.PI/3;//单个方向能再分出的最大角度范围 一个pi为180度,这里取120度
    //int dirCount = 3;//最少3个大方向
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
    //            //BrustsCount = 3;
    //            dirCount = 5;
    //            break;
    //        case GlobalData.GameRank.Hard:
    //            dirCount = 7;
    //            //BrustsCount = 5;
    //            break;
    //        case GlobalData.GameRank.Lunatic:
    //            //BrustsCount = 7;
    //            dirCount = 9;
    //            break;
    //    }

    //    for (int i = 0; i < dirCount; i++)
    //    {
    //        //确定大方向
    //        float clipAngle = 2 * Mathf.PI / dirCount;
    //        float x = Mathf.Cos(i * clipAngle);
    //        float y = Mathf.Sin(i * clipAngle);
    //        Vector2 dir = new Vector2(x, y);
    //        Vector2 pos = new Vector2(x * ShootRadius * 10, y * ShootRadius * 10);
    //        MultBulletDir.Add(dir);
    //        MultBulletPos.Add(pos);
    //        bulletNameList.Add(obj_OwnBulletTypeList[0].name);//默认列表里第一个子弹是大方向的
    //        //每个大方向都有几个偏移方向
    //        if (obj_OwnBulletTypeList.Count > 1)
    //        {
    //            for (int j = 1; j < obj_OwnBulletTypeList.Count; j++)
    //            {
    //                float clipOffAngle = 0;
    //                float clipOffDir = clipAngle / 2;
    //                if (j % 2 == 0)
    //                {
    //                    clipOffAngle = clipAngle + clipOffDir;
    //                }
    //                else
    //                {
    //                    clipOffAngle = clipAngle - clipOffDir;
    //                }
    //                float off_x = Mathf.Cos(i * clipOffAngle);
    //                float off_y = Mathf.Sin(i * clipOffAngle);
    //                Vector2 off_dir = new Vector2(off_x, off_y);
    //                Vector2 off_pos = new Vector2(off_x * ShootRadius * 10, off_y * ShootRadius * 10);
    //                MultBulletDir.Add(off_dir);
    //                MultBulletPos.Add(off_pos);
    //                bulletNameList.Add(obj_OwnBulletTypeList[j].name);
    //            }
    //        }

    //    }
    //    //bulletName = obj_OwnBullet.name;
    //    BrustSpeedSpace = 20;
    //    shootBulletSpeed = 150;
    //    //shootRate = AudioManager.EveryBeatsLenght;       
    //    dir_Bullets = MultBulletDir.ToArray();
    //    pos_Bullets = MultBulletPos.ToArray();

    //    setBulletSortingLayer(CommandString.EnemyBulletLayer);
    //    hitEnable = false;
    //    Bullet_dirSameSpeed = true;
    //}

    ////// Update is called once per frame
    //public override void Update()
    //{
    //    base.Update();
    //    if (isTurnToTarget)
    //    {
    //        float offDir = Mathf.PI / dirCount;//单个方向的偏移量
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
    //            bulletNameList.Add(obj_OwnBulletTypeList[0].name);
    //            //每个大方向都有几个偏移方向
    //            if (obj_OwnBulletTypeList.Count > 1)
    //            {
    //                for (int j = 1; j < obj_OwnBulletTypeList.Count; j++)
    //                {
    //                    float clipOffAngle = 0;
    //                    float clipOffDir = offDir / 2;
    //                    if (j % 2 == 0)
    //                    {
    //                        clipOffAngle = offAngle + clipOffDir;
    //                    }
    //                    else
    //                    {
    //                        clipOffAngle = offAngle - clipOffDir;
    //                    }
    //                    float off_x = Mathf.Cos(i * clipAngle + clipOffAngle);
    //                    float off_y = Mathf.Sin(i * clipAngle + clipOffAngle);
    //                    Vector2 off_dir = new Vector2(off_x, off_y);
    //                    Vector2 off_pos = new Vector2(off_x * ShootRadius * 10, off_y * ShootRadius * 10);
    //                    dir_temp.Add(off_dir);
    //                    pos_temp.Add(off_pos);
    //                    bulletNameList.Add(obj_OwnBulletTypeList[j].name);
    //                }
    //            }
    //        }
    //        MultBulletDir = dir_temp;
    //        MultBulletPos = pos_temp;    
    //        dir_Bullets = MultBulletDir.ToArray();
    //        pos_Bullets = MultBulletPos.ToArray();      
    //    }

    //}
}

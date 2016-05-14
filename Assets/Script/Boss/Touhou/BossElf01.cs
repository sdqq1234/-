using UnityEngine;
using System.Collections;

public class BossElf01 : BossBase {

    private MultDirShooter Emitter1;//发射器1
    private MultBulletDirShooter Emitter2;//发射器2
    //private GameObject Bullet1;//子弹1

	// Use this for initialization
    void Start()
    {
        Init();
        base.Start();
        if (enemyFsm != null)
        {
            enemyFsm.Fsm.Event("MoveIn");
        }
        else {
            Debug.Log(gameObject.name + "enemyFsm = null");
        }
    }

    

    void Init() {
        InitAllEmitter();
        
        HpValue = 10000;
        bossState = BossState.InNormal;
        ShooterIndex = 0;
        
        ChangeBossEmitter(ShooterIndex);
        curEmitter.isShootByRhythm = true;
    }

    public void UpdateCurEmitter() {
        if (ShooterIndex < BossEmitterList.Count)
        {
            if (BossEmitterList[ShooterIndex].CanShootBulletCount == 0)
            {
                ShooterIndex++;
            }
        }
        ChangeBossEmitter(ShooterIndex);
    }

    void InitAllEmitter() {
        Emitter1 = gameObject.AddComponent<MultDirShooter>();
        Emitter1.SpeedScale = 3;
        Emitter1.CanShootBulletCount = 10;
        Emitter1.shootSound = Resources.Load(CommandString.SoundPath + "se_tan00") as AudioClip;
        Emitter1.BulletPrefab = Resources.Load(CommandString.BulletPrefabPath+ "bullet4_0") as GameObject;
        Emitter1.setBulletPrefabColor(Color.green);
        BossEmitterList.Add(Emitter1);

        Emitter2 = gameObject.AddComponent<MultBulletDirShooter>();
        Emitter2.SpeedScale = 3;
        Emitter2.CanShootBulletCount = -1;
        Emitter2.shootSound = Resources.Load(CommandString.SoundPath + "se_tan00") as AudioClip;
        GameObject bulletPrefab1 = Resources.Load(CommandString.BulletPrefabPath + "bullet4_0") as GameObject;
        GameObject bulletPrefab2 = Resources.Load(CommandString.BulletPrefabPath + "bullet1_0") as GameObject;
        Emitter2.BulletPrefabList.Add(bulletPrefab1);
        Emitter2.BulletPrefabList.Add(bulletPrefab2);
        Emitter2.setBulletListColor(Color.green);
        BossEmitterList.Add(Emitter2);
    }

    //初始化发射器列表
    void ChangeBossEmitter(int index) {
        
        for (int i = 0; i < BossEmitterList.Count; i++)
        {
            BossEmitterList[i].enabled = false; 
        }
        curEmitter = BossEmitterList[index];
        curEmitter.enabled = true;
    }

    //设置下一个随机位置由PlayMaker调用
    public void SetNexRandamPosition() { 
        //随机 移动范围限制在屏幕上方3分之1的范围里
        float max_x = GlobalData.screenRightPoint.x - GlobalData.ScreenWidth/4;
        float min_x = GlobalData.screenLeftPoint.x + GlobalData.ScreenWidth / 4;
        float max_y = GlobalData.screenTopPoint.y - GlobalData.ScreenHeight/10;
        float min_y = GlobalData.screenBottomPoint.y + GlobalData.ScreenHeight / 3*2;
        float new_x = Random.Range(min_x,max_x);
        float new_y = Random.Range(min_y,max_y);
        if (this.enemyFsm != null)
        {
            this.enemyFsm.Fsm.Variables.FindFsmVector3("InPosition").Value = new Vector3(new_x, new_y, 0);
            this.enemyFsm.Fsm.Variables.FindFsmFloat("InTweenTime").Value = 1;
        }
    }

    //// Update is called once per frame
    void Update()
    {
        base.Update();
        UpdateCurEmitter();
        //BossEmitterList[ShooterIndex].Update();
        //if (mainShooter.CanShootBulletCount <= 0)
        //{  //这个波效果的子弹打完换下一波子弹
        //    if (ShooterIndex < BossShooterList.Count - 1)
        //    {
        //        ShooterIndex++;
        //        InitBossShooters();
        //    }

        //}
    }
}

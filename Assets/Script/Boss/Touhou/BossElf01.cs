using UnityEngine;
using System.Collections;

public class BossElf01 : BossBase {


	// Use this for initialization
    void Start()
    {
        base.Start();
        Init();
    }

    //初始化发射器列表
    void InitBossShooters() {
        //for (int i = 0; i < BossShooterList.Count; i++)
        //{

        //    BossShooterList[i].CanShootBulletCount = 5;
        //}
        ////mainShooter = BossShooterList[ShooterIndex];
        //SetMainShooterBullet(5, 0, 0, 0);
    }

    void Init() {
        bossState = BossState.InNormal;
        ShooterIndex = 0;
        InitBossShooters();
    }

    //设置下一个随机位置
    public void SetNexRandamPosition() { 
        //随机 移动范围限制在屏幕上方3分之1的范围里
        float max_x = GlobalData.screenRightPoint.x - GlobalData.ScreenWidth/4;
        float min_x = GlobalData.screenLeftPoint.x + GlobalData.ScreenWidth / 4;
        float max_y = GlobalData.screenTopPoint.y - GlobalData.ScreenHeight/10;
        float min_y = GlobalData.screenBottomPoint.y + GlobalData.ScreenHeight / 3*2;
        float new_x = Random.Range(min_x,max_x);
        float new_y = Random.Range(min_y,max_y);
        if (this.enemyFsm != null) {
            this.enemyFsm.Fsm.Variables.FindFsmVector3("InPosition").Value = new Vector3(new_x, new_y, 0);
            this.enemyFsm.Fsm.Variables.FindFsmFloat("InTweenTime").Value = 1;
        }
    }

    //// Update is called once per frame
    void Update()
    {
        base.Update();
        //if (mainShooter.CanShootBulletCount <= 0) {  //这个波效果的子弹打完换下一波子弹
        //    if (ShooterIndex < BossShooterList.Count -1) {
        //        ShooterIndex++;
        //        InitBossShooters();
        //    }
            
        //}
    }
}

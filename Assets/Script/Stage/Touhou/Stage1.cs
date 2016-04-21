using UnityEngine;
using System.Collections;

public class Stage1 : StageBase {

	// Use this for initialization
	public override void Start () {
        base.Start();
        InitStageNoteInRoad();
        //InitStage1Enemy();
        InitStageEnemyInRoad();
        
	}

    

    public void InitStageEnemyInRoad()
    {
        //base.InitStageEnemyInRoad();
        for (int i = 0; i < EnemyNoteList.Count; i++)
        {
            string enemyName = EnemyNoteList[i].StageObjectType.ToString();
            float time = EnemyNoteList[i].Time;
            Color appearColor = EnemyNoteList[i].StageColor;
            //float appearSpeed = EnemyNoteList[i].AppearSpeed;
            //float appearSpeed = 120;
            //Vector2 appearDir = EnemyNoteList[i].AppearDir;
            float clipScreen = 15;
            float inTweenTime = 1;//进入屏幕要用的时间
            float outTweenTime = 4;//出屏幕要的时间
            Vector3 inPosition = Vector3.zero;//进入屏幕的位置
            Vector3 outPosition = Vector3.zero;//出屏幕的位置
            float WaitTime = 5;//到达位置要等待的时间
                //第一波敌人 前12个
                if (i < 12)
                {
                    int index = i;
                    if (EnemyNoteList[i].StageObjectType == Note.ObjectType.EnemyA0)
                    { //红色敌人 从左边开始一次向右排开 离开的时候向右下
                        Vector2 appearPos = Vector2.zero;
                        appearPos.x = GlobalData.ScreenZeroPoint.x + (2f + index / 2) / clipScreen * GlobalData.ScreenWidth;//通过屏幕百分比来得到x出现的坐标
                        appearPos.y = GlobalData.ScreenZeroPoint.y;//通过屏幕百分比来得到y出现的坐标
                        inPosition = new Vector3(appearPos.x, appearPos.y - 1 - index / 2 * 0.1f, 0);
                        outPosition = new Vector3(GlobalData.screenRightPoint.x + 1, GlobalData.screenBottomPoint.y, 0);
                        CreateEnemy(enemyName, time, appearPos, appearColor, inTweenTime, outTweenTime,WaitTime, inPosition, outPosition, false, 10);
                    }
                    else if (EnemyNoteList[i].StageObjectType == Note.ObjectType.EnemyA1)
                    {//蓝色敌人  右边开始一次向左排开  离开的时候向左下
 
                        Vector2 appearPos = Vector2.zero;
                        appearPos.x = GlobalData.ScreenZeroPoint.x + (clipScreen - 2f - index / 2) / clipScreen * GlobalData.ScreenWidth;//通过屏幕百分比来得到x出现的坐标
                        appearPos.y = GlobalData.ScreenZeroPoint.y;//通过屏幕百分比来得到y出现的坐标
                        inPosition = new Vector3(appearPos.x, appearPos.y - 1 - index/2*0.1f, 0);
                        outPosition = new Vector3(GlobalData.screenLeftPoint.x - 1, GlobalData.screenBottomPoint.y, 0);
                        CreateEnemy(enemyName, time, appearPos, appearColor, inTweenTime, outTweenTime,WaitTime, inPosition, outPosition, false, 10);
                    }
                }
                else if (i < 24)
                {  //第二波12个敌人
                    int index = i - 12;
                    float offY = 0.1f;
                    if (EnemyNoteList[i].StageObjectType == Note.ObjectType.EnemyA2)
                    { //绿色敌人 从中间向左边排开，向右边退出
                        //AnimationCurve speedCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1.5f + index * offY, 0), new Keyframe(5, 0), new Keyframe(6, 1));
                        //AnimationCurve dirxCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(5, 0), new Keyframe(5, 1));
                        Vector2 appearPos = Vector2.zero;
                        appearPos.x = GlobalData.ScreenZeroPoint.x + GlobalData.ScreenWidth / 2 - (index / 2f) / clipScreen * GlobalData.ScreenWidth;//通过屏幕百分比来得到x出现的坐标
                        appearPos.y = GlobalData.ScreenZeroPoint.y;//通过屏幕百分比来得到y出现的坐标
                        //CreateEnemy(enemyName, time, appearPos, appearColor, appearSpeed, appearDir, speedCurve, dirxCurve, EnemyNoteList[i].DirY_Curve, false, 10);
                        inPosition = new Vector3(appearPos.x, appearPos.y - 1 - index / 2 * 0.1f, 0);
                        outPosition = new Vector3(GlobalData.screenRightPoint.x + 1, GlobalData.screenBottomPoint.y, 0);
                        CreateEnemy(enemyName, time, appearPos, appearColor, inTweenTime, outTweenTime, WaitTime, inPosition, outPosition, false, 10);
                    }
                    if (EnemyNoteList[i].StageObjectType == Note.ObjectType.EnemyA3)
                    { //黄色敌人  从中间向右边排开，向左边退出
                        //AnimationCurve speedCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1.5f + index * offY, 0), new Keyframe(5, 0), new Keyframe(6, 1));
                        //AnimationCurve dirxCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(5, 0), new Keyframe(5, -1));
                        Vector2 appearPos = Vector2.zero;
                        appearPos.x = GlobalData.ScreenZeroPoint.x + GlobalData.ScreenWidth / 2 + (index / 2 + 1f) / clipScreen * GlobalData.ScreenWidth;//通过屏幕百分比来得到x出现的坐标
                        appearPos.y = GlobalData.ScreenZeroPoint.y;//通过屏幕百分比来得到y出现的坐标
                        inPosition = new Vector3(appearPos.x, appearPos.y - 1 - index / 2 * 0.1f, 0);
                        outPosition = new Vector3(GlobalData.screenLeftPoint.x - 1, GlobalData.screenBottomPoint.y, 0);
                        CreateEnemy(enemyName, time, appearPos, appearColor, inTweenTime, outTweenTime, WaitTime, inPosition, outPosition, false, 10);
                        //CreateEnemy(enemyName, time, appearPos, appearColor, appearSpeed, appearDir, speedCurve, dirxCurve, EnemyNoteList[i].DirY_Curve, false, 10);
                    }
                }
                else if (i < 43)
                { //第三波18+1个敌人
                    int index = i - 24;
                    clipScreen = 20;
                    float offY = 0.1f;
                    if (EnemyNoteList[i].StageObjectType == Note.ObjectType.EnemyA2)
                    { //绿色敌人 从屏幕中间出来向左边排开 向右边退出
                        //AnimationCurve speedCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1.5f + index * offY, 0), new Keyframe(5, 0), new Keyframe(6, 1));
                        //AnimationCurve dirxCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(5, 0), new Keyframe(5, 1));
                        Vector2 appearPos = Vector2.zero;
                        appearPos.x = GlobalData.ScreenZeroPoint.x + GlobalData.ScreenWidth / 2 - (index / 2f) / clipScreen * GlobalData.ScreenWidth;//通过屏幕百分比来得到x出现的坐标
                        appearPos.y = GlobalData.ScreenZeroPoint.y;//通过屏幕百分比来得到y出现的坐标
                        inPosition = new Vector3(appearPos.x, appearPos.y - 1 - index / 2 * 0.1f, 0);
                        outPosition = new Vector3(GlobalData.screenRightPoint.x + 1, GlobalData.screenBottomPoint.y, 0);
                        CreateEnemy(enemyName, time, appearPos, appearColor, inTweenTime, outTweenTime, WaitTime, inPosition, outPosition, false, 10);
                        //CreateEnemy(enemyName, time, appearPos, appearColor, appearSpeed, appearDir, speedCurve, dirxCurve, EnemyNoteList[i].DirY_Curve, false, 10);
                    }
                    if (EnemyNoteList[i].StageObjectType == Note.ObjectType.EnemyA3)
                    { //黄色敌人 屏幕中间出来向右边排开  向左边退出
                        //AnimationCurve speedCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1.5f + index * offY, 0), new Keyframe(5, 0), new Keyframe(6, 1));
                        //AnimationCurve dirxCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(5, 0), new Keyframe(5, -1));
                        Vector2 appearPos = Vector2.zero;
                        appearPos.x = GlobalData.ScreenZeroPoint.x + GlobalData.ScreenWidth / 2 + (index / 2 + 1f) / clipScreen * GlobalData.ScreenWidth;//通过屏幕百分比来得到x出现的坐标
                        appearPos.y = GlobalData.ScreenZeroPoint.y;//通过屏幕百分比来得到y出现的坐标
                        inPosition = new Vector3(appearPos.x, appearPos.y - 1 - index / 2 * 0.1f, 0);
                        outPosition = new Vector3(GlobalData.screenLeftPoint.x - 1, GlobalData.screenBottomPoint.y, 0);
                        CreateEnemy(enemyName, time, appearPos, appearColor, inTweenTime, outTweenTime, WaitTime, inPosition, outPosition, false, 10);
                        //CreateEnemy(enemyName, time, appearPos, appearColor, appearSpeed, appearDir, speedCurve, dirxCurve, EnemyNoteList[i].DirY_Curve, false, 10);
                    }
                    if (EnemyNoteList[i].StageObjectType == Note.ObjectType.EnemyA1) {
                        clipScreen = 10f;
                        for (int j = 0; j < 7; j++) {
                            Vector2 appearPos = Vector2.zero;
                            appearPos.x = GlobalData.ScreenZeroPoint.x + (j+2) / clipScreen * GlobalData.ScreenWidth;//通过屏幕百分比来得到x出现的坐标
                            appearPos.y = GlobalData.ScreenZeroPoint.y;//通过屏幕百分比来得到y出现的坐标
                            inPosition = new Vector3(appearPos.x, appearPos.y - 1 - index / 2 * 0.1f, 0);
                            outPosition = new Vector3(appearPos.x, GlobalData.screenBottomPoint.y -1, 0);
                            CreateEnemy(enemyName, time, appearPos, appearColor, inTweenTime, outTweenTime, WaitTime, inPosition, outPosition, false, 10);
                            //CreateEnemy(enemyName, time, appearPos, appearColor, appearSpeed, appearDir, EnemyNoteList[i].Speed_Curve, EnemyNoteList[i].DirX_Curve, EnemyNoteList[i].DirY_Curve, false, 10);
                        }
                    }
                }
                else if (i < 50) { 
                    //第四波红色从左边出来 向右边退出
                    float clipYScreen = 8f; //屏幕高度分8份
                    Vector2 appearPos = Vector2.zero;
                    WaitTime = 0;
                    appearPos.x = GlobalData.ScreenZeroPoint.x;//通过屏幕百分比来得到x出现的坐标
                    appearPos.y = GlobalData.ScreenZeroPoint.y - GlobalData.ScreenHeight/clipYScreen ;//通过屏幕百分比来得到y出现的坐标
                    inPosition = new Vector3(appearPos.x+GlobalData.ScreenWidth + 1, appearPos.y, 0);
                    outPosition = inPosition;
                    CreateEnemy(enemyName, time, appearPos, appearColor, inTweenTime, outTweenTime, WaitTime, inPosition, outPosition, false, 10);
                    //CreateEnemy(enemyName, time, appearPos, appearColor, appearSpeed, newAppearDir, speedCurve, dirxCurve, dirYCurve,false,10);
                }
                else if (EnemyNoteList[i].StageObjectType == Note.ObjectType.BossElf01) { //道中bossElf01
                    //AnimationCurve speedCurve = new AnimationCurve(new Keyframe(0, 1));
                    //AnimationCurve dirxCurve = new AnimationCurve(new Keyframe(0, 0));
                    //AnimationCurve dirYCurve = new AnimationCurve(new Keyframe(0, -1));
                    Vector2 newAppearDir = -Vector2.up;
                    Vector2 appearPos = Vector2.zero;
                    appearPos.x = GlobalData.ScreenZeroPoint.x + GlobalData.ScreenWidth/2;//屏幕中间出来
                    appearPos.y = GlobalData.ScreenZeroPoint.y;

                    inPosition = new Vector3(appearPos.x, appearPos.y - 1, 0);
                    outPosition = Vector3.zero;
                    inTweenTime = 1; //1秒内到位
                    WaitTime = 1.5f;
                    CreateBoss(enemyName, time, appearPos, appearColor, inTweenTime,WaitTime, inPosition, true);
                    //CreateEnemy(enemyName, time, appearPos, appearColor, appearSpeed, newAppearDir, EnemyNoteList[i].Speed_Curve, EnemyNoteList[i].DirX_Curve, EnemyNoteList[i].DirY_Curve, true,-1);
                }
        }
        EnemyNoteList.Clear();
    }

    //void InitStage1Enemy() {
    //    for (int i = 0; i < enemylist.Count; i++) {
    //        if (i < 12) { //设置前12个敌人的曲线
    //            if (enemylist[i].BaseName == Note.ObjectType.EnemyA0.ToString()) { //红色敌人
    //                AnimationCurve speed = new AnimationCurve(new Keyframe(0, 1), new Keyframe(2 + i * 0.2f, 0), new Keyframe(5, 0), new Keyframe(6, 1));
    //                AnimationCurve dirx = new AnimationCurve(new Keyframe(0,0),new Keyframe(5,0),new Keyframe(5,1));
    //                Vector2 appearPos = Vector2.zero;
    //                appearPos.x = GlobalData.ScreenZeroPoint.x + (4 + i) / CommandString.ScreenCol * GlobalData.ScreenWidth;//通过屏幕百分比来得到x出现的坐标
    //                appearPos.y = GlobalData.ScreenZeroPoint.y;//通过屏幕百分比来得到y出现的坐标
    //                enemylist[i].SetAppear(null, appearPos, null, null);
    //                enemylist[i].SetSpeedCurve(speed,dirx,null);
    //            }else if(enemylist[i].BaseName == Note.ObjectType.EnemyA1.ToString()){//蓝色敌人
    //                AnimationCurve speed = new AnimationCurve(new Keyframe(0, 1), new Keyframe(2 + i * 0.2f, 0), new Keyframe(5, 0), new Keyframe(6, 1));
    //                AnimationCurve dirx = new AnimationCurve(new Keyframe(0, 0), new Keyframe(5, 0), new Keyframe(5, -1));
    //                enemylist[i].SetSpeedCurve(speed, dirx, null);
    //            }
    //        }
    //        if (i >= 24 && i < 42) { //第三波
    //            if (enemylist[i].BaseName == Note.ObjectType.EnemyA2.ToString()) { //绿色敌人

    //            }
    //            else if (enemylist[i].BaseName == Note.ObjectType.EnemyA3.ToString()) { //黄色敌人

    //            }
    //        }
    //        if (GlobalData.RankLevel == GlobalData.GameRank.Easy)
    //        {
    //            enemylist[i].SetMainShooterBullet(1, AudioManager.EveryBeatsLenght / 2, -1, -1); //设置各种难度敌人的子弹发射属性
    //        }
    //        else {
    //            enemylist[i].SetMainShooterBullet(5 * (int)GlobalData.RankLevel, AudioManager.EveryBeatsLenght / (int)GlobalData.RankLevel, -1, -1); //设置各种难度敌人的子弹发射属性
    //        }
    //        if (enemylist[i].BaseName == Note.ObjectType.BossElf01.ToString()) {
    //            enemylist[i].SetPlaneShootByRhythm(this);
    //        }
    //    }
    //}

	// Update is called once per frame
	void Update () {
        base.Update();
        //UpdataEnemy();
        //UpdateStageRhythmEvent();
	}
}

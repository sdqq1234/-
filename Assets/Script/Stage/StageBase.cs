using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageBase : MonoBehaviour{

    public StageManager manager;//管理类
    public AudioManager StageAudioManager;//当前关卡音乐播放器
    public SongData stageSondData;//当前关卡数据
    //public AudioClip stageBgm; //当前关卡背景音乐

    public List<EnemyBase> enemylist = new List<EnemyBase>(); //当前关卡所有的敌人
    //public List<MyPlane> myPlaneList = new List<MyPlane>(); //当前关卡所有的自机
    public MyPlane myPlane; //当前关卡的自机
    public List<EnemyBase> bosslist = new List<EnemyBase>(); //当前关卡所有的Boss
    public List<ItemBase> itemList = new List<ItemBase>();//当前场景里所有的普通道具
    

    protected List<Note> EventNoteList = new List<Note>();//所有的事件节点\
    protected List<Note> EnemyNoteList = new List<Note>();//所有敌人节点
    public delegate void StageBulletEventDelegate(); //场景节奏弹幕发射委托
    public event StageBulletEventDelegate StageBulletRhythmEvent;//根据节奏进行发射弹幕事件

    //public delegate void StageBulletCollisionDelegate();//场景物体碰撞委托
    //public event StageBulletCollisionDelegate StageBulletCollisionEvent;//场景物体碰撞事件

    public virtual void Start(){
        StageAudioManager.SetSongPlayer(stageSondData);
    }

    protected void UpdataEnemy()
    {
        foreach (EnemyBase eb in enemylist)
        {
            if (eb.activeTime < StageAudioManager.StageSongPlayer.GetCurrentBeat())
            {
                eb.gameObject.SetActive(true);
                
            }
            
        }
        for (int i = enemylist.Count - 1; i >= 0; i--)
        {
            if (enemylist[i].isDead)
            {
                AudioManager.AddDeadSound(enemylist[i].deadSound);
                enemylist[i].deletStageRhythmEvent(this);
                GameObject.Destroy(enemylist[i].gameObject);
                enemylist.Remove(enemylist[i]);
            }
            else if (enemylist[i].gameObject != null && enemylist[i].gameObject.activeSelf && enemylist[i].activeTime > 0)
            {   //飞出屏幕就销毁
                if (!enemylist[i].CheckInScreen())
                {
                    enemylist[i].deletStageRhythmEvent(this);
                    GameObject.Destroy(enemylist[i].gameObject);  
                    enemylist.Remove(enemylist[i]);
                }
            }
        }
    }

    /// <summary>
    /// 创建一个敌人
    /// </summary>
    /// <param name="name"></param>
    /// <param name="time">关卡中出现的时机</param>
    /// <param name="orgPos">出现的位置</param>
    /// <param name="color">颜色</param>
    /// <param name="speed">初始速度值</param>
    /// <param name="speedDir">初始速度方向</param>
    /// <param name="speedCurve">速度值曲线</param>
    /// <param name="DirXCurve">速度x方向曲线</param>
    /// <param name="DirYCurve">速度y方向曲线</param>
    protected void CreateEnemy(string name,float time, Vector2 orgPos,Color color, float speed,Vector2 speedDir ,AnimationCurve speedCurve, AnimationCurve DirXCurve, AnimationCurve DirYCurve,bool isShootByRhythm,int bulletCount)
    {
        GameObject enemy = GameObject.Instantiate(Resources.Load(CommandString.EnemyPrefabPath + name)) as GameObject;
        enemy.name = name + "_Num" + time;
        enemy.transform.parent = Camera.main.gameObject.transform;
        enemy.transform.localScale = Vector3.one;
        enemy.transform.position = Vector3.zero;
        EnemyBase enemyBase = enemy.GetComponent<EnemyBase>();
        //SetBaseEnemy(EnemyNoteList[i], enemyBase);
        //enemyBase.SetBaseName(name);
        enemyBase.setDeadValue(null, color);
        //enemyBase.Speed_CurValue = speed;
        //enemyBase.Dir_CurSpeed = speedDir;
        enemyBase.activeTime = time - 1;
        //enemyBase.SetSpeedCurve(speedCurve, DirXCurve, DirYCurve);
        enemyBase.SetMainShooterBullet(bulletCount);
        enemy.gameObject.transform.position = orgPos;
        enemy.SetActive(false);
        if (isShootByRhythm) {
            enemyBase.SetPlaneShootByRhythm(this);
        }
        
        enemylist.Add(enemyBase);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name">名称</param>
    /// <param name="AppearTime">出现时间点</param>
    /// <param name="orgPos">出现位置</param>
    /// <param name="color">颜色</param>
    /// <param name="InTweenTime">进入屏幕到达目标位置的动画时间</param>
    /// <param name="outTweenTime">出屏幕到达目标位置的动画时间</param>
    /// <param name="waitTime">屏幕里等待时间</param>
    /// <param name="InTargetPos">进入屏幕的目标位置</param>
    /// <param name="OutTargetPos">出屏幕的目标位置</param>
    /// <param name="isShootByRhythm">是否按照节奏射击</param>
    /// <param name="bulletCount">子弹个数</param>
    protected void CreateEnemy(string name, float AppearTime, Vector2 orgPos, Color color, float InTweenTime, float OutTweenTime,float waitTime,Vector3 InTargetPos,Vector3 OutTargetPos, bool isShootByRhythm, int bulletCount)
    {
        GameObject enemy = GameObject.Instantiate(Resources.Load(CommandString.EnemyPrefabPath + name)) as GameObject;
        enemy.name = name + "_Num" + AppearTime;
        enemy.transform.parent = Camera.main.gameObject.transform;
        enemy.transform.localScale = Vector3.one;
        enemy.transform.position = Vector3.zero;
        
        
        EnemyBase enemyBase = enemy.GetComponent<EnemyBase>();
        enemyBase.setDeadValue(null, color);
        enemyBase.activeTime = AppearTime - 1;
        enemyBase.SetMainShooterBullet(bulletCount);
        enemy.gameObject.transform.position = orgPos;
        if (enemyBase.enemyFsm != null)
        {
            enemyBase.enemyFsm.Fsm.Variables.FindFsmVector3("InPosition").Value = InTargetPos;
            enemyBase.enemyFsm.Fsm.Variables.FindFsmVector3("OutPosition").Value = OutTargetPos;
            enemyBase.enemyFsm.Fsm.Variables.FindFsmFloat("WaitTime").Value = waitTime;
            enemyBase.enemyFsm.Fsm.Variables.FindFsmFloat("InTweenTime").Value = InTweenTime;
            enemyBase.enemyFsm.Fsm.Variables.FindFsmFloat("OutTweenTime").Value = OutTweenTime;
        }
        enemy.SetActive(false);
        
        if (isShootByRhythm)
        {
            enemyBase.SetPlaneShootByRhythm(this);
        }

        enemylist.Add(enemyBase);
    }


    /// <summary>
    /// 创建一个boss
    /// </summary>
    /// <param name="name">名称</param>
    /// <param name="AppearTime">出现时间点</param>
    /// <param name="orgPos">出现位置</param>
    /// <param name="color">颜色</param>
    /// <param name="InTweenTime">进入屏幕到达目标位置的动画时间</param>
    /// <param name="waitTime">每次转换位置的时间间隔</param>
    /// <param name="InTargetPos">进入屏幕的目标位置</param>
    /// <param name="isShootByRhythm">是否按照节奏射击</param>
    protected void CreateBoss(string name, float AppearTime, Vector2 orgPos, Color color, float InTweenTime, float waitTime,Vector3 InTargetPos, bool isShootByRhythm)
    {
        GameObject enemy = GameObject.Instantiate(Resources.Load(CommandString.EnemyPrefabPath + name)) as GameObject;
        enemy.name = name + "_Num" + AppearTime;
        enemy.transform.parent = Camera.main.gameObject.transform;
        enemy.transform.localScale = Vector3.one;
        enemy.transform.position = Vector3.zero;
        
        EnemyBase enemyBase = enemy.GetComponent<EnemyBase>();
        enemyBase.setDeadValue(null, color);
        enemyBase.activeTime = AppearTime - 1;
        //enemyBase.SetMainShooterBullet(bulletCount);
        enemy.gameObject.transform.position = orgPos;
        if (enemyBase.enemyFsm != null)
        {
            enemyBase.enemyFsm.Fsm.Variables.FindFsmVector3("InPosition").Value = InTargetPos;
            enemyBase.enemyFsm.Fsm.Variables.FindFsmFloat("WaitTime").Value = waitTime;
            enemyBase.enemyFsm.Fsm.Variables.FindFsmFloat("InTweenTime").Value = InTweenTime;
        }
        enemy.SetActive(false);

        if (isShootByRhythm)
        {
            enemyBase.SetPlaneShootByRhythm(this);
        }

        enemylist.Add(enemyBase);
    }

    protected void UpdateItemInScreen() {
        //foreach (ItemBase item in itemList) {
        //    if (item.gameObject.transform.position.y < GlobalData.screenBottomPoint.y) {
        //        GameObject.Destroy(item.gameObject);
        //        itemList.Remove(item);
        //        break;
        //    }
        //    if (item.isBeRecovered) {
        //        AudioManager.AddItemGetSound(item.sound);
        //        item.giveValue(myPlane);
                
        //        GameObject.Destroy(item.gameObject);
        //        itemList.Remove(item);
        //        break;
        //    }
            
        //}
        if (MyPlane.MyPos.y > GlobalData.RecoverLinePoint.y)
        {
            StageManager.CurStage.myPlane.isOverLineCover = true;
        }
        else
        {
            StageManager.CurStage.myPlane.isOverLineCover = false;
        }
    }

    //初始化关卡道中所有事件
    protected void InitStageNoteInRoad()
    {
        for (int i = 0; i < StageAudioManager.StageSongPlayer.Song.Notes.Count; i++)
        {

            if (StageAudioManager.StageSongPlayer.Song.Notes[i].StageObjectType == Note.ObjectType.EnmeyBullet
                || StageAudioManager.StageSongPlayer.Song.Notes[i].StageObjectType == Note.ObjectType.Command_ShowStageTitle)
            {
                EventNoteList.Add(StageAudioManager.StageSongPlayer.Song.Notes[i]);
                //break;
            }
            else //不是事件就是敌人
            {
                EnemyNoteList.Add(StageAudioManager.StageSongPlayer.Song.Notes[i]);
                
            }
        }
    }

    protected void UpdateStageRhythmEvent()
    {
        foreach (Note n in EventNoteList)
        {
            if (n.Time - 1.25 <= StageAudioManager.StageSongPlayer.GetCurrentBeat())
            {
                switch (n.StageObjectType) { 
                    case Note.ObjectType.EnmeyBullet:
                        if (StageBulletRhythmEvent != null) {
                            StageBulletRhythmEvent();
                        }
                        break;
                    case Note.ObjectType.Command_ShowStageTitle:
                        manager.SetStageState(StageManager.enum_stageState.STATE_InRoad);
                        break;
                        
                }
                EventNoteList.Remove(n);
                break;
            }
        }
    }

    //protected void UpdateStageBulletEvent(ObjectBase CollisionOjb) {
    //    if (StageBulletCollisionEvent != null) {
    //        StageBulletCollisionEvent();
    //    }
    //}

    public virtual void Update() {
        UpdataEnemy();
        UpdateStageRhythmEvent();
        UpdateItemInScreen();
    }
}

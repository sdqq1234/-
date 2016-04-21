using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageManager : MonoBehaviour {

    //public AudioManager StageAudioManager;

    public Transform obj_ScreenRight;  //屏幕最右边
    public Transform obj_ScreenLeft;   //屏幕最左边
    public Transform obj_ScreenTop;     //屏幕最上边
    public Transform obj_ScreenBottom; //屏幕最下边
    public Transform obj_RecoverLine; //道具回收线
    

    public GameObject obj_RoadBackGround;//道中背景
    public GameObject obj_BossNormalBackGround;//boss普通战背景
    public GameObject obj_BossCardBackGround;//boss卡牌战背景

    private MeshRenderer[] meshRenders_InRoad; //道中里所有图层
    private MeshRenderer[] meshRenders_InBossNormal; //boss战普通战背景图层
    private MeshRenderer[] meshRenders_InBossCard;//boss战符卡战背景图层

    private float alphaChangeSpeed = 0.02f;//变换速度
    private float alpha_InRoad = 0;//道中背景层透明数
    private float alpha_InBossNormal = 0;//boss普通战背景透明度
    private float alpha_InBossCard = 0;//boss符卡战背景透明度

    private int StageIndex = 0;//当前关卡数
    //private SongData stageSondData;//当前关卡数据
    //private AudioClip stageBgm; //当前关卡背景音乐

    public List<StageBase> StageList = new List<StageBase>();//关卡管理器管理的所有关卡列表
    public static StageBase CurStage;
    //public delegate void stageEventDelegate(); //场景事件委托


    //public static List<EnemyBase> enemylist = new List<EnemyBase>();
    //public static List<MyPlane> myPlaneList = new List<MyPlane>();

    public enum enum_stageState {
        STATE_InBeforeRoad, //进入道中前
        STATE_InRoad, //道中
        STATE_InBossNormal,//boss非符号
        STATE_InBossCard //boss符卡
    }

    public enum_stageState stageState;

    void Awake() {
        FindGameObjectByName();
        InitStage();
    }

	// Use this for initialization
	void Start () {
        
	}

    void FindGameObjectByName() {
        if (obj_RoadBackGround != null) {
            meshRenders_InRoad = obj_RoadBackGround.transform.GetComponentsInChildren<MeshRenderer>();
        }
        if (obj_BossNormalBackGround != null) {
            meshRenders_InRoad = obj_BossNormalBackGround.transform.GetComponentsInChildren<MeshRenderer>();
        }
        if (obj_BossCardBackGround != null)
        {
            meshRenders_InBossCard = obj_BossCardBackGround.transform.GetComponentsInChildren<MeshRenderer>();
        }
    }

    void PlayEnemyBulletAudio() {
        
    }

    //初始化关卡道中所有敌人
    //void InitStageEnemyInRoad() {
    //    for (int i = 0; i < StageAudioManager.StageSongPlayer.Song.Notes.Count; i++)
    //    {
    //        string enmeyName = StageAudioManager.StageSongPlayer.Song.Notes[i].StageObjectType.ToString();
    //        if (StageAudioManager.StageSongPlayer.Song.Notes[i].StageObjectType == Note.ObjectType.EnmeyBullet
    //            || StageAudioManager.StageSongPlayer.Song.Notes[i].StageObjectType == Note.ObjectType.Command_ShowStageTitle)
    //        {
    //            //break;
    //        }
    //        else {
    //            GameObject enemy = GameObject.Instantiate(Resources.Load(CommandString.EnemyPrefabPath + enmeyName)) as GameObject;
    //            enemy.name = enmeyName + "_Num" + i;
    //            enemy.transform.parent = Camera.main.gameObject.transform;
    //            enemy.transform.localScale = Vector3.one;
    //            enemy.transform.position = Vector3.zero;
    //            EnemyBase enemyBase = enemy.GetComponent<EnemyBase>();
    //            enemyBase.Dir_CurSpeed = -Vector2.up;
    //            enemyBase.Speed_Cur = 80;
    //            enemyBase.SetSpeedCurve(StageAudioManager.StageSongPlayer.Song.Notes[i].Speed_Curve, StageAudioManager.StageSongPlayer.Song.Notes[i].DirX_Curve, StageAudioManager.StageSongPlayer.Song.Notes[i].DirY_Curve);
    //            enemyBase.setDeadValue(null, StageAudioManager.StageSongPlayer.Song.Notes[i].StageColor);
    //            Vector2 appearPos = GlobalData.ScreenZeroPoint + StageAudioManager.StageSongPlayer.Song.Notes[i].AppearPos;
    //            enemyBase.SetAppear(StageAudioManager.StageSongPlayer.Song.Notes[i].Time - 1, appearPos);
    //            enemy.SetActive(false);
    //            enemylist.Add(enemyBase);
    //        }
            
            
    //    }
    //}

    ////初始化关卡音乐数据
    //void InitStageBgmData() {
    //    switch (StageIndex) { 
    //        case 1:
    //            stageBgm = Resources.Load(CommandString.BGMPath + "春色小路 ～ ｔhe Colorful Alley") as AudioClip;
    //            break;
    //        case 2:
    //            break;
    //        case 3:
    //            break;
    //        case 4:
    //            break;
    //        case 5:
    //            break;
    //        case 6:
    //            break;
    //    }
    //}

    //初始化关卡
    void InitStage() {
        
        stageState = enum_stageState.STATE_InBeforeRoad;
        GlobalData.SetScreenRect(obj_ScreenLeft.position,obj_ScreenTop.position,obj_ScreenRight.position,obj_ScreenBottom.position,obj_RecoverLine.position);
        CurStage = StageList[StageIndex];
        CurStage.manager = this;
        //InitStageEnemyInRoad();
        //stageSoundPlayer.Play();
    }

    public void SetStageState(enum_stageState state) {
        stageState = state;
    }

    void OnGUI() { 
        //if(GUILayout.Button(50,))
    }

    //设置战斗背景透明度
    void SetBackGourndAlpha(MeshRenderer[] renders,float alpha) {
        if (renders != null) {
            foreach (MeshRenderer mr in renders)
            {
                Color curColor = mr.material.GetColor("_Color");
                curColor.a = alpha;
                mr.material.SetColor("_Color", curColor);
            }
        }
        
    }

    //void UpdataEnemy() {
    //    foreach (EnemyBase eb in enemylist)
    //    {
    //        if (eb.activeTime < StageAudioManager.StageSongPlayer.GetCurrentBeat())
    //        {
    //            eb.gameObject.SetActive(true);
                
    //        }
    //    }
    //    for (int i = enemylist.Count - 1; i >= 0; i--) {
    //        if (enemylist[i].isDead)
    //        {
    //            AudioManager.AddDeadSound(enemylist[i].deadSound);
    //            GameObject.Destroy(enemylist[i].gameObject);
    //            enemylist.Remove(enemylist[i]);

    //        }
    //    }

    //}

	// Update is called once per frame
	void Update () {
        //UpdataEnemy();
        //if (PlaneDeadEvent != null) {

        //    PlaneDeadEvent(deadAudioClip);
        //}
        //PlayDeadSound();
        switch (stageState)
        {
            case enum_stageState.STATE_InBeforeRoad:
                alpha_InRoad = 0;
                alpha_InBossNormal = 0;
                alpha_InBossCard = 0;
                break;
            case enum_stageState.STATE_InRoad:
                //RenderSettings.fog = true;
                alpha_InRoad = Mathf.Lerp(alpha_InRoad, 1, alphaChangeSpeed);
                alpha_InBossNormal = Mathf.Lerp(alpha_InBossNormal, 0, alphaChangeSpeed);
                alpha_InBossCard = Mathf.Lerp(alpha_InBossCard, 0, alphaChangeSpeed);
                break;
            case enum_stageState.STATE_InBossNormal:
                alpha_InRoad = Mathf.Lerp(alpha_InRoad, 0, alphaChangeSpeed);
                alpha_InBossNormal = Mathf.Lerp(alpha_InBossNormal, 1, alphaChangeSpeed);
                alpha_InBossCard = Mathf.Lerp(alpha_InBossCard, 0, alphaChangeSpeed);
                break;
            case enum_stageState.STATE_InBossCard:
                //RenderSettings.fog = false;
                alpha_InRoad = Mathf.Lerp(alpha_InRoad, 0, alphaChangeSpeed);
                alpha_InBossNormal = Mathf.Lerp(alpha_InBossNormal, 0, alphaChangeSpeed);
                alpha_InBossCard = Mathf.Lerp(alpha_InBossCard, 1, alphaChangeSpeed);
                break;
        }
            
        SetBackGourndAlpha(meshRenders_InRoad, alpha_InRoad);
        SetBackGourndAlpha(meshRenders_InBossNormal,alpha_InBossNormal);
        SetBackGourndAlpha(meshRenders_InBossCard,alpha_InBossCard);
	}
}

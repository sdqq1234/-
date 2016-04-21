using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//游戏场景数据显示
public class StageInfoManager : MonoBehaviour {

    public GameObject RankTitleShow;//飞行场景里显示的难度
    public GameObject ItemGetLine;//道具获取线
    public UILabel RankLevel;//难度
    public UILabel HiScore;//最大分数纪录
    public UILabel CurScore;//当前分数 
    public GameObject obj_PlayerPlaneBox;//玩家飞机数量显示
    public GameObject obj_SpellBox;//炸弹数量
    public UILabel RedPoint;//当前玩家红点值
    public UILabel BluePoint;//蓝点值
    public UILabel GreenPoint;//绿点值
    public UILabel GrazePoint;//擦弹数
    public UILabel FPS;//当前fps

    private UISprite[] PlayerList;//要显示的玩家飞机数量
    private UISprite[] SpellList;//要显示的炸弹数量
    private SpriteRenderer cur_RankShow;//当前飞行场景里显示的难度
    private float RankShowTime = 2;//道具线和难度显示的时间
    private float curShowTime = 0;//当前显示时间
    //private SpriteRenderer[] RankShowArray;//难度显示的数组

    //private List<UISprite> PlayerList = new List<UISprite>();//要显示的玩家飞机数量
    //private List<UISprite> SpellList = new List<UISprite>();//要显示的炸弹数量
	// Use this for initialization
	void Start () {
        FindObjectByName();
        Init();
	}

    void FindObjectByName() {
        //RankShowArray = RankTitleShow.GetComponentsInChildren<SpriteRenderer>();
    }

    /// <summary>
    /// 获取当前要显示的难度
    /// </summary>
    /// <param name="rankStr"></param>
    /// <returns></returns>
    private SpriteRenderer getCurRankRender(string rankStr) {
        SpriteRenderer[] RankShowArray = RankTitleShow.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sr in RankShowArray) {
            sr.gameObject.SetActive(false);
        }

        for (int i = 0; i < RankShowArray.Length; i++) {
            
            if (RankShowArray[i].gameObject.name == rankStr)
            {
                RankShowArray[i].gameObject.SetActive(true);
                return RankShowArray[i];
            }
        }
        return null;
    }

    void Init() {
        curShowTime = 0;
        SetRankLevel();
        InitItemLine();
        PlayerList = obj_PlayerPlaneBox.transform.GetComponentsInChildren<UISprite>();
        SpellList = obj_SpellBox.transform.GetComponentsInChildren<UISprite>();
        for (int i = 0; i < PlayerList.Length; i++) {
            if (i < GlobalData.PlaneCount)
            {
                PlayerList[i].color = Color.white;
            }
            else {
                PlayerList[i].color = Color.black;
            }
        }
        for (int i = 0; i < SpellList.Length; i++)
        {
            if (i < GlobalData.BombCount)
            {
                SpellList[i].color = Color.white;
            }
            else
            {
                SpellList[i].color = Color.black;
            }
        }
    }

    //初始化道具线
    void InitItemLine() {
        SpriteRenderer[] ItemLineArray = ItemGetLine.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sr in ItemLineArray)
        {
            sr.gameObject.SetActive(true);
        }
    }

    void SetRankLevel() {
        switch (GlobalData.RankLevel) { 
            case GlobalData.GameRank.Easy:
                RankLevel.text = "Easy";
                break;
            case GlobalData.GameRank.Normal:
                RankLevel.text = "Normal";
                break;
            case GlobalData.GameRank.Hard:
                RankLevel.text = "Hard";
                break;
            case GlobalData.GameRank.Lunatic:
                RankLevel.text = "Lunatic";
                break;
        }
        cur_RankShow = getCurRankRender(RankLevel.text);
    }

    int fps;
    float timeA;
    float lastFPS;
    void showFPS() {
        if (Time.timeSinceLevelLoad - timeA <= 1)
        {
            fps++;
        }
        else
        {

            lastFPS = fps + 1;

            timeA = Time.timeSinceLevelLoad;

            fps = 0;

        }
        FPS.text = lastFPS.ToString();
    }

    /// <summary>
    /// 更新道具线和屏幕顶端的难度显示
    /// </summary>
    void UpdateRankShow() {
        if (curShowTime < RankShowTime)
        {
            curShowTime += Time.deltaTime;
        }
        else {
            cur_RankShow.gameObject.SetActive(false);
            SpriteRenderer[] ItemLineArray = ItemGetLine.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer sr in ItemLineArray) {
                sr.gameObject.SetActive(false);
            }
        }
    }

	// Update is called once per frame
	void Update () {
        MyPlane plane = StageManager.CurStage.myPlane;
        showFPS();
        RedPoint.text = (plane.Power / 100f).ToString();
        GreenPoint.text = (plane.GreenPoint).ToString();
        BluePoint.text = (plane.BluePoint).ToString();
        CurScore.text = (plane.Score).ToString();
        HiScore.text = (plane.HiScore).ToString();
        GrazePoint.text = (plane.Graze).ToString();
        UpdateRankShow();
	}
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StageInfoManager1 : MonoBehaviour {

    //public GameObject RankTitleShow;//飞行场景里显示的难度
    public GameObject ItemGetLine;//道具获取线
    public Text Text_RankLevel;//难度
    public Text Text_HiScore;//最大分数纪录
    public Text Text_CurScore;//当前分数 
    public GameObject obj_PlayerPlaneBox;//玩家飞机数量显示
    public GameObject obj_SpellBox;//炸弹数量
    public Text Text_RedPoint;//当前玩家红点值
    private float Max_RedPoint = 4.00f;
    public Text Text_BluePoint;//蓝点值
    private int Max_BluePoint = 300;
    public Text Text_GreenPoint;//绿点值
    public Text Text_GrazePoint;//擦弹数
    public Text Text_FPS;//当前fps

    private Image[] PlayerList;//要显示的玩家飞机数量
    private Image[] SpellList;//要显示的炸弹数量
    private SpriteRenderer cur_RankShow;//当前飞行场景里显示的难度
    private float RankShowTime = 2;//道具线和难度显示的时间
    private float curShowTime = 0;//当前显示时间
    //private SpriteRenderer[] RankShowArray;//难度显示的数组

    //private List<UISprite> PlayerList = new List<UISprite>();//要显示的玩家飞机数量
    //private List<UISprite> SpellList = new List<UISprite>();//要显示的炸弹数量
    // Use this for initialization
    void Start()
    {
        Init();
    }

    void Init()
    {
        curShowTime = 0;
        SetRankLevel();
        InitItemLine();
        PlayerList = obj_PlayerPlaneBox.transform.GetComponentsInChildren<Image>();
        SpellList = obj_SpellBox.transform.GetComponentsInChildren<Image>();
        for (int i = 0; i < PlayerList.Length; i++)
        {
            if (i < GlobalData.PlaneCount)
            {
                PlayerList[i].color = Color.white;
            }
            else
            {
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
    void InitItemLine()
    {
        SpriteRenderer[] ItemLineArray = ItemGetLine.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sr in ItemLineArray)
        {
            sr.gameObject.SetActive(true);
        }
    }

    void SetRankLevel()
    {
        switch (GlobalData.RankLevel)
        {
            case GlobalData.GameRank.Easy:
                Text_RankLevel.text = "Easy";
                break;
            case GlobalData.GameRank.Normal:
                Text_RankLevel.text = "Normal";
                break;
            case GlobalData.GameRank.Hard:
                Text_RankLevel.text = "Hard";
                break;
            case GlobalData.GameRank.Lunatic:
                Text_RankLevel.text = "Lunatic";
                break;
        }
        Text_RankLevel.text = "Easy";
        //cur_RankShow = getCurRankRender(Text_RankLevel.text);
    }

    int fps;
    float timeA;
    float lastFPS;
    void showFPS()
    {
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
        Text_FPS.text = lastFPS.ToString()+"fps";
    }

    /// <summary>
    /// 更新道具线和屏幕顶端的难度显示
    /// </summary>
    void UpdateRankShow()
    {
        if (curShowTime < RankShowTime)
        {
            curShowTime += Time.deltaTime;
        }
        else
        {
            //cur_RankShow.gameObject.SetActive(false);
            SpriteRenderer[] ItemLineArray = ItemGetLine.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer sr in ItemLineArray)
            {
                sr.gameObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        MyPlane plane = StageManager.CurStage.myPlane;
        showFPS();
        Text_RedPoint.text = (plane.Power / 100f).ToString() + " / " + Max_RedPoint;
        Text_GreenPoint.text = (plane.GreenPoint).ToString();
        Text_BluePoint.text = (plane.BluePoint).ToString() + " / " + Max_BluePoint;
        Text_CurScore.text = (plane.Score).ToString();
        Text_HiScore.text = (plane.HiScore).ToString();
        Text_GrazePoint.text = (plane.Graze).ToString();
        UpdateRankShow();
    }
}

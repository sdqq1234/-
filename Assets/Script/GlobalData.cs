using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//全局数据类 全游戏中不删除用来记录各种数据
public class GlobalData {

    public static Vector2 screenLeftPoint;

    public static Vector2 screenRightPoint;

    public static Vector2 screenTopPoint;

    public static Vector2 screenBottomPoint;

    public static Vector2 RecoverLinePoint;

    //public static short NextSceneID = -1;//下一个场景id
    //public static short CurSceneID = 0;//当前场景ID

    public enum GameRank {  //游戏难度
        Easy = 0,
        Normal,
        Hard,
        Lunatic
    }

    public static GameRank RankLevel = GameRank.Normal;

    public static float BgmVol = 1;//背景音乐音量大小
    public static float SoundVol = 1;//音效声音大小

    public static int PlaneCount = 3;//设定的飞机数量
    public static int BombCount = 3;//设定的炸弹数量
    public enum PlaneType {     //选择的飞机
        Aya,           //射命丸文
        Marisa,         //魔理沙
        Reimu           //灵梦
    }
    private static PlaneType MyPlaneName = PlaneType.Aya;

    public enum WeaponType { 
        AyaWeaponA,     //诱导子弹
        AyaWeaponB,     //前方集中
        MarisaWeaponA,
        MarisaWeaponB,
        ReimuWeaponA,
        ReimuWeaponB
    }

    //显示区域左上为零点
    public static Vector2 ScreenZeroPoint {
        get {
            float x = screenLeftPoint.x;
            float y = screenTopPoint.y;
            Vector2 zero = new Vector2(x,y);
            return zero;
        }
    }

    public static float ScreenWidth {
        get {
            return screenRightPoint.x - screenLeftPoint.x;
        }
    }

    public static float ScreenHeight {
        get {
            return screenTopPoint.y - screenBottomPoint.y;
        }
    }

    private static WeaponType weaponType = WeaponType.AyaWeaponA;

	// Use this for initialization
	void Start () {
	}

    //设置选择的飞机
    public static void SetPlaneName(PlaneType PlaneName) {
        MyPlaneName = PlaneName;
    }

    public static void SetGameRank(GameRank Rank) {
        RankLevel = Rank;
    }

    //设置武器
    public static void SetWeapon(WeaponType Weapon) {
        weaponType = Weapon;
    }

    /// <summary>
    /// 设置可操作屏幕大小
    /// </summary>
    /// <param name="left"></param>
    /// <param name="top"></param>
    /// <param name="right"></param>
    /// <param name="bottom"></param>
    /// <param name="recover">道具回收线</param>
    public static void SetScreenRect(Vector2 left,Vector2 top,Vector2 right,Vector2 bottom,Vector2 recover) {
        screenBottomPoint = bottom;
        screenTopPoint = top;
        screenLeftPoint = left;
        screenRightPoint = right;
        RecoverLinePoint = recover;
    }
    ////读取下一个场景
    //public static void loadNextScene(short LoadSceneID) {
    //    CurSceneID = (short)Application.loadedLevel;
    //    NextSceneID = LoadSceneID;
    //}

	// Update is called once per frame
	void Update () {
	}
}

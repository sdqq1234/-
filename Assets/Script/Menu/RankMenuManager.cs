using UnityEngine;
using System.Collections;

public class RankMenuManager : MonoBehaviour {

    public UIButton Easy;
    public UIButton Normal;
    public UIButton Hard;
    public UIButton Lunatic;
    private MenuManager menuMangaer;

	// Use this for initialization
	void Start () {
        menuMangaer = GameObject.Find("2DUI Root").GetComponent<MenuManager>();
        UIEventListener.Get(Easy.gameObject).onClick += ClickEasy;
        UIEventListener.Get(Normal.gameObject).onClick += ClickNormal;
        UIEventListener.Get(Hard.gameObject).onClick += ClickHard;
        UIEventListener.Get(Lunatic.gameObject).onClick += ClickLunatic;
	}

    void ClickEasy(GameObject sender) {
        GlobalData.SetGameRank(GlobalData.GameRank.Easy);
        menuMangaer.InitNextState(MenuManager.MenuState.PlayerSelectTitle);
    }

    void ClickNormal(GameObject sender)
    {
        GlobalData.SetGameRank(GlobalData.GameRank.Normal);
        menuMangaer.InitNextState(MenuManager.MenuState.PlayerSelectTitle);
    }

    void ClickHard(GameObject sender)
    {
        GlobalData.SetGameRank(GlobalData.GameRank.Hard);
        menuMangaer.InitNextState(MenuManager.MenuState.PlayerSelectTitle);
    }

    void ClickLunatic(GameObject sender)
    {
        GlobalData.SetGameRank(GlobalData.GameRank.Lunatic);
        menuMangaer.InitNextState(MenuManager.MenuState.PlayerSelectTitle);
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(1))
        { //右键的话则返回主菜单
            menuMangaer.InitNextState(MenuManager.MenuState.MainTitle);
            menuMangaer.ReturnToBeforeState();
        }
	}
}

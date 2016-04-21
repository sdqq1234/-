using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TitleMenuManager : MonoBehaviour {
    public MenuManager menuMangaer;
    public List<UIButton> TitleButtonList = new List<UIButton>();

	// Use this for initialization
	void Start () {
        menuMangaer = GameObject.Find("2DUI Root").GetComponent<MenuManager>();
        foreach (UIButton button in TitleButtonList) {
            UIEventListener.Get(button.gameObject).onClick += ClickTitleButton;
        }
	}

    //初始化菜单状态
    void Init() { 

    }

    //点击按钮
    void ClickTitleButton(GameObject sender) {
        switch (sender.name) { 
            case "Start":
                menuMangaer.InitNextState(MenuManager.MenuState.RankTitle);
                break;
            case "Replay":
                menuMangaer.InitNextState(MenuManager.MenuState.ReplayTitle);
                break;
            case "Practice":
                menuMangaer.InitNextState(MenuManager.MenuState.RankTitle);
                break;
            case "Music Room":
                menuMangaer.InitNextState(MenuManager.MenuState.MusicTitle);
                break;
            case "Player Data":
                menuMangaer.InitNextState(MenuManager.MenuState.PlayerDataTitle);
                break;
            case "Option":
                menuMangaer.InitNextState(MenuManager.MenuState.OptionTitle);
                break;
            case "Manual":
                menuMangaer.InitNextState(MenuManager.MenuState.ManaulTitle);
                break;
            case "Exit":
                Application.Quit();
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}

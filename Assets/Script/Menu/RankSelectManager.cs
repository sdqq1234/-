using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RankSelectManager : MonoBehaviour {

    public List<UIButton> TitleButtonList = new List<UIButton>();

    public enum enum_menuState
    {
        Easy,
        Normal,
        Hard,
        Lunatic
    }

    public enum_menuState RankMenu;

    // Use this for initialization
    void Start()
    {
        RankMenu = enum_menuState.Easy;
        foreach (UIButton button in TitleButtonList)
        {
            UIEventListener.Get(button.gameObject).onClick += ClickTitleButton;
        }
    }

    //初始化菜单状态
    void Init()
    {

    }

    //点击按钮
    void ClickTitleButton(GameObject sender)
    {
        switch (sender.name)
        {
            case "Easy":
                RankMenu = enum_menuState.Easy;
                break;
            case "Normal":
                RankMenu = enum_menuState.Normal;
                break;
            case "Hard":
                RankMenu = enum_menuState.Hard;
                break;
            case "Lunatic":
                RankMenu = enum_menuState.Lunatic;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

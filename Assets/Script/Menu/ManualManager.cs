using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ManualManager : MonoBehaviour
{

    private MenuManager menuMangaer;
    public UIButton GameTypeManual;//游戏类型说明
    public UIButton StoryManual;//故事说明
    public UIButton ControlManual;//操作说明
    public UIButton UIManual;//ui说明
    public UIButton ItemManual;//道具说明
    public UIButton ScoreManual;//分数说明

    public List<GameObject> contentList = new List<GameObject>();

	// Use this for initialization
	void Start () {
        menuMangaer = GameObject.Find("2DUI Root").GetComponent<MenuManager>();
        UIEventListener.Get(GameTypeManual.gameObject).onClick += ClickManual;
        UIEventListener.Get(StoryManual.gameObject).onClick += ClickManual;
        UIEventListener.Get(ControlManual.gameObject).onClick += ClickManual;
        UIEventListener.Get(UIManual.gameObject).onClick += ClickManual;
        UIEventListener.Get(ItemManual.gameObject).onClick += ClickManual;
        UIEventListener.Get(ScoreManual.gameObject).onClick += ClickManual;
	}

    void SetObjShowByName(string objName) {
        foreach (GameObject go in contentList) {
            if (go.name == objName) {
                go.SetActive(true);
                break;
            }
        }
    }

    //重置所有显示内容
    void ResetContent() {
        foreach (GameObject go in contentList) {
            go.SetActive(false);
        }
    }

    void ClickManual(GameObject sender) {
        ResetContent();
        switch (sender.name) {
            case "GameTypeManual":
                SetObjShowByName("GameTypeContent");
                break;
            case "StoryManual":
                SetObjShowByName("StoryContent");
                break;
            case "ControlManual":
                SetObjShowByName("ControlContent");
                break;
            case "UIManual":
                SetObjShowByName("UIContent");
                break;
            case "ItemManual":
                SetObjShowByName("ItemContent");
                break;
            case "ScoreManual":
                SetObjShowByName("ScoreContent");
                break;
        }
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

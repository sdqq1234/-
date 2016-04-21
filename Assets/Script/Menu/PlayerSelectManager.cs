using UnityEngine;
using System.Collections;

public class PlayerSelectManager : MonoBehaviour {

    public GameObject PlayerList;
    //public UIButton PlayerInfo;
    private MenuManager menuMangaer;
	// Use this for initialization
	void Start () {
        menuMangaer = GameObject.Find("2DUI Root").GetComponent<MenuManager>();
        UIButton[] Charactors = PlayerList.transform.GetComponentsInChildren<UIButton>();
        foreach (UIButton button in Charactors)
        {
            UIEventListener.Get(button.gameObject).onClick += ClickPlayerInfo;
        }
        
	}

    void ClickPlayerInfo(GameObject sender) {
        switch(sender.name){
            case "AyaName":
                menuMangaer.InitNextState(MenuManager.MenuState.WeaponSelectTitle);
                break;
        }
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(1))
        { //右键的话则返回主菜单
            menuMangaer.InitNextState(MenuManager.MenuState.RankTitle);
            menuMangaer.ReturnToBeforeState();
        }
	}
}

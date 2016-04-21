using UnityEngine;
using System.Collections;

public class ReplayManager : MonoBehaviour {

    private MenuManager menuMangaer;
	// Use this for initialization
	void Start () {
        menuMangaer = GameObject.Find("2DUI Root").GetComponent<MenuManager>();
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

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponSelectManager : MonoBehaviour {

    public List<GameObject> WeaponList = new List<GameObject>();//武器列表
    public Transform ShowPoint;//显示当前武器位置
    public Transform HideRightPoint;//影藏武器的右边位置
    public Transform HideLeftPoint;//左边位置
    //UIButton[] Weapons;
    private int weaponIndex = 0;//武器索引
    private GameObject SelectedWeapon;//当前选择的武器
    private GameObject PreSelectWeapon;//上一次选择的武器
    private Transform CurHidePosition;//当前要隐藏的位置
    //public UIButton PlayerInfo;
    private MenuManager menuMangaer;
	// Use this for initialization
	void Start () {
        menuMangaer = GameObject.Find("2DUI Root").GetComponent<MenuManager>();
        Init();
	}

    /// <summary>
    /// 重置所有武器显示位置
    /// </summary>
    void ResetWeaponPos(Transform HidePoint)
    {
        foreach (GameObject go in WeaponList)
        {
            TweenTransform tt = go.GetComponent<TweenTransform>();
            tt.from = HidePoint;
            tt.to = HidePoint;
            tt.PlayForward();
        }
    }

    private void SetSelectedWeapon(Transform hidePos) {
        //ResetWeaponPos(hidePos);
        SelectedWeapon = WeaponList[weaponIndex];
        
        if (PreSelectWeapon == null) {
            PreSelectWeapon = SelectedWeapon;
        }
        TweenTransform cur_tt = SelectedWeapon.GetComponent<TweenTransform>();
        
        //TweenAlpha ta = SelectedWeapon.GetComponent<TweenAlpha>();
        cur_tt.from = hidePos;
        cur_tt.to = ShowPoint;
        cur_tt.ResetToBeginning();
        cur_tt.PlayForward();

        TweenTransform pre_tt = PreSelectWeapon.GetComponent<TweenTransform>();
        pre_tt.from = ShowPoint;
        pre_tt.to = hidePos;
        pre_tt.ResetToBeginning();
        pre_tt.PlayForward();
        
    }

    /// <summary>
    /// 下一个武器
    /// </summary>
    public void NextWeapon() {
        PreSelectWeapon = WeaponList[weaponIndex];
        if (weaponIndex < WeaponList.Count-1)
        {
            weaponIndex++;
        }
        else {
            weaponIndex = 0;
        }
        //SetSelectedWeapon(HideLeftPoint);
        SelectedWeapon = WeaponList[weaponIndex];

        if (PreSelectWeapon == null)
        {
            PreSelectWeapon = SelectedWeapon;
        }
        TweenTransform cur_tt = SelectedWeapon.GetComponent<TweenTransform>();

        //TweenAlpha ta = SelectedWeapon.GetComponent<TweenAlpha>();
        cur_tt.from = HideLeftPoint;
        cur_tt.to = ShowPoint;
        cur_tt.ResetToBeginning();
        cur_tt.PlayForward();

        TweenTransform pre_tt = PreSelectWeapon.GetComponent<TweenTransform>();
        pre_tt.from = ShowPoint;
        pre_tt.to = HideRightPoint;
        pre_tt.ResetToBeginning();
        pre_tt.PlayForward();
    }

    /// <summary>
    /// 上一个武器
    /// </summary>
    public void PreWeapon()
    {
        PreSelectWeapon = WeaponList[weaponIndex];
        if (weaponIndex > 0)
        {
            weaponIndex--;
        }
        else {
            weaponIndex = WeaponList.Count - 1;
        }

        //SetSelectedWeapon(HideRightPoint);
        SelectedWeapon = WeaponList[weaponIndex];

        if (PreSelectWeapon == null)
        {
            PreSelectWeapon = SelectedWeapon;
        }
        TweenTransform cur_tt = SelectedWeapon.GetComponent<TweenTransform>();

        //TweenAlpha ta = SelectedWeapon.GetComponent<TweenAlpha>();
        cur_tt.from = HideRightPoint;
        cur_tt.to = ShowPoint;
        cur_tt.ResetToBeginning();
        cur_tt.PlayForward();

        TweenTransform pre_tt = PreSelectWeapon.GetComponent<TweenTransform>();
        pre_tt.from = ShowPoint;
        pre_tt.to = HideLeftPoint;
        pre_tt.ResetToBeginning();
        pre_tt.PlayForward();
    }

    void Init() {
        //ResetWeaponPos();
        foreach (GameObject go in WeaponList) {
            UIEventListener.Get(go).onClick += ClickWeaponInfo;
        }
    }

    void ClickWeaponInfo(GameObject sender) {
        switch (sender.name)
        {
            case "WeaponA":
                GlobalData.SetWeapon(GlobalData.WeaponType.AyaWeaponA);
                Debug.Log("选择了武器A");
                break;
            case "WeaponB":
                GlobalData.SetWeapon(GlobalData.WeaponType.AyaWeaponB);
                Debug.Log("选择了武器B");
                break;
        }
        menuMangaer.LoadingManager.loadNextScene(CommandString.GameStageSceneID);
        
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(1))
        { //右键的话则返回主菜单
            menuMangaer.InitNextState(MenuManager.MenuState.PlayerSelectTitle);
            menuMangaer.ReturnToBeforeState();
        }
	}
}

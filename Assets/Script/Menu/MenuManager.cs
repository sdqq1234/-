using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {

    float ChangeSpeed = 5;//过度速度

    public UIPanel mainMenu; //主游戏界面
    public UIPanel RankMenu;//难度选择界面
    public UIPanel PlayerMenu;//选择飞机界面
    public UIPanel WeaponMenu;//选择武器界面
    public UIPanel ReplayMenu;//回放界面
    public UIPanel OptionMenu;//选项界面
    public UIPanel MusicMenu;//音乐界面
    public UIPanel PlayerDataMenu;//玩家数据界面
    public UIPanel ManaulMenu;//说明界面
    public UIButton RightArrow;//右箭头
    public UIButton LeftArrow;//左箭头
    public AudioSource MenuAudio;//界面背景音乐播放器
    public AudioClip ReturnClip;//返回音效
    public LoadingManager LoadingManager;//loading界面

    public enum MenuState { 
        MainTitle,          //主界面
        RankTitle,          //难度选择界面
        PlayerSelectTitle,  //飞机选择界面
        ReplayTitle,        //回放选择界面
        OptionTitle,        //设定界面
        MusicTitle,         //音乐界面
        PlayerDataTitle,    //玩家数据界面
        ManaulTitle,        //游戏说明界面
        WeaponSelectTitle   //武器选择界面
    }

    private MenuState state = MenuState.MainTitle;


	// Use this for initialization
	void Start () {
        InitNextState(state);
        UIEventListener.Get(RightArrow.gameObject).onClick += ClickRightArrow;
        UIEventListener.Get(LeftArrow.gameObject).onClick += ClickLeftArrow;
	}

    //重置游戏菜单状态
    void ResetState() {
        mainMenu.alpha = 1;
        mainMenu.gameObject.SetActive(false);
        RankMenu.alpha = 0;
        RankMenu.gameObject.SetActive(false);
        PlayerMenu.alpha = 0;
        PlayerMenu.gameObject.SetActive(false);
        WeaponMenu.alpha = 0;
        WeaponMenu.gameObject.SetActive(false);
        //RightArrow.defaultColor = new Color(RightArrow.defaultColor.r, RightArrow.defaultColor.g, RightArrow.defaultColor.b,0);
        RightArrow.gameObject.SetActive(false);
        //LeftArrow.defaultColor = new Color(LeftArrow.defaultColor.r, LeftArrow.defaultColor.g, LeftArrow.defaultColor.b, 0);
        LeftArrow.gameObject.SetActive(false);

        ReplayMenu.alpha = 0;
        ReplayMenu.gameObject.SetActive(false);
        OptionMenu.alpha = 0;
        OptionMenu.gameObject.SetActive(false);
        MusicMenu.alpha = 0;
        MusicMenu.gameObject.SetActive(false);
        PlayerDataMenu.alpha = 0;
        PlayerDataMenu.gameObject.SetActive(false);
        ManaulMenu.gameObject.SetActive(false);
        ManaulMenu.alpha = 0;

        TitleMenuManager title = mainMenu.gameObject.GetComponent<TitleMenuManager>();
        foreach (UIButton button in title.TitleButtonList)
        {
            button.collider.enabled = true;
        }
    }

    void SetAlphaMainMenu() {
        mainMenu.gameObject.SetActive(true);
        mainMenu.alpha = 0.3f;
        TitleMenuManager title = mainMenu.gameObject.GetComponent<TitleMenuManager>();
        foreach (UIButton button in title.TitleButtonList) {
            button.collider.enabled = false;
        }
    }

    /// <summary>
    /// 回到上一个界面播放的声音
    /// </summary>
    public void ReturnToBeforeState() {
        MenuAudio.PlayOneShot(ReturnClip);
    }

    /// <summary>
    /// 更新菜单状态 主要是一些过度效果
    /// </summary>
    void UpdataMenuState() {
        switch (state)
        {
            case MenuState.MainTitle:
                mainMenu.alpha = Mathf.Lerp(mainMenu.alpha,1,Time.deltaTime * ChangeSpeed);
                break;
            case MenuState.RankTitle:
                RankMenu.alpha = Mathf.Lerp(RankMenu.alpha,1,Time.deltaTime * ChangeSpeed);
                break;
            case MenuState.PlayerSelectTitle:
                PlayerMenu.alpha = Mathf.Lerp(PlayerMenu.alpha, 1, Time.deltaTime * ChangeSpeed);
                break;
            case MenuState.ReplayTitle:
                ReplayMenu.alpha = Mathf.Lerp(ReplayMenu.alpha, 1, Time.deltaTime * ChangeSpeed);
                break;
            case MenuState.OptionTitle:
                OptionMenu.alpha = Mathf.Lerp(OptionMenu.alpha, 1, Time.deltaTime * ChangeSpeed);
                break;
            case MenuState.MusicTitle:
                MusicMenu.alpha = Mathf.Lerp(MusicMenu.alpha, 1, Time.deltaTime * ChangeSpeed);
                break;
            case MenuState.PlayerDataTitle:
                PlayerDataMenu.alpha = Mathf.Lerp(PlayerDataMenu.alpha, 1, Time.deltaTime * ChangeSpeed);
                break;
            case MenuState.ManaulTitle:
                ManaulMenu.alpha = Mathf.Lerp(ManaulMenu.alpha, 1, Time.deltaTime * ChangeSpeed);
                break;
            case MenuState.WeaponSelectTitle:
                WeaponMenu.alpha = Mathf.Lerp(WeaponMenu.alpha, 1, Time.deltaTime * ChangeSpeed);
                break;
        }
    }

    //初始化每个状态
    public void InitNextState(MenuState nextState) {
        ResetState();
        state = nextState;
        switch (state) { 
            case MenuState.MainTitle:
                mainMenu.gameObject.SetActive(true);
                break;
            case MenuState.RankTitle:
                SetAlphaMainMenu();
                RankMenu.gameObject.SetActive(true);
                break;
            case MenuState.PlayerSelectTitle:
                PlayerMenu.gameObject.SetActive(true);
                RightArrow.gameObject.SetActive(true);
                LeftArrow.gameObject.SetActive(true);
                break;
            case MenuState.ReplayTitle:
                SetAlphaMainMenu();
                ReplayMenu.gameObject.SetActive(true);
                //设置replay画面
                break;
            case MenuState.OptionTitle:
                SetAlphaMainMenu();
                OptionMenu.gameObject.SetActive(true);
                break;
            case MenuState.MusicTitle:
                SetAlphaMainMenu();
                MusicMenu.gameObject.SetActive(true);
                break;
            case MenuState.PlayerDataTitle:
                SetAlphaMainMenu();
                PlayerDataMenu.gameObject.SetActive(true);
                break;
            case MenuState.ManaulTitle:
                SetAlphaMainMenu();
                ManaulMenu.gameObject.SetActive(true);
                break;
            case MenuState.WeaponSelectTitle:
                RightArrow.gameObject.SetActive(true);
                LeftArrow.gameObject.SetActive(true);
                WeaponMenu.gameObject.SetActive(true);
                break;
        }
    }

    void ClickRightArrow(GameObject sender) {
        switch (state) { 
            case MenuState.PlayerSelectTitle:
                Debug.Log("人物选择索引++");
                break;
            case MenuState.WeaponSelectTitle:
                WeaponSelectManager weaponManager = WeaponMenu.GetComponent<WeaponSelectManager>();
                weaponManager.NextWeapon();
                break;
        }
    }

    void ClickLeftArrow(GameObject sender)
    {
        switch (state)
        {
            case MenuState.PlayerSelectTitle:
                Debug.Log("人物选择索引--");
                break;
            case MenuState.WeaponSelectTitle:
                WeaponSelectManager weaponManager = WeaponMenu.GetComponent<WeaponSelectManager>();
                weaponManager.PreWeapon();
                break;
        }
    }

    void StartStage() { 

    }

	// Update is called once per frame
	void Update () {
        UpdataMenuState();
	}
}

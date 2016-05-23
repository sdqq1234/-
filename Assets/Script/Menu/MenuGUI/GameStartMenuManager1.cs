using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
public class GameStartMenuManager1 : MonoBehaviour {
    public AudioClip audio_ClickButton;
    public AudioClip audio_SelectButton;
    public AudioClip ReturnClip;//返回音效
    public AudioSource MenuAudio;//界面背景音乐播放器
    

    float ChangeSpeed = 5;//过度速度

    public GameObject MainMenu; //主游戏界面
    public Image RankMenu;//难度选择界面
    public Image PlayerMenu;//选择飞机界面
    public Image WeaponMenu;//选择武器界面
    public Image ReplayMenu;//回放界面
    public Image OptionMenu;//选项界面
    public Image MusicMenu;//音乐界面
    public Image PlayerDataMenu;//玩家数据界面
    public Image ManaulMenu;//说明界面
    public Image Loading;//loading界面
    public GameObject ManualContentGameType;
    public GameObject ManualContentStory;
    public GameObject ManualContentControl;
    public GameObject ManualContentUI;
    public GameObject ManualContentItem;
    public GameObject ManualContentScore;

    AsyncOperation Async;//场景读取异步对象
    private bool StartLoading = false;//是否开始读取
    private float MinLoadTime = 5;//最小读取时间为5秒，在这个时间内都显示loading界面
    private float CurLoadTime = 0;//当前的读取时间
    private short SceneID = 0;//读取的场景id

    public enum MenuState
    {
        MainTitle,          //主界面
        RankTitle,          //难度选择界面
        PlayerSelectTitle,  //飞机选择界面
        ReplayTitle,        //回放选择界面
        OptionTitle,        //设定界面
        MusicTitle,         //音乐界面
        PlayerDataTitle,    //玩家数据界面
        ManaulTitle,        //游戏说明界面
        ManaulGameType,     //游戏类型说明界面
        ManaulStroy,        //游戏故事说明界面
        ManaulControl,      //游戏操作说明界面
        ManaulUI,           //游戏中UI说明界面
        ManaulItem,         //游戏道具说明界面
        ManaulScore,        //游戏分数说明界面
        WeaponSelectTitle,   //武器选择界面
        LoadingTitle        //loading界面
    }

    private MenuState State = MenuState.MainTitle;
    private List<MenuState> SelectedStateList = new List<MenuState>();
    //private MenuState OldState = MenuState.MainTitle;


    // Use this for initialization
    void Start()
    {
        InitNextState(State);
        SelectedStateList.Add(State);
    }

    //重置游戏菜单状态
    void ResetState()
    {
        MainMenu.gameObject.SetActive(false);
        RankMenu.gameObject.SetActive(false);
        PlayerMenu.gameObject.SetActive(false);
        WeaponMenu.gameObject.SetActive(false);
        ReplayMenu.gameObject.SetActive(false);
        OptionMenu.gameObject.SetActive(false);
        MusicMenu.gameObject.SetActive(false);
        PlayerDataMenu.gameObject.SetActive(false);
        ManaulMenu.gameObject.SetActive(false);
        ManualContentGameType.SetActive(false);
        ManualContentStory.SetActive(false);
        ManualContentControl.SetActive(false);
        ManualContentUI.SetActive(false);
        ManualContentItem.SetActive(false);
        ManualContentScore.SetActive(false);
        Loading.gameObject.SetActive(false);
    }



    void SetAlphaMainMenu()
    {
        MainMenu.gameObject.SetActive(true);
        //MainMenu.alpha = 0.3f;
        //TitleMenuManager title = MainMenu.gameObject.GetComponent<TitleMenuManager>();
        //foreach (UIButton button in title.TitleButtonList)
        //{
        //    button.collider.enabled = false;
        //}
    }


    

    /// <summary>
    /// 更新菜单状态 主要是一些过度效果
    /// </summary>
    void UpdataMenuState()
    {
        //switch (state)
        //{
        //    case MenuState.MainTitle:
        //        MainMenu.alpha = Mathf.Lerp(MainMenu.alpha, 1, Time.deltaTime * ChangeSpeed);
        //        break;
        //    case MenuState.RankTitle:
        //        RankMenu.alpha = Mathf.Lerp(RankMenu.alpha, 1, Time.deltaTime * ChangeSpeed);
        //        break;
        //    case MenuState.PlayerSelectTitle:
        //        PlayerMenu.alpha = Mathf.Lerp(PlayerMenu.alpha, 1, Time.deltaTime * ChangeSpeed);
        //        break;
        //    case MenuState.ReplayTitle:
        //        ReplayMenu.alpha = Mathf.Lerp(ReplayMenu.alpha, 1, Time.deltaTime * ChangeSpeed);
        //        break;
        //    case MenuState.OptionTitle:
        //        OptionMenu.alpha = Mathf.Lerp(OptionMenu.alpha, 1, Time.deltaTime * ChangeSpeed);
        //        break;
        //    case MenuState.MusicTitle:
        //        MusicMenu.alpha = Mathf.Lerp(MusicMenu.alpha, 1, Time.deltaTime * ChangeSpeed);
        //        break;
        //    case MenuState.PlayerDataTitle:
        //        PlayerDataMenu.alpha = Mathf.Lerp(PlayerDataMenu.alpha, 1, Time.deltaTime * ChangeSpeed);
        //        break;
        //    case MenuState.ManaulTitle:
        //        ManaulMenu.alpha = Mathf.Lerp(ManaulMenu.alpha, 1, Time.deltaTime * ChangeSpeed);
        //        break;
        //    case MenuState.WeaponSelectTitle:
        //        WeaponMenu.alpha = Mathf.Lerp(WeaponMenu.alpha, 1, Time.deltaTime * ChangeSpeed);
        //        break;
        //}
        ShowLoading();
    }

    private void ReturnPreState()
    {
        if (State != MenuState.LoadingTitle) {
            if (SelectedStateList.Count > 1)
            {
                InitNextState(SelectedStateList[SelectedStateList.Count - 2]);
                SelectedStateList.Remove(SelectedStateList[SelectedStateList.Count - 1]);
                MenuAudio.PlayOneShot(ReturnClip);
            }
        }
        
        
    }

    //初始化每个状态
    public void InitNextState(MenuState nextState)
    {
        ResetState();
        //OldState = State;
        State = nextState;
        SetAlphaMainMenu();
        switch (State)
        {
            case MenuState.MainTitle:
                MainMenu.gameObject.SetActive(true);
                break;
            case MenuState.RankTitle:
                
                RankMenu.gameObject.SetActive(true);
                break;
            case MenuState.PlayerSelectTitle:
                PlayerMenu.gameObject.SetActive(true);
                break;
            case MenuState.ReplayTitle:
                
                ReplayMenu.gameObject.SetActive(true);
                //设置replay画面
                break;
            case MenuState.OptionTitle:
                
                OptionMenu.gameObject.SetActive(true);
                break;
            case MenuState.MusicTitle:
                
                MusicMenu.gameObject.SetActive(true);
                break;
            case MenuState.PlayerDataTitle:
                
                PlayerDataMenu.gameObject.SetActive(true);
                break;
            case MenuState.ManaulTitle:

                ManaulMenu.gameObject.SetActive(true);
                break;
            case MenuState.WeaponSelectTitle:
                WeaponMenu.gameObject.SetActive(true);
                break;
            case MenuState.ManaulGameType:
                ManaulMenu.gameObject.SetActive(true);
                ManualContentGameType.SetActive(true);
                break;
            case MenuState.ManaulStroy:
                ManaulMenu.gameObject.SetActive(true);
                ManualContentStory.SetActive(true);
                break;
            case MenuState.ManaulControl:
                ManaulMenu.gameObject.SetActive(true);;
                ManualContentControl.SetActive(true);
                break;
            case MenuState.ManaulUI:
                ManaulMenu.gameObject.SetActive(true);
                ManualContentUI.SetActive(true);
                break;
            case MenuState.ManaulItem:
                ManaulMenu.gameObject.SetActive(true);
                ManualContentItem.SetActive(true);
                break;
            case MenuState.ManaulScore:
                ManaulMenu.gameObject.SetActive(true);
                ManualContentScore.SetActive(true);
                break;
            case MenuState.LoadingTitle:
                Loading.gameObject.SetActive(true);
                break;
            

        }
    }

    void ClickRightArrow(GameObject sender)
    {
        switch (State)
        {
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
        switch (State)
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

    #region Button调用事件
    public void PlayOnClickButtonSound()
    {
        MenuAudio.PlayOneShot(audio_ClickButton);
    }
    public void PlayOnSelectButtonSound()
    {
        MenuAudio.PlayOneShot(audio_SelectButton);
    }

    /// <summary>
    /// 主菜单开始按钮
    /// </summary>
    public void OnClickStartButton() {
        InitNextState(MenuState.RankTitle);
        SelectedStateList.Add(State);
    }

    public void OnClickReplayButton() {
        InitNextState(MenuState.ReplayTitle);
        SelectedStateList.Add(State);
    }
    public void OnClickPractticeButton()
    {
        InitNextState(MenuState.RankTitle);
        SelectedStateList.Add(State);
    }
    public void OnClickMusicRoomButton()
    {
        InitNextState(MenuState.MusicTitle);
        SelectedStateList.Add(State);
    }
    public void OnClickPlayerDataButton()
    {
        InitNextState(MenuState.PlayerDataTitle);
        SelectedStateList.Add(State);
    }
    public void OnClickOptionButton()
    {
        InitNextState(MenuState.OptionTitle);
        SelectedStateList.Add(State);
    }
    public void OnClickManualButton()
    {
        InitNextState(MenuState.ManaulTitle);
        SelectedStateList.Add(State);
    }
    public void OnClickExitButton()
    {
        Application.Quit();
    }
    public void OnClickEasyButton()
    {
        GlobalData.SetGameRank(GlobalData.GameRank.Easy);
        InitNextState(MenuState.PlayerSelectTitle);
        SelectedStateList.Add(State);
    }
    public void OnClickNormalButton()
    {
        GlobalData.SetGameRank(GlobalData.GameRank.Normal);
        InitNextState(MenuState.PlayerSelectTitle);
        SelectedStateList.Add(State);
    }
    public void OnClickHardButton()
    {
        GlobalData.SetGameRank(GlobalData.GameRank.Hard);
        InitNextState(MenuState.PlayerSelectTitle);
        SelectedStateList.Add(State);
    }
    public void OnClickLunaticButton()
    {
        GlobalData.SetGameRank(GlobalData.GameRank.Lunatic);
        InitNextState(MenuState.PlayerSelectTitle);
        SelectedStateList.Add(State);
    }
    public void OnClickRightArrowSelectPlayer()
    {

    }
    public void OnClickRightArrowSelectWeapon()
    {

    }
    public void OnClickLeftArrowSelectPlayer()
    {

    }
    public void OnClickLeftArrowSelectWeapon()
    {

    }
    /// <summary>
    /// 选择人物
    /// </summary>
    public void OnClickPlayerNameButton() {
        InitNextState(MenuState.WeaponSelectTitle);
        SelectedStateList.Add(State);
    }
    /// <summary>
    /// 选择武器
    /// </summary>
    public void OnClickWeaponNameButton() {
        GlobalData.SetWeapon(GlobalData.WeaponType.AyaWeaponA);
        InitNextState(MenuState.LoadingTitle);
        loadNextScene(CommandString.GameStageSceneID);
        SelectedStateList.Add(State);
    }
    public void OnClickManualGameType() {
        InitNextState(MenuState.ManaulGameType);

    }
    public void OnClickManualStory()
    {
        InitNextState(MenuState.ManaulStroy);
        
    }
    public void OnClickManualControl()
    {
        InitNextState(MenuState.ManaulControl);
        
    }
    public void OnClickManualUI()
    {
        InitNextState(MenuState.ManaulUI);
        
    }
    public void OnClickManualItem()
    {
        InitNextState(MenuState.ManaulItem);
        
    }
    public void OnClickManualScore()
    {
        InitNextState(MenuState.ManaulScore);
        
    }
    #endregion Button调用事件

    

    private IEnumerator loadSence()
    {

        Async = Application.LoadLevelAsync(SceneID);
        Async.allowSceneActivation = false; //不让进入场景
        yield return Async.isDone;
        float waitTime = MinLoadTime - CurLoadTime;
        if (waitTime > 0)
        {
            //加载没有超过 规定时间
            yield return new WaitForSeconds(waitTime);
        }

        Async.allowSceneActivation = true;
    }

    //读取下一个场景
    public void loadNextScene(short LoadSceneID)
    {
        SceneID = LoadSceneID;
        StartLoading = true;
        //LoadingAnime.SetActive(true);
        //LoadingBackPanel.gameObject.SetActive(true);
        //GameObject.DontDestroyOnLoad(gameObject);
        //GameObject.DontDestroyOnLoad(LoadingPanel.gameObject);
        StartCoroutine(loadSence());

    }

    private void ShowLoading() {
        if (StartLoading)
        {
            //LoadingBackPanel.alpha = Mathf.Lerp(LoadingBackPanel.alpha, 1, Time.deltaTime * 5);
            CurLoadTime += Time.deltaTime;
            if (Async.isDone)
            { //如果读取时间在最小时间以内则延时

                if (CurLoadTime >= MinLoadTime)
                {
                    Async.allowSceneActivation = true;//超过最小读取时间则可以显示下一个场景
                    //LoadingAnime.SetActive(false);
                    StartLoading = false;//标记为读取结束
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        UpdataMenuState();
        if (Input.GetMouseButtonDown(1)) {
            ReturnPreState();
        }
    }
}

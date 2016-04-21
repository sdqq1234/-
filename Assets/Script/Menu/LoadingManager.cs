using UnityEngine;
using System.Collections;

public class LoadingManager : MonoBehaviour {

    AsyncOperation Async;//场景读取异步对象
    private bool StartLoading = false;//是否开始读取
    private float MinLoadTime = 5;//最小读取时间为5秒，在这个时间内都显示loading界面
    private float CurLoadTime = 0;//当前的读取时间
    private short SceneID = 0;//读取的场景id



    public UIPanel LoadingBackPanel;//背景
    public GameObject LoadingAnime;//文字动画
    // Use this for initialization
    void Start()
    {
        //LoadingPanel = gameObject.GetComponent<UIPanel>();
        LoadingAnime.SetActive(false);
    }

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
        LoadingAnime.SetActive(true);
        LoadingBackPanel.gameObject.SetActive(true);
        GameObject.DontDestroyOnLoad(gameObject);
        //GameObject.DontDestroyOnLoad(LoadingPanel.gameObject);
        StartCoroutine(loadSence());

    }

    // Update is called once per frame
    void Update()
    {
        if (StartLoading)
        {
            LoadingBackPanel.alpha = Mathf.Lerp(LoadingBackPanel.alpha, 1, Time.deltaTime * 5);
            CurLoadTime += Time.deltaTime;
            if (Async.isDone)
            { //如果读取时间在最小时间以内则延时

                if (CurLoadTime >= MinLoadTime)
                {
                    // Async.allowSceneActivation = true;//超过最小读取时间则可以显示下一个场景
                    LoadingAnime.SetActive(false);
                    StartLoading = false;//标记为读取结束
                }
            }
        }
        else
        {
            if (LoadingBackPanel.alpha > 0.1)
            {
                LoadingBackPanel.alpha = Mathf.Lerp(LoadingBackPanel.alpha, 0, Time.deltaTime * 5);
            }
            else
            {
                LoadingBackPanel.gameObject.SetActive(false);
            }
        }
    }
}

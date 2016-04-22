using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class ItemBase : MonoBehaviour
{
    public Vector2 bornSpeed = new Vector2(0,1);
    public int ItemValue = 1;//道具的值
    public int ScoreValue = 1000;//默认分数值
    private float LineRecoverSpeed = 8;//回收线的回收速度
    private float NormalRecoverSpeed = 4;//普通回收速度
    private bool isBeLineRecovered = false;//是否属于被回收线回收状态
    private bool isBeNormalRecovered = false;//是否属于普通回收
    private string soundName = "se_item00";
    private string PowerUpSoundName = "se_powerup";
    public string StringGiveShow = string.Empty; //收点显示的数字
    public string StringDoubleShow = string.Empty;//x2的标志
    protected BoxCollider2D Box2D;//碰撞框组件
    private Rigidbody2D rigidbody;//刚性组件
    private SpriteRenderer renderer;//渲染组件
    protected bool DestroyWhenOutOfScreen;//是否飞出屏幕
    //protected UILabel scoreShowLable = null;//显示单个道具分数的label；
    //protected UILabel doubleShowLablel = null;//显示单个道具双倍的label；
    public enum ItemType { 
        RedItem,
        BlueItem,
        GreenItem,
        GrazeItem
    }
    public ItemType Type = ItemType.RedItem;//默认红色
    public AudioClip sound;//获取道具的声音
    public bool isGrazed = false;//是否为擦弹生成
    private AudioClip PowerUpSound;//威力提升声音
	// Use this for initialization
    public void Start()
    {
        //base.Start();
        rigidbody2D.velocity = bornSpeed;
        sound = Resources.Load(CommandString.SoundPath + soundName) as AudioClip;
        PowerUpSound = Resources.Load(CommandString.SoundPath + PowerUpSoundName) as AudioClip;
        Box2D = GetComponent<BoxCollider2D>();
        rigidbody = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
    }

    //收道具
    void RecoverItem(float recoverSpeed) {
        if (StageManager.CurStage.myPlane.gameObject != null)
        {
            rigidbody2D.velocity = ((Vector2)MyPlane.MyPos - (Vector2)transform.position).normalized * recoverSpeed;
            rigidbody2D.gravityScale = 0;
        }
        else
        {
            rigidbody2D.velocity = -Vector2.up;
            rigidbody2D.gravityScale = 0;
        }
    }

    void UpdateItem() {
        if (StageManager.CurStage.myPlane.isOverLineCover)
        {
            isBeLineRecovered = true;
        }
        if (!isGrazed)
        {  //其他普通道具
            if (isBeLineRecovered)
            { //自机飞过收点线就自动追踪飞机过去
                RecoverItem(LineRecoverSpeed);
            }
            if (isBeNormalRecovered) {
                RecoverItem(NormalRecoverSpeed);
            }
        }
        else {
            //擦弹生成道具
            if (rigidbody2D.velocity.y <= 0)
            {
                Box2D.enabled = true;
                isBeNormalRecovered = true;
                
            }
            if (isBeNormalRecovered)
            {
                RecoverItem(LineRecoverSpeed);
            }
        }

        //出下屏的情况
        if (this.gameObject.transform.position.y < GlobalData.screenBottomPoint.y - 0.5f) {
            DestroyWhenOutOfScreen = true;
        }
        //bRecover();
    }

    //创建一个双倍显示
    void CreateDoubleShow(Vector3 pos) {
        GameObject doubleShow = GameObject.Instantiate(Resources.Load(CommandString.ItemPath + "DoubleShow")) as GameObject;
        
        doubleShow.transform.parent = transform.parent;
        doubleShow.transform.localScale = Vector3.one;
        doubleShow.transform.position = pos;
        AlphaMoveTool moveControl = doubleShow.GetComponent<AlphaMoveTool>();
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(0,1f);

        moveControl.randomDir = new Vector2(x,y) / 100;
    }

    /// <summary>
    /// 给值
    /// </summary>
    /// <param name="myPlane"></param>
    /// <param name="type"></param>
    public void giveValue(MyPlane myPlane)
    {
        int giveItemValue = ItemValue;
        int giveScoreValue = ScoreValue;

        if (isBeLineRecovered)
        { //收点线收就两倍
            giveItemValue = 2 * ItemValue;
            giveScoreValue = 2 * ScoreValue;
            StringGiveShow = "+" + ScoreValue;
            StringDoubleShow = "X2";
            CreateDoubleShow(myPlane.transform.position);
            
        }
        switch (Type)
        { 
            case ItemType.RedItem:
                float tmp = myPlane.Power;
                myPlane.Power += giveItemValue;
                if (tmp != myPlane.Power)
                {
                    if (myPlane.Power % 100 == 0)
                    {
                        //AudioClip PowerUp = Resources.Load(CommandString.SoundPath + "se_powerup") as AudioClip;
                        AudioManager.AddItemGetSound(PowerUpSound);
                        //StageData.SoundPlay("se_powerup.wav");
                    }
                }
                
                break;
            case ItemType.BlueItem:
                myPlane.BluePoint += giveItemValue;
                break;
            case ItemType.GreenItem:
                myPlane.GreenPoint += giveItemValue;
                break;
            case ItemType.GrazeItem: //擦弹道具除外
                giveItemValue = ItemValue;
                giveScoreValue = ScoreValue;
                StringGiveShow = string.Empty;
                StringDoubleShow = string.Empty;
                myPlane.Graze += giveItemValue;
                break;
        } 
        myPlane.Score += giveScoreValue;
    }

    //被回收了
    void bRecover()
    {
        //for (int i = hitObjectList.Count - 1; i >= 0; i--)
        //{
        //    if (hitObjectList[i] != null) {
        //        MyPlane myplane = hitObjectList[i].GetComponent<MyPlane>();
        //        if (myplane != null && myplane.gameObject != null)
        //        {
        //            isBeRecovered = true;
        //        }
        //    }
            
        //}
    }

    /// <summary>
    /// 检测是否与主机碰撞了
    /// </summary>
    /// <param name="otherCollider"></param>
    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        ItemRecoverRange recoverRange = otherCollider.GetComponent<ItemRecoverRange>();
        MyPlane myplane = otherCollider.GetComponent<MyPlane>();
        if (recoverRange != null)
        {
            isBeNormalRecovered = true;
        }
        if (myplane != null && myplane.renderer.sortingLayerName == CommandString.MyPlaneLayer)
        {
            //给道具
            giveValue(myplane);
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        // 出下屏幕则销毁
        if (DestroyWhenOutOfScreen)
        {
            if (renderer == null)
            {
                renderer = GetComponent<SpriteRenderer>();
            }

            if (renderer != null && renderer.isVisible == false)
            {
                Destroy(this.gameObject);
                //OnDestroy();
            }
        }
        else {
            //不出屏幕则检测是否要被回收
            UpdateItem();
        }
    }
    //void FixedUpdate()
    //{
    //    rigidbody2D.velocity = bornSpeed;
    //}

    //// Update is called once per frame
    //public virtual void Update () {
    //    base.Update();
    //    UpdateItem();
    //}
}

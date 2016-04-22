using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]//添加2d刚体用来检测碰撞
[RequireComponent(typeof(SpriteRenderer))]//所有物体都需要2d渲染组件
public class MyPlane : MonoBehaviour
{

    /// <summary>
    /// 自身机体属性
    /// </summary>
    private int life = 2;       //残机数
    private int spell = 2;      //Spell数
    private int power = 100;    //Power
    private long score = 0;     //分数
    private long hiScore = 0;   //高分
    private int nextBluePoint = 300;
    private int nextGreenPoint = 3000;
    private int bluePoint = 0;  //蓝点
    private int greenPoint = 0; //绿点
    private int blueLevel = 0;  //蓝色等级
    private int greenLevel = 0;//绿点等级
    private int unmatchedTime = 5;  //无敌时间  秒
    private int deltaGreen = 120;     //用于计算绿点奖B线
    private Color deadColor = Color.white;//死亡时特效颜色
    private bool isDead = false;//是否死亡
    private bool isCanShoot = true;//是否能发射子弹
    private float LifeTime = 0;//已经存活的时间
    private float Speed_CurValue = 0;//当前移动速度值
    private bool isShooting = false;//是否在发射子弹状态
    private Animator PlayerAnimationStatus; //每个飞机都有机体动画
    private AudioSource baseAudio;//每个飞机自身都带的音频播放设备，用来播放各种自己飞机受到的声音
    private List<GameObject> hitObjectList = new List<GameObject>();//碰到的其他gameobject列表

    public AudioClip deadSound;//每个飞机的死亡声音
    /// <summary>
    /// 死亡时间
    /// 用于产生绝死效果
    /// </summary>
    public float DeadBeforeTime;
    
    
    protected SpriteRenderer render //当前2d渲染变量
    {
        get
        {
            return gameObject.GetComponent<SpriteRenderer>();
        }
    }

    public string LayerName
    { //层级名称
        get
        {
            return render.sortingLayerName;
        }
        set
        {
            render.sortingLayerName = value;
        }
    }

    public int LayerOrder
    {//层级里的层数
        get
        {
            return render.sortingOrder;
        }
        set
        {
            render.sortingOrder = value;
        }
    }

    
    
    /// <summary>
    /// 装备类型
    /// </summary>

    public enum WeaponType
    {
        TypeA,
        TypeB
    }
    public WeaponType weaponType = WeaponType.TypeA;
    /// <summary>
    /// 全名（机体名+装备名）
    /// </summary>
    public string FullName
    {
        get { return gameObject.name + weaponType.ToString(); }
    }

    

    /// <summary>
    /// 残机数
    /// </summary>
    public int Life
    {
        get { return life; }
        set
        {
            if (value > 8)
            {
                life = 8;
            }
            else
            {
                life = value;
            }
        }
    }

    /// <summary>
    /// Spell数
    /// </summary>
    public int Spell
    {
        get { return spell; }
        set
        {
            if (value > 8)
            {
                spell = 8;
            }
            else if (value < 0)
            {
                spell = 0;
            }
            else
            {
                spell = value;
            }
        }
    }

    /// <summary>
    /// 获取或设置子弹威力
    /// <summary>
    public int Power
    {
        get { return power; }
        set
        {
            if (value > 400)
            {
                power = 400;
            }
            else if (value < 100)
            {
                power = 100;
            }
            else
            {
                power = value;
            }
        }
    }

    /// <summary>
    /// 获取自己子弹威力等级
    /// </summary>
    public int PowerLevel
    {
        get { return power / 100; }
    }

    /// <summary>
    /// 分数
    /// <summary>
    public long Score
    {
        get { return score; }
        set
        {
            if (value > 9999999999) //上限99E
            {
                score = 9999999999;
            }
            else if (value < 0)
            {
                score = 0;
            }
            else
            {
                score = value;
            }
        }
    }

    public long HiScore
    {
        get
        {
            if (score > hiScore)
            {
                return score;
            }
            return hiScore;
        }
        set
        {
            if (value > 9999999999) //上限99E
            {
                hiScore = 9999999999;
            }
            else if (value < 0)
            {
                hiScore = 0;
            }
            else
            {
                hiScore = value;
            }
        }
    }

    /// <summary>
    /// 获取和设置蓝点数量，
    /// 设置时，如果蓝点数量大于奖残线，则奖残线提升至下一级
    /// </summary>
    public int BluePoint
    {
        get
        {
            return bluePoint;
        }
        set
        {
            bluePoint = value;
            if (bluePoint >= nextBluePoint)
            {
                blueLevel++;
                nextBluePoint += blueLevel * 400;

                Extend();
            }
        }
    }

    /// <summary>
    /// 获取和设置绿点数量，
    /// 设置时，如果绿点数量大于奖B线，则奖B线提升至下一级
    /// </summary>
    public int GreenPoint
    {
        get
        {
            return greenPoint;
        }
        set
        {
            greenPoint = value;
            if (greenPoint >= nextGreenPoint)
            {
                SpellExtand();

                int temp = 3000;
                for (int i = 0; i < greenLevel; i++)
                {
                    temp += deltaGreen * i;
                }
                nextGreenPoint += temp;
                greenLevel++;
            }
        }
    }

    /// <summary>
    /// 获取和设置下一级蓝点数，即奖残线
    /// </summary>
    public int NextBluePoint
    {
        get
        {
            return nextBluePoint;
        }
        set
        {
            nextBluePoint = value;
        }
    }

    /// <summary>
    /// 获取和设置下一级绿点数，即奖B线
    /// </summary>
    public int NextGreenPoint
    {
        get
        {
            return nextGreenPoint;
        }
        set
        {
            nextGreenPoint = value;
        }
    }

    /// <summary>
    /// 获取上一级蓝点数
    /// </summary>
    public int LastBluePoint
    {
        get
        {
            if (blueLevel > 0)
            {
                return nextBluePoint - (blueLevel) * 400;
            }
            else
            {
                return 0;
            }
        }
    }

    /// <summary>
    /// 获取上一级绿点数
    /// </summary>
    public int LastGreenPoint
    {
        get
        {
            if (greenLevel > 0)
            {
                int temp = 3000;
                for (int i = 0; i < greenLevel - 1; i++)
                {
                    temp += deltaGreen * i;
                }
                return nextGreenPoint - temp;
            }
            else
            {
                return 0;
            }
        }
    }

    public int Graze { get; set; }

    /// <summary>
    /// 无敌时间
    /// 最初的一小段时间为无敌时间
    /// 中弹后的一小段时间也为无敌时间，防止连续中弹
    /// </summary>
    public int UnmatchedTime
    {
        get { return unmatchedTime; }
    }

    /// <summary>
    /// 僚机位置点
    /// </summary>
    public Vector3[] SubPlanePoint { get; set; }

    /// <summary>
    /// 主射击位置点
    /// </summary>
    public Vector3[] MainShooterPos { get; set; }

    public List<AyaSubShooter> MySubShooterList = new List<AyaSubShooter>(); //僚机列表
    public List<AyaMainShooter> MainShooterList = new List<AyaMainShooter>();//主发射器列表

    //private List<Shooter> SubPlaneList = new List<Shooter>();//僚机列表

    /// <summary>
    /// Spell使能，关尾一小段时间不允许放B，否则影响Rep
    /// </summary>
    public bool SpellEnabled { get; set; }

    public static Vector3 MyPos;
    private bool isSlow = false;//是否慢速
    public bool isCanMove = true;//是否能移动
    public bool isOverLineCover = false;//是否越过收点线
    //public bool isCanShoot = true;//是否能射击 


    //public float shootRate = 0.02f;//射击频率，每隔多少秒射一次，数字越小越快
    public GameObject HitCenter;//着弹点
    public float speedNormal = 5;//普通移动速度
    private float speed_Slow
    {
        get
        {
            return speedNormal / 2;
        }
        set
        {
            speed_Slow = value;
        }
    }

    public GrazeCenter GrazeCenter;//擦弹范围物体
    public GameObject ItemRecoverRange;//道具自动回收范围

    //慢速移动速度
    //private float speed_cur = 0;//当前移动速度
    private float shootAudioPlayRate = 0.04f;//射击声音播放频率，每隔多少秒播放一次，数字越小越快
    private float nextShootAudioPlay = 0.0F;//下次播放的时间

    //声音相关
    string bombSoundPath = CommandString.SoundPath + "se_nep00";//炸弹声音
    string deadSoundPath = CommandString.SoundPath + "se_pldead00";//死亡声音
    string getItemSoundPath = CommandString.SoundPath + "se_item00";//捡道具声音
    string extendSoundPath = CommandString.SoundPath + "se_extend";//奖励残机声音
    string bombExtendSoundPath = CommandString.SoundPath + "se_cardget";//奖B声音
    string grazeSoundPath = CommandString.SoundPath + "se_graze";//擦弹声音
    private string deadEffectName = "DeadEffect";

    public AudioClip grazeSound;//擦弹声音

    void Awake()
    {
        MyPos = gameObject.transform.position;
    }

    // Use this for initialization
    public void Start()
    {
        //base.Start();
        Init();
    }


    void Init()
    {
        StageManager.CurStage.myPlane = this;
        //HpValue = 1;
        //hitEnable = false;
        SubPlanePoint = new Vector3[4];
        deadSound = Resources.Load(deadSoundPath) as AudioClip;
        grazeSound = Resources.Load(grazeSoundPath) as AudioClip;
        weaponType = WeaponType.TypeA;
        SpellEnabled = true;
        Speed_CurValue = speedNormal;
        PlayerAnimationStatus = this.GetComponent<Animator>(); //获取机体移动动画
        //subShooterList = MySubShooterList;
        //mainShooter.SetShootBulletSpeed(mainBulletShootSpeed);
        //SetMainShooterBullet(-1);
        //SetSubShooterBullet(-1);
    }

    void InputControl()
    {
        if (isCanShoot)
        {
            if (Input.GetButton("Shoot"))
            {
                //僚机射击
                isShooting = true;
                if (isCanShoot && isShooting && MySubShooterList != null)
                {
                    foreach (AyaSubShooter s in MySubShooterList)
                    {
                        if (s.gameObject.activeSelf)
                            s.Shoot();
                    }

                    //主发射器射击 
                    foreach (AyaMainShooter m in MainShooterList)
                    {
                        if (m.gameObject.activeSelf)
                            m.Shoot();
                    }
                }
            }
            else
            {
                isShooting = false;
            }

        }

        if (Input.GetButton("Slow"))
        {
            isSlow = true;
            Speed_CurValue = speed_Slow;
        }
        else
        {
            isSlow = false;
            Speed_CurValue = speedNormal;
        }
        HitCenter.SetActive(isSlow);

        if (SpellEnabled)
        {
            if (Input.GetButtonDown("Bomb"))
            {
                Debug.Log("Bomp");
            }
        }

        if (isCanMove)
        {

            //Rect myRect = render.sprite.textureRect;
            float x = gameObject.transform.position.x;
            float y = gameObject.transform.position.y;
            if (Input.GetButton("Left"))
            {
                PlayerAnimationStatus.SetBool("Left", true);
                PlayerAnimationStatus.SetBool("Right", false);
                //setState(PlaneState.Left);
                if (x > GlobalData.screenLeftPoint.x)
                {
                    x -= Speed_CurValue;
                }

            }

            else if (Input.GetButton("Right"))
            {
                //setState(PlaneState.Right);
                PlayerAnimationStatus.SetBool("Right", true);
                PlayerAnimationStatus.SetBool("Left", false);
                if (x < GlobalData.screenRightPoint.x)
                {
                    x += Speed_CurValue;
                }
            }
            else
            {
                PlayerAnimationStatus.SetBool("Right", false);
                PlayerAnimationStatus.SetBool("Left", false);
                //setState(PlaneState.NoMove);
            }

            if (Input.GetButton("Up"))
            {
                if (y < GlobalData.screenTopPoint.y)
                {
                    //rigidbody2d.velocity.Set(rigidbody2d.velocity.x,Speed_CurValue);
                    y += Speed_CurValue;
                }

            }
            else if (Input.GetButton("Down"))
            {
                if (y > GlobalData.screenBottomPoint.y)
                {
                    //rigidbody2d.velocity.Set(rigidbody2d.velocity.x, -Speed_CurValue);
                    y -= Speed_CurValue;
                }

            }
            gameObject.transform.position = new Vector2(x, y);

        }
    }

    /// <summary>
    /// 奖残
    /// </summary>
    public void Extend()
    {
        if (Life < 8)
        {
            Life++;
        }
        else
        {
            Spell++;
        }
        //StageData.SoundPlay("se_extend.wav");
        
    }

    /// <summary>
    /// 奖B
    /// </summary>
    private void SpellExtand()
    {
        //StageData.SoundPlay("se_cardget.wav");
        //Spell++;
    }

    /// <summary>
    /// 更新僚机位置 相对于自机的坐标
    /// </summary>
    /// <param name="winCount">僚机数量</param>
    void UpdateSubPlanePos(int PowLevel)
    {
        if (isSlow)
        {
            switch (PowLevel)
            {
                case 1:
                    SubPlanePoint[0] = new Vector3(0, 0.3f, 0);
                    break;
                case 2:
                    SubPlanePoint[0] = new Vector3(-0.15f, 0.3f, 0);
                    SubPlanePoint[1] = new Vector3(0.15f, 0.3f, 0);
                    break;
                case 3:
                    SubPlanePoint[0] = new Vector3(0, 0.3f, 0);
                    SubPlanePoint[1] = new Vector3(0.2f, 0.2f, 0);
                    SubPlanePoint[2] = new Vector3(-0.2f, 0.2f, 0);
                    break;
                case 4:
                    SubPlanePoint[0] = new Vector3(-0.15f, 0.4f, 0);
                    SubPlanePoint[1] = new Vector3(0.15f, 0.4f, 0);
                    SubPlanePoint[2] = new Vector3(-0.3f, 0.3f, 0);
                    SubPlanePoint[3] = new Vector3(0.3f, 0.3f, 0);
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (PowLevel)
            {
                case 0:
                    break;
                case 1:
                    SubPlanePoint[0] = new Vector3(0, 0.4f, 0);
                    break;
                case 2:
                    SubPlanePoint[0] = new Vector3(-0.3f, 0, 0);
                    SubPlanePoint[1] = new Vector3(0.3f, 0, 0);
                    break;
                case 3:
                    SubPlanePoint[0] = new Vector3(0, 0.4f, 0);
                    SubPlanePoint[1] = new Vector3(0.3f, 0, 0);
                    SubPlanePoint[2] = new Vector3(-0.3f, 0, 0);
                    break;
                case 4:
                    SubPlanePoint[0] = new Vector3(-0.25f, -0.1f, 0);
                    SubPlanePoint[1] = new Vector3(0.25f, -0.1f, 0);
                    SubPlanePoint[2] = new Vector3(-0.5f, -0.25f, 0);
                    SubPlanePoint[3] = new Vector3(0.5f, -0.25f, 0);
                    break;
                default:
                    break;
            }
        }

        for (int i = 0; i < MySubShooterList.Count; i++)
        {
            Vector2 targetPos = SubPlanePoint[i] + transform.position;
            float speed = 10;
            MySubShooterList[i].transform.position = Vector2.Lerp(MySubShooterList[i].transform.position, targetPos, Time.deltaTime * speed);
        }
        for (int i = 0; i < MySubShooterList.Count; i++)
        {
            if (i < PowerLevel)
            {
                MySubShooterList[i].gameObject.SetActive(true);
            }
            else
            {
                MySubShooterList[i].gameObject.SetActive(false);
            }

            //MySubShooterList[i].SetShootBulletSpeed(subBulletShootSpeed);
        }
    }

    //擦弹逻辑
    public void InitGrazeItem()
    {
        //if (GrazeCenter.GrazeBulletList.Count > 0)
        //{
            Graze++;
            audio.PlayOneShot(grazeSound);
            //GrazeCenter.GrazeBulletList.RemoveAt(GrazeCenter.GrazeBulletList.Count - 1);
            //每擦弹一次生成3个绿点
            for (int i = 0; i < 3; i++)
            {
                GameObject Item_Obj = GameObject.Instantiate(Resources.Load(CommandString.ItemPath + "GrazeItem")) as GameObject;
                Item_Obj.name = "Item" + gameObject.name;
                Item_Obj.transform.parent = Camera.main.gameObject.transform;
                Item_Obj.transform.localScale = Vector3.one;
                Item_Obj.transform.position = transform.position;
                ItemBase item = Item_Obj.GetComponent<ItemBase>();
                float rand_x = Random.Range(-0.5f, 0.5f);
                item.bornSpeed = new Vector2(rand_x, 1);
                Item_Obj.rigidbody2D.gravityScale = 0.2f;
            }
        //}
    }

    /// <summary>
    /// 自机死亡
    /// </summary>
    protected void Dead()
    {
        GameObject deadEffect = GameObject.Instantiate(Resources.Load(CommandString.EffectPath + deadEffectName)) as GameObject; //创建一个死亡特效
        ParticleSystem ps = deadEffect.GetComponent<ParticleSystem>();
        ps.startColor = deadColor;
        deadEffect.transform.parent = UIShootRoot.tra_ShootRoot;
        deadEffect.transform.localScale = Vector3.one;
        deadEffect.transform.position = transform.position;
        isDead = true;
    }

    //被打中伤血
    public void bHit()
    {
        Dead();
        ClearAllBullet();  
    }

    //死亡以后清除所有屏幕上的弹幕 并且播放清除特效
    private void ClearAllBullet()
    {
        GameObject[] bulletInScreen = GameObject.FindGameObjectsWithTag("EnemyBullet");
        //BulletBase_Touhou[] bulletInScreen = UIShootRoot.tra_ShootRoot.GetComponentsInChildren<BulletBase_Touhou>();
        foreach (GameObject go in bulletInScreen)
        {
            BulletBase_Touhou bullet = go.GetComponent<BulletBase_Touhou>();
            bullet.ShowVanishEffect();
            GameObject.Destroy(go);
        }
    }

    // Update is called once per frame
    public void Update()
    {
        MyPos = gameObject.transform.position;
        InputControl();
        UpdateSubPlanePos(PowerLevel);
        //GrazeBullet();
        //bHit();
    }
}

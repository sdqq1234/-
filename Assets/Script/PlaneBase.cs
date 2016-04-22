using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//飞机体基类
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]//添加2d刚体用来检测碰撞

//[RequireComponent(typeof(AudioSource))]
public class PlaneBase : ObjectBase {

    public bool isCanShoot = true;//是否能发射子弹
    protected bool isShooting = false;//是否在发射子弹状态
    public bool isDead = false;//是否死亡
    protected Animator PlayerAnimationStatus; //每个飞机都有机体动画
    //public ShooterBase mainShooter;//每个机体至少有一个主要弹幕发射器
    //protected List<ShooterBase> subShooterList = new List<ShooterBase>();//子发射器列表
    protected float HpValue = 50;//每个飞机有血量
    protected AudioSource baseAudio;//每个飞机自身都带的音频播放设备，用来播放各种自己飞机受到的声音
    public AudioClip deadSound;//每个飞机的死亡声音
    private Color deadColor = Color.white;//死亡时特效颜色
    protected int OwnBulletCount = 10;//默认有10颗子弹
    private string deadEffectName = "DeadEffect";
    //protected List<ItemBase> ItemList = new List<ItemBase>();//拥有的道具列表
    protected List<string> ItemNameList = new List<string>();//拥有的道具列表
    protected bool isShootByRhythm = false;//是否按照节奏射击
    protected Rigidbody2D rigidbody2d;//飞机物理刚体

    public enum PlaneState
    {
        NoMove, //原地状态
        Left,//左移动中
        Right,//右移动中
        Dead//死亡了
    }
    public PlaneState enum_State = PlaneState.NoMove;//角色移动状态

	// Use this for initialization
	public virtual void Start () {
        //base.Start();
        //animatePlayer = gameObject.GetComponent<SpriteAnimation>();
        PlayerAnimationStatus = this.GetComponent<Animator>(); //获取机体移动动画
        baseAudio = gameObject.GetComponent<AudioSource>();
        baseAudio.panLevel = 0;
        baseAudio.volume = 0.8f;
        rigidbody2d = gameObject.GetComponent<Rigidbody2D>();
        //mainShooter.CanShootBulletCount = OwnBulletCount;
	}

    //设置道具列表
    public void SetItemList(List<string> items) {
        ItemNameList = items;
    }

    ////设置主发射器子弹
    public void SetMainShooterBullet(int BulletCount, float shootRate, float shootSpace, float ShootDuration)
    {
        OwnBulletCount = BulletCount;
        //mainShooter.CanShootBulletCount = BulletCount;
        //mainShooter.shootRate = shootRate;
        //mainShooter.shootSpace = shootSpace;
        //mainShooter.shootDuration = ShootDuration;
    }

    //设置主发射器子弹
    public void SetMainShooterBullet(int BulletCount)
    {
        OwnBulletCount = BulletCount;
        //mainShooter.CanShootBulletCount = BulletCount;
    }

    //设置子自发射器
    protected void SetSubShooterBullet(int BulletCount) {
        //foreach (ShooterBase sb in subShooterList) {
        //    sb.CanShootBulletCount = BulletCount;
        //}
    }
    //被打中伤血 用于继承的子类用
    public virtual void bHit(float hitPower)
    {
    }

    //设置死亡数据
    public void setDeadValue(string EffectName,Color color) {
        if (EffectName != null) {
            deadEffectName = EffectName;
        }
        
        deadColor = color;

    }

    //用于处理飞机死亡特效  这里用playmaker实现
    protected void Dead() {
        GameObject deadEffect =GameObject.Instantiate( Resources.Load(CommandString.EffectPath + deadEffectName)) as GameObject; //创建一个死亡特效
        ParticleSystem ps = deadEffect.GetComponent<ParticleSystem>();
        ps.startColor = deadColor;
        deadEffect.transform.parent = UIShootRoot.tra_ShootRoot;
        deadEffect.transform.localScale = Vector3.one;
        deadEffect.transform.position = transform.position;
        //stage.StageBulletRhythmEvent -= PlaneShoot;
        GiveItems();
        isDead = true;
    }

    /// <summary>
    /// 对象销毁时的特效和道具
    /// </summary>
    public void GiveEndEffect()
    {
        //new EndEnemy(StageData, "光点", Position, 0, 0);
    }

    //给道具
    protected void GiveItems()
    {
        for (int i = 0; i < ItemNameList.Count; i++) {
            GameObject Item_Obj = GameObject.Instantiate(Resources.Load(CommandString.ItemPath + ItemNameList[i])) as GameObject;
            Item_Obj.name = "Item" + gameObject.name;
            Item_Obj.transform.parent = Camera.main.gameObject.transform;
            Item_Obj.transform.localScale = Vector3.one;
            Item_Obj.transform.position = transform.position;
            ItemBase item = Item_Obj.GetComponent<ItemBase>();
            item.rigidbody2D.gravityScale = 0.1f;
            //item.Dir_CurSpeed = new Vector2(0, 1);
            //item.Speed_CurValue = 80;
            //item.Acceleration_Value = 1;
            //item.Dir_Acceleration = new Vector2(0, -1);
            StageManager.CurStage.itemList.Add(item);
        }
            //foreach (ItemBase item in ItemList)
            //{
            //    GameObject Item_Obj = GameObject.Instantiate(Resources.Load(CommandString.ItemPath + item.gameObject.name)) as GameObject;
            //    Item_Obj.name = "Item" + gameObject.name;
            //    Item_Obj.transform.parent = Camera.main.gameObject.transform;
            //    Item_Obj.transform.localScale = Vector3.one;
            //    Item_Obj.transform.position = Vector3.zero;
            //    item.Dir_CurSpeed = new Vector2(0, 1);
            //    item.Speed_Cur = 80;
            //    item.Acceleration = 10;
            //    item.Dir_Acceleration = new Vector2(0, -1);

            //}
        //new PowerItem(StageData, Position);
        //new PowerItem(StageData, Position);
        //new ScoreItem(StageData, Position);
        //new ScoreItem(StageData, Position);
        //new ScoreItem(StageData, Position);
    }

    //设置状态
    public void setState(PlaneState state) {
        enum_State = state;
        //animatePlayer.setSpriteState(state);
    }

    

    //飞机射击方法
    protected void PlaneShoot() {
        if (CheckInScreen())
        {
            //if (isCanShoot && isShooting && mainShooter != null)
            //{
            //    mainShooter.Shoot();
            //}
        }
    }



	// Update is called once per frame
    public virtual void Update()
    {
        LifeTime += Time.deltaTime;
        //base.Update();
        if (!isShootByRhythm)
        {
            PlaneShoot();
        }
    }
}

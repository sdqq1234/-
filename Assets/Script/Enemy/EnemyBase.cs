using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(UISpriteAnimation))]
//[RequireComponent(typeof(UISprite))]
[RequireComponent(typeof(Collider2D))]
public class EnemyBase : PlaneBase {

    private string str_HitSound = "se_damage01";
    private string str_DeadSound = "se_enep00";
    private AudioClip hitSound;
    public float activeTime = -1;//敌人出现在屏幕里的时间
    public PlayMakerFSM enemyFsm;//敌人身上的状态机
    private Vector3 curTargetPos;
    //public Vector2 activePos = Vector2.zero;//敌人行动时开始的地点
    //private AnimationCurve speed_Curve;
    //private AnimationCurve DirX_Curve;
    //private AnimationCurve DirY_Curve;
    float BornSpeed;//出生时的速度
    
    Vector2 BornSpeedDir;
    
    /// <summary>
    /// 构造
    /// </summary>
    public EnemyBase() {
        
    }

    void Awake() {
        enemyFsm = gameObject.GetComponent<PlayMakerFSM>();    
    }
	// Use this for initialization
	public override void Start () {
        base.Start();
        //BornSpeed = Speed_CurValue;
        //BornSpeedDir = Dir_CurSpeed;
        //animatePlayer.isNeedMirror = true;
        //mainShooter.setBulletSortingLayer(CommandString.EnemyBulletLayer);//敌人射出来的子弹层
        //mainShooter.setBulletSortingLayer("EnemyBullet");//敌人射出来的子弹层
        if (enemyFsm != null)
        {
            enemyFsm.Fsm.Event("MoveIn");
        }
        hitSound = Resources.Load(CommandString.SoundPath + str_HitSound) as AudioClip;
        deadSound = Resources.Load(CommandString.SoundPath + str_DeadSound) as AudioClip;
        hitEnable = true;
        isCanShoot = true;
        isShooting = true;
        //animatePlayer.setSpriteState(PlaneState.NoMove);
        //StageManager.enemylist.Add(this);
	}

    //设置当前要移动到的目标位置  这个方法由playmaker事件来调用
    public void setCurTargetPos(Vector3 targetPos) {
        curTargetPos = targetPos;
    }
    /// <summary>
    /// 设置速度曲线
    /// </summary>
    /// <param name="speed"></param>
    /// <param name="dir_x"></param>
    /// <param name="dir_y"></param>
    //public void SetSpeedCurve(AnimationCurve speed,AnimationCurve dir_x,AnimationCurve dir_y) {
        //if (speed != null) {
        //    speed_Curve = speed;
        //}
        //if (dir_x != null) {
        //    DirX_Curve = dir_x;
        //}
        //if (dir_y != null) {
        //    DirY_Curve = dir_y;
        //}
        
    //}

    //根据节奏射击
    public void SetPlaneShootByRhythm(StageBase stage)
    {
        isShootByRhythm = true;
        //mainShooter.shootRate = 0;
        //mainShooter.shootDuration = 0;
        //mainShooter.shootSpace = 0;
        stage.StageBulletRhythmEvent += PlaneShoot;
    }

    //去除场景节奏事件
    public void deletStageRhythmEvent(StageBase stage) {
        isShootByRhythm = false;
        stage.StageBulletRhythmEvent -= PlaneShoot;
    }

    private void UpdateAnimateByDir() {
        
        //Speed_CurValue = BornSpeed * speed_Curve.Evaluate(LifeTime);
        //Dir_CurSpeed.x = DirX_Curve.Evaluate(LifeTime);
        //Dir_CurSpeed.y = DirY_Curve.Evaluate(LifeTime);

        if (curTargetPos.x> transform.position.x)
        {
            
            PlayerAnimationStatus.SetBool("Right", true);
            //animatePlayer.setSpriteState(PlaneState.Right);
        }
        else if (curTargetPos.x < transform.position.x)
        {
            PlayerAnimationStatus.SetBool("Left", true);
            //animatePlayer.setSpriteState(PlaneState.Left);
        }
        else
        {
            PlayerAnimationStatus.SetBool("Right", false);
            PlayerAnimationStatus.SetBool("Left", false);
            //animatePlayer.setSpriteState(PlaneState.NoMove);
        }
    }

    /// <summary>
    /// 设置出现的时间 地点 速度 方向
    /// </summary>
    /// <param name="appearTime"></param>
    /// <param name="appearPos"></param>
    /// <param name="appearSpeed"></param>
    /// <param name="appearDir"></param>
    public void SetAppear(float appearTime, Vector2 appearPos,float appearSpeed,Vector2 appearDir)
    {
            Speed_CurValue = appearSpeed;
            rigidbody2d.velocity.Set(appearDir.x * appearSpeed,appearDir.y*appearSpeed);
            //Dir_CurSpeed = appearDir;
            activeTime = appearTime;
            gameObject.transform.position = appearPos;
    }

    private void checkCanShoot() {
        if (MyPlane.MyPos == null)
        {
            isCanShoot = false;
        }
        else {
            isCanShoot = true;
        }
    }


    /// <summary>
    /// 检测是否与子弹碰撞了
    /// </summary>
    /// <param name="otherCollider"></param>
    //void OnTriggerEnter2D(Collider2D otherCollider)
    //{
    //    BulletBase_Touhou bullet = otherCollider.GetComponent<BulletBase_Touhou>();
    //    if (bullet != null && bullet.renderer.sortingLayerName == CommandString.MyBulletLayer)
    //    {
    //        Destroy(bullet.gameObject);
    //        HpValue -= bullet.Power;
    //        if (HpValue <= 0)
    //        {
    //            Dead();
    //        }
    //        StageManager.CurStage.myPlane.Score += 100;
    //        PlayHitSound();
    //    }
    //}

    public void HitCheckAll()
    {
        //敌机与自机子弹判定
        //for (int i = MyBulletList.Count - 1; i >= 0; i--)
        //{
        //    if (HitCheck(MyBulletList[i]))
        //    {
        //        HealthPoint -= MyBulletList[i].Damage;
        //        MyBulletList[i].GiveEndEffect();

        //        MyBulletList.RemoveAt(i);
        //        MyPlane.Score += 100;

        //        if (HealthPoint <= 0)
        //        {
        //            GiveEndEffect();
        //            GiveItems();
        //            EnemyPlaneList.Remove(this);

        //            //StageData.SoundPlay("010.wav" );
        //            StageData.SoundPlay("se_enep00.wav");
        //            break;
        //        }
        //        else
        //        {
        //            StageData.SoundPlay("se_damage01.wav");
        //        }
        //    }
        //}

        ////敌机与自机判定
        //if (HitCheck(MyPlane))
        //{
        //    MyPlane.PreMiss();
        //}
    }

    protected float minHitAudioRate = 0.08f;//打击音效最快的播放间隔
    private float nextHitAudio = 0;//下次播放时间
    private void PlayHitSound() {
        if (Time.time > nextHitAudio) {
            nextHitAudio = Time.time + minHitAudioRate;
            baseAudio.PlayOneShot(hitSound);
        }
        
    }

    

    //
    /// <summary>
    /// 被打中伤血
    /// </summary>
    /// <param name="hitPower">被打中的子弹威力</param>
    public override void bHit(float hitPower)
    {

        if (hitEnable)
        {
            HpValue -= hitPower;
            if (HpValue <= 0)
            {
                Dead();
            }
            StageManager.CurStage.myPlane.Score += 100;
            PlayHitSound();
            //for (int i = hitObjectList.Count - 1; i >= 0; i--)
            //{
            //    BulletBase_Touhou bullet = hitObjectList[i].GetComponent<BulletBase_Touhou>();
            //    if (bullet != null && bullet.renderer.sortingLayerName == CommandString.MyBulletLayer)
            //    {
            //        HpValue -= bullet.Power;
            //        if (HpValue <= 0)
            //        {
            //            Dead();
            //        }
            //        StageManager.CurStage.myPlane.Score += 100;
            //        PlayHitSound();
            //        //GameObject obj_Bullet = hitObjectList[i].gameObject;
            //        GameObject.Destroy(hitObjectList[i]);
            //        hitObjectList.RemoveAt(i);
            //    }
            //    else {
            //        hitObjectList.RemoveAt(i);
            //    }
            //}
            
        }
    }

   

	// Update is called once per frame
    public override void Update()
    {
        base.Update();
        //bHit();
        //checkCanShoot();
        UpdateAnimateByDir();
    }
}

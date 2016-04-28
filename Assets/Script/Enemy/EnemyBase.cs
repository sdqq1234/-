using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class EnemyBase : PlaneBase {

    private string str_HitSound = "se_damage01";
    private string str_DeadSound = "se_enep00";
    private AudioClip hitSound;
    public float activeTime = -1;//敌人出现在屏幕里的时间
    public PlayMakerFSM enemyFsm;//敌人身上的状态机
    private Vector3 curTargetPos;
    
    Vector2 BornSpeedDir;

    void Awake() {
        enemyFsm = gameObject.GetComponent<PlayMakerFSM>();    
    }

	// Use this for initialization
	public override void Start () {
        base.Start();
        if (enemyFsm != null)
        {
            enemyFsm.Fsm.Event("MoveIn");
        }
        hitSound = Resources.Load(CommandString.SoundPath + str_HitSound) as AudioClip;
        deadSound = Resources.Load(CommandString.SoundPath + str_DeadSound) as AudioClip;
        hitEnable = true;
        isCanShoot = true;
        isShooting = true;
	}

    //设置当前要移动到的目标位置  这个方法由playmaker事件来调用
    public void setCurTargetPos(Vector3 targetPos)
    {
        curTargetPos = targetPos;
    }

    //根据节奏射击
    public void SetPlaneShootByRhythm(StageBase stage)
    {
        isShootByRhythm = true;
        stage.StageBulletRhythmEvent += PlaneShoot;
    }

    //去除场景节奏事件
    public void deletStageRhythmEvent(StageBase stage) {
        isShootByRhythm = false;
        stage.StageBulletRhythmEvent -= PlaneShoot;
    }

    private void UpdateAnimateByDir() {
        
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

    protected float minHitAudioRate = 0.08f;//打击音效最快的播放间隔
    private float nextHitAudio = 0;//下次播放时间
    private void PlayHitSound() {
        if (Time.time > nextHitAudio) {
            nextHitAudio = Time.time + minHitAudioRate;
            //baseAudio.PlayOneShot(hitSound);
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
        }
    }

   

	// Update is called once per frame
    public override void Update()
    {
        base.Update();
        UpdateAnimateByDir();
    }
}

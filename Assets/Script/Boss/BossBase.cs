using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossBase : EnemyBase {

    protected List<EmitterBase> BossEmitterList = new List<EmitterBase>(); //boss拥有的发射器列表
    
    protected int ShooterIndex = 0;//当前boss使用的的发射器索引
    public enum BossState { 
        InTalk = 0,
        InNormal,
        InCard
    }
    public BossState bossState = BossState.InNormal;
    public bool isInCard = false;//是否在符卡阶段

     //Use this for initialization
    void Start()
    {
        base.Start();

    }
	
    //////// Update is called once per frame
    //public virtual void Update()
    //{
    //    base.Update();
    //}
}

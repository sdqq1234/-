using UnityEngine;
using System.Collections;

//自动狙击类型子弹发射器
public class TargetShooter : EmitterBase
{
    //Vector2[] TargetBulletDir = { new Vector2(0f, 1) };//子弹射击出现的方向
    //Vector2[] TargetBulletPos = { Vector2.zero };//子弹射击出现的方向

    //// Use this for initialization
    //public override void Start()
    //{
    //    base.Start();
    //    string  targetBulletName = "bullet4_0";
    //    bulletNameList.Add(targetBulletName);
    //    //shootRate = AudioManager.EveryBeatsLenght;
    //    setBulletSortingLayer(CommandString.EnemyBulletLayer);
    //    hitEnable = false;
    //    dir_Bullets = TargetBulletDir;
    //    pos_Bullets = TargetBulletPos;
    //    Bullet_dirSameSpeed = true;
        
    //}
    //public override void Update() {
    //    base.Update();
    //    for (int i = 0; i < pos_Bullets.Length; i++)
    //    {
    //        if (MyPlane.MyPos != null)
    //        {
    //            dir_Bullets[i] = ((Vector2)MyPlane.MyPos.transform.position - (Vector2)transform.position).normalized;
    //        }
    //        else
    //        {
    //            dir_Bullets[i] = -Vector2.up;
    //        }
            
    //    }
    //}
}

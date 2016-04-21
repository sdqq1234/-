using UnityEngine;
using System.Collections;

//蓝点
public class BlueItem : ItemBase
{
    
    //// Use this for initialization
    void Start()
    {
        base.Start();
        ItemValue = 1;
        ScoreValue = 10000;
        Type = ItemType.BlueItem;
    }

    

    //// Update is called once per frame
    //void Update()
    //{
    //    base.Update();
    //}
}

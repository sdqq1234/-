using UnityEngine;
using System.Collections;

//红点
public class RedItem : ItemBase
{

	// Use this for initialization
    void Start()
    {
        base.Start();
        ItemValue = 5;
        ScoreValue = 1000;
        Type = ItemType.RedItem;
    }

    //void Update() {
    //    base.Update();
    //}

    //// Update is called once per frame
    //void Update () {
	
    //}
}

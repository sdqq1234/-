using UnityEngine;
using System.Collections;

//绿点
public class GreenItemBig : ItemBase
{

	// Use this for initialization
    void Start()
    {
        base.Start();
        ItemValue = 10;
        ScoreValue = 1000;
        Type = ItemType.GreenItem;
    }

    //// Update is called once per frame
    //void Update () {
	
    //}
}

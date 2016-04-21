using UnityEngine;
using System.Collections;

//绿点
public class GreenItemSmall : ItemBase
{

	// Use this for initialization
    void Start()
    {
        base.Start();
        ItemValue = 1;
        ScoreValue = 1000;
        Type = ItemType.GrazeItem;
    }

    //// Update is called once per frame
    //void Update () {
	
    //}
}

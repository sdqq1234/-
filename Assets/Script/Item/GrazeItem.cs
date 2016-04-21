using UnityEngine;
using System.Collections;

public class GrazeItem : ItemBase
{

	// Use this for initialization
    void Start()
    {
        base.Start();
        ItemValue = 1;
        ScoreValue = 1000;
        Type = ItemType.GrazeItem;
        isGrazed = true;
        Box2D.enabled = false;
    }
    //// Update is called once per frame
    //void Update () {
	
    //}
}

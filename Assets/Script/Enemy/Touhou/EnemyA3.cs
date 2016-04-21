using UnityEngine;
using System.Collections;

public class EnemyA3 : EnemyBase
{

	// Use this for initialization
	void Start () {
        base.Start();
        //setDeadValue(null, Color.red);
        HpValue = 10;
        ItemNameList.Add("RedItem");
	}
	
	// Update is called once per frame
    //void Update () {
    //    base.Update();
	
    //}
}

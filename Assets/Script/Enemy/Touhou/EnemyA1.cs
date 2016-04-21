using UnityEngine;
using System.Collections;

public class EnemyA1 : EnemyBase {

     //Use this for initialization
    void Start () {
        base.Start();
        //setDeadValue(null,Color.blue);
        HpValue = 10;
        ItemNameList.Add("BlueItem");
    }
	
	// Update is called once per frame
    //void Update () {
	
    //}
}

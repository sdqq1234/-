using UnityEngine;
using System.Collections;

public class EnemyA2 : EnemyBase {

     //Use this for initialization
    void Start () {
        base.Start();
        curEmitter = this.GetComponent<EmitterBase>();
        //setDeadValue(null,Color.green);
        HpValue = 10;
        ItemNameList.Add("GreenItemBig");
    }
	
	// Update is called once per frame
    //void Update () {
	
    //}
}

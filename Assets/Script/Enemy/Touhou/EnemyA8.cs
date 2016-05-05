
using UnityEngine;
using System.Collections;

public class EnemyA8 : EnemyBase
{

	// Use this for initialization
	void Start () {
        base.Start();
        curEmitter = this.GetComponent<EmitterBase>();
        //setDeadValue(null, Color.red);
        HpValue = 50;
        ItemNameList.Add("RedItem");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

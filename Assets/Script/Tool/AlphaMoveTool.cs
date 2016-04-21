using UnityEngine;
using System.Collections;

//加分显示逻辑,带透明渐变的移动效果
public class AlphaMoveTool : MonoBehaviour {
    public bool isAlphaChange = true;//是否为alpha渐变
    public Vector2 randomDir = Vector2.up;//默认移动方向为向上
    private float moveSpeed = 1.5f;//默认移动速度为30
    private float changeSpeed = 0.1f;//渐变速度
    private float live = 0f;//出生时间
    private SpriteRenderer sp;//当前显示的精灵

	// Use this for initialization
	void Start () {
        sp = gameObject.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        live += Time.deltaTime;
        if (isAlphaChange) {
            //alpha渐变
            Color curColor = sp.color;
            float a = sp.color.a;
            if (a > 0.03)
            {
                a = Mathf.Lerp(a, 0, live * changeSpeed);
                sp.color = new Color(curColor.r, curColor.g, curColor.b, a);
            }
            else {
                GameObject.Destroy(gameObject);
            }
            
        }

        //物体移动
        Vector2 cur_Positon = gameObject.transform.position;
        Vector2 target = randomDir * moveSpeed + cur_Positon;
        gameObject.transform.position = Vector2.Lerp(cur_Positon, target, live * moveSpeed);
	}
}

using UnityEngine;
using System.Collections;

public class UVMove : MonoBehaviour {

    Renderer objectRender;//物体的渲染组件
    Material objMat;//物体的材质球
    public bool isShake_Y = false;//是否摆动
    public float UVSpeed_X = 1;//x轴上的速度
    public float UVSpeed_Y = 1;//y轴上的速度
    public float ShakeRange = 0.025f;
    public float ShakeSpeed = 3;//抖动速度
	// Use this for initialization
	void Start () {
        objectRender = gameObject.GetComponent<MeshRenderer>();
        objMat = objectRender.material;
	}
	
	// Update is called once per frame
	void Update () {
        float uv_x = objMat.GetTextureOffset("_MainTex").x;
        float uv_y = objMat.GetTextureOffset("_MainTex").y;
        if (isShake_Y) {
            UVSpeed_Y = Mathf.Sin(Time.time * ShakeSpeed) * ShakeRange;
        }
        Vector2 offset = new Vector2(uv_x + Time.deltaTime * UVSpeed_X, uv_y + Time.deltaTime * UVSpeed_Y);
        objMat.SetTextureOffset("_MainTex", offset);
	}
}

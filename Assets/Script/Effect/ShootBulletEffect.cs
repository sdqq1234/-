using UnityEngine;
using System.Collections;

public class ShootBulletEffect : MonoBehaviour {

    private SpriteRenderer SpRender;//自身的渲染
    private Color SpColor;//需要渲染的颜色
    private float CurLifeTime = 0;//生存时间
    private float StartAlpha = 0.5f;//出生时的alpha值
    public float ChangeSpeed = 10f;//最大变化速度

	// Use this for initialization
	void Start () {
        SpRender = gameObject.GetComponent<SpriteRenderer>();
	}

    void ChangeLogic() {
        if (SpRender.color.a > 0.1)
        {
            float curAlpha = SpRender.color.a;
            curAlpha = Mathf.Lerp(curAlpha, 0, Time.deltaTime * ChangeSpeed);
            Color c = new Color(SpRender.color.r, SpRender.color.g, SpRender.color.b, curAlpha);
            SpRender.color = c;
        }
        else {
            GameObject.Destroy(gameObject);
        }
    }

	// Update is called once per frame
	void Update () {
        ChangeLogic();
	}
}

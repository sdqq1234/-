using UnityEngine;
using System.Collections;

public class LoadingAlphaShake : MonoBehaviour {

    public UIPanel LoadingPanel;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        LoadingPanel.alpha = Mathf.Abs(Mathf.Sin(Time.time));
	}
}

using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class OnButtonEvent : MonoBehaviour, IPointerEnterHandler
{
    public GameStartMenuManager1 Manager;
	// Use this for initialization
	void Start () {
        if (Manager == null) {
            Debug.Log(this.gameObject.name + " Manager is null");
        }
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
    public void OnPointerEnter(PointerEventData eventData)
    {
        Manager.PlayOnSelectButtonSound();
        //Debug.Log(this.gameObject.name + " was selected");
    }
}

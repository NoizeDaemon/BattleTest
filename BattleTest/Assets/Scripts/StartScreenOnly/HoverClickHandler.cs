using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverClickHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private BoxCollider2D coll;
    private StartMenuHandler startMenuHandler;

	// Use this for initialization
	void Start ()
	{
	    startMenuHandler = GameObject.Find("StartMenuHandler").GetComponent<StartMenuHandler>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Attached script registered Enter");
        startMenuHandler.OnLeftMidSpearEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Attached script registered Exit");
        startMenuHandler.OnLeftMidSpearExit();
    }
}

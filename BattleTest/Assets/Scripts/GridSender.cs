using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridSender : MonoBehaviour
{
    private GameHandler gameHandler;
    private Vector3 pos;

	// Use this for initialization
	void Start ()
	{
	    gameHandler = GameObject.Find("GameHandler").GetComponent<GameHandler>();
        //var all = this.name.Split('/');
        //pos = new Vector3(float.Parse(all[0]), float.Parse(all[1]), float.Parse(all[2]));
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //private void OnMouseOver()
    //{
    //    Debug.Log("Hovered: " + this.name);
    //}

    private void OnMouseUpAsButton()
    {
        //Debug.Log("Clicked: " + this.name);
        gameHandler.GridClick(this.gameObject);
    }
}

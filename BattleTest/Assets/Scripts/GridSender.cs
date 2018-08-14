using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridSender : MonoBehaviour
{
    private FloorHandler floorHandler;
    private Vector3 pos;

	// Use this for initialization
	void Start ()
	{
	    floorHandler = GameObject.Find("FloorHandler").GetComponent<FloorHandler>();
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
        StartCoroutine(floorHandler.GridClick(this.gameObject));
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridSender : MonoBehaviour
{
    private GameHandler gameHandler;
    private Vector3 pos;
    private Color movement, attack, skill;

	// Use this for initialization
	void Awake ()
	{
	    gameHandler = GameObject.Find("GameHandler").GetComponent<GameHandler>();
        movement = new Color(0,0.3f,0.7f,1);
        attack = new Color(1,0.5f,0,1);
        skill = new Color(0.09f, 0.8f, 0.01f, 1);

        //var all = this.name.Split('/');
        //pos = new Vector3(float.Parse(all[0]), float.Parse(all[1]), float.Parse(all[2]));
    }

    private void OnEnable()
    {
        switch (gameHandler.gridColor)
        {
            case 0:
                this.gameObject.GetComponent<SpriteRenderer>().color = movement;
                break;
            case 1:
                this.gameObject.GetComponent<SpriteRenderer>().color = attack;
                break;
            case 2:
                this.gameObject.GetComponent<SpriteRenderer>().color = skill;
                break;
            default:
                break;
        }
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

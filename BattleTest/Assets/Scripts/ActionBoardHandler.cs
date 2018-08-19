using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActionBoardHandler : MonoBehaviour {

    public Animator anim;
    public TextMeshProUGUI playerName;

    private bool hidden;
    private bool off;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Hide()
    {
        if (hidden)
        {
            hidden = false;
            anim.SetBool("Hide", false);
        }
        else
        {
            hidden = true;
            anim.SetBool("Hide", true);
        }
    }

    public void Toggle()
    {
        if (off)
        {
            off = false;
            anim.SetBool("Hide", false);
            anim.SetBool("EnemyTurn", false);
        }
        else
        {
            off = true;
            anim.SetBool("EnemyTurn", true);
        }
    }
}

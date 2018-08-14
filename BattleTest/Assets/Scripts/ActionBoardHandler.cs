using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActionBoardHandler : MonoBehaviour {

    public Animator anim;
    public TextMeshProUGUI playerName;

    private bool hidden;


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
}

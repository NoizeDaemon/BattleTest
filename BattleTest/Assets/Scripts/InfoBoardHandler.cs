using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoBoardHandler : MonoBehaviour {

    public Animator anim;
    private bool hidden;
    public List<GameObject> status;
    public GameObject slowed, noJump, noMove, noAction, dead, fine;
    public TextMeshProUGUI charName, hp, mp;


	// Use this for initialization
	void Start () {
        anim = this.GetComponent<Animator>();
        status = new List<GameObject>(){slowed, noJump, noMove, noAction, dead, fine};
        for (int i = 0; i < 5; i++) status[i].SetActive(false);
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

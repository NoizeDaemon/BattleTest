using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuHandler : MonoBehaviour
{

    public ParticleSystem leftMidSpear, midMidSpear, frontSword;
    public Button leftMidSpearBtn, midMidSpearBtn, frontSwordBtn;

    private bool isHovering;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnLeftMidSpearEnter()
    {
        Debug.Log("Enter triggered.");
        if (!isHovering)
        {
            var thisEmission = leftMidSpear.emission;
            thisEmission.rateOverTime = 5;
            var thisMain = leftMidSpear.main;
            thisMain.simulationSpeed = 5;
            //var thisMainSpeed = leftMidSpear.main.startSpeed;
            //thisMainSpeed.constantMin = 300;
            //thisMainSpeed.constantMax = 600;
            var thisMainLifetime = leftMidSpear.main.startLifetime;
            thisMainLifetime.constantMin = 1;
            thisMainLifetime.constantMax = 3;
            isHovering = true;
        }
    }

    public void OnLeftMidSpearExit()
    {
        Debug.Log("Exit triggered.");
        if (isHovering)
        {
            var thisEmission = leftMidSpear.emission;
            thisEmission.rateOverTime = 0.5f;
            var thisMain = leftMidSpear.main;
            thisMain.simulationSpeed = 1;
            //var thisMainSpeed = leftMidSpear.main.startSpeed;
            //thisMainSpeed.constantMin = 25;
            //thisMainSpeed.constantMax = 60;
            var thisMainLifetime = leftMidSpear.main.startLifetime;
            thisMainLifetime.constantMin = 6;
            thisMainLifetime.constantMax = 15;
            isHovering = false;
        }
    }
}

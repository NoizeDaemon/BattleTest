using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameHandler : MonoBehaviour {

    //UI
    public GameObject infoBoard, actionBoard, introBoard;
    public GameObject compass;

    //ActionBoard
    public List<Button> actionButton;

    //ScriptReferences
    private InfoBoardHandler infoBoardHandler;
    private ActionBoardHandler actionBoardHandler;
    public FloorHandler floorHandler;
    private CamController camController;

    //PlayerRelated
    [System.Serializable]
    public class CharInfo
    {
        public GameObject go;
        public int maxHp, currentHp, maxMp, currentMp, init;
        public bool[] status = new bool[6];
        public List<FloorHandler.GridInfo> movable;
        public List<FloorHandler.GridInfo> attackable;
    }
    public List<CharInfo> player;
    public List<CharInfo> enemy;
    public CharInfo activeChar;
    //MovementRelated
    [HideInInspector]
    public bool canBeCancelled;
    [HideInInspector]
    public bool canBeUndone;


    //Phases:
    //1) IntroSequence - Displaying Goals (Defeat all enemies, protect character, survive, etc.), Rules (No magic allowed, time limit, etc.)
    //2) PlacementSequence - Choosing and placing all characters on the field, being able to equip items or change skill setups
    //3) Battle:
    //  1] Introducing the turn order bar
    //  2] Actual battle stuff
    //  3] Trigger potential dialogues/events during or after the battle
    //4) EndSequence - List loot, xP & battle stats
    //5) LevelUpSequence

	// Use this for initialization
	void Start () {
        //ScriptReference Initialization
        infoBoardHandler = infoBoard.GetComponent<InfoBoardHandler>();
        actionBoardHandler = actionBoard.GetComponent<ActionBoardHandler>();
        //ObjectReference Initialization

        StartCoroutine(WaitForInitialization());
	}

    IEnumerator WaitForInitialization()
    {
        Debug.Log("Waiting for all scripts to finish Initialization...");
        Debug.Log("Started waiting at " + Time.time);
        yield return new WaitUntil(() => floorHandler.initComplete);
        Debug.Log("FloorHandler initialized at " + Time.time);
        floorHandler.UpdateMovementGrid(activeChar);
        Debug.Log("Waiting for UpdateMovementGrid to finish...");
        Debug.Log("Started waiting at " + Time.time);
        yield return new WaitUntil(() => floorHandler.calcComplete);
        Debug.Log("Completed at " + Time.time);
        floorHandler.calcComplete = false;
        HideIntroBoard();
        floorHandler.DisplayMovementGrid(activeChar);
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void MovementButtonPress()
    {
        if(!canBeCancelled && !canBeUndone)
        {
            floorHandler.UpdateMovementGrid(activeChar); //Should be done at the start of turn
            actionButton[0].GetComponentInChildren<TextMeshProUGUI>().text = "cancel";
            actionButton[0].interactable = false;
        }
        else if (canBeCancelled)
        {

        }
    }


    public void HideIntroBoard()
    {
        introBoard.GetComponent<Animator>().SetBool("Hide", true);
    }
}

//Calculate new position/grids of chars at the end of each turn, but check for changes to avoid too many calculations
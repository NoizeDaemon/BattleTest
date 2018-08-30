using System.Collections;
using System.Collections.Generic;
using Assets.UltimateIsometricToolkit.Scripts.Core;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameHandler : MonoBehaviour {

    //Testing
    public Skill skill;

    //PhaseManagement
    public byte gridColor; //
    public char action;

    //TurnManagement
    public int turnCount; 
    public byte charCount;
    public bool movementIsPerm;
    public bool endTurn;

    //UI
    public GameObject infoBoard, actionBoard, introBoard;
    public GameObject compass;

    //ActionBoard
    public List<Button> actionButton;

    //ScriptReferences
    private InfoBoardHandler infoBoardHandler;
    private ActionBoardHandler actionBoardHandler;
    public FloorHandler floorHandler;
    public CamController camController;

    //CharacterRelated
    [System.Serializable]
    public class CharInfo
    {
        public GameObject go;
        public int maxHp, currentHp, maxMp, currentMp, init, ms;
        public float jh;
        public bool[] status = new bool[6];
        public List<FloorHandler.GridInfo> movable, attackable;
    }
    public List<CharInfo> playerInfo, enemyInfo;

    public List<Character> tList;
    //[HideInInspector]
    public List<CharInfo> everyInfo;
    public CharInfo activeChar;

    //MovementRelated
    [HideInInspector]
    public bool canBeCancelled;
    [HideInInspector]
    public bool canBeUndone;

    //BattleRelated



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
	void Start ()
	{
	    //foreach (Character t in tList)
	    //{
	    //    t.currentHp -= (int) SkillCalculations.ForceDictionary[skill.forceIndex].DynamicInvoke(activeChar, t);
	    //}
         

	    gridColor = 1;
        //ScriptReference Initialization
        infoBoardHandler = infoBoard.GetComponent<InfoBoardHandler>();
        actionBoardHandler = actionBoard.GetComponent<ActionBoardHandler>();
        //ObjectReference Initialization
        everyInfo = new List<CharInfo>(playerInfo);
        everyInfo.AddRange(enemyInfo);
        everyInfo.Sort((a, b) => a.init.CompareTo(b.init));
        everyInfo.Reverse();

        StartCoroutine(WaitForInitialization());
	}

    IEnumerator WaitForInitialization()
    {
        Debug.Log("Waiting for all scripts to finish Initialization...");
        Debug.Log("Started waiting at " + Time.time);
        yield return new WaitUntil(() => floorHandler.initComplete);
        Debug.Log("FloorHandler initialized at " + Time.time);
        foreach (CharInfo chara in everyInfo)
        {
            floorHandler.UpdateMovementGrid(chara);
            Debug.Log("Waiting for UpdateMovementGrid to finish...");
            Debug.Log("Started waiting at " + Time.time);
            yield return new WaitUntil(() => floorHandler.calcComplete);
            Debug.Log("Completed at " + Time.time);
            floorHandler.calcComplete = false;
        }
        introBoard.GetComponent<Button>().enabled = true;
        NextChar();
    }

    IEnumerator MovementCycleTest()
    {
        for (int r = 1; r < 11; r++)
        {
            Debug.Log("ROUND: " + r);
            foreach (CharInfo t in everyInfo)
            {
                activeChar = t;
                UpdateInfoBoard(activeChar);
                floorHandler.ToggleMovementGrid(activeChar, true);
                yield return new WaitUntil(() => floorHandler.moveComplete);
                floorHandler.moveComplete = false;
                foreach (CharInfo chara in everyInfo)
                {
                    floorHandler.UpdateMovementGrid(chara);
                    Debug.Log("Waiting for UpdateMovementGrid(" + chara.go.name + ") to finish...");
                    Debug.Log("Started waiting at " + Time.time);
                    yield return new WaitUntil(() => floorHandler.calcComplete);
                    Debug.Log("Completed at " + Time.time);
                    floorHandler.calcComplete = false;
                }
            }
        }
    }

    private void NextChar()
    {
        activeChar = everyInfo[charCount];
        endTurn = false;
        movementIsPerm = false;
        StartCoroutine(camController.SmoothFocus(activeChar.go));
        camController.canBeMoved = true;
        //if (playerInfo.Contains(activeChar)) StartCoroutine(PlayerTurn());
        //else StartCoroutine(EnemyTurn());
        //testing
        StartCoroutine(PlayerTurn());
        
    }

    IEnumerator PlayerTurn()
    {
        actionBoardHandler.Toggle(true);
        yield return new WaitUntil(() => endTurn);
    }

    IEnumerator EnemyTurn()
    {
        yield return null;
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void MovementButtonPress()
    {
        gridColor = 0;
        action = 'm';
        if(!canBeCancelled && !canBeUndone)
        {
            foreach (Button b in actionButton)
            {
                if (b != null && b != actionButton[0]) b.interactable = false;
            }
            floorHandler.ToggleMovementGrid(activeChar, true);
            actionButton[0].GetComponentInChildren<TextMeshProUGUI>().text = "cancel";
            canBeCancelled = true;
            //actionButton[0].interactable = false;
        }
        else if (canBeCancelled)
        {
            foreach (Button b in actionButton)
            {
                if (b != null && b != actionButton[0]) b.interactable = true;
            }
            floorHandler.ToggleMovementGrid(activeChar, false);
            actionButton[0].GetComponentInChildren<TextMeshProUGUI>().text = "movement";
            canBeCancelled = false;
        }
        else if (canBeUndone)
        {
            foreach (Button b in actionButton)
            {
                if (b != null && b != actionButton[0]) b.interactable = false;
            }
            activeChar.go.GetComponent<IsoTransform>().Position = floorHandler.originalCharPos;
            StopCoroutine(floorHandler.MoveClick(floorHandler.lastClicked));
            floorHandler.ToggleMovementGrid(activeChar, true);
            actionButton[0].GetComponentInChildren<TextMeshProUGUI>().text = "cancel";
            canBeCancelled = true;
            canBeUndone = false;
        }
    }

    public void AttackButtonPress()
    {
        gridColor = 1;
        action = 'a';

        floorHandler.ToggleAttackGrid(activeChar, true);
    }

    public void EndButtonPress()
    {
        StartCoroutine(EndTurn());
    }

    public void ButtonStateCheck()
    {
        foreach (Button b in actionButton)
        {
            if (b != null && b != actionButton[0]) b.interactable = true;
        }
    }

    public void GridClick(GameObject g)
    {
        switch (action)
        {
            case 'm':
                StartCoroutine(floorHandler.MoveClick(g));
                break;
            case 'a':
                AttackClick(g);
                break;
        }
        
    }

    private void AttackClick(GameObject g)
    {
        var clicked = floorHandler.Grid.Find(x => x.go == g);
        if (clicked.pop != 0)
        {
            floorHandler.ToggleAttackGrid(activeChar, false);
            Debug.Log("Someone hit someone.");
            actionButton[1].interactable = false;
            if (canBeUndone)
            {
                actionButton[0].interactable = false;
                movementIsPerm = true;
            }
        }
    }

    IEnumerator EndTurn()
    {
        movementIsPerm = true;
        canBeUndone = false;
        canBeCancelled = false;
        actionButton[0].interactable = true;
        actionButton[1].interactable = true;
        if (floorHandler.lastDisplayed != null)
        {
            foreach (FloorHandler.GridInfo gr in floorHandler.lastDisplayed)
            {
                gr.go.SetActive(false);
            }
        }
        actionButton[0].GetComponentInChildren<TextMeshProUGUI>().text = "movement";
        yield return new WaitUntil(() => floorHandler.calcComplete);
        foreach (CharInfo chara in everyInfo)
        {
            floorHandler.UpdateMovementGrid(chara);
            Debug.Log("Waiting for UpdateMovementGrid to finish...");
            Debug.Log("Started waiting at " + Time.time);
            yield return new WaitUntil(() => floorHandler.calcComplete);
            Debug.Log("Completed at " + Time.time);
            floorHandler.calcComplete = false;
        }
        charCount += 1;
        if (charCount >= everyInfo.Count) charCount = 0;
        NextChar();
    }

    public void HideIntroBoard()
    {
        introBoard.GetComponent<Animator>().SetBool("Hide", true);
    }

    public void UpdateInfoBoard(CharInfo chara)
    {
        infoBoardHandler.charName.text = chara.go.name;
        infoBoardHandler.hp.text = chara.currentHp + "/" + chara.maxHp;
        infoBoardHandler.mp.text = chara.currentMp + "/" + chara.maxMp;
    }
}

//Calculate new position/grids of chars at the end of each turn, but check for changes to avoid too many calculations
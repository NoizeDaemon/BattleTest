using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public new string name;
    public int charIndex;
    public string description;
    public string mood;

    public Race race;
    public Class activeClass;

    public List<Skill> learnedSkills;

    public class SkillInLearning
    {
        public Skill skillToBeLearned;
        public int progress;
    }

    public List<SkillInLearning> skillsTobeLearned;

    //BaseStats
    public int currentHp, maxHp, currentMp, maxMp, init, luck, ad, ap, ar, mr, ev, im, ac;

    //Equipment
    public Equipment head;
    public Equipment neck;
    public Equipment torso;
    public Equipment arms;
    public Equipment leftHand;
    public Equipment rightHand;
    public Equipment waist;
    public Equipment legs;
    public Equipment feet;

    private List<Equipment> equipped;

    //Stored
    public List<Item> inventoryItems;
    public List<Equipment> inventoryEquipments;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    //BaseStats
    public int currentHp, maxHp, currentMp, maxMp, init, acc, ad, ap, ar, mr, ev, ac;

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

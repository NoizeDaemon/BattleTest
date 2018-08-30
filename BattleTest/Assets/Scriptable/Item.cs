using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newItem", menuName = "Item")]
public class Item : ScriptableObject {

    public new string name;
    public string description;
    public int newValue;
    public int usedValue;
    public byte rarity;
    public byte type;
    public int weight;

    public int hpRestore;
    public int mpRestore;
    public List<byte> statusRestore;

    //public float hpFactor;
    //public int hpFlat;
    //public float mpFactor;
    //public int mpFlat;

    //public int ad;
    //public int ap;
    //public int ar;
    //public int mr;

    //public byte init;
    //public float initFactor;
    //public float acFactor;
    //public float luckFactor;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newEquipment", menuName = "Equipment")]
public class Equipment : ScriptableObject {

    public new string name;
    public string description;
    public int newValue;
    public int usedValue;
    public byte rarity;
    public byte type;
    public int weight;

    public int mpFlat;

    public int ad;
    public int ap;
    public int ar;
    public int mr;

    public byte init;
    public float initFactor;
    public float acFactor;
    public float luckFactor;
}

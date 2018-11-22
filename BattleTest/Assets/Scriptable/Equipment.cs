using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newEquipment", menuName = "Equipment")]
public class Equipment : ScriptableObject {

    public new string name;
    public string description;
    public string type;
    public int newValue;
    public int usedValue;
    public byte rarity;
    public int weight;

    public int mpFlat;

    public int ad, ap, ar, mr, ev, im, ac, luck, init;

    public float initFactor;
    public float acFactor;
    public float luckFactor;
}

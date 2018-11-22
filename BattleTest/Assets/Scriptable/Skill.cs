using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newSkill", menuName = "Skill")]
public class Skill : ScriptableObject
{
    public new string name;
    public string description;
    public bool targetAllies;
    public bool targetEnemies;
    public sbyte castRange; //0 = cast at own pos, -1 = weapon range, -2 = weapon range -1
    public byte spreadPattern; //See reference table
    public byte effectRange; //0 = target only

    public byte forceIndex; //See reference table
    public byte chanceIndex; //See reference table

    public int requiredSkillPoints;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newClass", menuName = "Class")]
public class Class : ScriptableObject
{
    public class SkillReq
    {
        public Class parentClass;
        public int skillCount;
        //public List<Skill> specificSkill;
    }

    //General
    public new string name;
    public string description;

    //Requirements
    //public List<Race> raceReq;
    public List<SkillReq> skillReqs;

    //StatsGrowth
    public float
        hpGrowthRate,
        mpGrowthRate,
        adGrowthRate,
        apGrowthRate,
        arGrowthRate,
        mrGrowthRate,
        initGrowthRate,
        acGrowthRate,
        luckGrowthRate;

    public int
        hpFlat,
        mpFlat,
        adFlat,
        apFlat,
        arFlat,
        mrFlat,
        initFlat;

    //public List<Skill> classSkills;

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newRace", menuName = "Race")]
public class Race : ScriptableObject {


    public new string name;
    public string description;

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
}

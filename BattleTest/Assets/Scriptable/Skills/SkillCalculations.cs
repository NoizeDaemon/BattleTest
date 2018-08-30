using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public static class SkillCalculations
{
    public static Dictionary<int, Delegate> ForceDictionary = new Dictionary<int, Delegate>()
    {
        {0, new Func<Character, Character, int>(PhysicalNormal) },
        {1, new Func<Character, Character, int>(MagicalNormal) }
    };

    public static int PhysicalNormal(Character s, Character t)
    {
        return s.ad - t.ar;
    }
    public static int MagicalNormal(Character s, Character t)
    {
        return s.ap - t.mr;
    }

    public static Dictionary<int, Delegate> ChanceDictionary = new Dictionary<int, Delegate>()
    {
        {0, new Func<Character, Character, int>(Guaranteed) },
        {1, new Func<Character, Character, int>(PhysicalStandard) }
    };

    public static int Guaranteed(Character s, Character t)
    {
        return 100;
    }

    public static int PhysicalStandard(Character s, Character t)
    {
        return 80 - t.ev + s.ac;
    }

    public static int MagicalStandard(Character s, Character t)
    {
        return Mathf.RoundToInt(100 - (t.ap/s.ap + t.mr/(s.ap*2))*10);
    }

}

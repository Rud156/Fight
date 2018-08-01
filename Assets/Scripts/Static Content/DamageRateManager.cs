using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageRateManager
{

    public static float GetSwordContactDamage()
    {
        return Random.Range(20, 40);
    }

    public static float GetFootContactDamage()
    {
        return Random.Range(10, 20);
    }

    public static float GetArcContactDamage()
    {
        return Random.Range(70, 140);
    }

}

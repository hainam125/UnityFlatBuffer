using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UNightMare : UBaseTest
{
    public long MasterId { get; set; }
    public int Level { get; set; }
    public int Exp { get; set; }
    public int TimePlayBattle { get; set; }

    public static long currentId = 1;
    public static UNightMare GetSample()
    {
        currentId++;
        return new UNightMare
        {
            Id = currentId + 1,
            TimePlayBattle = Random.Range(2, 100),
            Level = Random.Range(3, 10),
            Exp = Random.Range(99, 1000),
            MasterId = idOfM[Random.Range(0, idOfM.Count)]
        };
    }

    public static List<long> idOfM = new List<long>() { 6001, 6002, 6003, 6004};
}

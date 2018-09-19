using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGrowthBoard : MBaseTest
{
    public List<long> IdsOfSkillPoint { set; get; }
    
    public long IdOfModel { set; get; }

    public static long currentId = 0;

    public static MGrowthBoard GetSample()
    {
        return new MGrowthBoard
        {
            Id = currentId++,
            IdsOfSkillPoint = new List<long>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 },
            Name = Helper.RandomName("MMa"),
            IdOfModel = Random.Range(2, 8)
        };
    }
}

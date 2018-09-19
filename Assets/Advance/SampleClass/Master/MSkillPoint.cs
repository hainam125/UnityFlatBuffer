using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSkillPoint : MBaseTest
{
    public List<long> IdsOfSkillPoint { set; get; }
    
    public List<long> IdsOfMaterial { set; get; }

    public static long currentId = 0;

    public static MSkillPoint GetSample()
    {
        currentId++;
        return new MSkillPoint
        {
            Id = currentId,
            IdsOfSkillPoint = GetFrom(currentId),
            Name = Helper.RandomName("MSP"),
            IdsOfMaterial = Helper.RandomList(4, 8)
        };
    }

    private static List<long> GetFrom(long i)
    {
        if(i < 8)
        {
            return new List<long>() { i + 1, i + 2 };
        }
        return new List<long>() { };
    }
}

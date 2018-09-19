using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UGrowthBoard : Base1
{
    public long MasterId { get; set; }
    public List<long> IdsOfSkillPoint { set; get; }

    public static long currentId = 0;

    public static UGrowthBoard GetSample()
    {
        return new UGrowthBoard
        {
            Id = currentId++,
            IdsOfSkillPoint = new List<long> { 1 },
            MasterId = Random.Range(1, 8)
        };
    }
}

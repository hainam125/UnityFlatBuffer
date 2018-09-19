using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UNightWalker: UBaseTest
{
    public long MasterId { get; set; }
    public int Level { get; set; }
    public long Exp { get; set; }
    public short OverLimit { get; set; }
    public int TimePlayBattle { get; set; }
    public long Weapon { get; set; }

    public int Experience { get; set; }
    public float Health { get; set; }
    public float Magic { get; set; }
    public float Attack { get; set; }
    public float Power { get; set; }
    public int Overlimit { get; set; }
    public int Ap { get; set; }
    public long IdOfMainWeapon { get; set; }
    public long IdOfSubWeapon { get; set; }
    public long IdOfEpisode { get; set; }
    public long IdOfGrowthBoard { get; set; }
    public long IdOfModel { get; set; }
    public List<long> IdsOfSkillPassive { get; set; }
    public List<long> IdsOfSkillActive { set; get; }
    public List<long> IdsOfMode { get; set; }

    public static long currentId = 1;
    public static  UNightWalker GetSample()
    {
        currentId++;
        return new UNightWalker
        {
            Id = currentId + 1,
            IdsOfSkillPassive = new List<long>() { 113, 114, 116 },
            IdsOfSkillActive = new List<long>() { 103, 104, 107, 111 },
            Level = Random.Range(3, 10),
            Experience = Random.Range(99, 1000),
            Health = Random.Range(55, 160),
            Magic = Random.Range(4, 9),
            Attack = Random.Range(2, 9),
            Power = Random.Range(10, 40),
            IdOfMainWeapon = MWeapon.ids[Random.Range(0, MWeapon.ids.Count)],
            IdOfSubWeapon = MWeapon.ids[Random.Range(0, MWeapon.ids.Count)],
            IdOfEpisode = Random.Range(1, 7),
            IdsOfMode = Helper.RandomList(5, 6),
            IdOfModel = Random.Range(1, 7),
            IdOfGrowthBoard = 1,
            MasterId = idOfM[Random.Range(0, idOfM.Count)]
        };
    }

    public static List<long> idOfM = new List<long>() { 10002000,10004000,10006000,10056000};
}

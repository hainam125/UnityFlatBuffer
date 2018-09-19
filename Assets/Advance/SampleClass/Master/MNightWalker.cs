using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MNightWalker : MBaseTest
{
    public TypeOfGender TypeOfGender { get; set; }
    public TypeOfWeapon TypeOfWeapon { get; set; }
    public TypeOfAttribute TypeOfAttribute { get; set; }
    public TypeOfRarity TypeOfRarity { get; set; }
    public float Hp { get; set; }
    public float Mp { get; set; }
    public float Damage { get; set; }
    public float Defence { get; set; }
    public float SpeedMovement { get; set; }
    public float SpeedRotation { get; set; }
    public float Radius { get; set; }
    public long IdOfThumbnail { get; set; }
    public long IdOfArtImage { get; set; }
    public long IdOfCharacterView { get; set; }
    public long IdOfCharacterMotionLibrary { get; set; }
    public long IdOfCharacterActionLibrary { get; set; }
    public List<long> IdsOfWeapon { get; set; }
    public List<long> IdsOfSkillPassive { get; set; }
    public List<long> IdsOfSkillActive { get; set; }

    public TypeOfNWType Type { get; set; }
    public List<long> IdsOfModel { set; get; }
    public List<long> IdsOfStrengthen { set; get; }
    public List<long> IdsOfEpisode { set; get; }
    public List<long> IdsOfMode { set; get; }

    public override string ToString()
    {
        return Id + " - " + Name;
    }

    public static long id = 1;
    public static MNightWalker GetSample()
    {
        id++;
        return new MNightWalker
        {
            Id = id,Name = Helper.RandomName("NK"),
            TypeOfAttribute = (TypeOfAttribute)Random.Range(0, 6),
            TypeOfGender = (TypeOfGender)Random.Range(0, 2),
            TypeOfWeapon = (TypeOfWeapon)Random.Range(0, 4),
            TypeOfRarity = (TypeOfRarity)Random.Range(0, 4),
            Type = (TypeOfNWType)Random.Range(0, 2),
            Hp = Random.Range(10, 40),
            Mp = Random.Range(10, 40),
            Damage = Random.Range(10, 40),
            Defence = Random.Range(5, 20),

            IdsOfWeapon = Helper.RandomList(10, 10),
            IdsOfSkillPassive = Helper.RandomList(9, 10),
            IdsOfSkillActive = Helper.RandomList(9, 10),

            IdsOfModel = Helper.RandomList(5, 5),
            IdsOfStrengthen = Helper.RandomList(10, 10),
            IdsOfEpisode = Helper.RandomList(5, 5),
            IdsOfMode = Helper.RandomList(4, 4)
        };
    }
}

public enum TypeOfPresentStatus
{
    Available, Claimed, Expired
}
public enum TypeOfGender
{
    Male = 1,
    Female = 2
}
public enum TypeOfWeapon
{
    OneHandedSword = 1,
    Bow = 2,
    Hammer = 3,
    Wand = 4,
    BareHand = 5,
    Spear = 6
}
public enum TypeOfAttribute
{
    Default = 0,
    Fire = 1,
    Water = 2,
    Wind = 3,
    Ice = 4,
    Grass = 5,
    Thunder = 6
}
public enum TypeOfRarity
{
    One = 1,
    Two = 2,
    Three = 3,
    Four = 4,
    Five = 5,
    Six = 6
}
public enum TypeOfNWType
{
    Magic,
    Attack
}
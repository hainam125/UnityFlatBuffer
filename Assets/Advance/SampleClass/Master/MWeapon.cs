using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MWeapon : MBaseTest
{
    public TypeOfRarity TypeOfRarity { get; private set; }
    public float Hp { get; private set; }
    public float Mp { get; private set; }
    public float Damage { get; private set; }
    public float Defence { get; private set; }
    public string Dummy { get; private set; }
    public long IdOfWeaponView { get; private set; }
    public List<long> IdsOfSkillPassive { get; private set; }
    public List<long> IdsOfSkillActive { get; private set; }
    public long IdOfThumbnail { get; private set; }
    public long IdOfArtImage { get; private set; }

    public TypeOfRarity Rarity { get; set; }
    public int Level { get; set; }
    public int Attack { get; set; }
    public string Description { get; set; }
    public List<long> idsOfStrengthen { get; set; }
    public override string ToString() {
        return Id + " - " + Name;
	}
    public static int id = -1;
    public static MWeapon GetSample()
    {
        id++;
        return new MWeapon
        {
            Id = ids[id],
            Name = Helper.RandomName("WP"),
            Attack = Random.Range(4, 10),
            Defence = Random.Range(3, 6),
            Rarity = (TypeOfRarity)Random.Range(0, 3),
            Damage = Random.Range(1, 5),
            Description = "Hello",
            Level = Random.Range(1, 5),
            Mp = Random.Range(2, 4)
        };
    }


    public static List<long> ids = new List<long>() { 10002, 10019, 20002, 21002, 40002 };
}
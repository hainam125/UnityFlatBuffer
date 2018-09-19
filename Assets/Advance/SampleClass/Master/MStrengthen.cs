using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeOfStrengthen
{
    HP, ATK, MP, EXP
}

public class MItem : MBaseTest
{
    public long IdOfIcon { set; get; }
    public string Description { set; get; }
    public float Price { set; get; }
}

public class MMaterial : MItem
{
    public static long currentId = 0;

    public static MMaterial GetSample()
    {
        return new MMaterial
        {
            Id = currentId++,
            IdOfIcon = Random.Range(1, 10),
            Description = Helper.RandomName("A") + Helper.RandomName("B"),
            Name = Helper.RandomName("MMa"),
            Price = Random.Range(300, 5000)
        };
    }
}

public class MStrengthen : MItem
{
    public static long currentId = 0;
    public int AddedValue { set; get; }
    public TypeOfStrengthen Type { set; get; }

    public static MStrengthen GetSample()
    {
        return new MStrengthen
        {
            Id = currentId++,
            IdOfIcon = Random.Range(1,10),
            AddedValue = Random.Range(3,15),
            Description = Helper.RandomName("A") + Helper.RandomName("B"),
            Name = Helper.RandomName("STR"),
            Price = Random.Range(300,5000),
            Type = (TypeOfStrengthen)Random.Range(0, 4)
        };
    }
}

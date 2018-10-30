using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MItem : MBaseTest
{
    public long IdOfIcon { set; get; }
    public string Description { set; get; }
    public float Price { set; get; }

    public static long currentId = 0;

    public static MItem GetSample()
    {
        return new MItem
        {
            Id = currentId++,
            IdOfIcon = Random.Range(1, 10),
            Description = Helper.RandomName("A") + Helper.RandomName("B"),
            Name = Helper.RandomName("STR"),
            Price = Random.Range(300, 5000)
        };
    }
}

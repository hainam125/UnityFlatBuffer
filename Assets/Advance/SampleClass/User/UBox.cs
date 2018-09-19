using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UBox : Base1
{
    public long Capacity { set; get; }
    public List<long> IdsOfNightWalker { set; get; }
    public List<long> IdsOfNightMare { set; get; }
    public List<long> IdsOfWeapon { set; get; }
    public List<long> IdsOfStrengthen { set; get; }
    public List<long> IdsOfMaterial { set; get; }

    public static long __id = 0;
    public static UBox GetSample()
    {
        __id++;
        return new UBox
        {
            Id = __id,
            Capacity = Random.Range(100, 200),
            IdsOfMaterial = Helper.RandomList(8, 8),
            IdsOfNightMare = Helper.RandomList(5, 6),
            IdsOfNightWalker = Helper.RandomList(5, 7),
            IdsOfStrengthen = Helper.RandomList(30, 10),
            IdsOfWeapon = MWeapon.ids
        };
    }
}

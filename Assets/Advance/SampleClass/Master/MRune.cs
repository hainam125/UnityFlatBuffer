using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum TypeOfRune
{
    Default, Start, Quest, Common, Overlimit
}

public class MRune : MBaseTest
{
    public int Ap { get; set; }
    public TypeOfRune Type { get; set; }

    public List<long> IdsOfRune { set; get; }

    private static long currentId;
    public static MRune GetSample()
    {
        currentId++;
        return new MRune
        {
            Id = currentId,
            Ap = Random.Range(1,3),
            IdsOfRune = currentId < 6 ? new List<long>() { currentId + 1, currentId + 2 } : new List<long>() { },
        };
    }
}

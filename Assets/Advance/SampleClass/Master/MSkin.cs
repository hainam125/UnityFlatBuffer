using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSkin : MBaseTest {
    public long IdOfGrowthBoard { set; get; }

    private static long currentId;
    public static MSkin GetSample()
    {
        currentId++;
        return new MSkin
        {
            Id = currentId,
            IdOfGrowthBoard = currentId,
        };
    }
}

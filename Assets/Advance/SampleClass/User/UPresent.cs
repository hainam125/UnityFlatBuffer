using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UPresent : UBaseTest
{
    public long IdOfIcon { get; set; }
    public List<long> IdsOfItem { set; get; }
    public long ExpiredTime { set; get; }
    public long ClaimedTime { set; get; }
    public TypeOfPresentStatus Status { set; get; }
    public string Name { set; get; }

    public static long id = 1;
    public static UPresent GetSample()
    {
        id++;
        return new UPresent
        {
            Id = id + 1,
            Name = Helper.RandomName("Present"),
            Status =(TypeOfPresentStatus)Random.Range(0, 3),
            IdsOfItem = Helper.RandomList(5, 8),
            ExpiredTime = Helper.RandomLongDate(),
            ClaimedTime = Helper.RandomLongDate(),
            IdOfIcon = Random.Range(0,12)
        };
    }
}

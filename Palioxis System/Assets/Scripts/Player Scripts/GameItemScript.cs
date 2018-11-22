using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;
/// <summary>
/// SQL based script to store Items
/// </summary>
public class GameItemScript
{

    [PrimaryKey, AutoIncrement]
    public int ItemId { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }
    public int Score { get; set; }
}
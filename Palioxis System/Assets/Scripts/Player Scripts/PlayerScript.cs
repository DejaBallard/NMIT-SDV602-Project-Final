using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;
/// <summary>
/// SQL script for storing player Data
/// </summary>
public class PlayerScript {
	[PrimaryKey]
	public string Id {get; set;}
    public string CurrentLocationID { get; set; }
    public string PlayerEmail {get; set;}
    public string Password { get; set; }
	public int PlayerHealth {get; set;}
	public int PlayerScore {get; set;}
    public int PlayerMoney { get; set;}
    public string InventoryList { get; set; }
    public string CollectList { get; set; }
    public string Visted { get; set; }
    public string Scanned { get; set; }

}

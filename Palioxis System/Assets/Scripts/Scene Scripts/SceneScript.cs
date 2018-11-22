using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;
//Script Purpose
//Storing Scene data
//--------------------------------------- Start Of SceneScript Class ------------------------------------------------------------------------------
public class SceneScript{
    //--------------------------------------- Start Of Top Level Variable Decalaring ------------------------------------------------------------
    [PrimaryKey]
    public string Name { get; set; }
    //Displayed for story
    public string SceneStoryDescription { get; set; }
    //Displayed if user comes back and hasn't scanned
    public string SceneNotScannedDescription { get; set; }
    //Displayed if user comes back and has scanned
    public string SceneScannedDescription { get; set; }
    //Scan Description
    public string Scan { get; set; }
    //if planet,enemy,shop or broken ship in scene
    public string Event { get; set; }
    //Help list
    public string Help { get; set; }
    //--------------------------------------- End Of Top Level Variable Declaring ---------------------------------------------------------


    //-------------------------------------- Start Of Methods ----------------------------------------------------------------------------	

    //-------------------------------------- End Of Methods ----------------------------------------------------------------------------	
}
//------------------------------------------ End Of SceneScript Class-------------------------------------------------------------------------------

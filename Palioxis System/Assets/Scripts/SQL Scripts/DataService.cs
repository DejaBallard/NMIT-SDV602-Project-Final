using SQLite4Unity3d;
using UnityEngine;
using System.Linq;
using System;

#if !UNITY_EDITOR
using System.Collections;
using System.IO;
#endif
using System.Collections.Generic;

/// <summary>
/// SQL lite connection libary for all types of platfroms
/// https://github.com/robertohuertasm/SQLite4Unity3d
/// Creation of my database
/// </summary>
public class DataService
{
    private string DatabaseName = "dbFinal.db";
    private SQLiteConnection _connection;
    public SQLiteConnection Connection
    {
        get
        {
            return _connection;
        }
    }
    /// <summary>
    /// Creates a database file
    /// </summary>
public DataService()
    {

#if UNITY_EDITOR
        var dbPath = string.Format(@"Assets/StreamingAssets/{0}", DatabaseName);
#else
        // check if file exists in Application.persistentDataPath
        var filepath = string.Format("{0}/{1}", Application.persistentDataPath, DatabaseName);

        if (!File.Exists(filepath))
        {
            Debug.Log("Database not in Persistent path");
            // if it doesn't ->
            // open StreamingAssets directory and load the db ->

#if UNITY_ANDROID
            var loadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/" + DatabaseName);  // this is the path to your StreamingAssets in android
            while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
            // then save to Application.persistentDataPath
            File.WriteAllBytes(filepath, loadDb.bytes);
#elif UNITY_IOS
                 var loadDb = Application.dataPath + "/Raw/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);
#elif UNITY_WP8
                var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);

#elif UNITY_WINRT
		var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);
		
#elif UNITY_STANDALONE_OSX
		var loadDb = Application.dataPath + "/Resources/Data/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);
#else
	var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
	// then save to Application.persistentDataPath
	File.Copy(loadDb, filepath);

#endif

            Debug.Log("Database written");
        }

        var dbPath = filepath;
#endif
        _connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        Debug.Log("Final PATH: " + dbPath);

    }


    /// <summary>
    /// Creates the database tables
    /// </summary>
    /// <param name="prTableTypes">array of scripts being added</param>
    public void CreateTables(System.Type[] prTableTypes)
    {
        var createList = prTableTypes.Where<System.Type>(x => { _connection.CreateTable(x); return true; }).ToList();
        if (!tablesFilled())
        {
            makeGameScenes();
            linkGameScenes();
            makeGameItems();
            linkItemsToScene();
        }
    }
    /// <summary>
    /// Filling the in game scenes with data
    /// </summary>
    private void makeGameScenes() {
        SceneScript[] Scenes = {
        #region Row A Scenes
        new SceneScript
        {
            Name = "A1",
            SceneStoryDescription = "AI: Looks like we are at the corner of the system \n" +
            "AI: Why not run a quick scan to see if anything is here, before turning back",
            Event = "",
            Scan = "Life: 000% \n"+
                   "Gravity: 0.0 m/s \n"+
                   "Oxygen: Null \n"+
                   "Water: Null \n"
        },
        new SceneScript
        {
            Name = "A2",
            SceneStoryDescription = "AI: Looks like something is aproaching us. \n" +
            "AI: What do you want to do?",
            Event = "Enemy",
            Scan = "Electical interference, unable to scan"
        },
        new SceneScript
        {
            Name = "A3",
            SceneStoryDescription = "AI: Well straight ahead of us is out of the system \n" +
            "AI: I can hear something to our left in A2. should we see what it is?",
            Event = "",
            Scan = "Life: 000 % \n"+
                   "Gravity: 0.0 m/s \n" +
                   "Oxygen: Null \n" +
                   "Water: Null \n"
        },
        new SceneScript
        {
            Name = "A4",
            SceneStoryDescription = "AI: I have picked up a distress signal, it looks like it is at Quadrant A5",
            Event = "",
            Scan = "Life: 0.10 % \n"+
                   "Gravity: 0.0 m/s \n" +
                   "Oxygen: Null \n" +
                   "Water: Null \n"
        },
        new SceneScript
        {
            Name = "A5",
            SceneStoryDescription = "AI: Looks like a broken down ship \n" +
            "AI: Let's run some scans to see if anyone is still alive",
            Event = "BrokenShip",
            Scan = "Life: 050 % \n"+
                   "Gravity: 9.8 m/s \n" +
                   "Oxygen: Yes \n" +
                   "Water: Null \n"
        },
        #endregion
        #region Row B Scenes
        new SceneScript
        {
            Name = "B1",
            SceneStoryDescription = "AI: Its so empty here,looks like nothing here worth reporting \n" +
            "AI: Run a scan quickly and lets get out of here, i don't like this",
            Event = "",
            Scan = "Life: 000 % \n"+
                   "Gravity: 0.0 m/s \n" +
                   "Oxygen: Null \n" +
                   "Water: Null \n"
        },
        new SceneScript
        {
            Name = "B2",
            SceneStoryDescription = "AI: Appoarching another planet",
            Event = "Planet",
            Scan = "Life: 002 % \n"+
                   "Gravity: 19.6 m/s \n" +
                   "Oxygen: Null \n" +
                   "Water: Yes \n"
        },
        new SceneScript
        {
            Name = "B3",
            SceneStoryDescription = "AI: Im picking up something strange \n" +
            "AI: Poor guys, I wonder what happened? \n" +
            "AI: Do we run a scan to see if anyone is alive?",
            Event = "BrokenShip",
            Scan = "Life: 000 % \n"+
                   "Gravity: 0.0 m/s \n" +
                   "Oxygen: Null \n" +
                   "Water: Yes \n"
        },
         new SceneScript
        {
            Name = "B4",
            SceneStoryDescription = "AI: Hey, ummm.... not to scare you or anything but, something is coming towards us \n" +
            "AI: What do we do?",
            Event = "Enemy",
            Scan = "They have shut our scanners down \n"+
                    "AI: Rebooting..."
        },
        new SceneScript
        {
            Name = "B5",
            SceneStoryDescription = "AI: Im detecting two signals, one at A1 and the other at B4 \n" +
            "AI: Which one should we check out?",
            Event = "",
            Scan = "Life: 000 % \n"+
                   "Gravity: 0.0 m/s \n" +
                   "Oxygen: Null \n" +
                   "Water: Null \n"
        },
        #endregion
        #region Row C Scenes
        new SceneScript
        {
            Name = "C1",
            SceneStoryDescription = "AI: We are aproaching another broken ship \n" +
            "AI: What is going on here?",
            Event = "BrokenShip",
            Scan = "Life: 010 % \n"+
                   "Gravity: 0.0 m/s \n" +
                   "Oxygen: Yes\n" +
                   "Water: Null \n"
        },
       new SceneScript
        {
            Name = "C2",
            SceneStoryDescription = "AI: Ive picked up a lot of old signals in the lower sector of this system \n" +
            "AI: We were told, this system has had no signs of live forms we arrived \n" +
            "AI: Scan this area and lets check it out",
            Event = "BrokenShip",
            Scan = "Life: 000 % \n"+
                   "Gravity: 0.0 m/s \n" +
                   "Oxygen: Null \n" +
                   "Water: Null \n"
        },
        new SceneScript
        {
            Name = "C3",
            SceneStoryDescription = "AI: Looks like nothing is here? \n" +
            "AI: See if the scanners can pick up anything",
            Event = "",
            Scan = "Life: 000 % \n"+
                   "Gravity: 1.2 m/s \n" +
                   "Oxygen: Null \n" +
                   "Water: Null \n"
        },
        new SceneScript
        {
            Name = "C4",
            SceneStoryDescription = "AI: The General sent out a drone for us to bring all things we find back to \n" +
            "AI: Type 'Show Shop' for us to dock",
            Event = "Shop",
            Scan = "Life: 000 % \n"+
                   "Gravity: 9.8 m/s \n" +
                   "Oxygen: Yes \n" +
                   "Water: Yes \n"
        },
        new SceneScript
        {
            Name = "C5",
            SceneStoryDescription = "AI: Looks like we found the drone we sent out 10 years ago \n" +
            "AI: It has seen better days, I wonder if there are any parts aboard",
            Event = "BrokenShip",
            Scan = "Life: 000 % \n"+
                   "Gravity: 0.0 m/s \n" +
                   "Oxygen: Null \n" +
                   "Water: Null \n"
        },
        #endregion
        #region Row D Scenes
        new SceneScript
        {
            Name = "D1",
            SceneStoryDescription = "AI: Looks like a ship graveyard, what happened?",
            Event = "BrokenShip",
            Scan = "Life: 000 % \n"+
                   "Gravity: 0.0 m/s \n" +
                   "Oxygen: Yes \n" +
                   "Water: Null \n"
        },
        new SceneScript
        {
            Name = "D2",
            SceneStoryDescription = "AI: Woah! did you see that shooting star!",
            Event = "",
            Scan = "Life: 100 % \n"+
                   "Gravity: 2.7 m/s \n" +
                   "Oxygen: Null \n" +
                   "Water: Null \n"
        },
        new SceneScript
        {
            Name = "D3",
            SceneStoryDescription = "AI: Looks like we are coming up on a unknown planet \n" +
            "AI: You should run a scan to see what we can find about the planet",
            Event = "Planet",
            Scan = "Life: 98 % \n"+
                   "Gravity: 9.8 m/s \n" +
                   "Oxygen: Null \n" +
                   "Water: Null \n"
        },
        new SceneScript
        {
            Name = "D4",
            SceneStoryDescription = "AI: This doesn't feel right...",
            Event = "",
            Scan = "Error, Systems down..."
        },
         new SceneScript
        {
            Name = "D5",
            SceneStoryDescription = "AI: Can you feel that? What is pulling us?",
            Event = "",
            Scan = "Life: 000 % \n"+
                   "Gravity: 30.8 m/s \n" +
                   "Oxygen: Null \n" +
                   "Water: Null \n"
        },
        #endregion
        #region Row E Scenes
         new SceneScript
        {
            Name = "E1",
            SceneStoryDescription = "AI: Why is this ship all the way out in the corner of the system?",
            Event = "BrokenShip",
            Scan = "Life: 020 % \n"+
                   "Gravity: 0.0 m/s \n" +
                   "Oxygen: Yes \n" +
                   "Water: Null \n"
        },
        new SceneScript
        {
            Name = "E2",
            SceneStoryDescription = "AI: So, how was your sleep anyways? \n" +
            "AI: You were asleep for about 40 years",
            Event = "",
            Scan = "Life: 000 % \n"+
                   "Gravity: 0.0 m/s \n" +
                   "Oxygen: Null \n" +
                   "Water: Null \n"
        },
        new SceneScript
        {
            Name = "E3",
            SceneStoryDescription = "AI:Good morning? afternoon? anyways, we have arrived. \n" +
            "AI:It has been awhile since you have woken, why not type 'show help' to get the basics?",
            Event = "",
            Scan = "Life: 000 % \n"+
                   "Gravity: 0.0 m/s \n" +
                   "Oxygen: Null \n" +
                   "Water: Null \n"
        },
         new SceneScript
        {
            Name = "E4",
            SceneStoryDescription = "AI: This area has strangely strong static \n" +
            "AI: I Wonder what is around here?",
            Event = "",
            Scan = "To much interference"
        },
        new SceneScript
        {
            Name = "E5",
            SceneStoryDescription = "AI: Comming up on another planet",
            Event = "Planet",
            Scan = "Life: 000 % \n"+
                   "Gravity: 40.2 m/s \n" +
                   "Oxygen: Null \n" +
                   "Water: Null \n"
        }
        #endregion
        };
        StoreAllIfNotExists(Scenes);
    }

    /// <summary>
    /// Linking the in game scenes
    /// </summary>
    private void linkGameScenes()
    {
        SceneDirectionScript[] LinkScenes = {
        #region Row A links
        new SceneDirectionScript { FromSceneName = "A1", ToSceneName = "A2", Direction = "right" },
        new SceneDirectionScript { FromSceneName = "A1", ToSceneName = "B2", Direction = "down" },

        new SceneDirectionScript { FromSceneName = "A2", ToSceneName = "A1", Direction = "left" },
        new SceneDirectionScript { FromSceneName = "A2", ToSceneName = "A3", Direction = "right" },
        new SceneDirectionScript { FromSceneName = "A2", ToSceneName = "B2", Direction = "down" },

        new SceneDirectionScript { FromSceneName = "A3", ToSceneName = "A2", Direction = "left" },
        new SceneDirectionScript { FromSceneName = "A3", ToSceneName = "A4", Direction = "right" },
        new SceneDirectionScript { FromSceneName = "A3", ToSceneName = "B3", Direction = "down" },

        new SceneDirectionScript { FromSceneName = "A4", ToSceneName = "A3", Direction = "left" },
        new SceneDirectionScript { FromSceneName = "A4", ToSceneName = "A5", Direction = "right" },
        new SceneDirectionScript { FromSceneName = "A4", ToSceneName = "B4", Direction = "down" },

        new SceneDirectionScript { FromSceneName = "A5", ToSceneName = "A4", Direction = "left" },
        new SceneDirectionScript { FromSceneName = "A5", ToSceneName = "B5", Direction = "down" },
            #endregion
        #region Row B Links
        new SceneDirectionScript { FromSceneName = "B1", ToSceneName = "A1", Direction = "up" },
        new SceneDirectionScript { FromSceneName = "B1", ToSceneName = "B2", Direction = "right" },
        new SceneDirectionScript { FromSceneName = "B1", ToSceneName = "C1", Direction = "down" },

        new SceneDirectionScript { FromSceneName = "B2", ToSceneName = "A2", Direction = "up" },
        new SceneDirectionScript { FromSceneName = "B2", ToSceneName = "B1", Direction = "left" },
        new SceneDirectionScript { FromSceneName = "B2", ToSceneName = "B3", Direction = "right" },
        new SceneDirectionScript { FromSceneName = "B2", ToSceneName = "C2", Direction = "down" },

        new SceneDirectionScript { FromSceneName = "B3", ToSceneName = "A3", Direction = "up" },
        new SceneDirectionScript { FromSceneName = "B3", ToSceneName = "B2", Direction = "left" },
        new SceneDirectionScript { FromSceneName = "B3", ToSceneName = "B4", Direction = "right" },
        new SceneDirectionScript { FromSceneName = "B3", ToSceneName = "C3", Direction = "down" },

        new SceneDirectionScript { FromSceneName = "B4", ToSceneName = "A4", Direction = "up" },
        new SceneDirectionScript { FromSceneName = "B4", ToSceneName = "B3", Direction = "left" },
        new SceneDirectionScript { FromSceneName = "B4", ToSceneName = "B5", Direction = "right" },
        new SceneDirectionScript { FromSceneName = "B4", ToSceneName = "C4", Direction = "down" },

        new SceneDirectionScript { FromSceneName = "B5", ToSceneName = "A5", Direction = "up" },
        new SceneDirectionScript { FromSceneName = "B5", ToSceneName = "B4", Direction = "left" },
        new SceneDirectionScript { FromSceneName = "B5", ToSceneName = "C5", Direction = "down" },
            #endregion
        #region Row C Links
            new SceneDirectionScript { FromSceneName = "C1", ToSceneName = "B1", Direction = "up" },
        new SceneDirectionScript { FromSceneName = "C1", ToSceneName = "C2", Direction = "right" },
        new SceneDirectionScript { FromSceneName = "C1", ToSceneName = "D1", Direction = "down" },

        new SceneDirectionScript { FromSceneName = "C2", ToSceneName = "B2", Direction = "up" },
        new SceneDirectionScript { FromSceneName = "C2", ToSceneName = "C1", Direction = "left" },
        new SceneDirectionScript { FromSceneName = "C2", ToSceneName = "C3", Direction = "right" },
        new SceneDirectionScript { FromSceneName = "C2", ToSceneName = "D2", Direction = "down" },

        new SceneDirectionScript { FromSceneName = "C3", ToSceneName = "B3", Direction = "up" },
        new SceneDirectionScript { FromSceneName = "C3", ToSceneName = "C2", Direction = "left" },
        new SceneDirectionScript { FromSceneName = "C3", ToSceneName = "C4", Direction = "right" },
        new SceneDirectionScript { FromSceneName = "C3", ToSceneName = "D3", Direction = "down" },

        new SceneDirectionScript { FromSceneName = "C4", ToSceneName = "B4", Direction = "up" },
        new SceneDirectionScript { FromSceneName = "C4", ToSceneName = "C3", Direction = "left" },
        new SceneDirectionScript { FromSceneName = "C4", ToSceneName = "C5", Direction = "right" },
        new SceneDirectionScript { FromSceneName = "C4", ToSceneName = "D4", Direction = "down" },

        new SceneDirectionScript { FromSceneName = "C5", ToSceneName = "B5", Direction = "up" },
        new SceneDirectionScript { FromSceneName = "C5", ToSceneName = "C4", Direction = "left" },
        new SceneDirectionScript { FromSceneName = "C5", ToSceneName = "D5", Direction = "down" },
            #endregion
        #region Row D Links
        new SceneDirectionScript { FromSceneName = "D1", ToSceneName = "C1", Direction = "up" },
        new SceneDirectionScript { FromSceneName = "D1", ToSceneName = "D1", Direction = "left" },
        new SceneDirectionScript { FromSceneName = "D1", ToSceneName = "D2", Direction = "right" },
        new SceneDirectionScript { FromSceneName = "D1", ToSceneName = "E1", Direction = "down" },

        new SceneDirectionScript { FromSceneName = "D2", ToSceneName = "C2", Direction = "up" },
        new SceneDirectionScript { FromSceneName = "D2", ToSceneName = "D1", Direction = "left" },
        new SceneDirectionScript { FromSceneName = "D2", ToSceneName = "D3", Direction = "right" },
        new SceneDirectionScript { FromSceneName = "D2", ToSceneName = "E2", Direction = "down" },

        new SceneDirectionScript { FromSceneName = "D3", ToSceneName = "C3", Direction = "up" },
        new SceneDirectionScript { FromSceneName = "D3", ToSceneName = "D2", Direction = "left" },
        new SceneDirectionScript { FromSceneName = "D3", ToSceneName = "D4", Direction = "right" },
        new SceneDirectionScript { FromSceneName = "D3", ToSceneName = "E3", Direction = "down" },

        new SceneDirectionScript { FromSceneName = "D4", ToSceneName = "C4", Direction = "up" },
        new SceneDirectionScript { FromSceneName = "D4", ToSceneName = "D3", Direction = "left" },
        new SceneDirectionScript { FromSceneName = "D4", ToSceneName = "D5", Direction = "right" },
        new SceneDirectionScript { FromSceneName = "D4", ToSceneName = "E4", Direction = "down" },

        new SceneDirectionScript { FromSceneName = "D5", ToSceneName = "C5", Direction = "up" },
        new SceneDirectionScript { FromSceneName = "D5", ToSceneName = "D4", Direction = "left" },
        new SceneDirectionScript { FromSceneName = "D5", ToSceneName = "E5", Direction = "down" },
            #endregion
        #region Row E Links
        new SceneDirectionScript { FromSceneName = "E1", ToSceneName = "D1", Direction = "up" },
        new SceneDirectionScript { FromSceneName = "E1", ToSceneName = "E2", Direction = "right" },

        new SceneDirectionScript { FromSceneName = "E2", ToSceneName = "D2", Direction = "up" },
        new SceneDirectionScript { FromSceneName = "E2", ToSceneName = "E1", Direction = "left" },
        new SceneDirectionScript { FromSceneName = "E2", ToSceneName = "E3", Direction = "right" },

        new SceneDirectionScript { FromSceneName = "E3", ToSceneName = "D3", Direction = "up" },
        new SceneDirectionScript { FromSceneName = "E3", ToSceneName = "E2", Direction = "left" },
        new SceneDirectionScript { FromSceneName = "E3", ToSceneName = "E4", Direction = "right" },

        new SceneDirectionScript { FromSceneName = "E4", ToSceneName = "D4", Direction = "up" },
        new SceneDirectionScript { FromSceneName = "E4", ToSceneName = "E3", Direction = "left" },
        new SceneDirectionScript { FromSceneName = "E4", ToSceneName = "E5", Direction = "right" },

        new SceneDirectionScript { FromSceneName = "E5", ToSceneName = "D5", Direction = "up" },
        new SceneDirectionScript { FromSceneName = "E5", ToSceneName = "E4", Direction = "left" },
            #endregion
        };
        StoreAllIfNotExists(LinkScenes);

    }

    /// <summary>
    /// Creating in game items/ collectables
    /// </summary>
    private void makeGameItems()
    {
        GameItemScript[] items =
        {
            new GameItemScript {Name = "Fuel",Price = 10, Score =100 },
            new GameItemScript {Name = "Rock", Price=1, Score = 15 },
            new GameItemScript {Name = "Unknown Metal",Price = 250, Score = 190 },
            new GameItemScript {Name = "Oil",Price = 9, Score = 25 },
            new GameItemScript {Name = "Star dust",Price = 200, Score =175 },
            new GameItemScript {Name = "Unkown Ship Part",Price = 350, Score =210 },
            new GameItemScript {Name = "Ammo",Price = 5, Score =25 },
            new GameItemScript {Name = "Samples",Price = 500, Score =800 },
            new GameItemScript {Name = "Unknown Clothes",Price = 200, Score =200 },
            new GameItemScript {Name = "Water",Price = 30, Score =45 },
            new GameItemScript {Name = "Life Form",Price = 1000, Score =2000 }
        };
        StoreAllIfNotExists(items);
    }

    /// <summary>
    /// adding itesm to in game scenes
    /// </summary>
    private void linkItemsToScene()
    {
        SceneItemScript[] linkItems =
        {
            #region Item IDs
            /*
            1 = Fuel
            2 = Rock
            3 = Unknown Metal
            4 = Oil
            5 = Star Dust
            6 = Unknown Ship Part
            7 = Ammo
            8 = Sample
            9 = Unknown Clothes
            10 = Water
            11 = life form
            */
#endregion
            #region Row A Items
            new SceneItemScript {SceneName ="A1",ItemId = 2},
            new SceneItemScript {SceneName = "A1",ItemId = 2},
            new SceneItemScript {SceneName = "A1",ItemId = 5},

            new SceneItemScript {SceneName = "A2",ItemId = 1},
            new SceneItemScript {SceneName = "A2",ItemId = 1},
            new SceneItemScript {SceneName = "A2",ItemId = 7},

            new SceneItemScript {SceneName = "A3",ItemId = 2},
            new SceneItemScript {SceneName = "A3",ItemId = 2},
            new SceneItemScript {SceneName = "A3",ItemId = 3},

            new SceneItemScript {SceneName = "A4",ItemId = 9},
            new SceneItemScript {SceneName = "A4", ItemId = 2},

            new SceneItemScript {SceneName = "A5", ItemId = 1},
            new SceneItemScript {SceneName = "A5",ItemId = 7},
            new SceneItemScript {SceneName = "A5",ItemId = 6},
            new SceneItemScript {SceneName = "A5",ItemId = 6},
            #endregion
            #region Row B Items
            new SceneItemScript {SceneName = "B1",ItemId = 3},

            new SceneItemScript {SceneName = "B2",ItemId = 11},
            new SceneItemScript {SceneName = "B2",ItemId = 10},
            new SceneItemScript {SceneName = "B2",ItemId = 2},
            new SceneItemScript {SceneName = "B2",ItemId = 3},
            new SceneItemScript {SceneName = "B2",ItemId = 2},

            new SceneItemScript {SceneName = "B3",ItemId = 9},
            new SceneItemScript {SceneName = "B3",ItemId = 1},
            new SceneItemScript {SceneName = "B3",ItemId = 4},
            new SceneItemScript {SceneName = "B3",ItemId = 6},

            new SceneItemScript {SceneName = "B4",ItemId = 5},

            new SceneItemScript {SceneName = "B5",ItemId = 2},
            new SceneItemScript {SceneName = "B5",ItemId = 2},
            new SceneItemScript {SceneName = "B5",ItemId = 5},
            #endregion
            #region Row C Items
            new SceneItemScript {SceneName = "C1",ItemId = 6},
            new SceneItemScript {SceneName = "C1",ItemId = 6},
            new SceneItemScript {SceneName = "C1",ItemId = 1},

            new SceneItemScript {SceneName = "C2",ItemId = 2},
            new SceneItemScript {SceneName = "C2",ItemId = 2},
            new SceneItemScript {SceneName = "C2",ItemId = 3},

            new SceneItemScript {SceneName = "C3",ItemId = 2},
            new SceneItemScript {SceneName = "C3",ItemId = 9},
            new SceneItemScript {SceneName = "C3",ItemId = 6},

            new SceneItemScript {SceneName = "C5",ItemId = 1},
            new SceneItemScript {SceneName = "C5",ItemId = 4},
            new SceneItemScript {SceneName = "C5",ItemId = 1},
            #endregion
            #region Row D Items
            new SceneItemScript {SceneName = "D1",ItemId = 9},
            new SceneItemScript {SceneName = "D1",ItemId = 6},
            new SceneItemScript {SceneName = "D1",ItemId = 6},

            new SceneItemScript {SceneName = "D2",ItemId = 5},

            new SceneItemScript {SceneName = "D3",ItemId = 11},
            new SceneItemScript {SceneName = "D3",ItemId = 8},
            new SceneItemScript {SceneName = "D3",ItemId = 3},

            new SceneItemScript {SceneName = "D4",ItemId = 2},

            new SceneItemScript {SceneName = "D5",ItemId = 11},
            #endregion
            #region Row E Items
            new SceneItemScript {SceneName = "E1",ItemId = 6},
            new SceneItemScript {SceneName = "E1",ItemId = 6},

            new SceneItemScript {SceneName = "E2",ItemId = 2},

            new SceneItemScript {SceneName = "E3",ItemId = 2},

            new SceneItemScript {SceneName = "E4",ItemId = 2},

            new SceneItemScript {SceneName = "E5",ItemId = 11},
            new SceneItemScript {SceneName = "E5",ItemId = 8},
            new SceneItemScript {SceneName = "E5",ItemId = 2}
#endregion
        };
        StoreAllIfNotExists(linkItems);
    }

    /// <summary>
    /// checks to see if the database exists before creating a new one
    /// </summary>
    /// <returns>if true, ignore creation. if false, create the database</returns>
    private bool tablesFilled()
    {
        int result = _connection.Table<SceneScript>().ToList<SceneScript>().Count;
        return result > 0;
    }

    /// <summary>
    /// stores one instance of data into the database
    /// </summary>
    /// <typeparam name="T">a script</typeparam>
    /// <param name="Record">the data</param>
    public void StoreIfNotExists<T>(T Record)
    {
        try
        {
            _connection.Insert(Record);
        }
        catch (Exception E)
        {
        }
    }

    /// <summary>
    /// Stores an array of data into the database
    /// </summary>
    /// <typeparam name="T">script</typeparam>
    /// <param name="RecordList"> array of data</param>
    public void StoreAllIfNotExists<T>(T[] RecordList)
    {

        try
        {
            _connection.InsertAll(RecordList);
        }
        catch (Exception E)
        {
            Debug.Log(E);
        }
    }

}

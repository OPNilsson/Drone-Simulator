using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UINavigator : MonoBehaviour
{
    //public GameObject mapRenderer;
    public GameObject menuDB;
    public GameObject menuDM;
    public GameObject menuMain;
    public Text MSx;
    public Text MSy;
    public Text MTS;
    public Text Seed;
    public Text SN;
    public GameObject Map;
    public GameObject PeopleSpawner;
    GameObject map = null;
    GameObject peopleSpawner = null;

    public void dB()
    {
        menuMain.SetActive(false);
        menuDM.SetActive(false);
        menuDB.SetActive(true);
    }

    public void dM()
    {
        menuDB.SetActive(false);
        menuMain.SetActive(false);
        menuDM.SetActive(true);
    }

    public void returnMain()
    {
        menuDM.SetActive(false);
        menuDB.SetActive(false);
        menuMain.SetActive(true);
    }

    public void updateMap()
    {
        int Sx = 0, Sy = 0, seed=0, survivors=0;
        float TS = 0;
        if (
        !(int.TryParse(MSx.text, out Sx)
        && int.TryParse(MSy.text, out Sy)
        && float.TryParse(MTS.text, out TS)
        && int.TryParse(Seed.text, out seed)
        && int.TryParse(SN.text, out survivors)))
        {
            Debug.Log("parse error");
            Sx = 1; Sy = 1; TS = 1; seed=0;
        }
        /*
        float Sx =float.Parse(MSx.text);
        float Sy =float.Parse(MSy.text);
        float TS =float.Parse(MTS.text);
        */

        //mapRenderer.setSize((int)(Sx / TS), (int)(Sy / TS), TS);

        if(map!=null){
            DestroyImmediate(map, true);
        }
        map = Instantiate(Map, new Vector3(0, 0, 0), Map.transform.rotation) as GameObject;
        map.GetComponent<GridManager>().Spawn(Sx,Sy,TS);

        // Generates the People
        if(peopleSpawner!=null){
            DestroyImmediate(peopleSpawner, true);
        }
        peopleSpawner = Instantiate(PeopleSpawner, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        //peopleSpawner.SendMessage("Exterminate"); // Makes Sure that there are no people already there
        peopleSpawner.GetComponent<PeopleSpawning>().Spawn(survivors, Sx, Sy, seed,TS);

        //mapRenderer.setSize((int)Sx, (int)Sy, TS);
    }

    public void SpawnMap()
    {
        
    }
}
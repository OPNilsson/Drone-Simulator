using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UINavigator : MonoBehaviour
{
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

        SettingsController settings = new SettingsController();


        settings.SpawnMap((int)Sx, (int)Sy, TS);



        //mapRenderer.setSize((int)Sx, (int)Sy, TS);
    }

}
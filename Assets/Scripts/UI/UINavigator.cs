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
        float Sx = 0, Sy = 0, TS = 0;

        if (
        !(float.TryParse(MSx.text, out Sx)
        && float.TryParse(MSy.text, out Sy)
        && float.TryParse(MTS.text, out TS)))
        {
            Debug.Log("parse error");
            Sx = 1; Sy = 1; TS = 1;
        }

        SettingsController settings = new SettingsController();

        settings.SpawnMap((int)Sx, (int)Sy, TS);
    }
}
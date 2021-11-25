using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UINavigator : MonoBehaviour
{
    public MapDraw mapRenderer;
    public GameObject menuDB;
    public GameObject menuDM;
    public GameObject menuMain;
    public Text MSx;
    public Text MSy;
    public Text MDSx;
    public Text MDSy;
    public Text MTS;
    public Text DMN;
    public Text DBN;
    public Text Seed;
    public Text SN;
    public Text PoIN;
    public GameObject DMmotor;
    public GameObject DMbat;
    public GameObject DMV;
    public GameObject DMA;
    public GameObject DMC;
    public GameObject DMT;
    public GameObject DBmotor;
    public GameObject DBbat;
    public GameObject DBV;
    public GameObject DBA;
    public GameObject DBC;
    public GameObject DBT;
    public GameObject worldSettings;
    public GameObject droneBase;

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
        int Sx =1, Sy=1,seed=0, sn=1, poin=1, dmn=1,dbn=0;
        float TS = 1, DX=0,DY=0;

        
        if (!int.TryParse(MSx.text, out Sx)) Debug.Log("MSx parse error");
        if (!int.TryParse(MSy.text, out Sy)) Debug.Log("MSy parse error");
        if (!int.TryParse(Seed.text, out seed)) Debug.Log("Seed parse error");
        if (!int.TryParse(SN.text, out sn)) Debug.Log("SN parse error");
        if (!int.TryParse(PoIN.text, out poin)) Debug.Log("PoIN parse error");
        if (!int.TryParse(DMN.text, out dmn)) Debug.Log("DMN parse error");
        if (!int.TryParse(DBN.text, out dbn)) Debug.Log("DBN parse error");
        if (!float.TryParse(MDSx.text, out DX)) Debug.Log("MDSx parse error");
        if (!float.TryParse(MDSy.text, out DY)) Debug.Log("MDSy parse error");
        if (!float.TryParse(MTS.text, out TS)) Debug.Log("MTS parse error");
        
        //mapRenderer.setSize(Sx, Sy, TS);
        
        //SettingsController setCont = worldSettings.GetComponent<SettingsController>();
        SettingsController.mapHeight=Sx;
        SettingsController.mapWidth=Sy;
        SettingsController.number_Drones=dbn;
        SettingsController.number_Interests=poin;
        SettingsController.numberOfSurvivors=sn;
        SettingsController.seed=seed;
        SettingsController.tileHeight=(int)TS;//shouldn't be int, but not an issue
        SettingsController.tileWidth=(int)TS;
        updateDM();
        updateDB();
        SettingsController sc =worldSettings.GetComponent<SettingsController>();
        DroneController dc = droneBase.GetComponent<DroneController>();
        dc.num_drones=dbn;
        dc.num_interest=poin;
        dc.num_people=sn;
        dc.seed=seed;
        dc.map_sizex=Sx;
        dc.map_sizey=Sy;
        dc.map_scale=TS;
        dc.gameObject.transform.position = new Vector3(DX, DY, 30);
        //sc.SpawnMap();
        //sc.ClearMap();
        dc.startSimulation();
    }

    public void updateDM(){
        float s=0,a=0,c=0,t=0;//should be made public and declared outside for external acces
        //OR, could set their values from here...
        switch(DMmotor.GetComponent<TMP_Dropdown>().value){
            //idealy set input for stat calculation. Currently set output directly
            case 0:
                s=10;
                a=1;
                break;
            case 1:
                s=15;
                a=2;
                break;
            default:
                s=20;
                a=3;
                break;
        }
        switch(DMbat.GetComponent<TMP_Dropdown>().value){
            //idealy set input for stat calculation. Currently set output directly
            case 0:
                c=10;
                t=1000;
                break;
            case 1:
                c=11;
                t=1500;
                break;
            default:
                c=12;
                t=2000;
                break;
        }
        //outputs "calculated", now set them
        DMV.GetComponent<TextMeshProUGUI>().text=s.ToString();
        DMA.GetComponent<TextMeshProUGUI>().text=a.ToString();
        DMC.GetComponent<TextMeshProUGUI>().text=c.ToString();
        DMT.GetComponent<TextMeshProUGUI>().text=t.ToString();
    }
    public void updateDB(){
        float s=0,a=0,c=0,t=0;//should be made public and declared outside for external acces
        //OR, could set their values from here...
        switch(DBmotor.GetComponent<TMP_Dropdown>().value){
            //idealy set input for stat calculation. Currently set output directly
            case 0:
                s=10;
                a=1;
                break;
            case 1:
                s=15;
                a=2;
                break;
            default:
                s=20;
                a=3;
                break;
        }
        switch(DBbat.GetComponent<TMP_Dropdown>().value){
            //idealy set input for stat calculation. Currently set output directly
            case 0:
                c=10;
                t=1000;
                break;
            case 1:
                c=11;
                t=1500;
                break;
            default:
                c=12;
                t=2000;
                break;
        }
        //outputs "calculated", now set them
        DBV.GetComponent<TextMeshProUGUI>().text=s.ToString();
        DBA.GetComponent<TextMeshProUGUI>().text=a.ToString();
        DBC.GetComponent<TextMeshProUGUI>().text=c.ToString();
        DBT.GetComponent<TextMeshProUGUI>().text=t.ToString();

        DroneController dc = droneBase.GetComponent<DroneController>();
        dc.drone_speed=s;
        dc.drone_battery=t;
    }
}
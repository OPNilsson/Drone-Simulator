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
    public Text WSX1;
    public Text WSY1;
    public Text WSX2;
    public Text WSY2;
    public Text WSD;
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
    private int dronebat=0, dronemotor=0;

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
        float TS = 1, DX=0,DY=0,wsx1=0,wsx2=0,wsy1=0,wsy2=0,wsd=0;

        
        if (!int.TryParse(MSx.text, out Sx)) Debug.Log("MSx parse error");
        if (!int.TryParse(MSy.text, out Sy)) Debug.Log("MSy parse error");
        if (!int.TryParse(Seed.text, out seed)) Debug.Log("Seed parse error");
        if (!int.TryParse(SN.text, out sn)) Debug.Log("SN parse error");
        if (!int.TryParse(PoIN.text, out poin)) Debug.Log("PoIN parse error");
        //if (!int.TryParse(DMN.text, out dmn)) Debug.Log("DMN parse error");
        if (!int.TryParse(DBN.text, out dbn)) Debug.Log("DBN parse error");
        if (!float.TryParse(MDSx.text, out DX)) Debug.Log("MDSx parse error");
        if (!float.TryParse(MDSy.text, out DY)) Debug.Log("MDSy parse error");
        if (!float.TryParse(MTS.text, out TS)) Debug.Log("MTS parse error");
        if (!float.TryParse(WSX1.text, out wsx1)) Debug.Log("WSX1 parse error");
        if (!float.TryParse(WSY1.text, out wsy1)) Debug.Log("WSY1 parse error");
        if (!float.TryParse(WSX2.text, out wsx2)) Debug.Log("WSX2 parse error");
        if (!float.TryParse(WSY2.text, out wsy2)) Debug.Log("WSY2 parse error");
        if (!float.TryParse(WSD.text, out wsd)) Debug.Log("WSD parse error");
        
        //mapRenderer.setSize(Sx, Sy, TS);
        
        //SettingsController setCont = worldSettings.GetComponent<SettingsController>();
        //SettingsController.mapHeight=Sx;
        //SettingsController.mapWidth=Sy;
        //SettingsController.number_Drones=dbn;
        //SettingsController.number_Interests=poin;
        //SettingsController.numberOfSurvivors=sn;
        //SettingsController.seed=seed;
        //SettingsController.tileHeight=(int)TS;//shouldn't be int, but not an issue
        //SettingsController.tileWidth=(int)TS;
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
        dc.time_scale=TS;
        dc.gameObject.transform.position = new Vector3(DX, DY, 30);
        dc.wsx1=wsx1;
        dc.wsy1=wsy1;
        dc.wsx2=wsx2;
        dc.wsy2=wsy2;
        dc.wd=wsd;
        //sc.SpawnMap();
        //sc.ClearMap();
        dc.startSim();
    }

    public void updateDM(){
        float s=0,a=0,c=0,t=0;//should be made public and declared outside for external acces
        //OR, could set their values from here...
        switch(DMmotor.GetComponent<TMP_Dropdown>().value){
            //idealy set input for stat calculation. Currently set output directly
            case 0:
                s=10;
                a=8;
                break;
            case 1:
                s=15;
                a=12;
                break;
            default:
                s=20;
                a=16;
                break;
        }
        switch(DMbat.GetComponent<TMP_Dropdown>().value){
            //idealy set input for stat calculation. Currently set output directly
            case 0:
                c=10;
                t=900;
                break;
            case 1:
                c=11;
                t=1400;
                break;
            default:
                c=12;
                t=1800;
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
        switch(dronemotor){
            //idealy set input for stat calculation. Currently set output directly
            case 0:
                s=10;
                a=8;
                break;
            case 1:
                s=15;
                a=12;
                break;
            default:
                s=20;
                a=16;
                break;
        }
        switch(dronebat){
            //idealy set input for stat calculation. Currently set output directly
            case 0:
                c=10;
                t=900;
                break;
            case 1:
                c=11;
                t=1400;
                break;
            default:
                c=12;
                t=1800;
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
    public void DBBat(int i){
        dronebat=i;
    }
    public void DBMot(int i){
        dronemotor=i;
    }
    public void caput(){
        Application.Quit();
    }
}
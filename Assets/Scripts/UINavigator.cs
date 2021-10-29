using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UINavigator : MonoBehaviour
{
    public GameObject menuMain;
    public GameObject menuDM;
    public GameObject menuDB;
    public MapDraw mapRenderer;
    public Text MSx;
    public Text MSy;
    public Text MTS;


    public void returnMain(){
        menuDM.SetActive(false);
        menuDB.SetActive(false);
        menuMain.SetActive(true);
    }

    public void dM(){
        menuDB.SetActive(false);
        menuMain.SetActive(false);
        menuDM.SetActive(true);
    }

    public void dB(){
        menuMain.SetActive(false);
        menuDM.SetActive(false);
        menuDB.SetActive(true);
    }
    public void updateMap(){
        float Sx =0,Sy =0,TS=0;
        if(
        !(  float.TryParse( MSx.text , out Sx )
        &&  float.TryParse( MSy.text , out Sy )
        &&  float.TryParse( MTS.text , out TS ))){
        
            Debug.Log("parse error");
            Sx=1;Sy=1;TS=1;
        }
        /*
        float Sx =float.Parse(MSx.text);
        float Sy =float.Parse(MSy.text);
        float TS =float.Parse(MTS.text);
        */
        mapRenderer.setSize((int)(Sx/TS) , (int) (Sy/TS), TS);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UINavigator : MonoBehaviour
{
    public GameObject menuMain;
    public GameObject menuDM;
    public GameObject menuDB;

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
}

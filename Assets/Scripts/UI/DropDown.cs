using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropDown : MonoBehaviour
{
    public List<GameObject> items;
    private bool expanded = false;

    public void clicked()
    {
        if(expanded){
            foreach(GameObject g in items){
                g.SetActive(false);
            }
        }else{
            foreach(GameObject g in items){
                g.SetActive(true);
            }
        }
        expanded=!expanded;
        gameObject.GetComponent<RectTransform>().Rotate(new Vector3(0, 0, 180));
    }
}

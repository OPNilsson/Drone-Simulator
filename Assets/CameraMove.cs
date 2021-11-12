using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float speed;
    public float decay;
    private Camera main;
    private Vector3 s=new Vector3(0,0,0);
    private float a;
    private float d;
    private float zoom=1;

    void Start(){
        main=GetComponent<Camera>();
    }

    void Update()
    {
        if(Input.GetKeyDown("q")){
            zoom/=2;
            main.orthographicSize =zoom*5;
        }
        if(Input.GetKeyDown("e")){
            zoom*=2;
            main.orthographicSize =zoom*5;
        }
        d=1/(decay+1);
        a=speed*(1-d);
        s.x+=Input.GetAxis("Horizontal")*a;
        s.y+=Input.GetAxis("Vertical")*a;
        s*=d;
        gameObject.transform.position+=s*zoom;
    }
}

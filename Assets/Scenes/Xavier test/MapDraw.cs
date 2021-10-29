using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDraw : MonoBehaviour
{
    public int targetSizeX=30;
    public int targetSizeY=30;
    public Texture2D mapText;
    private float scale;
    // Start is called before the first frame update
    public void setSize(int x, int y, float s)
    {
        targetSizeX=x;
        targetSizeY=y;
        scale=s;
        gameObject.transform.localScale=new Vector3((x*s)/10,1,(y*s)/10);
        generateTexture(x,y);
    }

    public void redraw(){
        Debug.Log("Got redraw");
        generateTexture(targetSizeX,targetSizeY);
    }

    private void generateTexture(int x, int y){
       mapText = new Texture2D(x,y); 
       Color[] pixels = mapText.GetPixels(0);//mipmap level 0, will generate rest later
       for (int i = 0; i < pixels.Length; ++i)
        {
            pixels[i] = Color.grey;
            mapText.SetPixels(pixels, 0);//mimap 0 again
        }
        mapText.Apply(true);//applies changes, recalculates mipmaps
        Material mapMaterial = gameObject.GetComponent<Renderer>().material;
        mapMaterial.SetTexture("_MainTex",mapText); //_MainTex is the material's property name for the texture
    }
}

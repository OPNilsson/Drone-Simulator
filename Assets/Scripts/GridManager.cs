using System;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private int sizeX, sizeY;
    public float sizeT;
    public Texture2D mapText;
    //private GameObject UI;
/*
    private void MakeTile(int x, int y, int value, float sizeX, float sizeY)
    {
        GameObject g = new GameObject();
        g.transform.position = new Vector3((x * sizeX) + (sizeX / 2), (y * sizeY) + (sizeY / 2), 100);
        g.transform.localScale = new Vector3(sizeX, sizeY, 0);
        g.transform.parent = gameObject.transform;
        var spriteRenderer = g.AddComponent<SpriteRenderer>();

        //spriteRenderer.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));

        spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f);
        spriteRenderer.sprite = sprite;
    }
*/
    //public float CellSize;
    public Vector3 OriginPos = new Vector3(-20, -10);
    //public Sprite sprite;
    //public int Width, Height;

    public (float sizeX, float sizeY) getTileSizes()
    {
        return (this.sizeX, this.sizeY);
    }

    public void Spawn(int x, int y, float s)
    {
        //UI = GameObject.Find("PanelMain");
        //var test = UI.GetComponent<RectTransform>().rect.width;
        //var size = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - test, Screen.height, 0));

        //var tileWidth = size.x / parameters.Item1;
        //var tileHeight = size.y / parameters.Item2;
        sizeX=x;
        sizeY=y;
        sizeT=s;
        OriginPos= new Vector3(x*s/2,y*s/2,0);
        Grid grid = new Grid(x, y, s, OriginPos, false);
        gameObject.transform.localScale = new Vector3((x * s) / 10, 1, (y * s) / 10);
        gameObject.transform.position = OriginPos; // Needs to be in the middle of the grid

        generateTexture(sizeX, sizeY);

        /*for (var i = 0; i < grid.GetGrid.GetLength(0); i++)
        {
            for (var j = 0; j < grid.GetGrid.GetLength(1); j++)
            {
                MakeTile(i, j, 0, tileWidth, tileHeight);
            }
        }*/
    }

    private void generateTexture(int x, int y)
    {
        mapText = new Texture2D((int)(x), (int)(y));

        Color[] pixels = mapText.GetPixels(0);//mipmap level 0, will generate rest later

        for (int i = 0; i < pixels.Length; ++i)
        {
            pixels[i] = Color.grey;
            mapText.SetPixels(pixels, 0);//mimap 0 again
        }

        mapText.Apply(true);//applies changes, recalculates mipmaps
        Material mapMaterial = gameObject.GetComponent<Renderer>().material;
        mapMaterial.SetTexture("_MainTex", mapText); //_MainTex is the material's property name for the texture
    }
}
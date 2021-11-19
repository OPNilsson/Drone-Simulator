using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DTU.Utils;
using System.Linq;
using System;

public class MapDraw : MonoBehaviour
{
    private Grid grid;
    private float scale;

    private void DebugClear()
    {
        Debug.ClearDeveloperConsole();

        // Gets rid of all the Grid_Text objects in the world
        var objects = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "Grid_Text");
        foreach (var gridText in objects)
        {
            Destroy(gridText);
        }
    }

    private void DebugDraw()
    {
        DebugClear();

        grid.DrawGrid();
    }

    private void generateTexture(int x, int y)
    {
        mapText = new Texture2D((int)(x), (int)(y));

        Color[] pixels = mapText.GetPixels(0);//mipmap level 0, will generate rest later

        // Creates a x y grid with each cell of size scale at the WorldPosition of 0 , 0
        grid = new Grid(x, y, scale, new Vector3(0, 0), false);

        // TODO: hide this option by a button

        // Enables debug mode on the grid to draw it out on screen
        grid.SetDebugMode(true);
        DebugDraw();

        for (int i = 0; i < pixels.Length; ++i)
        {
            pixels[i] = Color.grey;
            mapText.SetPixels(pixels, 0);//mimap 0 again
        }

        mapText.Apply(true);//applies changes, recalculates mipmaps
        Material mapMaterial = gameObject.GetComponent<Renderer>().material;
        mapMaterial.SetTexture("_MainTex", mapText); //_MainTex is the material's property name for the texture
    }

    // A test to see if left clicking on the grid cell can change the value showcasing the
    // SetValue() and GetValue() of Grid
    // TODO: Delete this method once the grid is understood.
    private void Update()
    {
        if (grid != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // Easy example grid.SetValue(x,y, value);

                // Esentialy grid.SetValue(WorldPos, value);
                grid.SetValue(Utils.GetMouseWorldPosition(), (grid.GetValue(Utils.GetMouseWorldPosition()) + 1));
            }
        }
    }

    public Texture2D mapText;
    public int targetSizeX = 30;
    public int targetSizeY = 30;

    public void redraw()
    {
        Debug.Log("Got redraw");
        generateTexture(targetSizeX, targetSizeY);
    }

    public void setSize(int x, int y, float s)
    {
        targetSizeX = x;
        targetSizeY = y;
        scale = s;

        // Transform the MapRender GameObject
        gameObject.transform.localScale = new Vector3((x * s) / 10, 1, (y * s) / 10);
        gameObject.transform.position = new Vector3((x / 2) * s, (y / 2) * s); // Needs to be in the middle of the grid

        generateTexture(x, y);
    }
}
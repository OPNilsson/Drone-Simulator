using System;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private float sizeX, sizeY;
    private GameObject UI;

    private void MakeTile(int x, int y, int value, float sizeX, float sizeY)
    {
        GameObject g = new GameObject();
        g.transform.position = new Vector3((x * sizeX) + (sizeX / 2), (y * sizeY) + (sizeY / 2), 100);
        g.transform.localScale = new Vector3(sizeX, sizeY, 0);
        var spriteRenderer = g.AddComponent<SpriteRenderer>();

        //spriteRenderer.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));

        spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f);
        spriteRenderer.sprite = sprite;
    }

    public float CellSize;
    public Vector3 OriginPos = new Vector3(-20, -10);
    public Sprite sprite;
    public int Width, Height;

    public (float sizeX, float sizeY) getTileSizes()
    {
        return (this.sizeX, this.sizeY);
    }

    public void Spawn(ValueTuple<int, int> parameters)
    {
        UI = GameObject.Find("PanelMain");
        var test = UI.GetComponent<RectTransform>().rect.width;
        var size = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - test, Screen.height, 0));

        var tileWidth = size.x / parameters.Item1;
        var tileHeight = size.y / parameters.Item2;

        Grid grid = new Grid(parameters.Item1, parameters.Item2, CellSize, OriginPos, false);

        for (var i = 0; i < grid.GetGrid.GetLength(0); i++)
        {
            for (var j = 0; j < grid.GetGrid.GetLength(1); j++)
            {
                MakeTile(i, j, 0, tileWidth, tileHeight);
            }
        }
    }
}
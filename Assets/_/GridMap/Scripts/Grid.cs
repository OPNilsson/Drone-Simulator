using UnityEngine;
using DTU.Utils;

public class Grid
{
    private float cellSize;
    private TextMesh[,] debugText;
    private int[,] grid;
    private int height;
    private Vector3 originPos;
    private int width;

    // Gets WorldPos of given x and y coordinate
    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPos;
    }

    // Gets XY grid coordinates of given WorldPos uses out parameters to return two values
    private void GetXY(Vector3 worldPos, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPos - originPos).x / cellSize);
        y = Mathf.FloorToInt((worldPos - originPos).y / cellSize);
    }

    // maybe change in the future to a 2D array of arrays for the 3D effect. Creates a 2D grid at
    // World Position with each cell being of cell size
    public Grid(int width, int height, float cellSize, Vector3 originPos)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPos = originPos;

        grid = new int[width, height];
    }

    // Used for debugging where the grid is drawn out. MUST ENABLE GIZMOS IN UNITY TO SEE THE LINES!
    public void DrawGrid()
    {
        debugText = new TextMesh[width, height];


            for (int x = 0; x < grid.GetLength(0); x++) {

                for (int y = 0; y < grid.GetLength(1); y++) {

                    //debugText[x,y] = Utils.CreateWorldText(grid[x, y].ToString(), null, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f, 20, Color.white, TextAnchor.MiddleCenter); // Draws the number inside the cell

                    //Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f); // Draws Left of the cell
                    //Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f); // Draws Bottom of the cell
                }
            }

        //Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f); // Draws Top of the grid
        //Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f); // Draws Right of the grid


        // Example setting of a cell text value.
        SetValue(2, 1, 69);
    }

    public int GetHeight()
    {
        return height;
    }

    // Gets the value of the cell at coordinates x | y
    public int GetValue(int x, int y)
    {
        // check if cell exists
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return grid[x, y];
        }
        else
        {
            return -1; // can return anything -1 just made sense to me
        }
    }

    // Gets the value of the cell at world position
    public int GetValue(Vector3 worldPos)
    {
        int x, y;

        GetXY(worldPos, out x, out y);

        return GetValue(x, y);
    }

    public int GetWidth()
    {
        return width;
    }

    // Sets the Value of a cell at x | y
    public void SetValue(int x, int y, int value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            grid[x, y] = value;
            //debugText[x, y].text = grid[x, y].ToString();
        }
    }

    // Sets the value of a cell at World Position
    public void SetValue(Vector3 worldPos, int value)
    {
        int x, y;

        GetXY(worldPos, out x, out y); // Converts Vector3 worldpos to XY coordinate
        SetValue(x, y, value);
    }

    // Gets the value of the cell at coordinates x | y
    public int GetValue(int x, int y)
    {
        // check if cell exists
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return grid[x, y];
        } else
        {
            return -1; // can return anything -1 just made sense to me
        }
    }

    // Gets the value of the cell at world position
    public int GetValue(Vector3 worldPos)
    {
        int x, y;

        GetXY(worldPos, out x, out y);

        return GetValue(x, y);
    }

	public int[,] GetGrid => grid;

}


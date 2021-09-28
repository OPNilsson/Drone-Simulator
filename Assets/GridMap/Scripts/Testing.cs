using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DTU.Utils;

public class Testing : MonoBehaviour {

    private Grid grid;

    private void Start() {

        // Creates a 5 x 3 grid with each cell of size 10f at the WorldPosition 
        grid = new Grid(5, 3, 10f, new Vector3(-20, -10));
    }

    private void Update()
    {

        // A test to see if left clicking on the grid cell can change the value showcasing the SetValue() and GetValue() of Grid
        if (Input.GetMouseButtonDown(0))
        {
            // Esentialy grid.SetValue(WorldPos, value);
            grid.SetValue(Utils.GetMouseWorldPosition(), (grid.GetValue(Utils.GetMouseWorldPosition()) + 1));
        }
    }
}
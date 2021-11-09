using System;
using UnityEngine;

public class SettingsController : MonoBehaviour
{
    private DroneController droneController;

    private GameObject UI;

    // Start is called before the first frame update
    private void Start()
    {
    }

    public static int mapHeight;
    public static int mapWidth;
    public static int number_Drones = 10;
    public static int number_Interests = 3;
    public static int numberOfSurvivors;
    public static int tileHeight, tileWidth;
    public GameObject Map;
    public GameObject PeopleSpawner;

    public void saveMapHeight(string newMapHeight)
    {
        mapHeight = int.Parse(newMapHeight);
    }

    public void saveMapWidth(string newMapWidth)
    {
        mapWidth = int.Parse(newMapWidth);
    }

    public void saveNumberOfSurvivors(string newNumberOfSurvivors)
    {
        numberOfSurvivors = int.Parse(newNumberOfSurvivors);
    }

    public void SpawnMap()
    {
        GameObject map = Instantiate(Map, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;

        map.SendMessage("Spawn", new ValueTuple<int, int>(mapWidth, mapHeight));

        // Generates the People
        GameObject peopleSpawner = Instantiate(PeopleSpawner, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        peopleSpawner.SendMessage("Spawn", new ValueTuple<int, int, int>(numberOfSurvivors, mapWidth, mapHeight));

        // Generats the Drones
    }
}
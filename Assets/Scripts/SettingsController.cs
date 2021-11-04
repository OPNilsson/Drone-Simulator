using System;
using UnityEngine;

public class SettingsController : MonoBehaviour
{
	public GameObject Map;
	public GameObject PeopleSpawner;

	public static int numberOfSurvivors;
	public static int mapHeight;
	public static int mapWidth;

	public static int tileHeight, tileWidth;
	
    // Start is called before the first frame update
    void Start()
    {
    }

    public void saveNumberOfSurvivors(string newNumberOfSurvivors)
	{
		numberOfSurvivors = int.Parse(newNumberOfSurvivors);
	}

	public void saveMapHeight(string newMapHeight)
	{
		mapHeight = int.Parse(newMapHeight);
	}

	public void saveMapWidth(string newMapWidth)
	{
		mapWidth = int.Parse(newMapWidth);
	}

	public void SpawnMap()
	{
		GameObject map = Instantiate(Map, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
		map.SendMessage("Spawn", new ValueTuple<int, int>(mapWidth, mapHeight));

		GameObject peopleSpawner = Instantiate(PeopleSpawner, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
		peopleSpawner.SendMessage("Spawn", new ValueTuple<int, int, int>(numberOfSurvivors, mapWidth, mapHeight));
	}

}

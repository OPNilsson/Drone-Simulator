using System;
using UnityEngine;
using Pathfinding;
using System.Linq;

public class SettingsController : MonoBehaviour
{
    public static int mapHeight;
    public static int mapWidth;
    public static int number_Drones = 10;
    public static int number_Interests = 3;
    public static int numberOfSurvivors;
    public static int tileHeight, tileWidth;

    public GameObject PeopleSpawner;
    public GameObject prefab_drone;
    public GameObject prefab_human;
    public GameObject prefab_interest;
    private GameObject UI;

    public void ClearMap()
    {
        // Waits for all Pathfinding to finish to be safe
        AstarPath.RegisterSafeUpdate(() =>
        {
            // Gets rid of all the Human Objects in the world
            GameObject[] humans = GameObject.FindGameObjectsWithTag("Human");
            foreach (var victim in humans)
            {
                DestroyImmediate(victim, true);
            }

            // Gets rid of all the Drone Bases in the world
            GameObject[] droneBases = GameObject.FindGameObjectsWithTag("Drone Base");
            foreach (GameObject droneBase in droneBases)
            {
                DestroyImmediate(droneBase, true);
            }

            // Gets rid of all the Drones in the world
            GameObject[] drones = GameObject.FindGameObjectsWithTag("Drone");
            foreach (GameObject drone in drones)
            {
                DestroyImmediate(drone, true);
            }

            // Gets rid of all the Obstacles in the world
            GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
            foreach (GameObject obstacle in obstacles)
            {
                DestroyImmediate(obstacle, true);
            }

            // Gets rid of all the Areas of Interest in the world
            GameObject[] AoIs = GameObject.FindGameObjectsWithTag("Area of Interest");
            foreach (GameObject AoI in AoIs)
            {
                DestroyImmediate(AoI, true);
            }
        });
    }

	public static int seed;
    public GameObject Map;
    public GameObject PeopleSpawner;
    GameObject map = null;
    GameObject peopleSpawner = null;


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

    public void SpawnDroneController()
    {
        // TODO: Implement Spawning of Drone Bases from UI
    }

    public void SpawnMap(int width, int height, float tileScale)
    {
        ClearMap();

	public void saveSeed(string newSeed)
	{
		seed = int.Parse(newSeed);
	}

    public void SpawnMap()
    {
        if(map!=null){
            DestroyImmediate(map, true);
        }
       map = Instantiate(Map, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;


        map.GetComponent<GridManager>().Spawn(Sx,Sy,TS);

        // Generates the People
        if(peopleSpawner!=null){
            DestroyImmediate(peopleSpawner, true);
        }
        peopleSpawner = Instantiate(PeopleSpawner, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        //peopleSpawner.SendMessage("Exterminate"); // Makes Sure that there are no people already there
        peopleSpawner.GetComponent<PeopleSpawning>().Spawn(survivors, Sx, Sy, seed,TS);


        // Generates the Drones

        // Generates the Pathfinding Grid
        var graph = (GridGraph)AstarPath.active.data.graphs[0]; // index 0 is the main one that holds all the pathing for the drones
        // Max size of the Pathfinding Grid is 1024 x 1024
        graph.center = new Vector3(0, 0, 0);
        graph.SetDimensions(width, height, tileScale); // More nodes create a smoother path but use a lot more CPU and MEMORY

        AstarPath.active.Scan(); // Will take a LONG time might be better to update only the different parts using GraphUpdateObject
    }


    public void SpawnPeople()
    {
        // TODO: Implement Spawning of people from UI
        //GameObject peopleSpawner = Instantiate(PeopleSpawner, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        //peopleSpawner.SendMessage("Spawn", new ValueTuple<int, int, int>(numberOfSurvivors, mapWidth, mapHeight));
    }

    private void Start()
    {
        // Make sure that drones cannot collide with humans
        Physics2D.IgnoreLayerCollision(prefab_drone.layer, prefab_human.layer);

        // Generates the People
        if(peopleSpawner!=null){
            DestroyImmediate(peopleSpawner, true);
        }
        peopleSpawner = Instantiate(PeopleSpawner, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        //peopleSpawner.SendMessage("Exterminate"); // Makes Sure that there are no people already there
        peopleSpawner.SendMessage("Spawn", new ValueTuple<int, int, int, int>(numberOfSurvivors, mapWidth, mapHeight, seed));

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class DroneController : MonoBehaviour
{
    private List<Drone> drones;
    private List<Interest> interests;
    private float x;
    private float y;

    private IEnumerator SpawnDrones()
    {
        drones = new List<Drone>();
        GameObject droneObject;
        for (int i = 0; i < num_drones; i++)
        {
            int index = UnityEngine.Random.Range(0, interests.Count); // Assigns a random interest area to the drone

            // Instatiate Game Object
            droneObject = Instantiate(prefab_drone, new Vector3(x, y, prefab_drone.transform.position.z), Quaternion.identity); // Drones should spawn at the drone controller coordinates

            droneObject.SendMessage("ChangeTarget", interests[index].transform);

            drones.Add((Drone)droneObject.GetComponent(typeof(Drone)));

            yield return new WaitForSeconds(SpawnTimer);
        }
    }

    private void Start()
    {
        // Gets the postion of the controller
        x = this.transform.position.x;
        y = this.transform.position.y;

        // Spawn Interests
        interests = new List<Interest>();
        GameObject interestObject;
        for (int i = 0; i < num_interest; i++)
        {
            float x_random = UnityEngine.Random.Range(50, width);
            float y_random = UnityEngine.Random.Range(50, height);

            // Instatiate Game Object
            interestObject = Instantiate(prefab_interest, new Vector3(x_random, y_random, prefab_interest.transform.position.z), Quaternion.identity);

            interests.Add((Interest)interestObject.GetComponent(typeof(Interest)));
        }

        // Spawn Drones on a timer so that it looks more realistic
        StartCoroutine(SpawnDrones());
    }

    private IEnumerator StartRefuel(Drone drone)
    {
        drone.Refuling = true;

        float distance = Vector2.Distance(drone.transform.position, this.transform.position); 

        while (drone.fuel < drone.fuel_max && distance <= drone.waypoint_distance)
        {
            drone.Refuel();
            yield return new WaitForSeconds(SpawnTimer);
        }

        drone.Refuling = false;

        AssignNewTarget(drone);
    }

    public const int SpawnTimer = 2; // Time to wait for to spawn next in seconds

    public float height = 50;

    // TODO: Merge this part with Settings Controller
    public int num_drones = 0;

    // TODO: Merge this part with Settings Controller
    public int num_interest = 0;

    public GameObject prefab_drone;

    // TODO: Merge this part with Settings Controller
    public GameObject prefab_interest;

    public float width = 50;

    public void AssignNewTarget(Drone drone)
    {
        // Check which one is the closest
        float closestDistance = width * height; // The largest distance possible

        // Check that there are still areas of intrests
        if (interests.Count > 0)
        {
            foreach (Interest interest in interests)
            {
                float distance = Vector2.Distance(drone.transform.position, interest.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;

                    drone.ChangeTarget(interest.transform);
                }
            }
        }
        else
        {
            // Try and find new Interests in the world
            GameObject[] interestObjects = GameObject.FindGameObjectsWithTag("Area of Interest");

            if (interestObjects.Length > 0)
            {
                foreach (GameObject iObject in interestObjects)
                {
                    interests.Add((Interest)iObject.GetComponent(typeof(Interest)));
                }

                AssignNewTarget(drone); // recursive call so that it assignes closest interest
            }
            else
            {
                // If not then the drone will go back home
                drone.ChangeTarget(gameObject.transform);
            }
        }
    }

    public void InterestDetroyed(Interest interest)
    {
        // Remove the Intrest from the controller's list
        try
        {
            interests.Remove(interest);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            Debug.LogWarning("Attempted to remove already removed Interest in InterestDetroyed() inside DroneController");
        }

        foreach (Drone drone in drones)
        {
            if (drone.GetTarget() == interest.transform)
            {
                AssignNewTarget(drone);
            }
        }
    }

    public void ReachedTarget(Drone drone)
    {
        Transform target = drone.target;

        // Check if the target was this drone base
        if (target == gameObject.transform)
        {
            
            StartCoroutine(StartRefuel(drone));
            

            return; // Don't do the rest of the code
        }

        // Check what interest it reached
        foreach (Interest interest in interests)
        {
            if (interest.transform == target)
            {
                interest.AddDroneToArea(drone);
                return; // Don't loop through the rest
            }
        }
    }
}
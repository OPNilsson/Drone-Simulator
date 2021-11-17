using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    public const float PathfindingTimer = 1f;

    // TODO: Merge this part with Settings Controller
    public const int SpawnTimer = 2;   // Time to wait for to spawn next in seconds

    // Time to update the pathfinding

    public float height = 50;
    public int num_drones = 0;
    public int num_interest = 0;
    public int num_people = 0;

    bool runCSVO = true;

    public ListToCSVConverter toCSVConverter;

    public GameObject prefab_drone;

    public GameObject prefab_human;

    public GameObject prefab_interest;

    public float width = 50;

    private List<Drone> drones;

    private List<Interest> interests;

    private List<People> people;
    private List<People> peopleFound;

    private float x;

    private float y;

    public void AssignNewTarget(Drone drone)
    {
        // Check which one is the closest
        float closestDistance = float.MaxValue;

        // Check that there are still areas of intrests
        if (interests.Count > 0)
        {
            foreach (Interest interest in interests)
            {
                if (interest != null)
                {
                    float distance = Vector2.Distance(drone.transform.position, interest.transform.position);

                    if (distance < closestDistance)
                    {
                        closestDistance = distance;

                        drone.ChangeTarget(interest.transform);
                    }
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
            }
            else
            {
                // If not then the drone will go back home
                drone.ChangeTarget(gameObject.transform);
            }
        }
    }

    public void HumanFound(Transform transform)
    {
        bool removePerson = false;
        People personToRemove = null;

        foreach (People person in people)
        {
            if (transform == person.transform)
            {
                person.XCords = transform.position.x;
                person.YCords = transform.position.y;
                removePerson = true;
                personToRemove = person;
                Debug.Log(personToRemove.YCords);
                Debug.Log(personToRemove.XCords);

                person.Spotted();

                Debug.Log("Person has been spotted by " + name + " @ " + transform.position + "!");
                Debug.Log("Still looking for " + (people.Count - 1) + " People");

                break;
            }
        }

        if (removePerson)
        {
            people.Remove(personToRemove);

            peopleFound.Add(personToRemove);
        }

        if(people.Count ==  0 && runCSVO) {
            runCSVO = false;
            toCSVConverter.ListToCSV(peopleFound);
        }
    }

    public void InterestDetroyed(Interest interest, Drone drone)
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

        AssignNewTarget(drone);
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
        peopleFound = new List<People>();
        toCSVConverter = new ListToCSVConverter();
        // Gets the postion of the controller
        x = this.transform.position.x;
        y = this.transform.position.y;

        // Spawn Interests and People
        interests = new List<Interest>();
        people = new List<People>();
        GameObject interestObject;
        for (int i = 0; i < num_interest; i++)
        {
            float x_random = UnityEngine.Random.Range(50, width);
            float y_random = UnityEngine.Random.Range(50, height);

            // Instatiate Game Object
            interestObject = Instantiate(prefab_interest, new Vector3(x_random, y_random, prefab_interest.transform.position.z), Quaternion.identity);

            // Makes the area of the cirlce random
            float area_x_random = UnityEngine.Random.Range(50, 300); // a human is 25 units van is 300 x 150
            float area_y_random = UnityEngine.Random.Range(50, 300);

            Vector3 scale = interestObject.transform.localScale;

            scale.x = area_x_random;
            scale.y = area_y_random;

            interestObject.transform.localScale = scale;

            interests.Add((Interest)interestObject.GetComponent(typeof(Interest)));

            // Spawn People
            GameObject peopleObject;
            for (int p = UnityEngine.Random.Range(0, num_people); p < num_people; p++)
            {
                // TODO: This method is not guranteed to be inside the area

                peopleObject = Instantiate(prefab_human, new Vector3(x_random + (p * 10), y_random + (p * 10), prefab_human.transform.position.z), Quaternion.identity);

                people.Add((People)peopleObject.GetComponent(typeof(People)));
            }
        }

        // InvokeRepeating("UpdatePathfinding", 1f, PathfindingTimer);

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

    /// <summary>
    /// Currently unsused due to drones creating their own obstacle on themselves <see cref="Drone.OnCollisionEnter2D(Collision2D)"/>
    /// </summary>
    private void UpdatePathfinding()
    {
        // Update the bounds based on drone area
        foreach (Drone drone in drones)
        {
            Bounds droneBounds = drone.GetComponent<CircleCollider2D>().bounds;

            Vector3 size = droneBounds.size * PathfindingTimer * 2; // Calculates drones sizes around the drone

            droneBounds.size = size;

            var guo = new GraphUpdateObject(droneBounds);

            guo.updatePhysics = true;

            AstarPath.active.UpdateGraphs(guo);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;

public class Drone : MonoBehaviour
{
    private const float UpdateTime = 0.05f;
    private Rigidbody2D body;
    private DroneController control;
    private Path path;

    // reached the end of its path
    private Seeker seeker;

    private int waypoint_current = 0;

    private bool EnoughFuel()
    {
        int wayPointsRemaining = path.vectorPath.Count - waypoint_current;

        if ((fuel - (FuelConsumptionRate * wayPointsRemaining)) > MinFuel)
        {
            return true;
        }
        else if (wayPointsRemaining <= 0)
        {
            return true;
        }
        else
        {
            return false; // TODO: Calculate the flight back
        }
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;

            waypoint_current = 1; // path.vectorPath [0] is drone position so you don't want to calculate the path to itself
        }
    }

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        body = GetComponent<Rigidbody2D>();

        Refuling = false;

        // Find all Drone Bases
        GameObject[] bases = GameObject.FindGameObjectsWithTag("Drone Base");

        // Check which one is the closest
        float closestDistance = 1000;
        foreach (GameObject droneBase in bases)
        {
            float distance = Vector2.Distance(body.position, droneBase.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;

                control = droneBase.GetComponent<DroneController>();
            }
        }

        // target is null when not spawned in manually
        if (target != null)
        {
            InvokeRepeating("UpdatePath", 0f, UpdateTime); // Change update time if slowing down CPU
        }
    }

    private void UpdatePath()
    {
        // Makes sure not to create a new path while already calculating one
        if (seeker.IsDone())
        {
            seeker.StartPath(body.position, target.position, OnPathComplete);
        }
    }

    private void UseFuel()
    {
        fuel -= FuelConsumptionRate;

        fuelSlider.value = fuel;
    }

    public float fuel = 100f;
    public float fuel_max = 100f;
    public float FuelConsumptionRate = 0.05f;
    public Slider fuelSlider;
    public int MinFuel = 10;

    // The amount of mininimum fuel that the drone can have before returning home
    public float speed_movement = 500f;

    public float speed_turn = 100f;
    public Transform sprite;

    // The sprite of the drone
    public Transform target;

    // The thing the drone should be moving towards.
    public float view_distance;

    public float waypoint_distance = 20f;
    public bool Refuling { get; set; }
    // The distance it needs to be to the next waypoint in it's path inorder to move on to the next waypoint

    public void ChangeTarget(Transform target)
    {
        this.target = target;
    }

    public void FixedUpdate()
    {
        if (path == null || target == null)
        {
            return;
        }

        // Finished it's path
        if (waypoint_current >= path.vectorPath.Count)
        {
            float distanceToTarget = Vector2.Distance(body.position, target.position);

            // Make sure it's at the target
            if (distanceToTarget <= waypoint_distance)
            {
                control.ReachedTarget(this);
            }

            return;
        }

        Vector2 direction = ((Vector2)path.vectorPath[waypoint_current] - body.position).normalized;

        Vector2 force = direction * speed_movement * Time.deltaTime;

        body.AddForce(force); // Moves the drone

        UseFuel();

        // If the drone doesn't have enough fuel to make it to it's destination then it will go home
        if (!EnoughFuel())
        {
            target = control.transform;
        }

        // Handles the rotation of the sprite so that the drone is facing towards where it's headed
        if (body.velocity != Vector2.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(Vector3.forward, force);

            sprite.rotation = Quaternion.RotateTowards(sprite.rotation, rotation, speed_turn * Time.deltaTime);
        }

        float distance = Vector2.Distance(body.position, path.vectorPath[waypoint_current]);

        if (distance <= waypoint_distance)
        {
            waypoint_current++;
        }
    }

    public DroneController GetDroneController()
    {
        return control;
    }

    public Transform GetTarget()
    {
        return target;
    }

    public void Refuel()
    {
        fuel += FuelConsumptionRate * 10;

        fuelSlider.value = fuel;
    }

    public void SetMaxFuel(int fuelMax)
    {
        fuelSlider.maxValue = fuelMax;

        fuel_max = fuelMax;

        fuel = fuelMax; // Maybe don't refuel

        fuelSlider.value = fuelMax;
    }
}
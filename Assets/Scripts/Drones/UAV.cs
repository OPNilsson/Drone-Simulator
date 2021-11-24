using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;

public class UAV : MonoBehaviour
{
    public MeshRenderer fovRenderer;
    public float fuel = 100f;
    public float fuel_max = 100f;
    public float FuelConsumptionRate = 0.05f;
    public Slider fuelSlider;
    public int MinFuel = 10;

    public float speed_movement = 500f;
    public float speed_turn = 100f;
    public SpriteRenderer sprite;
    public Transform target;
    public float waypoint_distance = 20f;
    private const float UpdateTime = 0.05f;
    private Rigidbody2D body;
    private DroneController control;
    private FOV fov;
    private Path path;
    private Seeker seeker;

    private int waypoint_current = 0;

    public bool Refuling { get; set; }

    public void ChangeTarget(Transform target)
    {
        this.target = target;
    }

    public void FixedUpdate()
    {
        // Check if it has spotted a human
        if (fov.visibleTargets.Count > 0)
        {
            foreach (Transform visibleTarget in fov.visibleTargets)
            {
                control.HumanFound(visibleTarget);
            }
        }

        if (target == null)
        {
            control.AssignNewTarget(this, null);
        }

        if (path == null)
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

            gameObject.transform.rotation = Quaternion.RotateTowards(gameObject.transform.rotation, rotation, speed_turn * Time.deltaTime);
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

    public Vector3 VectorDirectionFromDegrees(float angleDegrees)
    {
        return new Vector3(
            Mathf.Sin(angleDegrees * Mathf.Deg2Rad),
            0,
            Mathf.Cos(angleDegrees * Mathf.Deg2Rad));
    }

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Bounds droneBounds = GetComponent<CircleCollider2D>().bounds;

        Vector3 size = droneBounds.size * 5; // Calculates drones sizes around the drone

        droneBounds.size = size;

        var guo = new GraphUpdateObject(droneBounds);

        guo.updatePhysics = true;

        AstarPath.active.UpdateGraphs(guo);
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

        fov = GetComponent<FOV>();

        Refuling = false;

        // Find all Drone Bases
        GameObject[] bases = GameObject.FindGameObjectsWithTag("Drone Base");

        // Check which one is the closest
        float closestDistance = float.MaxValue;
        foreach (GameObject droneBase in bases)
        {
            float distance = Vector2.Distance(body.position, droneBase.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;

                control = droneBase.GetComponent<DroneController>(); // Assign it that base
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
        if (seeker.IsDone() && target != null)
        {
            seeker.StartPath(body.position, target.position, OnPathComplete);
        }
    }

    private void UseFuel()
    {
        fuel -= FuelConsumptionRate;

        fuelSlider.value = fuel;
    }
}
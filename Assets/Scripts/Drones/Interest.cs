using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interest : MonoBehaviour
{
    public SpriteRenderer sprite;
    private const float InterestDropRate = 0.05f;
    private const int InterestTimer = 1; // Time in seconds untill the interest falls again

    private List<Drone> drones; // The drones currently inside the area
    public float intrestLevel = 1;
    public float peekInterest=1;
    public float time_scale=1;
    public float sizex=1;
    public float sizey=1;

    public void AddDroneToArea(Drone drone)
    {
        bool repeat = false;

        // Make sure that the drone being added is not already counted
        foreach (Drone d in drones)
        {
            if (drone == d)
            {
                repeat = true;
            }
        }

        if (!repeat)
        {
            drones.Add(drone);

            InvokeRepeating("ReduceInterest", 0f, InterestTimer); // Change update time if slowing down CPU
        }
    }

    public void AddDroneToArea(UAV uav)
    {
        Destroy(gameObject);
    }

    private void ReduceInterest()
    {
        intrestLevel -= InterestDropRate*time_scale;
        if(intrestLevel<0){intrestLevel=0;}
        UpdateSprite();
    }

    private void Start()
    {
        drones = new List<Drone>();
    }

    private void UpdateSprite()
    {
        // If the sprite can no longer be seen then delte it
        if (intrestLevel > 0)
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, intrestLevel/peekInterest);
        }
        else
        {
            foreach (Drone drone in drones)
            {
                DroneController controller = drone.GetDroneController();

                controller.InterestDetroyed(this, drone);
            }

            CancelInvoke();

            Destroy(gameObject);
        }
    }

    public void destroy(){
        GameObject.Destroy(gameObject);
    }
}
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
    public int map_sizex=500;
    public int map_sizey=500;
    public float time_scale=1;
    public float drone_speed=20;
    public float drone_battery=1800;
    public float wsx1=0,wsy1=0,wsx2=0,wsy2=0,wd=0;

    public static DateTime currentStartTime;
	public int seed = 0;

    bool runCSVO = true;

    public ListToCSVConverter toCSVConverter;


    public int num_uavs = 0;

    public GameObject prefab_drone;
    public GameObject prefab_human;
    public GameObject prefab_interest;
    public GameObject prefab_uav;
    public float width = 50;
    private List<Interest> destinations;
    private List<Drone> drones;
    public List<Interest> interests;
    private List<People> people;
    private List<People> peopleFound;
    private List<UAV> uavs;
    public List<int> dronesOnPoI;

    public Vector2 wind=new Vector2(0,0);
    private bool alternating=false;
    private bool coroutineIsRunning=false;

    private float x;
    private float y;

    public int AssignNewTarget(Drone drone,bool closest)
    {
        // Check which one is the farthest
        float farthestDistance;
        if(closest){
            farthestDistance = float.MaxValue;
        }else{
            farthestDistance =0;
        }
        int currentDrones=int.MaxValue;
        int poI=-1;
        drone.patroling=false;
        // Check that there are still areas of intrests
        if (interests.Count > 0)
        {
            for(int i =0; i< interests.Count;i++)
            {
                if (interests[i] != null)
                {
                    float distance = Vector2.Distance(drone.transform.position, interests[i].transform.position);
                    if(currentDrones > dronesOnPoI[i]){
                        if(poI!=-1){dronesOnPoI[poI]--;}
                        farthestDistance = distance;
                        poI = i;
                        currentDrones=dronesOnPoI[i];
                        dronesOnPoI[i]++;
                        drone.ChangeTarget(interests[i].transform);
                    }else if (currentDrones == dronesOnPoI[i]){
                        if ((distance > farthestDistance) ^closest)
                        {
                            if(poI!=-1){dronesOnPoI[poI]--;}
                            farthestDistance = distance;
                            poI = i;
                            dronesOnPoI[i]++;
                            drone.ChangeTarget(interests[i].transform);
                        }
                    }
                }
            }
        }
        else
        {
            // Try and find new Interests in the world
            
            GameObject[] interestObjects = GameObject.FindGameObjectsWithTag("Area of Interest");

            if (interestObjects != null && interestObjects.Length > 0)
            {
                // !!!this shouldn't run...
                foreach (GameObject iObject in interestObjects)
                {
                    interests.Add((Interest)iObject.GetComponent(typeof(Interest)));
                }
            }
            else
            {
                // If not then the drone will go back home
                drone.ChangeTarget(gameObject.transform);
				if (runCSVO)
				{
					runCSVO = false;
					toCSVConverter.ListToCSV(peopleFound);
				}
			}
        }
        return poI;
    }

    public void AssignNewTarget(UAV uav, Interest oldDestination)
    {/*
        // Remove previous destination from list
        destinations.Remove(oldDestination);

        // Create a new destination point
        GameObject interestObject;

        float x_random = UnityEngine.Random.Range(50, width * 0.75f);  // TOOD: Change the target based on some search heuristic
        float y_random = UnityEngine.Random.Range(50, height * 0.75f); // TOOD: Change the target based on some search heuristic

        // Instatiate Game Object
        interestObject = Instantiate(prefab_interest, new Vector3(x_random, y_random, prefab_interest.transform.position.z), Quaternion.identity);

        float area_x_random = 10;
        float area_y_random = 10;

        Vector3 scale = interestObject.transform.localScale;

        scale.x = area_x_random;
        scale.y = area_y_random;

        interestObject.transform.localScale = scale;

        destinations.Add((Interest)interestObject.GetComponent(typeof(Interest)));

        uav.ChangeTarget(interestObject.transform);*/ //without UAV
    }

    public void HumanFound(Transform trns, Drone drone, Transform poi)
    {
        bool removePerson = false;
        People personToRemove = null;
        Interest interest= poi.gameObject.GetComponent<Interest>();
        foreach (People person in people)
        {
            if (trns == person.transform)
            {
                person.XCords = trns.position.x;
                person.YCords = trns.position.y;
                removePerson = true;
                personToRemove = person;
                float deltax = trns.position.x-poi.position.x;
                float deltay = trns.position.y-poi.position.y;
                float dx=deltax/interest.sizex;
                float dy=deltay/interest.sizey;
                float dist2 =(dx*dx+dy*dy);
                if(dist2 >0.25f && dist2<0.5f){ //Outside PoI
                    // Instatiate new PoI
                    GameObject interestObject = Instantiate(prefab_interest, new Vector3(trns.position.x+deltax, trns.position.y+deltay, prefab_interest.transform.position.z), Quaternion.identity);

                    Vector3 scale = interestObject.transform.localScale;

                    scale.x *= interest.sizex;
                    scale.y *= interest.sizey;

                    interestObject.transform.localScale = scale;

                    Interest interestSpawned=interestObject.GetComponent<Interest>();
                    interests.Add(interestSpawned);
                    interestSpawned.time_scale=time_scale;
                    interestSpawned.sizex=interest.sizex;
                    interestSpawned.sizey=interest.sizey;
                    interestSpawned.intrestLevel=interest.sizex*interest.sizey/5000f;
                    interestSpawned.peekInterest=interest.intrestLevel;

                    dronesOnPoI.Add(0);
                }else if (dist2 >0.5f){//randomly found, place PoI on person
                    GameObject interestObject = Instantiate(prefab_interest, new Vector3(trns.position.x, trns.position.y, prefab_interest.transform.position.z), Quaternion.identity);

                    Vector3 scale = interestObject.transform.localScale;

                    scale.x *= map_sizex/(num_interest);
                    scale.y *= map_sizey/(num_interest);
                    interestObject.transform.localScale = scale;
                    Interest interestSpawned=interestObject.GetComponent<Interest>();
                    interests.Add(interestSpawned);
                    interestSpawned.time_scale=time_scale;
                    interestSpawned.sizex=interest.sizex;
                    interestSpawned.sizey=interest.sizey;
                    interestSpawned.intrestLevel=map_sizex*map_sizey/(num_interest*num_interest*5000f);
                    interestSpawned.peekInterest=interest.intrestLevel;
                    dronesOnPoI.Add(0);
                }                

                person.Spotted();

                Debug.Log("Person has been spotted by " + name + " @ " + trns.position + "!");
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
        if (drone != null)
        {
            // Remove the Intrest from the controller's list
            try
            {
                int i=interests.IndexOf(interest);
				if(i == -1)
					return;
                interests.Remove(interest);
                dronesOnPoI.RemoveAt(i);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                Debug.LogWarning("Attempted to remove already removed Interest in InterestDetroyed() inside DroneController");
            }

        }
        foreach(Drone i in drones){
            if(i.target==interest){
                AssignNewTarget(i,true);
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

        if(target == drone.subtarget){
            //new subtarget
            Interest interest=drone.target.gameObject.GetComponent<Interest>();
            Vector3 p = drone.subtarget.transform.position;
            p.x=UnityEngine.Random.Range(-interest.sizex,interest.sizex)+interest.transform.position.x;
            p.y=UnityEngine.Random.Range(-interest.sizey,interest.sizey)+interest.transform.position.y;
            drone.subtarget.transform.position=p;
        }

        // Check what interest it reached
        foreach (Interest interest in interests)
        {
            if (interest.transform == target)
            {
                interest.AddDroneToArea(drone);
                if(drone.subtarget==null){
                    drone.subtarget=new GameObject("subtarget");
                }
                drone.subtarget.transform.parent=interest.transform;
                Vector3 p = drone.subtarget.transform.position;
                p.x=UnityEngine.Random.Range(-interest.sizex,interest.sizex)+interest.transform.position.x;
                p.y=UnityEngine.Random.Range(-interest.sizey,interest.sizey)+interest.transform.position.y;
                drone.subtarget.transform.position=p;
                drone.patroling=true;
                return; // Don't loop through the rest
            }
        }
    }

    public void ReachedTarget(UAV uav)
    {/*
        Transform target = uav.target;

        // Check if the target was this drone base
        if (target == gameObject.transform)
        {
            StartCoroutine(StartRefuel(uav));

            return; // Don't do the rest of the code
        }

        // Check what destination it reached
        foreach (Interest destination in destinations)
        {
            if (destination.transform == target)
            {
                destination.AddDroneToArea(uav);

                AssignNewTarget(uav, destination);

                return; // Don't loop through the rest
            }
        }*/
    }

    private IEnumerator SpawnDrones()
    {
        drones = new List<Drone>();
        GameObject droneObject;
        Drone drone;
        for (int i = 0; i < num_drones; i++)
        {
            
            //int index = UnityEngine.Random.Range(0, interests.Count); // Assigns a random interest area to the drone
            //better to use the target asign, no?
            // Instatiate Game Object
            droneObject = Instantiate(prefab_drone, new Vector3(x, y, prefab_drone.transform.position.z), Quaternion.identity); // Drones should spawn at the drone controller coordinates
            drone=droneObject.GetComponent<Drone>();
            int index =AssignNewTarget(drone,false);
            drone.speed_movement=drone_speed*time_scale;
            drone.speed_turn=200f*time_scale;
            drone.fuel_max=drone_battery;
            drone.fuel=drone_battery;
            drone.FuelConsumptionRate=time_scale;
            drone.timescale=time_scale;
            drone.MinFuel=drone_battery/10f;
            droneObject.GetComponent<Rigidbody2D>().drag=0.8f*time_scale;
            if(index!=-1){
                droneObject.SendMessage("ChangeTarget", interests[index].transform);
                drones.Add((Drone)droneObject.GetComponent(typeof(Drone)));
            }else{
                Debug.Log("failed to asign initial target");
            }
            
            
            yield return new WaitForSeconds(SpawnTimer * 2f / time_scale);
        }
    }

    private void SpawnDronesTimer()
    {
        StartCoroutine(SpawnDrones());
    }

    private IEnumerator SpawnUAVs()
    {/*
        uavs = new List<UAV>();
        GameObject uavObject;
        GameObject interestObject;

        destinations = new List<Interest>();

        for (int i = 0; i < num_uavs; i++)
        {
            // Spawn Initial Destination Area for UAV
            float x_random = UnityEngine.Random.Range(50, width * 0.75f);
            float y_random = UnityEngine.Random.Range(50, height * 0.75f);

            // Instatiate Game Object
            interestObject = Instantiate(prefab_interest, new Vector3(x_random, y_random, prefab_interest.transform.position.z), Quaternion.identity);

            float area_x_random = 10;
            float area_y_random = 10;

            Vector3 scale = interestObject.transform.localScale;

            scale.x = area_x_random;
            scale.y = area_y_random;

            interestObject.transform.localScale = scale;

            destinations.Add((Interest)interestObject.GetComponent(typeof(Interest)));

            // Instatiate Game Object
            uavObject = Instantiate(prefab_uav, new Vector3(x, y, prefab_drone.transform.position.z), Quaternion.identity); // UAVs should spawn at the drone controller coordinates

            uavs.Add((UAV)uavObject.GetComponent(typeof(UAV)));

            uavs[i].ChangeTarget(destinations[i].transform);
        */
            yield return new WaitForSeconds(SpawnTimer);
       // }
    }

    private void Start()
    {
        startSim();
    }

    public void startSim()
    {
        runCSVO=true;
        int peopleSpawned=0;
        alternating= (wd!=0);
        wind.x=wsx1;wind.y=wsy1;
        currentStartTime = DateTime.Now;
		UnityEngine.Random.InitState(seed);
        if(peopleFound==null){
            peopleFound = new List<People>();
        }else{
            foreach(People i in peopleFound){
                i.destroy();
            }
            peopleFound.Clear();
        }
        
        toCSVConverter = new ListToCSVConverter();
        // Gets the postion of the controller
        x = this.transform.position.x;
        y = this.transform.position.y;

        // Spawn Interests and People
        if(interests==null){
            interests = new List<Interest>();
        }else{
            foreach(Interest i in interests){
                i.destroy();
            }
            interests.Clear();
        }
        if(people==null){
            people = new List<People>();
        }else{
            foreach(People i in people){
                i.destroy();
            }
            people.Clear();
        }
        if(dronesOnPoI==null){
            dronesOnPoI = new List<int>();
        }else{
            dronesOnPoI.Clear();
        }
        if(drones==null){
            drones = new List<Drone>();
        }else{
            foreach(Drone i in drones){
                i.destroy();
            }
            drones.Clear();
        }
        GameObject[] rebelDrones = GameObject.FindGameObjectsWithTag("Drone");
        foreach(GameObject i in rebelDrones){
            i.GetComponent<Drone>().destroy();
        }
        GameObject interestObject;
        
        for (int i = 0; i < num_interest; i++)
        {
            float x_random = UnityEngine.Random.Range(0, map_sizex);
            float y_random = UnityEngine.Random.Range(0, map_sizey);

            // Instatiate Game Object
            interestObject = Instantiate(prefab_interest, new Vector3(x_random, y_random, prefab_interest.transform.position.z), Quaternion.identity);

            // Makes the area of the cirlce random
            float area_x_random = UnityEngine.Random.Range(map_sizex/(2*num_interest), 2*map_sizex/num_interest); // a human is 25 units van is 300 x 150
            float area_y_random = UnityEngine.Random.Range(map_sizey/(2*num_interest), 2*map_sizey/num_interest);

            Vector3 scale = interestObject.transform.localScale;

            scale.x *= area_x_random;
            scale.y *= area_y_random;

            interestObject.transform.localScale = scale;

            Interest interest=interestObject.GetComponent<Interest>();
            interests.Add(interest);
            interest.time_scale=time_scale;
            interest.intrestLevel=area_x_random*area_y_random/800f;
            interest.peekInterest=interest.intrestLevel;
            interest.sizex=area_x_random;
            interest.sizey=area_y_random;
            dronesOnPoI.Add(0);
            // Spawn People
            GameObject peopleObject;
            int peopleToSpawn=UnityEngine.Random.Range(num_people/(2*num_interest),Mathf.Min((2*num_people/num_interest),num_people-peopleSpawned));
            if(i==num_interest-1){peopleToSpawn=num_people-peopleSpawned;}
            peopleSpawned+=peopleToSpawn;
            for (int p = 0; p < peopleToSpawn; p++)
            {
                float xrand=UnityEngine.Random.Range(0,area_x_random)+UnityEngine.Random.Range(0,area_x_random)-area_x_random;
                float yrand=UnityEngine.Random.Range(0,area_y_random)+UnityEngine.Random.Range(0,area_y_random)-area_y_random;
                xrand/=4;yrand/=4;
                peopleObject = Instantiate(prefab_human, new Vector3(x_random + xrand, y_random + yrand, prefab_human.transform.position.z), Quaternion.identity);
                People person = peopleObject.GetComponent<People>();
                person.time_scale=time_scale;
                people.Add(person);
            }
        }

        // InvokeRepeating("UpdatePathfinding", 1f, PathfindingTimer);

        // Spawn Drones on a timer so that it looks more realistic
        StartCoroutine(SpawnUAVs()); // UAVs First

        Invoke("SpawnDronesTimer", SpawnTimer * num_uavs); // Drones are spawned after all the uavs have been launched
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

        AssignNewTarget(drone,true);
    }

    private IEnumerator StartRefuel(UAV uav)
    {/*
        uav.Refuling = true;

        float distance = Vector2.Distance(uav.transform.position, this.transform.position);

        while (uav.fuel < uav.fuel_max && distance <= uav.waypoint_distance)
        {
            uav.Refuel();*/
            yield return new WaitForSeconds(SpawnTimer);/*
        }

        uav.Refuling = false;

        AssignNewTarget(uav, null);*/
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


    void Update()
    {
        WindAffect();
    }

    //Wind pushing on the drone
    void WindAffect()
    {
        foreach(Drone d in drones){
            d.gameObject.GetComponent<Rigidbody2D>().AddForce(wind*(0.8f*time_scale*time_scale), ForceMode2D.Force);
        }

        if (alternating && !coroutineIsRunning)
        {

            StartCoroutine(ChangeWindDirection(wsx1, wsy1,wsx2,wsy2, wd));
        }         
    }

    IEnumerator ChangeWindDirection(float x1, float y1,float x2,float y2, float duration)
    {
        coroutineIsRunning = true;
        float elapsed = 0.0f;
        if (wind.x == x1)
        {
            while (elapsed < duration)
            {
                wind.x = Mathf.Lerp(x1, x2, elapsed / duration);
                wind.y = Mathf.Lerp(y1, y2, elapsed / duration);
                elapsed += Time.deltaTime*time_scale;
                yield return null;
            }

            wind.x = x2;
            wind.y = y2;
        }
        else { 
        while (elapsed < duration)
        {
            wind.x = Mathf.Lerp(x2, x1, elapsed / duration);
            wind.y = Mathf.Lerp(y2, y1, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        wind.x = x1;
        wind.y = y1;
        }
        coroutineIsRunning = false;
    }
}
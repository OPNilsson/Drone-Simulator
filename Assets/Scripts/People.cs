using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class People : MonoBehaviour
{
    public float XCords { get; set; }
    public float YCords { get; set; }

    public float TimeToFind { get; set; }
    public float TimeuntilFound { get; set; }

    public bool IsSaved {get; set;}
    public GameObject Human;

    public People (float xCords, float yCords, float timeToFind, GameObject human)
    {
        XCords = xCords;
        YCords = yCords;
        TimeToFind = timeToFind;
        IsSaved = false;
        Human = human;
    }

    public void HumanDied() {
        Destroy(Human, TimeToFind);  
    }
}

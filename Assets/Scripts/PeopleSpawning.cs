﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleSpawning : MonoBehaviour
{
    [Range(1, 200)]
    public int population;
    public List<People> humans = new List<People>();
    public GameObject human;

    // Start is called before the first frame update
    void Start()
    {
        SpawnPeople(100,100);
    }

    public void SpawnPeople(int width, int height) {
        float x;
        float y;
        GameObject tempHuman;
        for (int i = 0; i < population; i++) {
            x = Random.Range(1, width);
            y = Random.Range(1, height);
            
            tempHuman = Instantiate(human, new Vector3(x, y, 1), Quaternion.identity);

            humans.Add(new People(x, y, 100, tempHuman));
        }    

       
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class PeopleSpawning : MonoBehaviour
{
    private GameObject UI;

    // Start is called before the first frame update
    private void Spawn(ValueTuple<int, int, int, int> paramters)
    {
        SpawnPeople(paramters.Item1, paramters.Item2, paramters.Item3, paramters.Item4);
    }

    public GameObject human;

    public List<People> humans;

    [Range(1, 200)]
    public int population;

    //public void SpawnPeople(int width, int height) {
    //    float x;
    //    float y;
    //    GameObject tempHumanObj;
    //    People tempHuman;

    // for (int i = 0; i < population; i++) { x = Random.Range(1, width); y = Random.Range(1, height);

    // tempHumanObj = Instantiate(human, new Vector3(x, y, 1), Quaternion.identity);

    //        tempHuman = new People(x, y, 100, tempHumanObj);
    //        tempHuman.HumanDied();
    //        humans.Add(tempHuman);
    //    }
    //}

    // Deletes ALL Human Game Objects in the world
    public void Exterminate()
    {
        // Gets rid of all the Human Objects in the world
        var objects = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "Human");
        foreach (var person in objects)
        {
            DestroyImmediate(person, true);
        }
    }

    public void SpawnPeople(int numberOfPeople, int width, int height, int seed)
    {
        humans = new List<People>();

        UI = GameObject.Find("PanelMain");
        var test = UI.GetComponent<RectTransform>().rect.width;
        var size = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - test, Screen.height, 0));

        var tileWidth = size.x / width;
        var tileHeight = size.y / height;

        float x;
        float y;
        GameObject tempHumanObj;
        People tempHuman;

		UnityEngine.Random.InitState(seed);

        for (int i = 0; i < numberOfPeople; i++)
        {
            x = UnityEngine.Random.Range(0, width);
            y = UnityEngine.Random.Range(0, height);

            // Instantiate Game Object
            tempHumanObj = Instantiate(human, new Vector3((x * tileWidth) + (tileWidth / 2), (y * tileHeight) + (tileHeight / 2), 1), Quaternion.identity);
            tempHumanObj.transform.localScale = new Vector3(tileWidth, tileHeight);
            tempHuman = new People(x, y, 100, tempHumanObj);
            tempHuman.HumanDied();
            humans.Add(tempHuman);
        }
    }
}
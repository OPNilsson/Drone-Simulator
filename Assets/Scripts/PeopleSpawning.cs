using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleSpawning : MonoBehaviour
{
    [Range(1, 200)]
    public int population;
    public List<People> humans = new List<People>();
    public GameObject human;
	private GameObject UI;

	// Start is called before the first frame update
	void Spawn(ValueTuple<int, int, int> paramters)
    {
        SpawnPeople(paramters.Item1, paramters.Item2, paramters.Item3);
    }

    //public void SpawnPeople(int width, int height) {
    //    float x;
    //    float y;
    //    GameObject tempHumanObj;
    //    People tempHuman;

    //    for (int i = 0; i < population; i++) {
    //        x = Random.Range(1, width);
    //        y = Random.Range(1, height);
            
    //        tempHumanObj = Instantiate(human, new Vector3(x, y, 1), Quaternion.identity);

    //        tempHuman = new People(x, y, 100, tempHumanObj);
    //        tempHuman.HumanDied();
    //        humans.Add(tempHuman);
    //    }       
    //}

	public void SpawnPeople(int numberOfPeople, int width, int height)
	{

		UI = GameObject.Find("PanelMain");
		var test = UI.GetComponent<RectTransform>().rect.width;
		var size = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - test, Screen.height, 0));

		var tileWidth = size.x / width;
		var tileHeight = size.y / height;

		float x;
		float y;
		GameObject tempHumanObj;
		People tempHuman;

		for (int i = 0; i < numberOfPeople; i++)
		{
			x = UnityEngine.Random.Range(0, width);
			y = UnityEngine.Random.Range(0, height);

			tempHumanObj = Instantiate(human, new Vector3((x*tileWidth) + (tileWidth / 2), (y*tileHeight) + (tileHeight / 2), 1), Quaternion.identity);
			tempHumanObj.transform.localScale = new Vector3(tileWidth, tileHeight);
			tempHuman = new People(x, y, 100, tempHumanObj);
			tempHuman.HumanDied();
			humans.Add(tempHuman);
		}
	}
}

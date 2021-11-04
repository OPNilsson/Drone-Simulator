using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleSpawning : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        SpawnPeople(420, 200);
    }

    public GameObject human;

    public List<People> humans;

    [Range(1, 200)]
    public int population;

    public void SpawnPeople(int width, int height)
    {
        humans = new List<People>();

        float x;
        float y;
        GameObject tempHumanObj;
        People tempHuman;

        for (int i = 0; i < population; i++)
        {
            x = Random.Range(1, width);
            y = Random.Range(1, height);

            tempHumanObj = Instantiate(human, new Vector3(x, y, 1), Quaternion.identity);

            tempHuman = new People(x, y, 100, tempHumanObj);
            tempHuman.HumanDied();
            humans.Add(tempHuman);
        }
    }
}
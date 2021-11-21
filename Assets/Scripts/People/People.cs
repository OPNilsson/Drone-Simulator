using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class People : MonoBehaviour
{
    public Rigidbody2D body;
    public Canvas canvas;
    public GameObject Human;
    public float speed_movement = 100f;
    public SpriteRenderer sprite;
    public Text text;
    private Vector2 direction;
    private float timer = 0;
    public People(float xCords, float yCords, float timeToFind, GameObject human)
    {       
        XCords = xCords;
        YCords = yCords;
        TimeToFind = timeToFind;
        IsSaved = false;
        Human = human;
    }
    public DateTime TimeWhenFound {get; set;}
    public bool IsSaved { get; set; }

    // The time it takes for the human to change direction
    public float TimeToFind { get; set; }

    // The time interval it takes to move in seconds
    public float TimeuntilFound { get; set; }

    public float XCords { get; set; }
    public float YCords { get; set; }

    public void HumanDied()
    {
        Destroy(Human, TimeToFind);
    }

    public void Spotted()
    {
        TimeWhenFound = DateTime.Now;
        // Stop the human from moving
        speed_movement = 0f;

        // Change Color to Green
        Color color = sprite.color;

        color.r = 0;
        color.b = 0;

        sprite.color = color;

        TimeToFind = timer % 60;

        text.text = TimeToFind.ToString();

        canvas.gameObject.SetActive(true);
    }

    private void FixedUpdate()
    {
        Vector2 force = direction * speed_movement * Time.deltaTime;

        body.AddForce(force); // Moves the drone
    }

    private void Start()
    {
        float x = UnityEngine.Random.Range(-1, 1.01f);
        float y = UnityEngine.Random.Range(-1, 1.01f);

        direction = new Vector2(x, y);

        speed_movement = UnityEngine.Random.Range(0, speed_movement); // So that some humans stay where they are
    }

    private void Update()
    {
        timer += Time.deltaTime;
    }
}
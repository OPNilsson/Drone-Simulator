using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherWind : MonoBehaviour
{
    //The direction of the weather/gravity
    public float xWinddirection, yWinddirection = 0;
    //Set direction of wind
    Vector2 windDir;
    //Gravities force on a object.
    public float windSpeed = 9.8f;
    public bool staticWinddirection = true;
    bool coroutineIsRunning = false;

    public int minXWinddirection, maxXWinddirection;

    private Rigidbody2D rb;

    void Start()
    {
        windDir = new Vector2(xWinddirection, yWinddirection);
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        WindAffect();
    }

    //Wind pushing on the drone
    void WindAffect()
    {
        windDir = new Vector2(xWinddirection, yWinddirection);
          rb.AddForce(windDir * windSpeed * 1, ForceMode2D.Force);

        if (!staticWinddirection && !coroutineIsRunning)
        {

            StartCoroutine(ChangeWindDirection(minXWinddirection, maxXWinddirection, 1f));
        }
        else if (staticWinddirection)
        {
            StopAllCoroutines();
        }
         
    }

    IEnumerator ChangeWindDirection(float min, float max, float duration)
    {
        coroutineIsRunning = true;
        float elapsed = 0.0f;
        if (xWinddirection == max)
        {
            while (elapsed < duration)
            {
                xWinddirection = Mathf.Lerp(max, min, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            xWinddirection = min;
        }
        else { 
        while (elapsed < duration)
        {
            xWinddirection = Mathf.Lerp(min, max, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        xWinddirection = max;
        }
        coroutineIsRunning = false;
    }

}



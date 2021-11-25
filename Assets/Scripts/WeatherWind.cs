using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherWind : MonoBehaviour
{
    //The direction of the weather
    public float xWinddirection1, yWinddirection1,xWinddirection2, yWinddirection2 = 0;
    public float duration=0;
    //Set direction of wind
    Vector2 windDir;
    //Gravities force on a object.
    public bool staticWinddirection = true;
    bool coroutineIsRunning = false;

    private Rigidbody2D rb;
    public float time_scale;

    void Start()
    {
        windDir = new Vector2(xWinddirection1, yWinddirection1);
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
        rb.AddForce(windDir*(0.8f*time_scale*time_scale), ForceMode2D.Force);

        if (!staticWinddirection && !coroutineIsRunning)
        {

            StartCoroutine(ChangeWindDirection(xWinddirection1, yWinddirection1,xWinddirection2,yWinddirection2, duration));
        }
        else if (staticWinddirection)
        {
            StopAllCoroutines();
        }
         
    }

    IEnumerator ChangeWindDirection(float x1, float y1,float x2,float y2, float duration)
    {
        coroutineIsRunning = true;
        float elapsed = 0.0f;
        if (windDir.x == x1)
        {
            while (elapsed < duration)
            {
                windDir.x = Mathf.Lerp(x1, x2, elapsed / duration);
                windDir.y = Mathf.Lerp(y1, y2, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            windDir.x = x2;
            windDir.y = y2;
        }
        else { 
        while (elapsed < duration)
        {
            windDir.x = Mathf.Lerp(x2, x1, elapsed / duration);
            windDir.y = Mathf.Lerp(y2, y1, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        windDir.x = x1;
        windDir.y = y1;
        }
        coroutineIsRunning = false;
    }

}



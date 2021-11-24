using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private float a;
    private float d;
    private Camera main;
    private Vector3 s = new Vector3(0, 0, 0);
    private float zoom = 100;

    private void RedrawCameraBorder(float width, float height)
    {
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);

        rect.ForceUpdateRectTransforms();
    }

    private void Start()
    {
        main = GetComponent<Camera>();
    }

    private void Update()
    {
        // Zoom In
        if (Input.GetKeyDown("q"))
        {
            zoom /= 2;
            main.orthographicSize = zoom * 5;

            Vector2 screenBounds = main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

            RedrawCameraBorder(screenBounds.x, screenBounds.y);
        }

        // Zoom Out
        if (Input.GetKeyDown("e"))
        {
            zoom *= 2;
            main.orthographicSize = zoom * 5;

            Vector2 screenBounds = main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

            RedrawCameraBorder(screenBounds.x, screenBounds.y);
        }

        d = 1 / (decay + 1);
        a = speed * (1 - d);

        s.x += Input.GetAxis("Horizontal") * a;
        s.y += Input.GetAxis("Vertical") * a;

        s *= d;
        gameObject.transform.position += s;
    }

    public float decay;
    public RectTransform rect;
    public float speed;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapController : MonoBehaviour
{
    private void LateUpdate()
    {
        if (followCamera)
        {
            Vector3 position = target.position;

            position.y = target.position.y;
            position.z = target.position.z;

            transform.position = position;

            if (rotate)
            {
                transform.rotation = Quaternion.Euler(90f,
                target.eulerAngles.y, 0f);
            }
        }
    }

    public bool followCamera = false;
    public bool rotate = false;
    public Transform target;
}
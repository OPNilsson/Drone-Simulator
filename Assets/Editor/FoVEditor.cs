using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary> This code was made following a tutorial by Sebastian Lague on Youtube
/// https://www.youtube.com/watch?v=rQG9aUWarwE&list=PLFt_AvWsXl0dohbtVgHDNmgZV_UY7xZv7&index=1 and
/// adpated for 2D use </summary>
[CustomEditor(typeof(FOV))]
public class FoVEditor : Editor
{
    private void OnSceneGUI()
    {
        FOV fov = (FOV)target;

        Handles.color = Color.white;

        Handles.DrawWireArc(fov.transform.position, Vector3.forward, Vector3.right, 360, fov.viewRadius); // Changed for 2D

        // Draws the Cone of view
        Vector3 viewAngleA = fov.DirFromAngle(-fov.viewAngle / 2, false);
        Vector3 viewAngleB = fov.DirFromAngle(fov.viewAngle / 2, false);

        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleA * fov.viewRadius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleB * fov.viewRadius);

        // Draws a Red Line that connects from the fov origin to the target center
        //Debug.Log(fov.visibleTargets.Count);
        Handles.color = Color.red;
        foreach (Transform visibleTarget in fov.visibleTargets)
        {
            Handles.DrawLine(fov.transform.position, visibleTarget.position);
        }
    }
}
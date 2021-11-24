using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> This code was made following a tutorial by Sebastian Lague however it was modified to
/// work in 2D for the Drone System Branch Update on Youtube
/// https://www.youtube.com/watch?v=rQG9aUWarwE&list=PLFt_AvWsXl0dohbtVgHDNmgZV_UY7xZv7&index=1 </summary>
public class FOV : MonoBehaviour
{
    /// <summary>
    /// How far away two edges need to be before it stops looking for more edges
    /// </summary>
    public float EdgeDstThreshold;

    /// <summary>
    /// How many iterations of checking where the edge of an object is
    /// </summary>
    public int EdgeResolveIterations;

    public float meshResolution;
    public LayerMask ObstacleMask;
    public LayerMask TargetMask;

    [Range(0, 360)]
    public float viewAngle;

    public MeshFilter viewMeshFilter;
    public float viewRadius;

    [HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();

    private Mesh viewMesh;

    /// <summary>
    /// This Method has beed Modified to work in 2D
    /// </summary>
    public Vector2 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees -= transform.eulerAngles.z;
        }
        return new Vector2(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    /// <summary>
    /// This is better explained in Part 2 of the series on how to create this FOV class
    /// </summary>
    public void DrawFOV()
    {
        viewMesh.Clear();

        int rayCount = (int)(viewAngle * meshResolution);

        float rayAngleSize = viewAngle / rayCount;

        List<Vector2> viewPoints = new List<Vector2>();
        ViewCastInfo oldViewCast = new ViewCastInfo();
        for (int i = 0; i <= rayCount; i++)
        {
            float angle = (-(transform.eulerAngles.z) - viewAngle / 2 + rayAngleSize * i);

            //Debug.DrawLine(transform.position, ((Vector2)transform.position) +
            //DirFromAngle(angle, true) * viewRadius, Color.blue);

            ViewCastInfo viewCast = ViewCast(angle);

            if (i > 0)
            {
                bool edgeDstThresholdExceeded = Mathf.Abs(oldViewCast.dst - viewCast.dst) > EdgeDstThreshold;
                if (oldViewCast.hit != viewCast.hit || (oldViewCast.hit && viewCast.hit && edgeDstThresholdExceeded))
                {
                    EdgeInfo edge = FindEdge(oldViewCast, viewCast);
                    if (edge.pointA != Vector2.zero)
                    {
                        viewPoints.Add(edge.pointA);
                    }
                    if (edge.pointB != Vector2.zero)
                    {
                        viewPoints.Add(edge.pointB);
                    }
                }
            }

            viewPoints.Add(viewCast.point);
        }

        int vertexPoints = viewPoints.Count + 1; // +1 Due to the origin vertex

        Vector3[] vertices = new Vector3[vertexPoints];

        int[] triangles = new int[(vertexPoints - 2) * 3]; // Unity draws rays by going to point a b c

        vertices[0] = Vector3.zero;

        for (int i = 0; i < vertexPoints - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            if (i < vertexPoints - 2)
            {
                triangles[i * 3] = 0; // Points ABC
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    private EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector2 minPoint = Vector2.zero;
        Vector2 maxPoint = Vector2.zero;

        for (int i = 0; i < EdgeResolveIterations; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.dst - newViewCast.dst) > EdgeDstThreshold;
            if (newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceeded)
            {
                minAngle = angle;
                minPoint = newViewCast.point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }

        return new EdgeInfo(minPoint, maxPoint);
    }

    /// <summary>
    /// This Method has beed Modified to work in 2D
    /// </summary>
    private void FindVisibleTargets()
    {
        visibleTargets.Clear();
        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, TargetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;

            Vector2 dirToTarget = target.transform.position - transform.position;

            // If the target is within the view angle
            if (Vector2.Angle(dirToTarget, transform.up) < viewAngle / 2)
            {
                float dstToTarget = Vector2.Distance(transform.position, target.position);

                // If there is no obstacle between the target and this
                if (!Physics2D.Raycast(transform.position, dirToTarget, dstToTarget, ObstacleMask))
                {
                    visibleTargets.Add(target);
                }
            }
        }
    }

    private void LateUpdate()
    {
        FindVisibleTargets();
        DrawFOV();
    }

    private void Start()
    {
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;
    }

    /// <summary>
    /// Instances the Raycast to return the ViewCastInfo structure
    /// </summary>
    /// <param name="globalAngle">
    /// The angle at which to cast the ray in terms of clockwise 0 - 360 degrees
    /// </param>
    /// <returns> </returns>
    private ViewCastInfo ViewCast(float globalAngle)
    {
        Vector2 dir = DirFromAngle(globalAngle, true);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, dir, out hit, viewRadius, ObstacleMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else
        {
            return new ViewCastInfo(false, (Vector2)(transform.position) + dir * viewRadius, viewRadius, globalAngle);
        }
    }

    public struct EdgeInfo
    {
        public Vector2 pointA;
        public Vector2 pointB;

        public EdgeInfo(Vector2 _pointA, Vector2 _pointB)
        {
            pointA = _pointA;
            pointB = _pointB;
        }
    }

    /// <summary>
    /// Is an object containg information from a Raycasting instancing
    /// </summary>
    public struct ViewCastInfo
    {
        public float angle;
        public float dst;
        public bool hit;
        public Vector2 point;

        public ViewCastInfo(bool _hit, Vector2 _point, float _dst, float _angle)
        {
            hit = _hit;
            point = _point;
            dst = _dst;
            angle = _angle;
        }
    }
}
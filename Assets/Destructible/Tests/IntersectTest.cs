using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntersectTest : MonoBehaviour
{
    public NormalFinder NormalFinder;
    public Transform PlaneGO;
    public Transform LineStart, LineEnd;

    [ContextMenu("Test Intersect")]
    public void TestIntersect()
    {
        Vector3 normal;
        if(NormalFinder.Find(PlaneGO, out normal))
        {
            Plane plane = new Plane(PlaneGO.position, normal);
            Line line = new Line(LineStart.position, LineEnd.position);

            Vector3 point;

            if(Intersector.Intersect(plane, line, out point))
            {
                Debug.Log("Intersect !");
                Debug.Log(point);
            } else
            {
                Debug.Log("Don't intersect :(");
            }
        } else
        {
            Debug.Log("Don't Raycast :(");
        }
    }
}

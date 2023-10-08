using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public static class MovementPathFactory
{
    public static SplineContainer CreateMovementPath(Vector3[] waypoints)
    {
        GameObject movementPath = new GameObject("Movement Path");

        SplineContainer container = movementPath.AddComponent<SplineContainer>();
        Spline spline = container.AddSpline();
        BezierKnot[] knots = new BezierKnot[waypoints.Length];

        for (int i = 0; i < waypoints.Length; i++)
        {
            knots[i] = new BezierKnot(waypoints[i], Vector3.zero, Vector3.zero);
            knots[i].Rotation = Quaternion.Euler(180f, 0f, 0f);
        }

        spline.Knots = knots;

        return container;
    }
}

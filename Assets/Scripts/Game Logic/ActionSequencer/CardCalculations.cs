using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CardCalculations
{
    public static Quaternion CardRotation(PlacementFacing facing, Camera camera, Vector3 lookDirection)
    {
        if (facing == PlacementFacing.Up) return Quaternion.LookRotation(Vector3.up, lookDirection);

        if (facing == PlacementFacing.Down) return Quaternion.LookRotation(Vector3.down, lookDirection);

        if (facing == PlacementFacing.ToCamera) return Quaternion.LookRotation(-camera.transform.forward, Vector3.up);

        if (facing == PlacementFacing.FromCamera) return Quaternion.LookRotation(camera.transform.forward, Vector3.up);

        return Quaternion.identity;
    }
}


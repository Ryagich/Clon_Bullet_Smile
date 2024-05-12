using UnityEngine;

public static class FOVUtils
{
    public static bool IsInFOV(Transform transform, Vector3 target, float radius, float angle)
    {
        var center = transform.position;
        var distance = Vector3.Distance(target, center);
        if (distance > radius)
        {
            return false;
        }

        var dirToTarget = target - center;
        var inFOVCondition = Vector3.Angle(transform.right, dirToTarget) < angle / 2;
        if (!inFOVCondition)
        {
            return false;
        }

        return true;
    }
}
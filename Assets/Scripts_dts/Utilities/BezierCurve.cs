using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurve 
{
    public static Vector3 Bezeir(Vector3 start, Vector3 control, Vector3 end, float by)
    {
        Vector3 result =  Vector3.Lerp(Vector3.Lerp(start,control, by),
            Vector3.Lerp(control,end,by),by);
        return result;
    }
    
    public static Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;
        return p;
    }
}

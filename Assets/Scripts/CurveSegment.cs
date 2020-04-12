using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions {
    public static Vector4 Mult (this Vector4 v, Matrix4x4 m) {
        Vector4 r = v;
        r[0] = Vector4.Dot(v, m.GetColumn(0));
        r[1] = Vector4.Dot(v, m.GetColumn(1));
        r[2] = Vector4.Dot(v, m.GetColumn(2));
        r[3] = Vector4.Dot(v, m.GetColumn(3));
        return r;
    }

    public static Matrix4x4 MultFloat(this Matrix4x4 m, float f) {
        for (int i = 0; i < 4; i++) {
            Vector4 v = m.GetRow(i);
            for (int j = 0; j < 4; j++) {
                v[j] = v[j] * f;
            }
            m.SetRow(i, v);
        }
        return m;
    }
}

public class CurveSegment
{
    Matrix4x4 M = Matrix4x4.identity;
    Vector4 U = Vector4.one;
    Vector4 B = Vector4.one;

    public CurveSegment(CurveType curveType, float p0, float p1, float p2, float p3) {
        // set B and M

        if (curveType == CurveType.Bezier) M = setBezier();
        if (curveType == CurveType.CatmullRom) M = setCatmullRom();
        if (curveType == CurveType.Hermite) M = setHermite();
        if (curveType == CurveType.BSpline) M = setBSpline();
        // Debug.Log(M.ToString());

        B[0] = p0;
        B[1] = p1;
        B[2] = p2;
        B[3] = p3;

        // p : spline function
        // u : conversion table
        // s : speed control function
    }

    public float Evaluate(float u) {
        u = u - (int) u;

        // update U based on u
        U[0] = u * u * u;
        U[1] = u * u;
        U[2] = u;
        U[3] = 1;

        //Vector4 MB = M * B; 
        float res = Vector4.Dot(U.Mult(M), B);

        return res;
    }

    public float EvaluateDv(float u) {
        u = u - (int) u;

        // update U based on u
        U[0] = 3 * u * u;
        U[1] = 2 * u;
        U[2] = 1;
        U[3] = 0;

        // Vector4 MB = M * B; 
        float res = Vector4.Dot(U.Mult(M), B);

        return res;
    }

    public Matrix4x4 setCatmullRom() {
        Matrix4x4 m = Matrix4x4.identity;

        m.SetRow(0, new Vector4(-1, 3, -3, 1));
        m.SetRow(1, new Vector4(2, -5, 4, -1));
        m.SetRow(2, new Vector4(-1, 0, 1, 0));
        m.SetRow(3, new Vector4(0, 2, 0, 0));

        m = m.MultFloat(0.5f);

        return m;
    }

    public Matrix4x4 setHermite() {
        Matrix4x4 m = Matrix4x4.identity;

        m.SetRow(0, new Vector4(2, -2, 1, 1));
        m.SetRow(1, new Vector4(-3, 3, -2, -1));
        m.SetRow(2, new Vector4(0, 0, 1, 0));
        m.SetRow(3, new Vector4(1, 0, 0, 0));

        return m;
    }

    public Matrix4x4 setBezier() {
        Matrix4x4 m = Matrix4x4.identity;

        m.SetRow(0, new Vector4(-1, 3, -3, 1));
        m.SetRow(1, new Vector4(3, -6, 3, 0));
        m.SetRow(2, new Vector4(-3, 3, 0, 0));
        m.SetRow(3, new Vector4(1, 0, 0, 0));

        return m;
    }

    public Matrix4x4 setBSpline() {
        Matrix4x4 m = Matrix4x4.identity;

        m.SetRow(0, new Vector4(-1, 3, -3, 1));
        m.SetRow(1, new Vector4(3, -6, 3, 0));
        m.SetRow(2, new Vector4(-3, 0, 3, 0));
        m.SetRow(3, new Vector4(1, 4, 1, 0));

        m = m.MultFloat(1f / 6f);

        return m;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentripetalCatmullRom {

    private int resolution; //Amount of points between control points. [Tesselation factor]

    //Store points on the Catmull curve so we can visualize them
    public List<Vector3> newPoints = new List<Vector3>();

    private Vector3[] controlPoints;

    public float alpha = 0.5f;

    public CentripetalCatmullRom(Vector3[] controlPoints, int resolution)
    {
        if (controlPoints == null || controlPoints.Length <= 2 || resolution < 2)
        {
            throw new ArgumentException("Catmull Rom Error: Too few control points or resolution too small");
        }

        this.controlPoints = new Vector3[controlPoints.Length];
        for (int i = 0; i < controlPoints.Length; i++)
        {
            this.controlPoints[i] = controlPoints[i];
        }

        this.resolution = resolution;

        GenerateSplinePoints();
    }

    //Updates control points
    public void Update(Vector3[] controlPoints)
    {
        if (controlPoints.Length <= 0 || controlPoints == null)
        {
            throw new ArgumentException("Invalid control points");
        }

        this.controlPoints = new Vector3[controlPoints.Length];
        for (int i = 0; i < controlPoints.Length; i++)
        {
            this.controlPoints[i] = controlPoints[i];
        }

        GenerateSplinePoints();
    }

    //Updates resolution and closed loop values
    public void Update(int resolution)
    {
        if (resolution < 2)
        {
            throw new ArgumentException("Invalid Resolution. Make sure it's >= 1");
        }
        this.resolution = resolution;

        GenerateSplinePoints();
    }



    //Math stuff to generate the spline points
    private void GenerateSplinePoints()
    {
        newPoints.Clear();

        for (int onePoint = 0; controlPoints.Length - 1 > onePoint; onePoint++)
        {
            Debug.Log(onePoint);
            Vector3 p0, p1, p2, p3;

            if (onePoint == 0)
            {
                p0 = controlPoints[0] + (controlPoints[0] - controlPoints[1]);
            }
            else
            {
                p0 = controlPoints[1];
            }

            p1 = controlPoints[onePoint];
            p2 = controlPoints[onePoint + 1];

            if (controlPoints.Length - 2 == onePoint)
            {
                p3 = controlPoints[1] + (controlPoints[0] - controlPoints[1]);
            }
            else
            {
                p3 = controlPoints[onePoint + 2];
            }

            float t0 = 0.0f;
            float t1 = GetT(t0, p0, p1);
            float t2 = GetT(t1, p1, p2);
            float t3 = GetT(t2, p2, p3);

            for (float t = t1; t < t2; t += ((t2 - t1) / resolution))
            {
                Vector3 A1 = (t1 - t) / (t1 - t0) * p0 + (t - t0) / (t1 - t0) * p1;
                Vector3 A2 = (t2 - t) / (t2 - t1) * p1 + (t - t1) / (t2 - t1) * p2;
                Vector3 A3 = (t3 - t) / (t3 - t2) * p2 + (t - t2) / (t3 - t2) * p3;

                Vector3 B1 = (t2 - t) / (t2 - t0) * A1 + (t - t0) / (t2 - t0) * A2;
                Vector3 B2 = (t3 - t) / (t3 - t1) * A2 + (t - t1) / (t3 - t1) * A3;

                Vector3 C = (t2 - t) / (t2 - t1) * B1 + (t - t1) / (t2 - t1) * B2;

                newPoints.Add(C);

            }
        }

    }

    float GetT(float t, Vector3 p0, Vector3 p1)
    {
        float a = Mathf.Pow((p1.x - p0.x), 2.0f) + Mathf.Pow((p1.y - p0.y), 2.0f);
        float b = Mathf.Pow(a, 0.5f);
        float c = Mathf.Pow(b, alpha);

        return (c + t);
    }

}

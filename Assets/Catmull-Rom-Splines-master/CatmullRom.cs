using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JPBotelho
{
    /*  
        Catmull-Rom splines are Hermite curves with special tangent values.
        Hermite curve formula:
        (2t^3 - 3t^2 + 1) * p0 + (t^3 - 2t^2 + t) * m0 + (-2t^3 + 3t^2) * p1 + (t^3 - t^2) * m1
        For points p0 and p1 passing through points m0 and m1 interpolated over t = [0, 1]
        Tangent M[k] = (P[k+1] - P[k-1]) / 2
    */
    public class CatmullRom
    {

        private int resolution; //Amount of points between control points. [Tesselation factor]
        private bool closedLoop;

        private Vector3[] splinePoints; //Generated spline points

        private Vector3[] controlPoints;

        //Returns spline points. Count is contorolPoints * resolution + [resolution] points if closed loop.
        public Vector3[] GetPoints()
        {
            if(splinePoints == null)
            {
                throw new System.NullReferenceException("Spline not Initialized!");
            }

            return splinePoints;
        }

        public CatmullRom(Vector3[] controlPoints, int resolution, bool closedLoop)
        {
            if(controlPoints == null || controlPoints.Length <= 2 || resolution < 2)
            {
                throw new ArgumentException("Catmull Rom Error: Too few control points or resolution too small");
            }

            this.controlPoints = new Vector3[controlPoints.Length];
            for(int i = 0; i < controlPoints.Length; i++)
            {
                this.controlPoints[i] = controlPoints[i];             
            }

            this.resolution = resolution;
            this.closedLoop = closedLoop;

            GenerateSplinePoints();
        }

        //Updates control points
        public void Update(Vector3[] controlPoints)
        {
            if(controlPoints.Length <= 0 || controlPoints == null)
            {
                throw new ArgumentException("Invalid control points");
            }

            this.controlPoints = new Vector3[controlPoints.Length];
            for(int i = 0; i < controlPoints.Length; i++)
            {
                this.controlPoints[i] = controlPoints[i];             
            }

            GenerateSplinePoints();
        }

        //Updates resolution and closed loop values
        public void Update(int resolution, bool closedLoop)
        {
            if(resolution < 2)
            {
                throw new ArgumentException("Invalid Resolution. Make sure it's >= 1");
            }
            this.resolution = resolution;
            this.closedLoop = closedLoop;

            GenerateSplinePoints();
        }

        //Sets the length of the point array based on resolution/closed loop.
        private void InitializeProperties()
        {
            int pointsToCreate;
            if (closedLoop)
            {
                pointsToCreate = resolution * controlPoints.Length; //Loops back to the beggining, so no need to adjust for arrays starting at 0
            }
            else
            {
                pointsToCreate = resolution * (controlPoints.Length - 1);
            }

            splinePoints = new Vector3[pointsToCreate];       
        }

        //Math stuff to generate the spline points
        private void GenerateSplinePoints()
        {
            InitializeProperties();

            Vector3 p0, p1; //Start point, end point
            Vector3 m0, m1; //Tangents

            // First for loop goes through each individual control point and connects it to the next, so 0-1, 1-2, 2-3 and so on
            int closedAdjustment = closedLoop ? 0 : 1;
            for (int currentPoint = 0; currentPoint < controlPoints.Length - closedAdjustment; currentPoint++)
            {
                bool closedLoopFinalPoint = (closedLoop && currentPoint == controlPoints.Length - 1);

                p0 = controlPoints[currentPoint];
                
                if(closedLoopFinalPoint)
                {
                    p1 = controlPoints[0];
                }
                else
                {
                    p1 = controlPoints[currentPoint + 1];
                }

                // m0
                if (currentPoint == 0) // Tangent M[k] = (P[k+1] - P[k-1]) / 2
                {
                    if(closedLoop)
                    {
                        m0 = p1 - controlPoints[controlPoints.Length - 1];
                    }
                    else
                    {
                        m0 = p1 - p0;
                    }
                }
                else
                {
                    m0 = p1 - controlPoints[currentPoint - 1];
                }

                // m1
                if (closedLoop)
                {
                    if (currentPoint == controlPoints.Length - 1) //Last point case
                    {
                        m1 = controlPoints[(currentPoint + 2) % controlPoints.Length] - p0;
                    }
                    else if (currentPoint == 0) //First point case
                    {
                        m1 = controlPoints[currentPoint + 2] - p0;
                    }
                    else
                    {
                        m1 = controlPoints[(currentPoint + 2) % controlPoints.Length] - p0;
                    }
                }
                else
                {
                    if (currentPoint < controlPoints.Length - 2)
                    {
                        m1 = controlPoints[(currentPoint + 2) % controlPoints.Length] - p0;
                    }
                    else
                    {
                        m1 = p1 - p0;
                    }
                }

                m0 *= 0.5f; //Doing this here instead of  in every single above statement
                m1 *= 0.5f;

                float pointStep = 1.0f / resolution;

                if ((currentPoint == controlPoints.Length - 2 && !closedLoop) || closedLoopFinalPoint) //Final point
                {
                    pointStep = 1.0f / (resolution - 1);  // last point of last segment should reach p1
                }
                
                // Creates [resolution] points between this control point and the next
                for (int tesselatedPoint = 0; tesselatedPoint < resolution; tesselatedPoint++)
                {
                    float t = tesselatedPoint * pointStep;

                    Vector3 point = CalculatePosition(p0, p1, m0, m1, t);

                    splinePoints[currentPoint * resolution + tesselatedPoint] = point;
                }
            }
        }

        //Calculates curve position at t[0, 1]
        public static Vector3 CalculatePosition(Vector3 start, Vector3 end, Vector3 tanPoint1, Vector3 tanPoint2, float t)
        {
            // Hermite curve formula:
            // (2t^3 - 3t^2 + 1) * p0 + (t^3 - 2t^2 + t) * m0 + (-2t^3 + 3t^2) * p1 + (t^3 - t^2) * m1
            Vector3 position = (2.0f * t * t * t - 3.0f * t * t + 1.0f) * start
                + (t * t * t - 2.0f * t * t + t) * tanPoint1
                + (-2.0f * t * t * t + 3.0f * t * t) * end
                + (t * t * t - t * t) * tanPoint2;

            return position;
        }  
    }
}
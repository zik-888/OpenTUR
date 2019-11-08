using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using JPBotelho;

public class PointGO : MonoBehaviour
{

    public GameObject ScencePoint { set; get; }
    public GameObject ItemPoint { set; get; }
    protected Vector2Int num;
    protected Vector5 transformVector;

    public Vector5 TransformVector
    {
        set {
            transformVector = value;
            ScencePoint.transform.position = TransformVector.GetVector3();
            ItemPoint.gameObject.GetComponentInChildren<Text>().text = "      " + Math.Round(transformVector.X, 2).ToString() 
                                                                          + " " + Math.Round(transformVector.Y, 2).ToString() 
                                                                          + " " + Math.Round(transformVector.Z, 2).ToString();
        }

        get { return transformVector; }
    }

    public Vector2Int Num
    {
        set
        {
            num = value;
            ItemPoint.GetComponent<Item>().gNum = num;
        }

        get { return num; }
    }

    public void InitPointGO(PrefabCollection prefabCollection, GameObject trajectory, Vector2Int Num)
    {
        ItemPoint = Instantiate(prefabCollection.ItemPrefab);
        ItemPoint.transform.SetParent(trajectory.transform, false);
        ItemPoint.gameObject.GetComponentInChildren<Text>().text = "      point";
        this.Num = Num;
        //---------------------------------------------------//
        ScencePoint = Instantiate(prefabCollection.taregetPointPrefab);
        TransformVector = new Vector5(new Vector3(0, 14.63f, 5.26f), new Vector2(25, 0), 0);
    }

    public void DestroyPointGO()
    {
        Destroy(ItemPoint.gameObject);
        Destroy(ScencePoint.gameObject);
        Destroy(this);
    }

}

public class Trajectory : MonoBehaviour {
    
    [SerializeField] protected GameObject trajectory;
    [SerializeField] protected GameObject content;
    protected PrefabCollection prefabCollection;

    public List<PointGO> pointList = new List<PointGO>();

    protected int num;
    public int idPoint;

    public int Num
    {
        set
        {
            num = value;
            trajectory.transform.Find("MainItem").GetComponent<Item>().gNum.Set(num, -1);
            trajectory.gameObject.GetComponentInChildren<TrItem>().num = value;

            for(int i = 0; pointList.Count > i; i++)
            {
                pointList[i].Num = new Vector2Int(num, i);
            }
        }
        get { return num; }
    }

    protected void Awake()
    {
        Messenger<int>.AddListener(GameEvent.ADD_POINT_IN_TRAJECTORY, AddPoint);
    }

    protected void OnDestroy()
    {
        Messenger<int>.RemoveListener(GameEvent.ADD_POINT_IN_TRAJECTORY, AddPoint);
    }
    

    public virtual void InitItem(int Num, PrefabCollection prefabCollection)
    {
        this.prefabCollection = prefabCollection;
        this.content = prefabCollection.content;
        trajectory = Instantiate(prefabCollection.trajectoryPrefab);
        trajectory.transform.SetParent(content.transform, false);
        this.Num = Num;
        trajectory.AddComponent<LineRenderer>();
        // настройка линии
        AnimationCurve curve = new AnimationCurve();
        curve.AddKey(0f, 0.25f);
        trajectory.gameObject.GetComponent<LineRenderer>().widthCurve = curve;
    }

    public virtual void DestrItem()
    {

        foreach (PointGO point in pointList)
        {
            point.DestroyPointGO();
        }

        Destroy(trajectory.gameObject);
        Destroy(this);
    }



    public virtual void AddPoint(int where)
    {
        if (where == num && trajectory != null)
        {
            pointList.Add(gameObject.AddComponent<PointGO>());

            
            pointList[pointList.Count - 1].InitPointGO(prefabCollection, trajectory, new Vector2Int(num, pointList.Count - 1));
            idPoint = pointList.Count - 1;
            Messenger<Vector2Int>.Broadcast(GameEvent.ID_ITEM, new Vector2Int(num, idPoint));
            PaintTrajectory();
        }
    }

    public void RemovePoint(int numPoint)
    {
        pointList[numPoint].DestroyPointGO();
        pointList.RemoveAt(numPoint);

        for (int i = numPoint; i < pointList.Count; i++)
        {
            pointList[i].ItemPoint.GetComponent<Item>().gNum.Set(num, i);
        }

        PaintTrajectory();
    }

    public void SetOptionPoint(Vector5 vector5)
    {
        pointList[idPoint].TransformVector = vector5;
        PaintTrajectory();
    }

    public Vector5 GetOptionPoint()
    {
        return pointList[idPoint].TransformVector;
    }

    public virtual void PaintTrajectory() { }

    public virtual Vector5[] GetApproximatedArray() { return null; }

}

public class Line : Trajectory
{
    public int resolution = 100;

    public override void InitItem(int Num, PrefabCollection prefabCollection)
    {
        base.InitItem(Num, prefabCollection);
        trajectory.gameObject.GetComponentInChildren<Text>().text = "Line";
        
    }

    public override void PaintTrajectory()
    {
        Vector3[] vectors = new Vector3[pointList.Count];

        for(int i = 0; vectors.Length > i; i++)
        {
            vectors[i] = pointList[i].TransformVector.GetVector3();
        }

        trajectory.gameObject.GetComponent<LineRenderer>().positionCount = pointList.Count;
        trajectory.gameObject.GetComponent<LineRenderer>().SetPositions(vectors);
    }

    public override Vector5[] GetApproximatedArray()
    {
        Vector5[] vector5s = new Vector5[(pointList.Count - 1) * resolution];

        vector5s[0] = pointList[0].TransformVector;

        for (int i = 0; i < pointList.Count - 1; i++)
        {
            for (int j = 1; j <= resolution && j * (i + 1) < vector5s.Length; j++)
            {
                vector5s[j * (i + 1) + (resolution - j) * i].SetVector5(Vector3.Lerp(pointList[i].TransformVector.GetVector3(), pointList[i + 1].TransformVector.GetVector3(), j / (float)resolution),
                                                                        Vector2.Lerp(pointList[i].TransformVector.GetVector2(), pointList[i + 1].TransformVector.GetVector2(), j / (float)resolution),
                                                                        Mathf.Lerp(pointList[i].TransformVector.S, pointList[i + 1].TransformVector.S, j / (float)resolution));
            }
        }

        return vector5s;
    }

    public Vector3 Lerp(Vector3 l, Vector3 r, float t)
    {
        return l * (1 - t) + r * t;
    }

}

public class Spline : Trajectory
{
    public CatmullRom spline;
    public Vector3[] splineVectors;
    public int resolution = 100;
    public bool closedLoop = false;

    //public override void AddPoint(int where)
    //{
    //    base.AddPoint(where);

    //}

    public override void InitItem(int Num, PrefabCollection prefabCollection)
    {
        base.InitItem(Num, prefabCollection);
        trajectory.gameObject.GetComponentInChildren<Text>().text = "Spline";

        AddPoint(num);
        AddPoint(num);
        AddPoint(num);
        AddPoint(num);
    }

    public override void PaintTrajectory()
    {
        Vector3[] controlPoints = new Vector3[pointList.Count];

        for (int i = 0; controlPoints.Length > i; i++)
        {
            controlPoints[i] = pointList[i].TransformVector.GetVector3();
        }
        
        try
        {
            if (spline != null)
            {
                spline.Update(controlPoints);
                spline.Update(resolution, closedLoop);
            }
            else
            {
                spline = new CatmullRom(controlPoints, resolution, closedLoop);
            }
            

            splineVectors = spline.GetPoints();
            trajectory.gameObject.GetComponent<LineRenderer>().positionCount = splineVectors.Length;
            trajectory.gameObject.GetComponent<LineRenderer>().SetPositions(splineVectors);
        }
        catch { print("Exeption PaintTrajectory()"); }
    }

    public override Vector5[] GetApproximatedArray()
    {
        Vector5[] vector5s = new Vector5[splineVectors.Length];

        vector5s[0] = pointList[0].TransformVector;

        for (int i = 0; i < pointList.Count - 1; i++)
        {
            for (int j = 1; j <= resolution && j * (i + 1) < vector5s.Length; j++)
            {
                vector5s[j * (i + 1) + (resolution - j) * i].SetVector5(splineVectors[j * (i + 1) + (resolution - j) * i],
                                                                 Vector2.Lerp(pointList[i].TransformVector.GetVector2(), pointList[i + 1].TransformVector.GetVector2(), j / (float)resolution),
                                                                 Mathf.Lerp(pointList[i].TransformVector.S, pointList[i + 1].TransformVector.S, j / (float)resolution));
            }
        }

        return vector5s;
    }
}

public class Circle : Trajectory
{
    //public int resolution = 100;
    //public Vector3[] circleVectors;

    //public override void InitItem(int Num, PrefabCollection prefabCollection)
    //{
    //    base.InitItem(Num, prefabCollection);
    //    trajectory.gameObject.GetComponentInChildren<Text>().text = "Circle";

    //    AddPoint(num);
    //    AddPoint(num);
    //    AddPoint(num);
    //}

    //public override void PaintTrajectory()
    //{
        

    //    if (pointList.Count > 2)
    //    {
    //        Vector3 centr = CircleCalc.CalcCentrCircle(pointList[0].TransformVector.GetVector3(), 
    //                                                    pointList[1].TransformVector.GetVector3(), 
    //                                                    pointList[2].TransformVector.GetVector3());

    //        circleVectors = new Vector3[resolution];

    //        circleVectors[0] = pointList[0].TransformVector.GetVector3();

            

    //        for (int i = 1; resolution > i; i++)
    //        {
    //            circleVectors[i] = Vector3.SlerpUnclamped(pointList[0].TransformVector.GetVector3(), pointList[2].TransformVector.GetVector3(), (float)i / (float)resolution);
    //            //print((float)i / (float)resolution);
    //        }

            

    //        trajectory.gameObject.GetComponent<LineRenderer>().positionCount = circleVectors.Length;
    //        trajectory.gameObject.GetComponent<LineRenderer>().SetPositions(circleVectors);
    //    }

        
    //}

}


public class CircleCalc
{

    // Определитель третьего порядка через его колонки
    protected static float Det(Vector3 col1, Vector3 col2, Vector3 col3)
    {
        float d;
        d = col1.x * col2.y * col3.z +
            col1.y * col2.z * col3.x +
            col1.z * col2.x * col3.y -
            col1.z * col2.y * col3.x -
            col1.y * col2.x * col3.z -
            col1.x * col2.z * col3.y;
        return d;
    }

    // Плоскость через точку K перпендикулярно отрезку AB
    protected static Vector4 plane1(Vector3 A, Vector3 B, Vector3 K)
    {
        Vector4 P = new Vector4(B.x - A.x,
                                B.y - A.y,
                                B.z - A.z,
                                -(K.x * (B.x - A.x) + K.y * (B.y - A.y) + K.z * (B.z - A.z)));

        return P;
    }

    // Плоскость через точки A, B, C
    protected static Vector4 plane2(Vector3 A, Vector3 B, Vector3 C)
    {
        Vector3 E, X, Y, Z;

        E.x = 1; E.y = 1; E.z = 1;

        X.x = A.x; X.y = B.x; X.z = C.x;
        Y.x = A.y; Y.y = B.y; Y.z = C.y;
        Z.x = A.z; Z.y = B.z; Z.z = C.z;
        // вычисление коэффициентов уравнения плоскости
        Vector4 P = new Vector4(Det(E, Y, Z), Det(X, E, Z), Det(X, Y, E), -Det(X, Y, Z));
        return P;
    }

    public static Vector3 CalcCentrCircle(Vector3 A, Vector3 B, Vector3 C)
    {
        Vector3 K, col1, col2, col3, col4, O;
        Vector4 P1, P2, P3;
        float d;

        // первые две плоскости через середины сторон
        K.x = (A.x + B.x) / 2; K.y = (A.y + B.y) / 2; K.z = (A.z + B.z) / 2;
        P1 = plane1(A, B, K);
        K.x = (A.x + C.x) / 2; K.y = (A.y + C.y) / 2; K.z = (A.z + C.z) / 2;
        P2 = plane1(A, C, K);
        // третья плоскость через три точки
        P3 = plane2(A, B, C);
        // Транспонирование плоскостей в колонки расширенной матрицы
        col1.x = P1.x; col1.y = P2.x; col1.z = P3.x;
        col2.x = P1.y; col2.y = P2.y; col2.z = P3.y;
        col3.x = P1.z; col3.y = P2.z; col3.z = P3.z;
        col4.x = -P1.w; col4.y = -P2.w; col4.z = -P3.w;
        // Формулы Крамера
        d = Det(col1, col2, col3);
        O.x = Det(col4, col2, col3) / d;
        O.y = Det(col1, col4, col3) / d;
        O.z = Det(col1, col2, col4) / d;

        return O;
    }

}


using UnityEngine;
using System;
using System.Collections;

public static class Constant
{
    public const int DegToADC = 182;
}


public struct Joint6
{
    public float J1 { get; private set; }
    public float J2 { get; private set; }
    public float J3 { get; private set; }
    public float J4 { get; private set; }
    public float J5 { get; private set; }
    public float J6 { get; private set; }

    public float this[int index]
    {
        get
        {
            switch (index)
            {
                case 0:
                    return J1;
                case 1:
                    return J2;
                case 2:
                    return J3;
                case 3:
                    return J4;
                case 4:
                    return J5;
                case 5:
                    return J6;
                default:
                    return 0;
            }
        }

        set
        {
            switch (index)
            {
                case 0:
                    J1 = value;
                    break;
                case 1:
                    J2 = value;
                    break;
                case 2:
                    J3 = value;
                    break;
                case 3:
                    J4 = value;
                    break;
                case 4:
                    J5 = value;
                    break;
                case 5:
                    J6 = value;
                    break;
            }
        }
    }

    public Joint6(float J1, float J2, float J3, float J4, float J5, float J6)
    {
        this.J1 = J1;
        this.J2 = J2;
        this.J3 = J3;
        this.J4 = J4;
        this.J5 = J5;
        this.J6 = J6;
    }

    public void SetJoint6(float J1, float J2, float J3, float J4, float J5, float J6)
    {
        this.J1 = J1;
        this.J2 = J2;
        this.J3 = J3;
        this.J4 = J4;
        this.J5 = J5;
        this.J6 = J6;
    }

    public void SetOrientation(float J4, float J5)
    {
        this.J4 = J4;
        this.J5 = J5;
    }

    public static Joint6 operator *(Joint6 a, float d)
    {
        return new Joint6(a.J1*d, a.J2 * d, a.J3 * d, a.J4 * d, a.J5 * d, a.J6 * d);
    }

    public static bool operator ==(Joint6 a, Joint6 b)
    {
        bool A;

        if (a.J1 == b.J1 && a.J2 == b.J2 && a.J3 == b.J3 && a.J4 == b.J4 && a.J5 == b.J5 && a.J6 == b.J6)
        {
            A = true;
        }
        else
        {
            A = false;
        }

        return A;
    }

    public override bool Equals(object o)
    {
        return true;
    }

    public override int GetHashCode()
    {
        return 0;
    }

    public static bool operator !=(Joint6 a, Joint6 b)
    {
        bool A;

        if (a.J1 == b.J1 && a.J2 == b.J2 && a.J3 == b.J3 && a.J4 == b.J4 && a.J5 == b.J5 && a.J6 == b.J6)
        {
            A = false;
        }
        else
        {
            A = true;
        }

        return A;
    }

    public override string  ToString()
    {
        return '(' + J1.ToString() + ", " + J2.ToString() + ", " + J3.ToString() + ", " + J4.ToString() + ", " + J5.ToString() + ", " + J6.ToString() + ')';
    }
    
    
    public float[] ToArray()
    {
        return new float[6] { J1, J2, J3, J4, J5, J6 }; 
    }

    public void IntoArray(float[] data)
    {
        J1 = data[0]; J2 = data[1]; J3 = data[2]; J4 = data[3]; J5 = data[4]; J6 = data[5];

    }

    public byte[] ToByteSteam()
    {
        // считываем поля стурктуры и записываем ихх в массив
        float[] jointArray = ToArray();
        // создаём массив размерностью =
        // 6 (кол-во э-тов структуры) * 2 (размерность ushort)
        byte[] fullByte = new byte[12];

        for (int i = 0; i < 6; i++)
        {
            jointArray[i] += 180;
            // [0, 360] -> [0, 2^16]
            jointArray[i] = jointArray[i] * Constant.DegToADC;
            // конвертируем в поток байт
            byte[] pieceByte = BitConverter.GetBytes((ushort)jointArray[i]);
            //Debug.Log(jointArray[0]);
            // складываем данные последовательно в один массив
            pieceByte.CopyTo(fullByte, 2 * i);
        }

        return fullByte;
    }

    public void IntoByteSteam(byte[] data)
    {
        // размер одной переменной в байтах
        int sizeWord = 2;
        // создаём массив типа float[]
        float[] floatArray = new float[6];
        int j = 0;
        for (int i = 0; i < floatArray.Length; i++)
        {
            // считываем из массива данных необходимые байты и конвертируем их в float
            floatArray[i] = (float)BitConverter.ToUInt16(data, j) / Constant.DegToADC - 180;
            j = j + sizeWord;
        }
        // записываем в поля структуры
        IntoArray(floatArray);
    }

    static public float[] IntoByteSteamStatic(byte[] data)
    {
        // размер одной переменной в байтах
        int sizeWord = 2; 
        // создаём массив типа float[]
        float[] floatArray = new float[6];
        int j = 0;
        for (int i = 0; i < floatArray.Length; i++)
        {
            // считываем из массива данных необходимые байты и конвертируем их в float
            floatArray[i] = (float)BitConverter.ToUInt16(data, j) / Constant.DegToADC;
            j = j + sizeWord;
        }
        return floatArray;
    }

}

public struct Vector5
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
    public float A { get; set; }
    public float B { get; set; }
    public float S { get; set; }

    public float this[int index]
    {
        get
        {
            switch (index)
            {
                case 0:
                    return X;
                case 1:
                    return Y;
                case 2:
                    return Z;
                case 3:
                    return A;
                case 4:
                    return B;
                case 5:
                    return S;
                default:
                    return 0;
            }
        }

        set
        {
            switch (index)
            {
                case 0:
                    X = value;
                    break;
                case 1:
                    Y = value;
                    break;
                case 2:
                    Z = value;
                    break;
                case 3:
                    A = value;
                    break;
                case 4:
                    B = value;
                    break;
                case 5:
                    S = value;
                    break;
            }
        }
    }

    public Vector5(float X, float Y, float Z, float A, float B, float S)
    {
        this.X = X;
        this.Y = Y;
        this.Z = Z;
        this.A = A;
        this.B = B;
        this.S = S;
    }

    public Vector5(Vector3 XYZ, float A, float B, float S)
    {
        this.X = XYZ.x;
        this.Y = XYZ.y;
        this.Z = XYZ.z;
        this.A = A;
        this.B = B;
        this.S = S;
    }

    public Vector5(Vector3 XYZ, Vector2 AB, float S)
    {
        this.X = XYZ.x;
        this.Y = XYZ.y;
        this.Z = XYZ.z;
        this.A = AB.x;
        this.B = AB.y;
        this.S = S;
    }

    public Vector3 GetVector3()
    {
        return new Vector3(X, Y, Z);
    }

    //static public Vector5 SmoothDamp(Vector5 current, Vector5 target, ref Vector5 currentVelocity, float smoothTime)
    //{
    //    Vector3.SmoothDamp(current.GetVector3(), target.GetVector3(), ref currentVelocity.GetVector3(), smoothTime);
    //    Vector2.SmoothDamp(current, target, ref currentVelocity, smoothTime);
    //}

    static public float Distance(Vector5 a, Vector5 b)
    {
        return Vector3.Distance(a.GetVector3(), b.GetVector3());
    }

    public Vector2 GetVector2()
    {
        return new Vector3(A, B);
    }

    public void SetVector5(Vector3 XYZ, Vector2 AB, float S)
    {
        this.X = XYZ.x;
        this.Y = XYZ.y;
        this.Z = XYZ.z;
        this.A = AB.x;
        this.B = AB.y;
        this.S = S;
    }

    public void SetVector5(float X, float Y, float Z, float A, float B, float S)
    {
        this.X = X;
        this.Y = Y;
        this.Z = Z;
        this.A = A;
        this.B = B;
        this.S = S;
    }
}

public struct MovLimitation
{
    public int upperLine;
    public int bottomLine;

    public MovLimitation(int upperLine, int bottomLine)
    {
        this.upperLine = upperLine;
        this.bottomLine = bottomLine;
    }

    public float Limitation(float value)
    {
        if (value > upperLine)
        {
            return upperLine;
        }
        else if (value < bottomLine)
        {
            return bottomLine;
        }
        else
        {
            return value;
        }
    }
}

public struct MovLimitationExt
{
    // параметры ограничения в согнутом состоянии
    public int upperLineInBent;
    public int bottomLineBent;
    // параметры ограничения в разогнутом состоянии
    public int upperLineInUnBent;
    public int bottomLineUnBent;
    // граница
    public int deathLineUnBent;
    public int deathLineBent;

    //

    public float A;
    public float B;

    public MovLimitationExt(int upperLineInBent, int bottomLineBent, int upperLineInUnBent, int bottomLineUnBent, int deathLineUnBent, int deathLineBent, float A = 0, float B = 0)
    {
        this.upperLineInBent = upperLineInBent;
        this.bottomLineBent = bottomLineBent;
        this.upperLineInUnBent = upperLineInUnBent;
        this.bottomLineUnBent = bottomLineUnBent;
        this.deathLineUnBent = deathLineUnBent;
        this.deathLineBent = deathLineBent;

        this.A = upperLineInUnBent; this.B = bottomLineUnBent;
    }

    public float Limitation(float q_1, float q)
    {
        // если положения предшествующего положения больше 10 градусов
        // то расчёт ведётся обычными методами
        if (q_1 > deathLineUnBent)
        {
            // если выше верхней линии ограничения
            if (q > upperLineInUnBent)
            {
                A = upperLineInUnBent;
                return upperLineInUnBent;
            }
            // если меньше нижней линии ограничения
            // при правильной работе код не заходит в эту часть
            else if (q < bottomLineUnBent)
            {
                B = bottomLineUnBent;
                return bottomLineUnBent;
            }
            // если ограничение не превышенно
            else
            {
                return q;
            }
        }
        else
        {
            // вычисляем коэфицент интерполяции
            float t = Mathf.InverseLerp(deathLineBent, deathLineUnBent, q_1);
            // вычисляем верхнюю линию ограничения при данном значении сочлинений 
            A = Mathf.LerpAngle(upperLineInBent, upperLineInUnBent, t);
            // вычисляем нижнюю линию ограничения при данном значении сочлинений 
            B = Mathf.LerpAngle(bottomLineBent, bottomLineUnBent, t);

            if (q > A)
            {
                return A;
            }
            else if (q < B)
            {
                return B;
            }
            else
            {
                return q;
            }
        }
    }
}

public class Kinematic : MonoBehaviour {

    protected Joint6 inpJoint;
    protected Vector5 inpVector;    

    // joint position input
    public Joint6 InpJoint
    {
        get
        {
            return inpJoint;
        }

        set
        {
            inpJoint = value;
            ForwardKinematic();
            JointLimitation();
            // InverseKinematic();
            JointMove(inpJoint, goJoints);
        }
    }
    // vector position input
    public Vector5 InpVector
    {
        get
        {
            return inpVector;
        }

        set
        {
            inpVector = value;

            InverseKinematic();
            //ForwardKinematic();
            JointLimitation();
            JointMove(inpJoint, goJoints);
            //StopCoroutine(JointMoveC(inpJoint, goJoints));
            //StartCoroutine(JointMoveC(inpJoint, goJoints));
        }
    }

    protected float l1 = 7, l2 = 5, l3 = 6.7f, l4 = 3;
    [SerializeField] protected GameObject[] goJoints = new GameObject[6];

    // ограничения 
    protected MovLimitation lim1 = new MovLimitation(170, -170);
    protected MovLimitation lim2 = new MovLimitation(45, -45);
    public MovLimitationExt lim3Ext = new MovLimitationExt(140, 110, 115, 45, 10, -45);
    //protected MovLimitation lim3 = new MovLimitation(25 + 90, -45 + 90);
    protected MovLimitation lim4 = new MovLimitation(90, -90);
    protected MovLimitation lim5 = new MovLimitation(180, -180);
    protected MovLimitation lim6 = new MovLimitation(100, 0);

    protected GameObject testSfear1;
    protected GameObject testSfear2;
    [SerializeField] protected GameObject SUPER;

    private void Awake()
    {
        testSfear1 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        testSfear1.transform.parent = SUPER.transform;
        testSfear2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        testSfear2.transform.localScale = SUPER.transform.localScale / 2;
        testSfear2.transform.parent = SUPER.transform;
        InpJoint = new Joint6(0, -45, 90, 0, 0, 0);
    }

    static protected Vector3 VectorZ(Joint6 InpJoint) // вектор вдоль последнего звена
    {
        float r13 = Mathf.Sin(InpJoint.J1) * Mathf.Sin(InpJoint.J2 + InpJoint.J3 + InpJoint.J4);
        float r23 = Mathf.Cos(InpJoint.J1) * Mathf.Sin(InpJoint.J2 + InpJoint.J3 + InpJoint.J4);
        float r33 = Mathf.Cos(InpJoint.J2 + InpJoint.J3 + InpJoint.J4);
        return new Vector3(r13, r33, r23);
    }

    private void ForwardKinematic()
    {
        // Inp J1, J2 ...
        // Out x,y,z, a,b,c
        float l1 = 7, l2 = 5, l3 = 6.7f, l4 = 3;

        Joint6 localInpJoint = inpJoint * Mathf.Deg2Rad;

        float xz = l2 * Mathf.Sin(localInpJoint.J2) + l3 * Mathf.Sin(localInpJoint.J2 + localInpJoint.J3) + l4 * Mathf.Sin(localInpJoint.J2 + localInpJoint.J3 + localInpJoint.J4);
        float x = Mathf.Sin(localInpJoint.J1) * xz;
        float z = Mathf.Cos(localInpJoint.J1) * xz;
        float y = l1 + l2 * Mathf.Cos(localInpJoint.J2) + l3 * Mathf.Cos(localInpJoint.J2 + localInpJoint.J3) + l4 * Mathf.Cos(localInpJoint.J2 + localInpJoint.J3 + localInpJoint.J4);

        Vector3 orientVector = new Vector3(Mathf.Sin(localInpJoint.J2 + localInpJoint.J3 + localInpJoint.J4) * Mathf.Sin(localInpJoint.J1),
                                           Mathf.Cos(localInpJoint.J2 + localInpJoint.J3 + localInpJoint.J4),
                                           Mathf.Sin(localInpJoint.J2 + localInpJoint.J3 + localInpJoint.J4) * Mathf.Cos(localInpJoint.J1));

        float angleA = Mathf.Atan2(orientVector.y, Mathf.Sqrt(Mathf.Pow(orientVector.x, 2) + Mathf.Pow(orientVector.z, 2))) * Mathf.Rad2Deg;

        //inpVector = new Vector5(x, y, z, angleA, inpJoint.J5, inpJoint.J6);
        inpVector.SetVector5(x, y, z, angleA, inpJoint.J5, inpJoint.J6);
    }

    private void InverseKinematic()
    {
        // Inp x,y,z, a,b,c
        // Out J1, J2 ...

        Vector3 InpVector3 = inpVector.GetVector3();

        float atan = Mathf.Atan2(InpVector3.x, InpVector3.z);
        Vector3 orientVector = new Vector3(l4 * Mathf.Cos(inpVector.A * Mathf.Deg2Rad) * Mathf.Sin(atan),
                                   l4 * Mathf.Sin(inpVector.A * Mathf.Deg2Rad),
                                   l4 * Mathf.Cos(inpVector.A * Mathf.Deg2Rad) * Mathf.Cos(atan)); // вектор подхода n

        //if (InpVector3.z < 0)
          //  orientVector = new Vector3(-1 * orientVector.x, 1 * orientVector.y, -1 * orientVector.z);

        Vector3 XYZ_1 = InpVector3 - orientVector;
        

        float q1 = Mathf.Atan2(XYZ_1.x, XYZ_1.z); // ??? Mathf.Atan2(XYZ_1.X, XYZ_1.Z)

        float q2_acos = Mathf.Acos((Mathf.Pow(XYZ_1.x, 2) + Mathf.Pow(XYZ_1.y - l1, 2) + Mathf.Pow(XYZ_1.z, 2) + Mathf.Pow(l2, 2) - Mathf.Pow(l3, 2)) 
                                    / (2 * Mathf.Sqrt(Mathf.Abs(Mathf.Pow(XYZ_1.x, 2) + Mathf.Pow(XYZ_1.y - l1, 2) + Mathf.Pow(XYZ_1.z, 2))) * l2));

        float q2 = Mathf.PI / 2 - Mathf.Atan2(XYZ_1.y - l1, Mathf.Sqrt(Mathf.Pow(XYZ_1.x, 2) + Mathf.Pow(XYZ_1.z, 2)));

        if (!float.IsNaN(q2_acos))
        {
            q2 = Mathf.PI / 2 - Mathf.Atan2(XYZ_1.y - l1, Mathf.Sqrt(Mathf.Pow(XYZ_1.x, 2) + Mathf.Pow(XYZ_1.z, 2))) - q2_acos;
        }
        

        float q3_acos = (-Mathf.Pow(XYZ_1.x, 2) - Mathf.Pow(XYZ_1.y - l1, 2) - Mathf.Pow(XYZ_1.z, 2) + Mathf.Pow(l2, 2) + Mathf.Pow(l3, 2)) / (2 * l3 * l2);
        float q3 = Mathf.PI - Mathf.Acos(q3_acos);

        if(q3_acos > 1)
            q3 = Mathf.PI;
        
        if (q3_acos < -1 || float.IsNaN(q3))
            q3 = 0;


        Vector3 orientVector_1 = new Vector3(Mathf.Sin(q2 + q3) * Mathf.Sin(q1),
                                       Mathf.Cos(q2 + q3), 
                                       Mathf.Sin(q2 + q3) * Mathf.Cos(q1)); // вектор подхода n-1

        float q4 = Vector3.SignedAngle(orientVector_1, orientVector, Vector3.Cross(orientVector, orientVector_1)) * Mathf.Deg2Rad;

        if (Mathf.Abs(Quaternion.LookRotation(orientVector_1, orientVector).eulerAngles.z) > 60)
        {
            q4 *= -1;
        }

        SUPER.transform.position = XYZ_1;
        //testSfear1.transform.localPosition = orientVector_1 * l4;
        //testSfear2.transform.localPosition = orientVector;
        testSfear1.transform.localPosition = Vector3.zero;
        testSfear2.transform.localPosition = Vector3.zero;

        float q5 = inpVector.B * Mathf.Deg2Rad;

        //Debug.Log(q1.ToString() +" "+ q2.ToString() + " " + q3.ToString());
        //inpJoint = new Joint6(q1 * Mathf.Rad2Deg, q2 * Mathf.Rad2Deg, q3 * Mathf.Rad2Deg, q4 * Mathf.Rad2Deg, q5 * Mathf.Rad2Deg, inpVector.S);
        inpJoint.SetJoint6(q1 * Mathf.Rad2Deg, q2 * Mathf.Rad2Deg, q3 * Mathf.Rad2Deg, q4 * Mathf.Rad2Deg, q5 * Mathf.Rad2Deg, inpVector.S);
    }


    private void JointLimitation()
    {
        inpJoint.SetJoint6(lim1.Limitation(inpJoint.J1),
                           lim2.Limitation(inpJoint.J2),
                           lim3Ext.Limitation(inpJoint.J2, inpJoint.J3),
                           lim4.Limitation(inpJoint.J4),
                           lim5.Limitation(inpJoint.J5),
                           inpJoint.J6);
    }

    private void JointMove(Joint6 jointVar, GameObject[] joints)
    {
        joints[0].transform.localRotation = Quaternion.Euler(0, jointVar.J1, 0);
        joints[1].transform.localRotation = Quaternion.Euler(jointVar.J2, 0, 0);
        joints[2].transform.localRotation = Quaternion.Euler(jointVar.J3, 0, 0);
        joints[3].transform.localRotation = Quaternion.Euler(jointVar.J4, 0, 0);
        joints[4].transform.localRotation = Quaternion.Euler(0, jointVar.J5, 0);
    }

    protected IEnumerator JointMoveC(Joint6 jointVar, GameObject[] joints)
    {
        int resolution = 5;
        float[] smoothTime = new float[] {1, 1, 1, 1, 1 };
        GameObject[] joint = new GameObject[5];

        for (int i = 0; i < joint.Length; i++)
        {
            joint[i] = joints[i];
        }

        joints[2].transform.localRotation = Quaternion.Euler(lim3Ext.Limitation(jointVar.J2, jointVar.J3), 0, 0);

        for (int i = 1; i <= resolution; i++)
        {
            joints[0].transform.localRotation = Quaternion.Euler(0, Mathf.SmoothDampAngle(joint[0].transform.localRotation.eulerAngles.y, lim1.Limitation(jointVar.J1), ref smoothTime[0],  i / (float)resolution), 0);
            joints[1].transform.localRotation = Quaternion.Euler(Mathf.SmoothDampAngle(joint[1].transform.localRotation.eulerAngles.x, lim2.Limitation(jointVar.J2), ref smoothTime[1], i / (float)resolution), 0, 0);
            joints[3].transform.localRotation = Quaternion.Euler(Mathf.SmoothDampAngle(joint[3].transform.localRotation.eulerAngles.x, lim4.Limitation(jointVar.J4), ref smoothTime[3], i / (float)resolution), 0, 0);
            joints[4].transform.localRotation = Quaternion.Euler(0, Mathf.SmoothDampAngle(joint[4].transform.localRotation.eulerAngles.y, lim5.Limitation(jointVar.J5), ref smoothTime[4], i / (float)resolution), 0);

            yield return null;
        }
    }

    static public Vector5 ForwardKinematicStatic(Joint6 inpJoint)
    {
        // Inp J1, J2 ...
        // Out x,y,z, a,b,c
        float l1 = 7, l2 = 5, l3 = 6.7f, l4 = 3;

        Joint6 localInpJoint = inpJoint * Mathf.Deg2Rad;

        float xz = l2 * Mathf.Sin(localInpJoint.J2) + l3 * Mathf.Sin(localInpJoint.J2 + localInpJoint.J3) + l4 * Mathf.Sin(localInpJoint.J2 + localInpJoint.J3 + localInpJoint.J4);
        float x = Mathf.Sin(localInpJoint.J1) * xz;
        float z = Mathf.Cos(localInpJoint.J1) * xz;
        float y = l1 + l2 * Mathf.Cos(localInpJoint.J2) + l3 * Mathf.Cos(localInpJoint.J2 + localInpJoint.J3) + l4 * Mathf.Cos(localInpJoint.J2 + localInpJoint.J3 + localInpJoint.J4);

        Vector3 orientVector = new Vector3(Mathf.Sin(localInpJoint.J2 + localInpJoint.J3 + localInpJoint.J4) * Mathf.Sin(localInpJoint.J1),
                                           Mathf.Cos(localInpJoint.J2 + localInpJoint.J3 + localInpJoint.J4),
                                           Mathf.Sin(localInpJoint.J2 + localInpJoint.J3 + localInpJoint.J4) * Mathf.Cos(localInpJoint.J1));

        Vector2 orientVectorP = new Vector2(Mathf.Sqrt(Mathf.Pow(orientVector.x, 2) + Mathf.Pow(orientVector.z, 2)) * Mathf.Sign(orientVector.x + orientVector.z), orientVector.y);

        if (inpJoint.J1 < -45 || inpJoint.J1 > 135)
            orientVectorP.x = -orientVectorP.x;

        float angleA = Vector2.SignedAngle(Vector2.right, orientVectorP);

        float angleB = inpJoint.J5;

        return new Vector5(x, y, z, angleA, angleB, inpJoint.J6);
    }

    static public Joint6 InverseKinematicStatic(Vector5 inpVector)
    {
        // Inp x,y,z, a,b,c
        // Out J1, J2 ...
        float l1 = 7, l2 = 5, l3 = 6.7f, l4 = 3;

        Vector3 InpVector3 = inpVector.GetVector3();

        float atan = Mathf.Atan2(InpVector3.x, InpVector3.z);
        Vector3 orientVector = new Vector3(l4 * Mathf.Cos(inpVector.A * Mathf.Deg2Rad) * Mathf.Sin(atan),
                                   l4 * Mathf.Sin(inpVector.A * Mathf.Deg2Rad),
                                   l4 * Mathf.Cos(inpVector.A * Mathf.Deg2Rad) * Mathf.Cos(atan)); // вектор подхода n

        Vector3 XYZ_1 = InpVector3 - orientVector;


        float q1 = Mathf.Atan2(XYZ_1.x, XYZ_1.z); // ??? Mathf.Atan2(XYZ_1.X, XYZ_1.Z)

        float q2_acos = Mathf.Acos((Mathf.Pow(XYZ_1.x, 2) + Mathf.Pow(XYZ_1.y - l1, 2) + Mathf.Pow(XYZ_1.z, 2) + Mathf.Pow(l2, 2) - Mathf.Pow(l3, 2))
                                    / (2 * Mathf.Sqrt(Mathf.Abs(Mathf.Pow(XYZ_1.x, 2) + Mathf.Pow(XYZ_1.y - l1, 2) + Mathf.Pow(XYZ_1.z, 2))) * l2));

        float q2 = Mathf.PI / 2 - Mathf.Atan2(XYZ_1.y - l1, Mathf.Sqrt(Mathf.Pow(XYZ_1.x, 2) + Mathf.Pow(XYZ_1.z, 2)));

        if (!float.IsNaN(q2_acos))
        {
            q2 = Mathf.PI / 2 - Mathf.Atan2(XYZ_1.y - l1, Mathf.Sqrt(Mathf.Pow(XYZ_1.x, 2) + Mathf.Pow(XYZ_1.z, 2))) - q2_acos;
        }


        float q3_acos = (-Mathf.Pow(XYZ_1.x, 2) - Mathf.Pow(XYZ_1.y - l1, 2) - Mathf.Pow(XYZ_1.z, 2) + Mathf.Pow(l2, 2) + Mathf.Pow(l3, 2)) / (2 * l3 * l2);
        float q3 = Mathf.PI - Mathf.Acos(q3_acos);

        if (q3_acos > 1)
            q3 = Mathf.PI;

        if (q3_acos < -1 || float.IsNaN(q3))
            q3 = 0;

        Vector3 orientVector_1 = new Vector3(Mathf.Sin(q2 + q3) * Mathf.Sin(q1),
                                       Mathf.Cos(q2 + q3),
                                       Mathf.Sin(q2 + q3) * Mathf.Cos(q1)); // вектор подхода n-1

        float q4 = Vector3.Angle(orientVector_1, orientVector) * Mathf.Deg2Rad;

        float sign = orientVector_1.y * Mathf.Sqrt(Mathf.Pow(orientVector.x, 2) + Mathf.Pow(orientVector.z, 2)) -
                orientVector.y * Mathf.Sqrt(Mathf.Pow(orientVector_1.x, 2) + Mathf.Pow(orientVector_1.z, 2)); // считаем определитель в плоскости

        q4 = q4 * Mathf.Sign(sign);

        float q5 = inpVector.B * Mathf.Deg2Rad;
        
        return new Joint6(q1 * Mathf.Rad2Deg, q2 * Mathf.Rad2Deg, q3 * Mathf.Rad2Deg, q4 * Mathf.Rad2Deg, q5 * Mathf.Rad2Deg, inpVector.S);
    }

}

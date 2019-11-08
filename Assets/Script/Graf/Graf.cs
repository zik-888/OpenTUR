using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;
using UnityEngine.UI;
using System.IO;
using System;
using System.Globalization;


public class Graf : MonoBehaviour {

    public UILineRenderer[] lines = new UILineRenderer[6];
    public Image image;
    protected Vector2 sizeWindow;
    //protected int maxInputSignal = 65535;
    protected int maxInputSignal = 360;
    protected Vector2 dataToCOORD; // константа для перевода
    protected static int resolution = 10;
    protected Vector2[][] vectorsArray = new Vector2[6][];

    public Joint6 jointGraf;

    public Joint6 JointGraf
    {
        set
        {
            jointGraf = value;
        }
        get
        {
            return jointGraf;
        }
    }

    public int Resolution
    {
        set
        {
            resolution = value;
            dataToCOORD = new Vector2(sizeWindow.x / (resolution - 1), sizeWindow.y / maxInputSignal);
            for (int i = 0; i < vectorsArray.Length; i++)
            {
                vectorsArray[i] = new Vector2[resolution];
            }
        }

        get
        {
            return resolution;
        }
    }

    // Use this for initialization
    void Start () {
        
        for(int i = 0; i < vectorsArray.Length; i++)
        {
            vectorsArray[i] = new Vector2[resolution];
        }

        sizeWindow = image.GetComponent<RectTransform>().sizeDelta;
        dataToCOORD = new Vector2(sizeWindow.x / (resolution-1), sizeWindow.y / maxInputSignal);

        StartCoroutine(Invoke());
        //StartCoroutine(NewDate());
    }

    protected IEnumerator NewDate()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);

            jointGraf.SetJoint6((Mathf.Sin(Time.time * 10) + 1) * maxInputSignal / 2,
                                (Mathf.Sin(Time.time * 9) + 1) * maxInputSignal / 2,
                                (Mathf.Sin(Time.time * 8) + 1) * maxInputSignal / 2,
                                (Mathf.Sin(Time.time * 7) + 1) * maxInputSignal / 2,
                                (Mathf.Sin(Time.time * 6) + 1) * maxInputSignal / 2,
                                (Mathf.Sin(Time.time * 5) + 1) * maxInputSignal / 2);
        }
    }

    protected IEnumerator Invoke()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            

            GetPlot(jointGraf, 1);

            for(int i = 0; i < vectorsArray.Length; i++)
            {
                lines[i].Points = vectorsArray[i];
            }
        }
    }

    protected void GetPlot(Joint6 joint, int deltaTime)
    {
        for (int i = 0; i < vectorsArray.Length; i++)
        {
            for(int j = vectorsArray[i].Length - 1; j > 0; j--)
            {
                vectorsArray[i][j] = vectorsArray[i][j - 1];
                vectorsArray[i][j].x -= deltaTime * dataToCOORD.x;
            }
            vectorsArray[i][0].Set(sizeWindow.x / 2, joint.ToArray()[i] * dataToCOORD.y - sizeWindow.y / 2);
        }
    }


    string path = @"C:\Users\User\YandexDisk\SS_ISH\ТУР-10\Документы\hta.CSV";

    public void GetCSV()
    {
        try
        {
            for (int i = 0; resolution > i; i++)
            {
                using (StreamWriter sw = new StreamWriter(path, i != 0, System.Text.Encoding.Default))
                {
                    sw.WriteLine("{0};{1};{2};{3};{4};{5}", vectorsArray[0][i].y.ToString("F1", CultureInfo.InvariantCulture),
                                                            vectorsArray[1][i].y.ToString("F1", CultureInfo.InvariantCulture),
                                                            vectorsArray[2][i].y.ToString("F1", CultureInfo.InvariantCulture),
                                                            vectorsArray[3][i].y.ToString("F1", CultureInfo.InvariantCulture),
                                                            vectorsArray[4][i].y.ToString("F1", CultureInfo.InvariantCulture),
                                                            vectorsArray[5][i].y.ToString("F1", CultureInfo.InvariantCulture));
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

    }
}

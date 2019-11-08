using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenceControl : MonoBehaviour {

    [SerializeField] protected Kinematic KinematicField;
    [SerializeField] public TargetProperties targetProperties;

    protected Vector5 targetPoint = new Vector5(0, 14.63f, 5.26f, 25, 0, 0);

    //public GameObject v;

    //public void testPoint()
    //{
    //    targetPoint.SetVector5(v.transform.position, new Vector2(0, 0), 0);
    //    //workProgram = true;
    //    StartCoroutine(TestPointC(v));
    //    print('t');
    //}

    //IEnumerator TestPointC(GameObject T)
    //{
    //    while (!workProgram)
    //    {
    //        yield return null;
    //        targetPoint.SetVector5(T.transform.position, new Vector2(T.transform.rotation.eulerAngles.x, T.transform.rotation.eulerAngles.y), 0);
    //    }
        
    //}

    public bool workProgram = false;

    protected Vector5[] programVectors = null;

    public Vector5[] ProgramVectors
    {
        set
        {
            programVectors = value;
            StartCoroutine(WORKProgram(targetPoint));
        }
        get
        {
            return programVectors;
        }
    }

    public void SetOptionPoint(Vector5 targetPoint)
    {
        this.targetPoint = targetPoint;
        targetProperties.SetInputField(targetPoint);
        StartCoroutine(UpdateSliderCoroutine());
    }

    IEnumerator UpdateSliderCoroutine()
    {
        yield return new WaitForFixedUpdate();
        targetProperties.SetSlider(GetOptionJoint());
        targetProperties.SetProp3Slider(KinematicField.lim3Ext.B, KinematicField.lim3Ext.A);
    }

    public Vector5 GetOptionPoint()
    {
        return KinematicField.InpVector;
    }
    
    public Joint6 GetOptionJoint()
    {
        return KinematicField.InpJoint;
    }

    //----------------------------//

    public void ReadSlider()
    {
        targetProperties.SetInputField(Kinematic.ForwardKinematicStatic(targetProperties.GetSlider()));
        targetPoint = targetProperties.GetInputField();
        targetProperties.SetProp3Slider(KinematicField.lim3Ext.B, KinematicField.lim3Ext.A);
    }

    public void ReadInputField()
    {
        targetProperties.SetSlider(Kinematic.InverseKinematicStatic(targetProperties.GetInputField()));
        targetPoint = targetProperties.GetInputField();
        targetProperties.SetProp3Slider(KinematicField.lim3Ext.B, KinematicField.lim3Ext.A);
    }

    //-------------------------------------------//

    float distance;
    float time;

    public void STOPWORKProgram()
    {
        //workProgram = false;
        workProgram = false;
        //StopCoroutine(WORKProgram(true));
        //StopAllCoroutines();
    }


    private IEnumerator WORKProgram(Vector5 T)
    {
        workProgram = true;
        
        
        for (int i = 0; programVectors.Length > i; i++)
        {
            
            KinematicField.InpVector = programVectors[i];
            //SetOptionPoint(programVectors[i]);

            if (programVectors.Length - 1 > i)
            {
                distance = Vector5.Distance(programVectors[i], programVectors[i + 1]) * 100;
                time = distance / targetProperties.speed;
                yield return new WaitForSeconds(time);
            }
            else
            {
                targetPoint = programVectors[i];
            }

            if(workProgram == false)
            {
                break;
            }


        }

        workProgram = false;

    }

    public float smoothTime = 1F;

    Vector5 vector;
    Vector5 vector2;

    private void Start()
    {
        KinematicField.InpJoint = new Joint6(0, -45, 90, 0, 0, 0);
        targetPoint = KinematicField.InpVector;
        SetOptionPoint(targetPoint);
    }

    void Update() {

        if (workProgram == false)
        {

            //KinematicField.InpJoint = KinematicField.InpJoint * Mathf.Sin(Time.deltaTime);
            //print(targetPoint.GetVector3());

            KinematicField.InpVector = targetPoint;


            //KinematicField.InpVector = Kinematic.ForwardKinematicStatic(new Joint6(KinematicField.InpJoint.J1 + Mathf.Sin(Time.time),
            //                                                                        KinematicField.InpJoint.J2,
            //                                                                        KinematicField.InpJoint.J3 + Mathf.Sin(Time.time * 0.5f),
            //                                                                        KinematicField.InpJoint.J4 + Mathf.Sin(Time.time * 0.5f),
            //                                                                        KinematicField.InpJoint.J5 + Mathf.Sin(Time.time * 0.5f),
            //                                                                        KinematicField.InpJoint.J6 + Mathf.Sin(Time.time * 0.5f)));

            //    vector = KinematicField.InpVector;
            //    for (int i = 0; i < 6; i++)
            //    {
            //        vector2[i] = Mathf.SmoothStep(vector[i], targetPoint[i], smoothTime);
            //    }

            //    KinematicField.InpVector = vector2;
        }

    }
}

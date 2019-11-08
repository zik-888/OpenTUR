using System;
using System.Collections;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PID : MonoBehaviour
{

    [SerializeField] protected InputField[] inputFields = new InputField[18];

    Vector3[] koef = new Vector3[6];


    [Serializable]
    public class InputFieldPIDEvent : UnityEvent<Vector3[]> { }

    public InputFieldPIDEvent OnIFEvent;


    public Vector3[] ReadIF()
    {
        Vector3[] actualKoef = new Vector3[6];

        for(int i = 0; actualKoef.Length > i; i++)
        {
            actualKoef[i].Set(float.Parse(inputFields[i * 3].text), float.Parse(inputFields[(i * 3) + 1].text), float.Parse(inputFields[(i * 3) + 2].text)); 
        }

        return actualKoef;
    }

    public void WriteIF()
    {
        for (int i = 0; koef.Length > i; i++)
        {
            koef[i].Set(float.Parse(inputFields[i * 3].text), float.Parse(inputFields[(i * 3) + 1].text), float.Parse(inputFields[(i * 3) + 2].text));
        }
    }

    public void Default()
    {
        foreach (InputField d in inputFields)
        {
            d.text = "1";
        }
    }

    public bool Equals()
    {
        Vector3[] present =  ReadIF();
        Vector3[] past = koef;
        bool b = true;

        for (int i = 0; present.Length > i; i++)
        {
            b = b && (present[i] == past[i]);
        }

        return b;
    }

    private void Start()
    {
        if(OnIFEvent == null)
        {
            OnIFEvent = new InputFieldPIDEvent();
        }

        foreach(Vector3 vector3 in koef)
        {
            vector3.Set(1, 1, 1);
        }

        Default();
    }

    public void Invoke()
    {
        if (!Equals())
        {
            WriteIF();

            OnIFEvent.Invoke(koef);
        }
    }

}

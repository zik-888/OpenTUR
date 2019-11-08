using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ConnectedButton : MonoBehaviour {
    
    [SerializeField] protected Text tcpButtonText;
    [SerializeField] protected InputField ipInputField;
    [SerializeField] protected InputField portInputField;

    //---------------------------------------
    [Serializable]
    public class IPEvent : UnityEvent<string> { }
    [Serializable]
    public class PortEvent : UnityEvent<string> { }
    [Serializable]
    public class WorkModEvent : UnityEvent<WorkMode> { }
    //[Serializable]
    //public class WorkModEvent : UnityEvent<bool> { }
    //---------------------------------------
    public IPEvent ipEvent;
    public PortEvent portEvent;
    public WorkModEvent workModEvent;
    //---------------------------------------

    protected int workMode = 0;
    protected string ip = "192.168.4.1";
    protected string port = "1024";
    protected bool work = false;

    public int WorkMode
    {
        set
        {
            if (work == false)
            {
                workModEvent.Invoke((WorkMode)2);
            }
            else
            {
                if (value == -1)
                    workMode = 0;
                else
                    workMode = value;
                workModEvent.Invoke((WorkMode)workMode);

            }

            

        }
        get
        {
            return workMode;
        }
    }

    public string TCPButtonText
    {
        set
        {
            tcpButtonText.text = value;
        }
    }



    public string IPInputField
    {
        get
        {
            return ipInputField.text;
        }
        set
        {
            ipInputField.text = value;
        }
    }

    public string PortInputField
    {
        get
        {
            return portInputField.text;
        }
        set
        {
            portInputField.text = value;
        }
    }

    private void Start()
    {
        if (ipEvent == null)
        {
            ipEvent = new IPEvent();
        }
        if (portEvent == null)
        {
            portEvent = new PortEvent();
        }
        if (workModEvent == null)
        {
            workModEvent = new WorkModEvent();
        }
    }

    public void Invoke()
    {
        work = !work;

        WorkMode = workMode;
        

        if (ip != IPInputField)
        {
            ipEvent.Invoke(IPInputField);
            ip = IPInputField;
        }

        if (port != PortInputField)
        {
            portEvent.Invoke(IPInputField);
            port = PortInputField;
        }
        
    }





}

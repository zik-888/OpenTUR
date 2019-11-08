using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public enum WorkMode { Manual, Auto, NOTCONNECT }

public class DataManagerOutput : MonoBehaviour {

    [SerializeField] protected ScenceControl scenceControl;

    protected WorkMode workMode = WorkMode.NOTCONNECT;

    [SerializeField] protected ConnectedButton connectedField;

    [SerializeField] protected TCPIP protocol;

    //--------------- out bufer for massage
    protected Vector5[] programmVectorPackage;
    protected bool programmWork = false;  // 
    protected Joint6 jointVector;
    protected int speed;
    //----------------

    public WorkMode WorkModeProp
    {
        set
        {
            print(value);

            workMode = value;

            switch (value)
            {
                case WorkMode.Manual:


                    StartConnect();

                    StopCoroutine(AutoWork());
                    StartCoroutine(ManualWork());

                    break;
                case WorkMode.Auto:

                    StartConnect();

                    StopCoroutine(ManualWork());
                    //StartCoroutine(AutoWork());

                    break;

                case WorkMode.NOTCONNECT:

                    StopCoroutine(ManualWork());
                    StopCoroutine(AutoWork());
                    StopConnect();

                    break;
            }
        }

    }

    protected void StartConnect()
    {
        if (protocol.tcpClient == null)
        {
            bool e = protocol.TCPStart();

            if (e)
            {
                connectedField.TCPButtonText = "Connected";
            }
            else
            {
                connectedField.TCPButtonText = "Fail connected";
            }
            
        }
    }

    protected void StopConnect()
    {
        if (protocol.tcpClient != null)
        {
            protocol.TCPClose();
            connectedField.TCPButtonText = "Disconnected";
        }

    }


    protected IEnumerator ManualWork()
    {
        

        while (workMode == WorkMode.Manual)
        {
            //yield return new WaitForSeconds(0.1f);
            yield return null;
            // записываем актуальное расчитанное положение джоинтов
            Joint6 actualJointVector = scenceControl.GetOptionJoint();
            int atualSpeed = scenceControl.targetProperties.speed;

            // отправляем изменённое положение джоинтов
            if (jointVector != actualJointVector)
            {

                jointVector = actualJointVector;
                protocol.SendRequest(actualJointVector.ToByteSteam(), "J");
            }

            // отправляем изменёнyю скорость
            if (speed != atualSpeed)
            {
                speed = atualSpeed;
                protocol.SendRequest(BitConverter.GetBytes((ushort)speed), "S");
            }

        }
    }

    protected IEnumerator AutoWork()
    {
        int atualSpeed;

        while (workMode == WorkMode.Auto)
        {
            yield return new WaitForSeconds(1f);

            atualSpeed = scenceControl.targetProperties.speed;

            // отправляем изменённую программу
            if (scenceControl.ProgramVectors != programmVectorPackage && scenceControl.ProgramVectors != null && programmWork == true)
            {
                programmVectorPackage = scenceControl.ProgramVectors;

                Joint6[] jointsArray = new Joint6[programmVectorPackage.Length];
                byte[] byteArray = new byte[programmVectorPackage.Length * 12];

                for (int i = 0; programmVectorPackage.Length > i; i++)
                {
                    jointsArray[i] = Kinematic.InverseKinematicStatic(programmVectorPackage[i]);
                    byte[] pieceByte = jointsArray[i].ToByteSteam();
                    pieceByte.CopyTo(byteArray, 12 * i);
                }
                
                protocol.SendRequest(byteArray, "P");
            }

            // отправляем изменёнyю скорость
            if (speed != atualSpeed)
            {
                speed = atualSpeed;
                protocol.SendRequest(BitConverter.GetBytes((ushort)speed), "S");
            }

        }
    }

    protected void ProgrammWork()
    {
        if (workMode == WorkMode.Auto)
        {
            programmWork = !programmWork;
        }
    }


    public void PIDKoef(Vector3[] vectors)
    {
        //print(vectors[1].ToString());
    }
    

    public void TerminalSend(string message)
    {

        protocol.SendRequest(BitConverter.GetBytes(ushort.Parse(message.Substring(0, message.Length - 2))),
                                                                   message[message.Length - 1].ToString());
    }
}

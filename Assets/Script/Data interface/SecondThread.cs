using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;
using UnityEngine.Networking;


public class SecondThread : TCPIP
{
    //static public TCPIP tcpClient;
    [SerializeField] protected Kinematic kinematic;

    
    static protected byte[] inpByteBuff;
    static Thread myThread;
    

    static public byte[] outJointByte = new byte[6];
    protected static object locker = new object();

    protected static bool isEnable = false;

    public bool TCPIsEnable
    {
        get
        {
            return isEnable;
        }

        set
        {
            isEnable = value;

            switch (isEnable)
            {
                case true:
                    myThread = new Thread(new ThreadStart(Count))
                    {
                        IsBackground = true
                    };
                    TCPStart();
                    myThread.Start();
                    break;

                case false:
                    myThread.Join();
                    myThread.Abort();
                    TCPClose();
                    myThread = null;
                    break;
            }
        }
    }

    private void FixedUpdate()
    {
        outJointByte = kinematic.InpJoint.ToByteSteam();
        //SendRequest(outJointByte, "J", stream);

        //if (contextMessage == ContextMessage.ControllerMaster)  // если пришло сообщение с контроллера
        //{
        //    MessageManager(inpByteBuff); // отправляем пришедшее сообщение в менеджер
        //    contextMessage = ContextMessage.PCMaster; // переключаемся на отправку с ПК
        //}
    }

    public static void Count()
    {
        //do
        //{
            
        //    lock (locker)
        //    {
        //        switch (typeMessage)
        //        {
        //            case TypeMessage.JOINT:
        //                SendRequest(outJointByte, "J", stream);
        //                //Debug.Log("vhod: " + BitConverter.ToString(outJointByte, 0));
        //                break;
        //            case TypeMessage.COMMAND:
        //                break;
        //            case TypeMessage.SERVICE:
        //                break;
        //            case TypeMessage.SPEED:
        //                break;
        //        }

        //        inpByteBuff = WaiteRequest();
        //        if (inpByteBuff != null)
        //            contextMessage = ContextMessage.ControllerMaster;
        //    }
        //    print("Thread works");
        //    //Thread.Sleep(10);
            
        //} while (isEnable);
    }

    public void MessageManager(byte[] data)
    {
        //try
        //{
        //    switch ((char)data[data.Length - 1])
        //    {
        //        case 'J':
        //            kinematic.InpJoint.IntoByteSteam(data);
        //            break;
        //        default:
        //            Debug.Log("Неизвестное сообщение: " + System.Text.Encoding.UTF8.GetString(data, 0, data.Length));
        //            break;
        //    }
        //}
        //catch(NullReferenceException)
        //{
        //    print("Нет сообщения");
        //}
        
    }

}



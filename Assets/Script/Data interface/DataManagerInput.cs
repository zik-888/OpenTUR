using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.Events;

public class DataManagerInput : MonoBehaviour {

    [SerializeField] protected TCPIP protocol;
    [SerializeField] protected Graf graf;
    protected byte[] inpByteBuff;

    //---------структуры-хранения----------
    protected Joint6 inpJointBuff;
    protected float speed;
    //-------------------------------------
    [Serializable]
    public class TerminalEvent : UnityEvent<string> { }
    public TerminalEvent terminalEvent;

    private void Start()
    {
        StartCoroutine(ListenStream());
    }

    protected IEnumerator ListenStream()
    {
        while (true)
        {
            yield return null;

            inpByteBuff = protocol.WaitRequest();

            if (inpByteBuff != null)
            {
                TerminalMethod(BitConverter.ToString(inpByteBuff));
                //Debug.Log(BitConverter.ToInt32(inpByteBuff, 0));
                MessageManager(inpByteBuff);
            }
                
        }
    }

    protected void MessageManager(byte[] data)
    {
        try
        {
            

            print((char)data[data.Length - 1]);
            switch ((char)data[data.Length - 1]) 
            {   
                case 'J':
                    //kinematic.InpJoint.IntoByteSteam(data);
                    byte[] sortData = Unpack(data);
                    print(sortData.Length);
                    if (sortData.Length == 12)
                    {
                        Joint6 test = new Joint6();
                        test.IntoByteSteam(sortData);
                        print(test.ToString());
                        graf.jointGraf.SetJoint6(test.J1 + 180, test.J2 + 180, test.J3 + 180, test.J4 + 180, test.J5 + 180, test.J6 + 180);
                        //float[] s = Joint6.IntoByteSteamStatic(sortData);

                    }
                    else
                    {
                        Debug.Log("Сообщение не полное");
                        TerminalMethod("Сообщение не полное");
                    }
                     

                    break;
                case 'S':

                    break;
                
                default:
                    Debug.Log("Неизвестное сообщение: " + System.Text.Encoding.UTF8.GetString(data, 0, data.Length));
                    TerminalMethod("Неизвестное сообщение");
                    break;
            }
        }
        catch (NullReferenceException)
        {
            print("Нет сообщения");
        }
        catch (IndexOutOfRangeException)
        {

        }

    }

    protected byte[] Unpack(byte[] data)
    {
        byte[] buffByte = null;
        byte[] rawData = null;
        
        int indexLastSOH = Array.LastIndexOf(data, (byte)'Z');

        //print(indexLastSOH);

        if(indexLastSOH != -1)
        {
            rawData = new byte[indexLastSOH];
            buffByte = new byte[data.Length - 2 - indexLastSOH];

            for (int i = 0; data.Length - 1 > i; i++)
            {
                if (i < indexLastSOH)
                {
                    rawData[i] = data[i];
                }

                if (i > indexLastSOH)
                {
                    buffByte[i - indexLastSOH - 1] = data[i];
                }
            }
            
            if(rawData != null && indexLastSOH != 0)
                MessageManager(rawData); // рекурсия. разбираем пакет, пока не вычленим всю информацию
        }
        else
        {
            Debug.Log("Неизвестное сообщение");
        }

        return buffByte;
    }

    public void TerminalMethod(string value)
    {
        terminalEvent.Invoke(value);
    }
}

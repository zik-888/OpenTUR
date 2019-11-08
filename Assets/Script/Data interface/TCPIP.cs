using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System;
using System.IO;
using UnityEngine;

public enum TypeMessage { JOINT, COMMAND, SPEED, SERVICE };
public enum ContextMessage { ControllerMaster, PCMaster };

public class TCPIP: MonoBehaviour
{

    public int port = 1024;
    public string server = "192.168.4.1";
    //public string server = "192.168.1.3";
    //public string server = "10.193.17.103";
    public TcpClient tcpClient = null;
    public NetworkStream stream = null;

    public string Port { set { port = int.Parse(value);  } get { return port.ToString(); } }
    public string IPServerName { set { server = value; } get { return server; } }
    
    // подключение интерфеса
    public bool TCPStart()  
    {
        try
        {
            tcpClient = new TcpClient();
            tcpClient.Connect(server, port);
            stream = tcpClient.GetStream();
            tcpClient.ReceiveTimeout = 1;
            stream.ReadTimeout = 1;
            return true;
        }
        catch
        {
            Debug.Log("Fail connect");
            return false;
        }
    }

    // закрытие интерфеса
    public void TCPClose()
    {
        tcpClient.Close();
        tcpClient = null;
        stream.Close();
    }

    // отправка сообщения с тегом
    public void SendRequest(byte[] data, string tag)
    {
        byte[] tagByte = System.Text.Encoding.UTF8.GetBytes(tag);
        byte[] sohByte = System.Text.Encoding.UTF8.GetBytes("Z");
        byte[] fullData = new byte[data.Length + tagByte.Length + sohByte.Length];

        for (int i = 0; i < fullData.Length; i++)
        {
            if (i < sohByte.Length)
            {
                fullData[i] = sohByte[i];
            }
            else if (i < data.Length + sohByte.Length)
            {
                fullData[i] = data[i - sohByte.Length];
            }
            else
            {
                fullData[i] = tagByte[i - data.Length - sohByte.Length];
            }
        }
        
        stream.Write(fullData, 0, fullData.Length);
    }

    // приём сообщения
    public byte[] WaitRequest(/*object conteiner*/)
    {
        byte[] data = null;

        try
        {
            byte[] buff = new byte[256];

            do
            {
                int bytes = stream.Read(buff, 0, buff.Length);

                data = new byte[bytes];

                for(int i = 0; i < bytes; i++)
                {
                    data[i] = buff[i];
                }
                
            }
            while (stream.DataAvailable); // пока данные есть в потоке

            //Debug.Log(Encoding.UTF8.GetString(data, 0, data.Length));

            // Закрываем потоки
            
        }
        catch (SocketException e)
        {
            Debug.Log("SocketException: {0} " + e);
        }
        catch (Exception)
        {
            //Debug.Log("Exception: {0} " + e.Message);
        }

        return data;
    }


    public byte[] Serialization(ushort[] jointArray)
    {

        byte[] fullByte = new byte[jointArray.Length * 2];

        for (int i = 0; i < jointArray.Length; i++)
        {
            byte[] pieceByte = BitConverter.GetBytes((ushort)jointArray[i]);
            pieceByte.CopyTo(fullByte, 2 * i);
        }

        return fullByte;
    }
}


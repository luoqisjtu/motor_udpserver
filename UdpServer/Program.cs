using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Diagnostics;
using System.IO;

using AnalogIO;
using MccDaq;
using ErrorDefs;

using System.IO.Ports;
using System.Linq;
using System.Text.RegularExpressions;

namespace UDP
{
    class Program
    {

        //static MccDaq.MccBoard DaqBoard = new MccDaq.MccBoard(0);

        //private int str_temp;
        //private static MccDaq.Range Range;        //定义A/D和D/A转换范围

        //const int NumPoints = 5000;     //  Number of data points to collect
        //const int ArraySize = 5000;       //  size of data array
        //private ushort[] DataBuffer;    //  declare data array
        //string FileName;                //  name of file in which data will be stored

        //AnalogIO.clsAnalogIO AIOProps = new AnalogIO.clsAnalogIO();

        //int Count = NumPoints;
        ////  it may be necessary to add path to file name for data file to be found
        //int Rate = 1000;
        //int LowChan = 0;
        //int HighChan = 0;
        //MccDaq.ScanOptions Options = MccDaq.ScanOptions.Default;

        //private int NumAIChans;
        //private int ADResolution;


        private static SerialPort sp1 = new SerialPort();    //CCH 


        static void Main(string[] args)
        {
            //int recv;
            //byte[] data = new byte[1024];

            ////得到本机IP，设置TCP端口号         
            //IPEndPoint ip = new IPEndPoint(IPAddress.Any, 8001);
            //Socket newsock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            ////绑定网络地址
            //newsock.Bind(ip);

            //Console.WriteLine("This is a Server, host name is {0}", Dns.GetHostName());

            ////等待客户机连接
            //Console.WriteLine("Waiting for a client");

            ////得到客户机IP
            //IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            //EndPoint Remote = (EndPoint)(sender);
            //recv = newsock.ReceiveFrom(data, ref Remote);
            //Console.WriteLine("Message received from {0}: ", Remote.ToString());
            //Console.WriteLine(Encoding.ASCII.GetString(data, 0, recv));

            ////客户机连接成功后，发送信息
            //string welcome = "你好 ! ";

            ////字符串与字节数组相互转换
            //data = Encoding.ASCII.GetBytes(welcome);

            ////发送信息
            //newsock.SendTo(data, data.Length, SocketFlags.None, Remote);

            //float EngUnits;
            //double HighResEngUnits;
            //MccDaq.ErrorInfo ULStat;//存储和报告错误代码和消息
            //System.UInt16 DataValue;
            //System.UInt32 DataValue32;
            //int Chan = 0;     //输入通道编号
            //int Options = 0;

            //textBox1.Text += String.Format("{0}{1}", BitConverter.ToString(byteBuffer), Environment.NewLine);//显示串口读取原始数据 


            sp1 = new SerialPort("COM5", 115200, Parity.None, 8, StopBits.One);
            sp1.Open();

            int byteBufferLength = 13;
            byte[] byteBuffer = new byte[byteBufferLength];

            byte ID = 0x01;
            byte Instuction = 0x03;
            byte MemAddr = 0x2A;
            int position = 2000;
            byte Position_L = (byte)(position & 0xFF);
            byte Position_H = (byte) (position >> 8);
            byte time_L = 0x00;
            byte time_H = 0x00;
            int speed= 500;
            byte speed_L = (byte)(speed & 0xFF);
            byte speed_H = (byte)(speed >> 8);
            byte nLen = 0x07;

            byte msgLen = (byte)(nLen + 0x02);
            byte CheckSum = (byte) ~ (ID + msgLen + Instuction + MemAddr + Position_L + Position_H + time_L + time_H + speed_L + speed_H);

            byteBuffer[0] = 0xFF;
            byteBuffer[1] = 0xFF;
            byteBuffer[2] = ID;
            byteBuffer[3] = msgLen;
            byteBuffer[4] = Instuction;
            byteBuffer[5] = MemAddr;
            byteBuffer[6] = Position_L; 
            byteBuffer[7] = Position_H;
            byteBuffer[8] = time_L;
            byteBuffer[9] = time_H;
            byteBuffer[10] = speed_L;
            byteBuffer[11] = speed_H;
            byteBuffer[12] = CheckSum;
         
            while (true)
            {
                //Send something to Motor

                byte[] byteRead = new byte[sp1.BytesToRead];    //BytesToRead:sp1接收的字符个数

                Byte[] receivedData = new Byte[sp1.BytesToRead];

                sp1.Write(byteBuffer, 0, byteBuffer.Length);

            }
        }

    }
}
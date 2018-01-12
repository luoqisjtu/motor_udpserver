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

using System.IO.Ports;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;


namespace UDP
{
    class Program
    {

        private static SerialPort sp1 = new SerialPort();    //CCH 

        static void Main(string[] args)
        {

            //textBox1.Text += String.Format("{0}{1}", BitConverter.ToString(byteBuffer), Environment.NewLine);//显示串口读取原始数据 


            sp1 = new SerialPort("COM5", 115200, Parity.None, 8, StopBits.One);

            //sp1.DataReceived += new SerialDataReceivedEventHandler(sp1_DataReceived);
            sp1.Open();

            int byteBufferLength = 13;
            byte[] byteBuffer = new byte[byteBufferLength];

            int configReadLength = 8;
            byte[] configRead = new byte[configReadLength];

            /* int m = sp1.BytesToRead;  */                           //BytesToRead:sp1接收的字符个数
                                                                      //byte[] receivedData = new byte[sp1.BytesToRead];
            int receivedDataLength = 8;
            byte[] receivedData = new byte[receivedDataLength];//声明一个临时数组存储当前来的串口数据   /创建接收字节数组

            ////先转到位置为0处
            //byteBuffer[0] = 0xFF;
            //byteBuffer[1] = 0xFF;
            //byteBuffer[2] = 0x01;
            //byteBuffer[3] = 0x09;
            //byteBuffer[4] = 0x03;
            //byteBuffer[5] = 0x2A;
            //byteBuffer[6] = 0x00;
            //byteBuffer[7] = 0x00;
            //byteBuffer[8] = 0x00;
            //byteBuffer[9] = 0x00;
            //byteBuffer[10] = 0xE8;
            //byteBuffer[11] = 0x03;
            //byteBuffer[12] = 0xDD;
            //sp1.Write(byteBuffer, 0, byteBuffer.Length);
            //System.Threading.Thread.Sleep(6000);
           
            int n = 0;
           
            string filename = "test.csv";
            File.WriteAllText(filename, "");

            while (true)
                {
                    //Send something to Motor

                    double positionangle = Math.Sin(n * Math.PI / 180) * 2047;
                    n++;     //n = n + 1; 
                    int position = (int)positionangle;
                    //position = Math.Abs(position);

                    byte ID = 0x01;
                    byte Instuction = 0x03;
                    byte MemAddr = 0x2A;
                    //int position = 1000;
                    byte position_L = (byte)(position & 0xFF);
                    byte position_H = (byte)(position >> 8);
                    byte time_L = 0x00;
                    byte time_H = 0x00;
                    int speed =1500;
                    byte speed_L = (byte)(speed & 0xFF);
                    byte speed_H = (byte)(speed >> 8);
                    byte nLen = 0x07;

                    byte msgLen = (byte)(nLen + 0x02); 
                    byte CheckSum = (byte)~(ID + msgLen + Instuction + MemAddr + position_L + position_H + time_L + time_H + speed_L + speed_H);

                    byteBuffer[0] = 0xFF;
                    byteBuffer[1] = 0xFF;
                    byteBuffer[2] = ID;
                    byteBuffer[3] = msgLen;
                    byteBuffer[4] = Instuction;
                    byteBuffer[5] = MemAddr;
                    byteBuffer[6] = position_L;
                    byteBuffer[7] = position_H;
                    byteBuffer[8] = time_L;
                    byteBuffer[9] = time_H;
                    byteBuffer[10] = speed_L;
                    byteBuffer[11] = speed_H;
                    byteBuffer[12] = CheckSum;

                    sp1.Write(byteBuffer, 0, byteBuffer.Length);
                    System.Threading.Thread.Sleep(5);

                    configRead[0] = 0xFF;
                    configRead[1] = 0xFF;
                    configRead[2] = 0x01;
                    configRead[3] = 0x04;
                    configRead[4] = 0x02;
                    configRead[5] = 0x38;
                    configRead[6] = 0x02;
                    configRead[7] = 0xBE;

                    //sp1.BaseStream.Write(configRead, 0, configRead.Length);
                    sp1.Write(configRead, 0, configRead.Length);
                    System.Threading.Thread.Sleep(5);

                //Console.WriteLine("Target position:" + position);
              
                sp1.Read(receivedData, 0, receivedDataLength);
                //sp1.basestream.readasync(receiveDdata, 0, 8);

                ushort Currentpos = 0;   //一个16位整形变量，初值为 0000 0000 0000 0000
                //byte b1 = receivedData[6];   //一个byte的变量，作为转换后的高8位
                //byte b2 = receivedData[5];   //一个byte的变量，作为转换后的低8位
                //Currentpos = (short)(Currentpos ^ b1);  //将b1赋给Currentpos的低8位
                //Currentpos = (short)(Currentpos << 8);  //Currentpos的低8位移动到高8位
                //Currentpos = (short)(Currentpos ^ b2); //在b2赋给Currentpos的低8位      

                Currentpos = (ushort)((receivedData[6] << 8) + receivedData[5]);

                if (receivedData[2] == 0x01 && receivedData[3] == 0x04)
                {

                    //Console.WriteLine("Current pos:" + Currentpos); //以十进制输出Currentpos          

                    string s = Currentpos.ToString();
                    s += "\n";
                    File.AppendAllText(filename, s);
              
                }

                //else
                //{

                //    Console.WriteLine("0");

                //}

                //if (n == 1000)
                //    break;

            }
        }
                
      }
 }





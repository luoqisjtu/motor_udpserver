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

            //int modelWriteLength = 8;
            //byte[] modelWrite = new byte[modelWriteLength];

            int byteBufferLength = 13;
            byte[] byteBuffer = new byte[byteBufferLength];

            int configReadLength = 8;
            byte[] configRead = new byte[configReadLength];

            /* int m = sp1.BytesToRead;  */                           //BytesToRead:sp1接收的字符个数
                                                                      //byte[] receivedData = new byte[sp1.BytesToRead];
            int receivedDataLength = 8;
            byte[] receivedData = new byte[receivedDataLength];//声明一个临时数组存储当前来的串口数据   /创建接收字节数组


            //int n = 0;

            string filename = "test.csv";
            File.WriteAllText(filename, "");

            while (true)
                {
                //Send something to Motor

                //modelWrite[0] = 0xFF;      //contorl model
                //modelWrite[1] = 0xFF;
                //modelWrite[2] = 0x01;
                //modelWrite[3] = 0x04;
                //modelWrite[4] = 0x03;
                //modelWrite[5] = 0x23;
                //modelWrite[6] = 0x01;
                //modelWrite[7] = 0xD3;
                //sp1.Write(modelWrite, 0, modelWrite.Length);


                //double positionangle = Math.Sin(n * Math.PI / 180) * 2047;      //Write position —— position model 
                //n++;     //n = n + 1; 
                //int position = (int)positionangle;
                ////position = Math.Abs(position);       

                //byte ID = 0x01;
                //byte Instuction = 0x03;
                //byte MemAddr = 0x2A;
                ////int position = 4095;
                //byte position_L = (byte)(position & 0xFF);
                //byte position_H = (byte)(position >> 8);
                //byte time_L = 0x00;
                //byte time_H = 0x00;
                //int speed = 1500;
                //byte speed_L = (byte)(speed & 0xFF);
                //byte speed_H = (byte)(speed >> 8);
                //byte nLen = 0x07;

                //byte msgLen = (byte)(nLen + 0x02);
                //byte CheckSum = (byte)~(ID + msgLen + Instuction + MemAddr + position_L + position_H + time_L + time_H + speed_L + speed_H);

                //byteBuffer[0] = 0xFF;           //Write position
                //byteBuffer[1] = 0xFF;
                //byteBuffer[2] = ID;
                //byteBuffer[3] = msgLen;
                //byteBuffer[4] = Instuction;
                //byteBuffer[5] = MemAddr;
                //byteBuffer[6] = position_L;
                //byteBuffer[7] = position_H;
                //byteBuffer[8] = time_L;
                //byteBuffer[9] = time_H;
                //byteBuffer[10] = speed_L;
                //byteBuffer[11] = speed_H;
                //byteBuffer[12] = CheckSum;


                byte ID = 0x01;                          //Write speed —— speed model 
                byte Instuction = 0x03;
                byte param = 0x2E;
                int speed = 0;
                byte speed_L = (byte)(speed & 0xFF);
                byte speed_H = (byte)(speed >> 8);
                byte nLen = 0x03;

                byte msgLen = (byte)(nLen + 0x02);
                byte CheckSum = (byte)~(ID + msgLen + Instuction + param + speed_L + speed_H);

                byteBuffer[0] = 0xFF;           //Write speed 
                byteBuffer[1] = 0xFF;
                byteBuffer[2] = ID;
                byteBuffer[3] = msgLen;
                byteBuffer[4] = Instuction;
                byteBuffer[5] = param;
                byteBuffer[6] = speed_L;
                byteBuffer[7] = speed_H;
                byteBuffer[8] = CheckSum;


                //byte ID = 0x01;                           //Write force —— force model
                //byte Instuction = 0x03;
                //byte param = 0x2C;
                //int  force = 0;
                //byte force_L = (byte)(force & 0xFF);
                //byte force_H = (byte)(force >> 8);
                //byte nLen = 0x03;

                //byte msgLen = (byte)(nLen + 0x02);
                //byte CheckSum = (byte)~(ID + msgLen + Instuction + param + force_L + force_H);

                //byteBuffer[0] = 0xFF;           //Write force  
                //byteBuffer[1] = 0xFF;
                //byteBuffer[2] = ID;
                //byteBuffer[3] = msgLen;
                //byteBuffer[4] = Instuction;
                //byteBuffer[5] = param;
                //byteBuffer[6] = force_L;
                //byteBuffer[7] = force_H;
                //byteBuffer[8] = CheckSum;


                //byteBuffer[0] = 0xFF;   //Write torque (5000)88  13  4B  (200)C8 00 1E   (1000)E8 03 FB   (100)64 00 82  (80)50 00 96
                //byteBuffer[1] = 0xFF;
                //byteBuffer[2] = 0x01;
                //byteBuffer[3] = 0x05;
                //byteBuffer[4] = 0x03;
                //byteBuffer[5] = 0x10;
                //byteBuffer[6] = 0xE8;
                //byteBuffer[7] = 0x03;
                //byteBuffer[8] = 0xFB;

                sp1.Write(byteBuffer, 0, byteBuffer.Length);
                    System.Threading.Thread.Sleep(5);
            

                //configRead[0] = 0xFF;      //read position
                //configRead[1] = 0xFF;
                //configRead[2] = 0x01;
                //configRead[3] = 0x04;              
                //configRead[4] = 0x02;
                //configRead[5] = 0x38;
                //configRead[6] = 0x02;
                //configRead[7] = 0xBE;

                configRead[0] = 0xFF;      //read current load
                configRead[1] = 0xFF;
                configRead[2] = 0x01;
                configRead[3] = 0x04;
                configRead[4] = 0x02;
                configRead[5] = 0x3C;
                configRead[6] = 0x02;
                configRead[7] = 0xBA;

                //configRead[0] = 0xFF;    //read output torque- It is determined by the torque of writing      
                //configRead[1] = 0xFF;
                //configRead[2] = 0x01;
                //configRead[3] = 0x04;
                //configRead[4] = 0x02;
                //configRead[5] = 0x10;
                //configRead[6] = 0x02;
                //configRead[7] = 0xE6;

                ////sp1.BaseStream.Write(configRead, 0, configRead.Length);
                sp1.Write(configRead, 0, configRead.Length);
                System.Threading.Thread.Sleep(5);

                //Console.WriteLine("Target position:" + position);

                sp1.Read(receivedData, 0, receivedDataLength);
                //sp1.basestream.readasync(receiveDdata, 0, 8);

                ushort Load = 0;   //一个16位整形变量，初值为 0000 0000 0000 0000
                //byte b1 = receivedData[6];   //一个byte的变量，作为转换后的高8位
                //byte b2 = receivedData[5];   //一个byte的变量，作为转换后的低8位
                //Load = (short)(Load ^ b1);  //将b1赋给Currentpos的低8位
                //Load = (short)(Load << 8);  //Currentpos的低8位移动到高8位
                //Load = (short)(Load ^ b2); //在b2赋给Currentpos的低8位      

                Load = (ushort)((receivedData[6] << 8) + receivedData[5]);

                if (receivedData[2] == 0x01 && receivedData[3] == 0x04)
                {

                    Console.WriteLine("Current load:" + Load); //以十进制输出Currentpos
                    string s = Load.ToString();
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

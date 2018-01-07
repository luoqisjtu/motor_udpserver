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

namespace UDP
{
    class Program
    {
              
        private static SerialPort sp1 = new SerialPort();    //CCH 


        static void Main(string[] args)
        {
        
            //textBox1.Text += String.Format("{0}{1}", BitConverter.ToString(byteBuffer), Environment.NewLine);//显示串口读取原始数据 


            sp1 = new SerialPort("COM5", 115200, Parity.None, 8, StopBits.One);
            sp1.Open();

            int byteBufferLength = 13;
            byte[] byteBuffer = new byte[byteBufferLength];

            
            int n = 0;
                        
            while (true)
            {
                //Send something to Motor

                byte[] byteRead = new byte[sp1.BytesToRead];    //BytesToRead:sp1接收的字符个数

                Byte[] receivedData = new Byte[sp1.BytesToRead];
                                              

                double positionangle = Math.Sin(n * Math.PI / 180) * 4095;
                n++;     //n = n + 1;     
                int position = (int)positionangle;
                position = Math.Abs(position);

                byte ID = 0x01;
                byte Instuction = 0x03;
                byte MemAddr = 0x2A;
                //int position = 1000;
                byte position_L = (byte)(position & 0xFF);
                byte position_H = (byte)(position >> 8);
                byte time_L = 0x00;
                byte time_H = 0x00;
                int speed = 1000;
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

                System.Threading.Thread.Sleep(20);

            if (n==500)

                    break;

            }
        }

    }
}
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
using System.Threading.Tasks;

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
                                                                      //int receivedDataLength = 8;
                                                                      //byte[] receivedData = new byte[receivedDataLength];//声明一个临时数组存储当前来的串口数据   /创建接收字节数组


            int n = 0;


            Task.Factory.StartNew(async () =>
            {
                while (true)
                {


                    configRead[0] = 0xFF;
                    configRead[1] = 0xFF;
                    configRead[2] = 0x01;
                    configRead[3] = 0x04;
                    configRead[4] = 0x02;
                    configRead[5] = 0x38;
                    configRead[6] = 0x02;
                    configRead[7] = 0xBE;
                    await sp1.BaseStream.FlushAsync();

                    await sp1.BaseStream.WriteAsync(configRead, 0, configRead.Length);
                    //sp1.read(receiveddata, 0, 8);         //读取数据


                    Byte[] receivedData = new Byte[8];        //创建接收字节数组

                    await sp1.BaseStream.ReadAsync(receivedData, 0, 8);
                    

                    Console.WriteLine("Current pos:" + receivedData[5]);

                    //if (n == 500)
                    //    break;
                    await Task.Delay(20);

                }

            });

            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    //Send something to Motor

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


                    Console.WriteLine("Target position:" + byteBuffer[6]);

                    await sp1.BaseStream.WriteAsync(byteBuffer, 0, byteBuffer.Length);

                    //System.Threading.Thread.Sleep(20);


                    //configRead[0] = 0xFF;
                    //configRead[1] = 0xFF;
                    //configRead[2] = 0x01;
                    //configRead[3] = 0x04;
                    //configRead[4] = 0x02;
                    //configRead[5] = 0x3C;
                    //configRead[6] = 0x02;
                    //configRead[7] = 0xBA;


                    await Task.Delay(10);

                }
            });
                
            string read;
            do
            {
                read = Console.ReadLine();
            } while (read != "q");




        }











        //static void sp1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        //{
        //    if (sp1.IsOpen)     //此处可能没有必要判断是否打开串口，但为了严谨性，我还是加上了
        //    {


        //        byte[] byteRead = new byte[sp1.BytesToRead];    //BytesToRead:sp1接收的字符个数

        //        {
        //            try
        //            {
        //                Byte[] receivedData = new Byte[sp1.BytesToRead];        //创建接收字节数组
        //                sp1.Read(receivedData, 0, receivedData.Length);         //读取数据
        //                //string text = sp1.Read();   //Encoding.ASCII.GetString(receivedData);
        //                //sp1.DiscardInBuffer();                                  //清空SerialPort控件的Buffer
        //                Console.WriteLine(receivedData[5]);

        //            }
        //            catch (System.Exception ex)
        //            {
        //                Console.WriteLine(ex.Message, "出错提示");
        //            }
        //        }
        //    }
        //    else
        //    {
        //        Console.WriteLine("请打开某个串口", "错误提示");
        //    }
        //}



    }
}










using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;
using System.Windows.Threading;
using System.Windows;

namespace LightText.Common
{
    public class CommSerialPort
    {
        private readonly SerialPort serialPort;

        public bool Opened;

        public CommSerialPort(string name,string config)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(config))
            {
                return;
            }

            try
            {
                string[] configs = config.ToUpper().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (configs != null & configs.Length == 4)
                {
                    Parity parity;
                    StopBits stopBits;

                    bool b = int.TryParse(configs[0], out int baudRate);

                    switch (configs[1])
                    {
                        case "E":
                        case "EVEN":
                            parity = Parity.Even;
                            break;
                        case "M":
                        case "MARK":
                            parity = Parity.Mark;
                            break;
                        case "N":
                        case "NONE":
                            parity = Parity.None;
                            break;
                        case "O":
                        case "ODD":
                            parity = Parity.Odd;
                            break;
                        case "S":
                        case "SPACE":
                            parity = Parity.Space;
                            break;
                        default:
                            parity = Parity.None;
                            break;
                    }

                    b &= int.TryParse(configs[2], out int dataBits);

                    switch (configs[3])
                    {
                        case "0":
                            stopBits = StopBits.None;
                            break;
                        case "1":
                            stopBits = StopBits.One;
                            break;
                        case "1.5":
                            stopBits = StopBits.OnePointFive;
                            break;
                        case "2":
                            stopBits = StopBits.Two;
                            break;
                        default:
                            stopBits = StopBits.None;
                            break;
                    }

                    if (b)
                    {
                        serialPort = new SerialPort(name, baudRate, parity, dataBits, stopBits);
                    }
                    else
                    {
                        serialPort = new SerialPort();
                        MessageBox.Show($"{name}串口设置异常");
                    }

                }
            }
            catch(Exception ex)
            {
                serialPort = new SerialPort();
                MessageBox.Show($"{name}串口设置异常");
            }
        }

        /// <summary>
        /// 打开串口
        /// </summary>
        /// <returns></returns>
        public bool Open()
        {
            try
            {
                if (serialPort.IsOpen)
                {
                    serialPort.Close();
                }
                serialPort.Open();
                Opened = serialPort.IsOpen;
                return Opened;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 关闭窜口
        /// </summary>
        public void Close()
        {
            try
            {
                if (serialPort.IsOpen)
                {
                    serialPort.Close();
                }
            }
            catch(Exception ex)
            {

            }
        }

        public bool SendAndFeedback(byte[]cmd,ref byte[] feedback,int resLeng=7,int ResponseReadTimeOut=500)
        {
            if (!Opened || cmd == null || cmd.Length == 0)
            {
                return false;
            }

            serialPort.DiscardInBuffer();
            serialPort.DiscardOutBuffer();

            try
            {
                serialPort.Write(cmd, 0, cmd.Length);
            }
            catch(Exception ex)
            {
                return false;
            }


            int leng = 0;
            DateTime dt = DateTime.Now;
            do
            {
                leng = serialPort.BytesToRead;
                if (leng < resLeng)
                {
                    Thread.Sleep(20);
                    continue;
                }
            } while (leng < resLeng && (DateTime.Now - dt).TotalMilliseconds < ResponseReadTimeOut);

            if (leng < resLeng)
            {
                return false;
            }

            byte[] readbuffer = new byte[leng];
            serialPort.Read(readbuffer, 0, leng);
            feedback = readbuffer;
            return true;
        }


    }
}

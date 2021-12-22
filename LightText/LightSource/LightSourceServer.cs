using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using LightText.Common;
using System.Windows;
using System.Text.RegularExpressions;
using System.Threading;

namespace LightText.LightSource
{
    public class LightSourceServer : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        private object objLock = new object();

        public CommSerialPort SerialPort { get; set; }

        private string name { get; set; }

        private string config { get; set; }

        public bool BConnected { get; set; }

        /// <summary>
        /// 通道1亮度值
        /// </summary>
        private int lightChannelValue1;
        public int LightChannelValue1
        {
            get
            {
                return lightChannelValue1;
            }
            set
            {
                if (value >= 0 && value <= 255)
                {
                    lightChannelValue1 = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LightChannelValue1"));

                }
            }
        }

        private int lightChannelValue2;
        /// <summary>
        /// 通道2亮度值
        /// </summary>
        public int LightChannelValue2
        {
            get { return lightChannelValue2; }
            set
            {
                if (value >= 0 && value <= 255)
                {
                    lightChannelValue2 = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LightChannelValue2"));
                }
            }
        }

        private int lightChannelValue3;
        /// <summary>
        /// 通道3亮度值
        /// </summary>
        public int LightChannelValue3
        {
            get { return lightChannelValue3; }
            set
            {
                if (value >= 0 && value <= 255)
                {
                    lightChannelValue3 = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LightChannelValue3"));
                }
            }
        }

        private int lightChannelValue4;
        /// <summary>
        /// 通道4亮度值
        /// </summary>
        public int LightChannelValue4
        {
            get { return lightChannelValue4; }
            set
            {
                if (value >= 0 && value <= 255)
                {
                    lightChannelValue4 = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LightChannelValue4"));
                }
            }
        }

        public LightSourceServer(string name,string config)
        {

            if (name == null || config == null)
            {
                return;
            }
            this.name = name;
            this.config = config;

            SerialPort = new CommSerialPort(name, config);

            Init();
        }

        public void Init()
        {
            if (SerialPort.Open())
            {
                BConnected = true;
                MessageBox.Show($"初始化光源控制器{name}成功！");
            }
            else
            {
                BConnected = false;
                MessageBox.Show($"初始化光源控制器{name}失败！");
            }
        }

        

        /// <summary>
        /// 获取光源亮度
        /// </summary>
        /// <returns></returns>
        public bool GetLightSource()
        {
            bool bOk = false;
            byte[] feedback = null;
            string str = $"SA#"+$"SB#"+$"SC#"+$"SD#";
            Regex Re = new Regex("^[a-zA-Z]{1}[a-zA-Z0-9]{4}[a-zA-Z]{1}[a-zA-Z0-9]{4}[a-zA-Z]{1}[a-zA-Z0-9]{4}[a-zA-Z]{1}[a-zA-Z0-9]{4}", RegexOptions.None);

            try
            {
                bool b = true;
                do
                {

                    if (SerialPort.SendAndFeedback(Encoding.ASCII.GetBytes(str), ref feedback, 4 * 5, 3000))
                    {
                        string strRes = Encoding.ASCII.GetString(feedback);

                        if (Re.IsMatch(strRes))
                        {
                            if (strRes.Length == 4 * 5)
                            {
                                LightChannelValue1 = System.Convert.ToInt32(strRes.Substring(1, 4));
                                LightChannelValue2 = System.Convert.ToInt32(strRes.Substring(6, 4));
                                LightChannelValue3 = System.Convert.ToInt32(strRes.Substring(11, 4));
                                LightChannelValue4 = System.Convert.ToInt32(strRes.Substring(16, 4));

                                MessageBox.Show($"获取光源多通道亮度值成功！端口{name}！通道1：{LightChannelValue1} 通道2：{LightChannelValue2} 通道3：{LightChannelValue3} 通道4：{LightChannelValue4}");
                                bOk = true;
                                b = false;
                            }
                            else
                            {
                                MessageBox.Show($"获取光源多通道亮度值失败！端口{name}返回值{strRes}！");
                            }
                        }
                        else
                        {
                            MessageBox.Show($"获取光源多通道亮度值失败！端口{name}返回值{strRes}！");
                        }
                    }
                    else
                    {
                        MessageBox.Show($"获取光源亮度值失败！端口号{name}！");
                        break;
                    }
                    Thread.Sleep(400);
                } while (b);
            }
            catch(Exception ex)
            {
                bOk = false;
                MessageBox.Show($"获取光源亮度值失败！端口号{name}！{ex.Message}");
                return bOk;
            }
            return bOk;
        }

        /// <summary>
        /// 设置光源亮度
        /// </summary>
        /// <returns></returns>
        public bool SetLightSource()
        {
            lock (objLock)
            {
                bool bOK = false;
                byte[] feedback = null;
                string str = $"SA{LightChannelValue1.ToString("D4")}#" + $"SB{LightChannelValue2.ToString("D4")}#" 
                            + $"SC{LightChannelValue3.ToString("D4")}#" + $"SD{LightChannelValue4.ToString("D4")}#";
                try
                {
                    if(SerialPort.SendAndFeedback(Encoding.ASCII.GetBytes(str),ref feedback, 4 * 1, 3000))
                    {
                        string strRes = Encoding.ASCII.GetString(feedback);

                        if (strRes.ToUpper() == "ABCD")
                        {
                            bOK = true;
                            MessageBox.Show($"设置光源多通道亮度值成功！端口{name}！");
                        }
                        else
                        {
                            bOK = false;
                            GetLightSource();
                            MessageBox.Show($"设置光源多通道亮度值失败！端口{name}！");
                        }
                    }
                    else
                    {
                        MessageBox.Show($"设置光源亮度值失败！端口号{name}！");
                    }
                }
                catch(Exception ex)
                {
                    bOK = false;
                    MessageBox.Show($"设置光源亮度值失败！端口号{name}！{ex.Message}");
                }
                return bOK;
            }
        }

        /// <summary>
        /// 设置多通道亮度
        /// </summary>
        public bool SetMultChannelValue()
        {
            try
            {
                if (BConnected)
                {
                    Task.Run(() => SetLightSource());
                }
                else
                {
                    if (SerialPort.Open())
                    {
                        Task.Run(() => SetLightSource());
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LightText.Common;
using LightText.LightSource;
using LightText.Setting;

namespace LightText
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public SettingPage Setting { get; set; }

        [Description("夹具光源串口名")]
        public string LightSourceFixtureSerialPortName { get; set; } = "com1";

        [Description("夹具光源串口配置")]
        public string LightSourceFixtureSerialPortConfig { get; set; } = "9600,N,8,1";

        [Description("检测光源串口名")]
        public string LightSourceDetectSerialPortName { get; set; } = "com2";

        [Description("检测光源串口配置")]
        public string LightSourceDetectSerialPortConfig { get; set; } = "9600,N,8,1";

        [Description("贴膜光源串口名")]
        public string LightSourceStickingFilmSerialPortName { get; set; } = "com3";

        [Description("贴膜光源串口配置")]
        public string LightSourceStickingFilmSerialPortConfig { get; set; } = "9600,N,8,1";

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            Setting = new SettingPage();
            
            

            Setting.CSLightSourceDetect = new LightSourceServer(LightSourceDetectSerialPortName, LightSourceDetectSerialPortConfig);

            Setting.CSLightSourceFixture = new LightSourceServer(LightSourceFixtureSerialPortName, LightSourceFixtureSerialPortConfig);

            Setting.CSLightSourceStickingFilm = new LightSourceServer(LightSourceStickingFilmSerialPortName, LightSourceStickingFilmSerialPortConfig);

            Init();
        }
        
        public void Init()
        {
            //    Setting.CSLightSourceDetect.GetLightSource();
            //    Setting.CSLightSourceFixture.GetLightSource();
            //    Setting.CSLightSourceStickingFilm.GetLightSource();
            //    Task.Run(() =>SetLightSourceValues());
        }

        private void SetLightSourceValues()
        {
            Task.Run(() =>
            {
                if (Setting.CSLightSourceFixture != null)
                {
                    Setting.CSLightSourceFixture.SetMultChannelValue();
                }
            });

            Task.Run(() =>
            {
                if (Setting.CSLightSourceDetect != null)
                {
                    Setting.CSLightSourceDetect.SetMultChannelValue();
                }
            });

            Task.Run(() =>
            {
                if (Setting.CSLightSourceStickingFilm != null)
                {
                    Setting.CSLightSourceStickingFilm.SetMultChannelValue();
                }
            });
        }




        private void btnSetLightSourceForCS_Click(object sender, RoutedEventArgs e)
        {
            bool b = Setting.CSLightSourceFixture.SetMultChannelValue();
            b &= Setting.CSLightSourceDetect.SetMultChannelValue();
            b &= Setting.CSLightSourceStickingFilm.SetMultChannelValue();

            if (!b)
            {
                MessageBox.Show($"应用到光源失败！");
            }
        }
    }
}

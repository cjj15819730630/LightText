using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightText.Common;
using LightText.LightSource;

namespace LightText.Setting
{
    public class SettingPage : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region 光源控制串口
        


        public LightSourceServer CSLightSourceFixture { get; set; }

        public LightSourceServer CSLightSourceDetect { get; set; }

        public LightSourceServer CSLightSourceStickingFilm { get; set; }

        #endregion

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightText.Common;
using LightText.LightSource;
using LightText.Setting;

namespace LightText
{
    public class GlobalParam
    {
        public SettingPage Setting { get; set; }

        public static GlobalParam globalParam;
        private GlobalParam()
        {

        }

        public static GlobalParam GetInstance()
        {
            if (globalParam == null)
            {
                globalParam = new GlobalParam();
            }
            return globalParam;
        }

        /// <summary>
        /// 全局参数实例
        /// </summary>
        public static GlobalParam Instance
        {
            get
            {
                if (globalParam == null)
                {
                    globalParam = new GlobalParam();
                }
                return globalParam;
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using CrayfishMonitor_Desktop.Services;

namespace CrayfishMonitor_Desktop.Services
{
    public static class SettingsService
    {
        public static void ArduinoSettings()
        {
            try
            {
                var settings = ResourceLoader.GetForCurrentView("ArduinoSettings");
                var tag = settings.GetString("BaudRate");
                Debug.WriteLine(tag);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

        }
    }
}

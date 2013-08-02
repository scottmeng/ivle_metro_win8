using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.ApplicationSettings;

namespace icreate_test2.Utils
{
    class SettingsHelper
    {
        public static void AddSettingsCommands(SettingsPaneCommandsRequestedEventArgs args)
        {
            args.Request.ApplicationCommands.Clear();

            SettingsCommand privacyPref = new SettingsCommand("privacyPref", "Privacy Policy", (uiCommand) =>
            {
                Windows.System.Launcher.LaunchUriAsync(new Uri("http://kaizhimeng.wordpress.com/2013/08/02/metro-ivle-privacy-policy/"));
            });

            args.Request.ApplicationCommands.Add(privacyPref);
        }
    }
}

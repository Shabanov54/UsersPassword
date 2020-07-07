using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Users_Password
{
    public class FileContext
    {
        string settingfile = "setting.xml";

        public FileContext()
        {
            if (!File.Exists(settingfile))
            {
                var settings = new Server();

                SaveSettings(settings);
            }
        }

        private void SaveSettings(Server settings)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Server));
            using (FileStream file = new FileStream(settingfile, FileMode.OpenOrCreate))
            {
                xmlSerializer.Serialize(file, settings);
            }
        }

        internal Server GetSettings()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Server));
            Server settings;
            using (FileStream fileStream = new FileStream(settingfile, FileMode.Open))
            {
                settings = (Server)xmlSerializer.Deserialize(fileStream);
            }
            return settings;
        }

    }
}


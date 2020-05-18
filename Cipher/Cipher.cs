using System;
using Plugin;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ZipPlugin
{
    public class Cipher : IPlugin
    {
        public string  name { get; } = "Кодирование base64";
        public string extension = ".bs";
        public void PLoad(ref string path)
        {
            if (path.Contains(".dat"))
            {
                string input = File.ReadAllText(path);
                var enTextBytes = Convert.FromBase64String(input);
                path = path.Replace(extension, "");
                File.WriteAllBytes(path, enTextBytes);
            }
            else
            {
                string input = File.ReadAllText(path);
                var enTextBytes = Convert.FromBase64String(input);
                string deText = Encoding.UTF8.GetString(enTextBytes);
                path = path.Replace(extension, "");
                File.WriteAllText(path, deText);
            }
        }

        public void PSave(string path)
        {
            if (path.Contains(".dat"))
            {
                var fContent = File.ReadAllBytes(path);
                string enText = Convert.ToBase64String(fContent);
                File.WriteAllText(path + extension, enText);
            }
            else
            {
                string fContent = File.ReadAllText(path);
                var simpleTextBytes = Encoding.UTF8.GetBytes(fContent);
                string enText = Convert.ToBase64String(simpleTextBytes);
                File.WriteAllText(path + extension, enText);  
            }
            File.Delete(path);
        }

        public string getExtension()
        {
            return extension;
        }
    }
}

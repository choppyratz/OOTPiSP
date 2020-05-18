using System;
using System.Collections.Generic;
using System.Text;

namespace Plugin
{
    public interface IPlugin
    {
        string name { get; }
        void PSave(string path);
        void PLoad(ref string path);

        string getExtension();
    }
}
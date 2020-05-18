using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;
using System.Reflection;

namespace WindowsFormsApp1
{
    public class TextSerializer : ISerialize
    {
        public void serialize(string fileName, List<Car> list)
        {
            string output = "";
            foreach (Car p in list)
            {
                output += "type: " + p.GetType() + ", ";
                foreach (FieldInfo fi in p.GetType().GetFields())
                {
                    if (fi.FieldType == typeof(System.String))
                    {
                        output += fi.Name + ": " + "'" + fi.GetValue(p) + "'" + ", ";
                    }
                    else if ((fi.FieldType != typeof(int)) && (!fi.FieldType.IsEnum) && (fi.FieldType != typeof(string)))
                    {
                        output +=  fi.Name + ": {";
                        output += "type: " + fi.FieldType.ToString() + ", ";
                        foreach (FieldInfo fi2 in fi.FieldType.GetFields())
                        {
                            output += fi2.Name + ": " + fi2.GetValue(fi.GetValue(p)) + ", ";
                        }
                        output = output.Remove(output.Length - 2);
                        output += "}";
                    }
                    else
                    {
                        output += fi.Name + ": " + fi.GetValue(p) + ", ";
                    }
                }
                output += ";\r\n";
            }
            File.WriteAllText(fileName, output);
        }

        public List<Car> deserialize(string fileName)
        {
            List<Car> listC = new List<Car> { };
            using (StreamReader sr = new StreamReader(fileName, System.Text.Encoding.Default))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    ArrayList list = ParseObj(line);
                    Type key = Type.GetType(list[0].ToString());
                    object obj = Activator.CreateInstance(key);
                    int i = 1;
                    foreach (FieldInfo fi in key.GetFields())
                    {
                        if ((fi.FieldType != typeof(int)) && (!fi.FieldType.IsEnum) && (fi.FieldType != typeof(string)))
                        {
                            Type key2 = Type.GetType(((ArrayList)list[i])[0].ToString());
                            object obj2 = Activator.CreateInstance(key2);
                            int j = 1;
                            foreach (FieldInfo fi2 in fi.FieldType.GetFields())
                            {
                                var convert = TypeDescriptor.GetConverter(fi2.FieldType);
                                fi2.SetValue(obj2, convert.ConvertFrom(((ArrayList)list[i])[j]));
                                j++;
                            }
                            //((Car)obj).engine = (Engine)obj2;
                            fi.SetValue(obj, obj2);
                        }
                        else
                        {
                            list[i] = list[i].ToString().Replace("'", "");
                            var convert = TypeDescriptor.GetConverter(fi.FieldType);
                            fi.SetValue(obj, convert.ConvertFrom(list[i]));
                            i++;
                        }
                    }
                    listC.Add((Car)obj);
                }
            }
            return listC;
        }

        public string GetVal(string elem)
        {
            string val = "";
            bool trigger = false;
            for (int i = 0; i < elem.Length; i++)
            {
                if ((elem[i] == ':') && (elem[i+1] != '{'))
                {
                    trigger = true;
                    continue;
                }

                if (trigger)
                {
                    val += elem[i];
                }
            }

            return val.Trim();
        }


        public ArrayList ParseObj(string objStr)
        {
            ArrayList list = new ArrayList();
            string temp = "";
            for (int i = 0; i < objStr.Length; i++)
            {
                if ((objStr[i] == ',') || (objStr[i] == '}'))
                {
                    list.Add(GetVal(temp));
                    temp = "";
                }
                else if (objStr[i] == '{')
                {
                    string subObject = objStr.Substring(objStr.IndexOf("{") + 1, objStr.LastIndexOf("}") - objStr.IndexOf("{"));
                    ArrayList subList = ParseObj(subObject);
                    list.Add(subList);
                    i = objStr.LastIndexOf("}");
                }
                else
                {
                    temp += objStr[i];
                }
            }
            return list;
        }
    }
}

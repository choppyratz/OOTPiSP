using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Plugin;
using System.IO;
using System.Reflection;

namespace WindowsFormsApp1
{
   
    public partial class Form1 : Form
    {
        private static List<Car> cars = new List<Car> {};
        private readonly string pluginPath = System.IO.Path.Combine(
                                                Directory.GetCurrentDirectory(),
                                                "Plugins");
        private List<IPlugin> plugins = new List<IPlugin>();
        public Form1()
        {
            InitializeComponent();

            listBox1.Tag = new List<Type> { typeof(Sedan), typeof(SUV), typeof(Pickup)};
            comboBox1.Tag = new List<Type> { typeof(BinarySerializer), typeof(JSONSerializer), typeof(TextSerializer) };
            comboBox1.SelectedIndex = 0;
            comboBox2.Items.Clear();
            comboBox2.Items.Add("Не делать постобработку");
            comboBox2.SelectedIndex = 0;
            LoadPlugins();
        }

        private void LoadPlugins()
        {
            plugins.Clear();
            DirectoryInfo pluginDirectory = new DirectoryInfo(pluginPath);
            if (!pluginDirectory.Exists)
                pluginDirectory.Create();

            var pluginFiles = Directory.GetFiles(pluginPath, "*.dll");
            foreach (var file in pluginFiles)
            {
                Assembly asm = Assembly.LoadFrom(file);
                var types = asm.GetTypes().
                                Where(t => t.GetInterfaces().
                                Where(i => i.FullName == typeof(IPlugin).FullName).Any());
                foreach (var type in types)
                {
                    var plugin = asm.CreateInstance(type.FullName) as IPlugin;
                    plugins.Add(plugin);
                    comboBox2.Items.Add(plugin.name);
                }
            }
        }

        private object obj;
        private Type typeCar;
        private void button1_Click(object sender, EventArgs e)
        {
                typeCar = ((List<Type>)listBox1.Tag)[listBox1.SelectedIndex];
                obj = Activator.CreateInstance(typeCar);
                Form2 form = new Form2(typeCar, obj, false, 0, cars);
                form.ShowDialog();
                updateListBox(listBox2, cars);
        }


        private void button3_Click(object sender, EventArgs e)
        {
            cars.RemoveAt(listBox2.SelectedIndex);
            listBox2.Items.RemoveAt(listBox2.SelectedIndex);
        }

        private void button2_Click(object sender, EventArgs e)
        {
                Form2 form = new Form2(cars[listBox2.SelectedIndex].GetType(), cars[listBox2.SelectedIndex], true, listBox2.SelectedIndex, cars);
                form.ShowDialog();
                updateListBox(listBox2, cars);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;

            string fileName = openFileDialog1.FileName;
            Console.WriteLine(fileName);
            int i = 0;
            bool trigger = false;
            foreach (var pl in plugins)
            {
                if (fileName.Contains(pl.getExtension()))
                {
                    trigger = true;
                    break;
                }
                i++;
            }
            if (trigger)
            {
                plugins[i].PLoad(ref fileName);
                Console.WriteLine(fileName);
            }
            ISerialize serializator = Activator.CreateInstance(((List<Type>)comboBox1.Tag)[comboBox1.SelectedIndex]) as ISerialize;
            cars = serializator.deserialize(fileName);
            updateListBox(listBox2, cars);

            if (trigger)
            {
                File.Delete(fileName);
            }
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;

            string fileName = saveFileDialog1.FileName;
            ISerialize serializator = Activator.CreateInstance(((List<Type>)comboBox1.Tag)[comboBox1.SelectedIndex]) as ISerialize;
            serializator.serialize(fileName, cars);
            if (comboBox2.SelectedIndex != 0)
            {
                plugins[comboBox2.SelectedIndex - 1].PSave(fileName);
            }
        }

        private void updateListBox(ListBox lb, List<Car> list)
        {
            lb.Items.Clear();
            foreach (Car p in list)
            {
                lb.Items.Add(p.name);
            }
        }
    }
}



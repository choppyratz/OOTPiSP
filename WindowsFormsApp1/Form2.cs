using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        private List<TextBox> TextList = new List<TextBox> { };
        private object changeobj;
        private Type MainType;
        private object engine;
        private bool isChangeMode;
        private int changeIndex;
        private List<Car> list;
        public Form2(Type _typeCar, object _obj, bool _changeMode, int _changeIndex, List<Car> _list)
        {
            InitializeComponent();
            changeobj = _obj;
            list = _list;
            MainType = _typeCar;
            isChangeMode = _changeMode;
            changeIndex = _changeIndex;
            int yPos = 50;
            int xPos = 10;
            foreach (FieldInfo fi in MainType.GetFields())
            {
                if (fi.FieldType.GetInterfaces().Contains(typeof(IEngine)))
                {
                    engine = changeobj;
                    addLabel(fi.Name, xPos, yPos);
                    addButton("Изменить", xPos, yPos - 1, ChangeBtn_Click);
                    yPos += 55;
                    continue;
                }

                addLabel(fi.Name, xPos, yPos);
                if (!isChangeMode)
                    addTextBox(xPos, yPos);
                else
                    addTextBox(xPos, yPos, fi.GetValue(changeobj).ToString());
                yPos += 55;
            }
            addButton("Добавить", (Width / 2) - (70 / 2) - 3, yPos - 20, AddBtn_Click);
        }

        private void ChangeBtn_Click(object sender, EventArgs e)
        {
            Form2 fm2;
            if (isChangeMode)
            {
                engine = (changeobj as Car).engine;
                
                fm2 = new Form2(typeof(Engine), engine, true, changeIndex, list);
            }
            else
            {
                engine = new Engine();
                fm2 = new Form2(engine.GetType(), engine, false, 0, list);
            }
            fm2.ShowDialog();
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            int i = 0;
            foreach (FieldInfo fi in MainType.GetFields())
            {

                if (fi.FieldType.GetInterfaces().Contains(typeof(IEngine)))
                {
                    fi.SetValue(changeobj, engine);
                    if (!isChangeMode)
                    {
                        list.Add(changeobj as Car);
                    }
                    else
                    {
                        list[changeIndex] = changeobj as Car;
                    }
                    i++;
                    continue;
                }
                var convert = TypeDescriptor.GetConverter(fi.FieldType);
                fi.SetValue(changeobj, convert.ConvertFrom(TextList.ElementAt(i).Text));
                i++;
            }
        }

        public void addTextBox(int x, int y, string title = "")
        {
            TextBox tBox = new TextBox();
            tBox.Text = title;
            tBox.Location = new Point(x, y);
            tBox.Width = 300;
            Controls.Add(tBox);
            TextList.Add(tBox);
        }

        public void addLabel(string name, int x, int y)
        {
            Label tLabel = new Label();
            tLabel.Text = name;
            tLabel.Location = new Point(x, y - 25);
            Controls.Add(tLabel);
        }

        public void addButton(string title, int x, int y, Handler trigger)
        {
            Button tButton = new Button();
            tButton.Text = title;
            tButton.Click += new EventHandler(trigger);
            tButton.Width = 70;
            
            tButton.Location = new Point(x, y);
            Controls.Add(tButton);
        }
        public delegate void Handler(object sender, EventArgs e);
    }
}

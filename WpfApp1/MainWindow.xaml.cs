using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace WpfApp1
{

    class User
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public string Speciality { get; set; }
        public DateTime? BirthDate { get; set; }
    }
   
    interface IAdapter
    {
        void Write(User u);
    }
    class XmlWriter
    {
        public void WriteToXml(User u)
        {
            string fileName = $"{u.Name}.xml";
            System.Xml.XmlWriter xmlWirter = System.Xml.XmlWriter.Create(fileName);
            xmlWirter.WriteStartDocument();
            xmlWirter.WriteStartElement("Etiket");
            xmlWirter.WriteString(u.Name + " " + u.Surname + " " + u.Speciality + " " + u.BirthDate);
            xmlWirter.WriteEndElement();
            xmlWirter.WriteEndDocument();
            xmlWirter.Close();
        }
    }
    class XmlAdapter : IAdapter
    {
        private XmlWriter _writer;

        public XmlAdapter(XmlWriter writer)
        {
            _writer = writer;
        }

        public void Write(User u)
        {
            _writer.WriteToXml(u);
        }
    }
    class JsonAdapter : IAdapter
    {
        private JsonWriter _writer;

        public JsonAdapter(JsonWriter writer)
        {
            _writer = writer;
        }

        public void Write(User u)
        {
            _writer.WriteToJson(u);
        }
    }

    class JsonWriter
    {
        public void WriteToJson(User u)
        {
            JsonSerializerSettings _options = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };

            var jsonString = JsonConvert.SerializeObject(u, _options);
            File.WriteAllText(u.Name + ".json", jsonString);
        }
    }

    class Converter
    {
        private readonly IAdapter _adapter;

        public Converter(IAdapter adapter)
        {
            _adapter = adapter;
        }
        public void Write(User u)
        {
            _adapter.Write(u);
        }
    }


    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            User user = new User()
            {
                Name = NameTxtb.Text,
                Surname = SurnameTxtb.Text,
                Age = int.Parse(AgeTxtb.Text),
                BirthDate = BirtdateDatepicker.SelectedDate,
                Speciality = SpecialityTxtb.Text,
            };


            IAdapter adapter;
            if (XmlRadioButton.IsChecked == true)
            {
                XmlWriter xmlWriter = new XmlWriter();
                adapter=new XmlAdapter(xmlWriter);
            }
            else if (JsonRadioButton.IsChecked == true)
            {
                JsonWriter jsonWriter = new JsonWriter();
                adapter = new JsonAdapter(jsonWriter);
            }
            else { adapter= null; }
            adapter.Write(user);

            System.Windows.MessageBox.Show("Writed Succesfully");
        }
    }
}

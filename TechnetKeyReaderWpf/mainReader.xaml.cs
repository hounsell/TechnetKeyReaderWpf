using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace TechnetKeyReaderWpf
{
    /// <summary>
    /// Interaction logic for mainReader.xaml
    /// </summary>
    public partial class mainReader : Window
    {
        public mainReader()
        {
            InitializeComponent();
        }

        private void loadData(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();

            /* Initialisation Code for openFile */
            openFile.AddExtension = true;
            openFile.CheckFileExists = true;
            openFile.DefaultExt = "*.xml";
            openFile.Multiselect = false;
            openFile.Title = "Select a Technet or MSDN XML file";
            openFile.Filter = "XML File|*.xml";

            if (openFile.ShowDialog() == true)
            {
                parseXml(openFile.OpenFile());
            }
        }

        private void parseXml(Stream xmlFile)
        {
            XElement keyXml = XElement.Parse(new StreamReader(xmlFile).ReadToEnd());

            prodBox.ItemsSource = from product in keyXml.Elements("Product_Key").Where(k => k.Attribute("Name").Value != "")
                                  select new Product
                                  {
                                      name = product.Attribute("Name").Value.Trim(),
                                      keys = from keyData in product.Elements("Key")
                                             select new Key
                                             {
                                                 type = typeCast(keyData.Attribute("Type").Value),
                                                 dateClaimed = (keyData.Attribute("ClaimedDate").Value == null ? DateTime.Parse(keyData.Attribute("ClaimedDate").Value) : DateTime.Parse("01/01/1990")),
                                                 key = keyData.Value
                                             }
                                  };
        }

        private KeyType typeCast(string keyType)
        {
            switch (keyType)
            {
                case "RTL":
                    return KeyType.RTL;
                case "STA":
                    return KeyType.STA;
                default:
                    return KeyType.ERR;
            }
        }
    }

    public struct Product
    {
        public string name { get; set; }
        public IEnumerable<Key> keys { get; set; }
        public string keysNum
        {
            get
            {
                return keys.Count<Key>() + " Key(s)";
            }
        }
    }

    public struct Key
    {
        public KeyType type { get; set; }
        public DateTime dateClaimed { get; set; }
        public string key { get; set; }
    }

    public enum KeyType
    {
        RTL, STA, VL1, VL2, MAK, CUS, ERR
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TechnetKeyReaderWpf
{
    public partial class controlTemplates : ResourceDictionary
    {
        public controlTemplates()
        {
            InitializeComponent();
        }

        public void keyCopyName(object sender, EventArgs e)
        {
            string test = (sender as FrameworkElement).Parent.ToString();
            Clipboard.SetText(test);
        }

        public void keyCopyKey(object sender, EventArgs e)
        {
            string test = ((TextBlock)((ContextMenu)((MenuItem)sender).Parent).PlacementTarget).Text.ToString();
            Clipboard.SetText(test);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PTM.Terminal.CustomControls
{
    /// <summary>
    /// Szablon dla nagłówków typu Ribbon
    /// </summary>
    public partial class CtrlRibbonHeader : UserControl
    {

        /// <summary>
        /// Zmienia właściwość nagłówka
        /// </summary>
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("HeaderDescription", typeof(string), typeof(CtrlRibbonHeader), new PropertyMetadata(null));

        /// <summary>
        /// Zmienia ikone
        /// </summary>
        public static readonly DependencyProperty IconData = DependencyProperty.Register("IconSource", typeof(ImageSource), typeof(CtrlRibbonHeader), new PropertyMetadata(null));
        
        /// <summary>
        ///  Dane do zmiany nagłówka
        /// </summary>
        public string HeaderDescription 
        { 
            get 
            {
                return base.GetValue(HeaderProperty) as string;
            }
            set 
            {
                base.SetValue(HeaderProperty, value);
            }
        }

      
        /// <summary>
        /// Source ikony
        /// </summary>
        public ImageSource IconSource
        {
            get 
            { 
                return base.GetValue(IconData) as ImageSource; 
            }
            set 
            { 
                base.SetValue(IconData, value); 
            }
        }

        public CtrlRibbonHeader()
        {
            InitializeComponent();
        }
        
    }
}


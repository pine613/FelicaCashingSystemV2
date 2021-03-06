﻿using System;
using System.Collections.Generic;
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

namespace FelicaCashingSystemV2.Controls
{
    /// <summary>
    /// SystemInformationButton.xaml の相互作用ロジック
    /// </summary>
    public partial class SystemInformationButton : UserControl
    {
        public event EventHandler<RoutedEventArgs> Click;

        public SystemInformationButton()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.Click != null)
            {
                this.Click(this, e);
            }
        }
    }
}

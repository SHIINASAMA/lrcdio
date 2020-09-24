﻿using LrcLib;
using System;
using System.Collections;
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
using Window = HandyControl.Controls.Window;

namespace LrcEditor
{
    /// <summary>
    /// Info.xaml 的交互逻辑
    /// </summary>
    public partial class Info : Window
    {
        public Hashtable LrcHeaders;

        public Info()
        {
            
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (ALTextBox.Text == null) ALTextBox.Text = "NULL";
            if (ARTextBox.Text == null) ARTextBox.Text = "NULL";
            if (BYTextBox.Text == null) BYTextBox.Text = "NULL";
            if (TITextBox.Text == null) TITextBox.Text = "NULL";
            if (OFFSETTextBox.Text == null) OFFSETTextBox.Text = "NULL";
        }
    }
}

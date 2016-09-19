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
using Star_Dundee_WPF.Models;

namespace Star_Dundee_WPF
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : NavigationWindow
    {
        CRC8 crc8 = new CRC8();
        RMAP rmap = new RMAP();
        public MainWindow()
        {
            rmap.buildPacket("0c 00 57 ff fa 00 00 00 08 c7 a9 b6 d1 e8 de ff 7e 8b 76");
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            string cargo = string.Empty;
            crc8.Check(cargo);
        }


        private void button1_Click(object sender, RoutedEventArgs e)
        {
            FileParser myFileParser = new FileParser();

            myFileParser.readFile();
        }
    }
}
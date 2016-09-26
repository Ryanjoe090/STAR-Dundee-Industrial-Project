﻿using Star_Dundee_WPF.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.IO;
using Microsoft.Win32;
using System.Data;
using System.ComponentModel;

namespace Star_Dundee_WPF
{
    /// <summary>
    /// Interaction logic for SpaceHome.xaml
    /// </summary>
    public partial class SpaceHome : Page
    {
        FileParser myFileParser;     

        public SpaceHome()
        {

            
            InitializeComponent();
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Recording files (*.rec;)|*.rec;|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (openFileDialog.ShowDialog() == true)
            {
                string[] files = openFileDialog.FileNames;
                myFileParser = new FileParser();
                myFileParser.startParsing(files);

                // Set the ItemsSource to autogenerate the columns.
                List<GridColumn> listToDisplay = myFileParser.listOfColumns;
                //printListOfColumns(listToDisplay);
                dataGrid1.ItemsSource = listToDisplay;

                //Set the recording to the datacontext
                this.DataContext = myFileParser.mainRecording;
            }
        }

        private void printListOfColumns(List<GridColumn> listToDisplay)
        {
            foreach (GridColumn item in listToDisplay)
            {
                Console.WriteLine();
                Console.Write(item.getTime() + "\t");

                for (int i = 1; i < 9; i++)
                {
                    Console.Write(item.getPort(i) + " ");
                }
            }
        }

        private void dataDrid1_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // make sure only 1 cell is clicked
            if (dataGrid1.SelectedCells.Count != 1)
                return;

            // Get column header
            string portHeader = (string)dataGrid1.SelectedCells[0].Column.Header;
            int portIndex = dataGrid1.SelectedCells[0].Column.DisplayIndex;
            Console.WriteLine("port Index: " + portIndex);
            Console.WriteLine("Port Clicked: " + portHeader);

            updatePortSummury(portIndex);
            //get row index
            GridColumn row = (GridColumn)dataGrid1.CurrentItem;
            
            Console.WriteLine("Cell Time: " + row.time.ToString());
            //string row1 = (string)dataGrid1.SelectedCells[0]
            //int rowIndex = dataGrid1.SelectedCells.
            //int index = dataGrid1.Items.IndexOf(cell.Item))

            //get timestamp 



        }

        private void updatePortSummury(int port)
        {

            string[]portSummary = new string[6] { "", "", "", "", "", "" };
            myFileParser.mainRecording.portSummary = portSummary;
            //check if port exists
            bool exists = false;
            int portIndex = port;
            
            for (int i = 0; i < myFileParser.mainRecording.ports.Count; i++)
            {
                if (myFileParser.mainRecording.ports[i].portNumber == port)
                {
                    exists = true;
                    portIndex = i;
                }
                    
            }
            port -= 1;
            if (exists)
            {
                myFileParser.mainRecording.portSummary = getPortSummary(portIndex);
                portPanel.Visibility = System.Windows.Visibility.Visible;
            }
            
                
            DataContext = myFileParser.mainRecording;

            //if empty hide lables
            if (!exists)
            {
                portPanel.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        public string[] getPortSummary(int port)
        {
            string[] portSummary = new string[6] { "", "", "", "", "", ""};

            portSummary[0] = myFileParser.mainRecording.ports[port].totalPackets.ToString();
            portSummary[1] = myFileParser.mainRecording.ports[port].totalErrors.ToString();
            portSummary[2] = myFileParser.mainRecording.ports[port].totalCharacters.ToString();
            portSummary[3] = myFileParser.mainRecording.ports[port].dataRate.ToString();
            portSummary[4] = myFileParser.mainRecording.ports[port].packetRate.ToString();
            portSummary[5] = myFileParser.mainRecording.ports[port].errorRate.ToString();

            return portSummary;
        }
    }


    public class ErrorHighlighter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var errorString = value as string;
            if (errorString == null) return null;

            if (errorString == "noError") return Brushes.Green;
            else if (errorString == "disconnect") return Brushes.Crimson;
            else if (errorString == "parity") return Brushes.DarkRed;
            else if (errorString == "crcHeader") return Brushes.DarkSalmon;
            else if (errorString == "crcData") return Brushes.DarkSalmon;
            else if (errorString == "eep") return Brushes.Red;
            else if (errorString == "timeout") return Brushes.IndianRed;
            else if (errorString == "babblingIdiot") return Brushes.Plum;
            else if (errorString == "length") return Brushes.Bisque;
            else if (errorString == "sequence") return Brushes.BurlyWood;
            else if (errorString == "") return Brushes.LightBlue;
            else return Brushes.LightSteelBlue;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();

        }
    }
}

   

   
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
using System.Windows.Shapes;

namespace WFInfo
{
    /// <summary>
    /// Interaction logic for SearchIt.xaml
    /// </summary>
    /// 

    public partial class SearchIt : Window
    {

        public SearchIt()
        {
            InitializeComponent();
        }

        public bool isInUse = false;

        public void Start()
        {
	        Main.searchBox.Show();
	        Main.searchBox.placeholder.Content = "Search for warframe.market Items";
            if (!Main.dataBase.IsJwtAvailable())
            {
	            Main.searchBox.placeholder.Content = "Please log in first";
                Login wfmLogin = new Login();
                wfmLogin.Show();
                wfmLogin.Top = Main.searchBox.Top - wfmLogin.Height;
                wfmLogin.Left = Left;
                return;
	        }
            isInUse = true;
            Main.searchBox.Show();
            searchField.Focusable = true;
        }

        private void Search(object sender, RoutedEventArgs e)
        {
            Main.AddLog(searchField.Text);
            finish();
        }
        internal void finish()
        {
            searchField.Text = "";
            placeholder.Visibility = Visibility.Visible;
            searchField.Focusable = false;
            isInUse = false;
            Hide();
        }

        private void textChanged(object sender, TextChangedEventArgs e)
        {
            if (searchField.Text != "")
                placeholder.Visibility = Visibility.Hidden;
            List<string> closest = Main.dataBase.ClosestAutoComplete(searchField.Text, 1);
            foreach (string result in closest)
            {
                Main.AddLog(result);
            }
        }
    }
}



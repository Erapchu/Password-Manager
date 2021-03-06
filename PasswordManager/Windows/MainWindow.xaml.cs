﻿using Autofac;
using PasswordManager.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PasswordManager.Windows
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainViewModel viewModel;
        ILifetimeScope _lifetimeScope;
        public MainWindow(ILifetimeScope lifetimeScope)
        {
            InitializeComponent();
            _lifetimeScope = lifetimeScope.BeginLifetimeScope();
            var wih = new WindowInteropHelper(this);
            var windowHandle = wih.EnsureHandle();
            viewModel = _lifetimeScope.Resolve<MainViewModel>(new NamedParameter("windowHandle", windowHandle));
            DataContext = viewModel;
            StateChanged += MainWindow_StateChanged;
        }

        private void MainWindow_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                MainWindowBorder.BorderThickness = new Thickness(8);
                RestoreButton.Visibility = Visibility.Visible;
                MaximizeButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                MainWindowBorder.BorderThickness = new Thickness(1);
                RestoreButton.Visibility = Visibility.Collapsed;
                MaximizeButton.Visibility = Visibility.Visible;
            }
        }

        // Can execute
        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        // Minimize
        private void CommandBinding_Executed_Minimize(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        // Maximize
        private void CommandBinding_Executed_Maximize(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MaximizeWindow(this);
        }

        // Restore
        private void CommandBinding_Executed_Restore(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.RestoreWindow(this);
        }

        // Close
        private void CommandBinding_Executed_Close(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.CloseWindow(this);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _lifetimeScope.Dispose();
            _lifetimeScope = null;
        }

        private void ExitItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

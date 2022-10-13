﻿using Go.ViewModels.Components;
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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Go.Views.Components
{
    /// <summary>
    /// PredictItem.xaml 的交互逻辑
    /// </summary>
    public partial class PredictItem : UserControl
    {

        public readonly PredictItemViewModel viewModel;

        public PredictItem()
        {
            InitializeComponent();
            viewModel = new PredictItemViewModel();
            this.DataContext = viewModel;
        }
        public PredictItem(float pridict)
        {
            InitializeComponent();
            viewModel = new PredictItemViewModel(pridict);
            this.DataContext = viewModel;
        }
    }
}

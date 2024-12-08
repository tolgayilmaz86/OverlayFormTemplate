using Avalonia.Controls;
using OverlayFormTemplate.ViewModels;
using System;

namespace OverlayFormTemplate.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        this.InitializeComponent();

        // Overlay PopUp on load

        //this.DataContextChanged += (s, arg) => {
        //    var vm = (MainWindowViewModel)this.DataContext!;
        //    try
        //    {
        //        vm.Init();
        //    }
        //    catch (Exception exc)
        //    {
        //    }
        //};
    }
}

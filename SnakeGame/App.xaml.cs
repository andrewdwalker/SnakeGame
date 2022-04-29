using SnakeGame.Models;
using SnakeGame.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SnakeGame
{
   /// <summary>
   /// Interaction logic for App.xaml
   /// </summary>
   public partial class App : Application
   {
      protected override void OnStartup(StartupEventArgs e)
      {
         MainWindow view = new MainWindow();

         GameGridModel gameGridModel = new GameGridModel(12, 12, 12, 1);
        
         SnakeViewModel viewModel = new SnakeViewModel(gameGridModel);

         
         view.DataContext = viewModel;
         view.Show();

      }
   }
}

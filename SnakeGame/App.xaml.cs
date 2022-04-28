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

         GameGridModel gameGridModel = new GameGridModel(5, 5);
         Snake snake = new Snake(gameGridModel.GetGridElement(2,2));
         Food food = new Food(new GridElement[] { gameGridModel.GetGridElement(3, 3), gameGridModel.GetGridElement(3, 4) } );

         SnakeViewModel viewModel = new SnakeViewModel(gameGridModel, snake, food);

         //IStage stageModel = new Stage(new HardwareStage());
         //ILight lightModel = new Light();
         //EighteenFiftyNineViewModel viewModel = new EighteenFiftyNineViewModel(stageModel, lightModel);
         view.DataContext = viewModel;
         view.Show();

      }
   }
}

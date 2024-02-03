﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MineSweeper.Controller
{
    public class Controller : ApplicationContext
    {
        private View.IMainView mainView;
        private Model.IGameModel gameModel;
        public Controller()
        {
            gameModel = new Model.GameModel();
            gameModel.NewGame();

            mainView = new View.MainWindow(gameModel);
            mainView.OnWindowClosing += mainView_OnWindowClosing;
            mainView.OnMarkField += mainView_OnMarkField;
            mainView.Show();
            mainView.UpdateView();
        }

        private void mainView_OnWindowClosing()
        {
            Application.Exit();
        }

        private void mainView_OnMarkField(int row, int column)
        {
            gameModel.MarkField(row, column);
            mainView.UpdateView();
        }
    }
}

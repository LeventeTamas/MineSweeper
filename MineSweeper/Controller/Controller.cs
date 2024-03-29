﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MineSweeper.Controller
{
    public class Controller : ApplicationContext
    {
        private View.IMainView mainView;
        private Model.IGameModel gameModel;
        private System.Timers.Timer timer;

        public Controller()
        {

            gameModel = new Model.GameModel();
            RegisterGameModelEvents();

            mainView = new View.MainWindow(gameModel);
            mainView.OnWindowClosing += mainView_OnWindowClosing;
            mainView.OnMarkField += mainView_OnMarkField;
            mainView.OnRevealField += mainView_OnRevealField;
            mainView.OnClearFieldsAround += mainView_OnClearFieldsAround;
            mainView.OnNewGame += mainView_OnNewGame;
            mainView.OnPauseGame += mainView_OnPauseGame;
            mainView.OnSaveGame += mainView_OnSaveGame;
            mainView.OnLoadSavedGame += mainView_OnLoadSavedGame;
            mainView.OnRestartGame += mainView_OnRestartGame;
            mainView.OnChangeSettings += mainView_OnChangeSettings;
            mainView.Show();

            timer = new System.Timers.Timer();
            timer.Interval = 1000;
            timer.Elapsed += timer_OnElapsed;

            StartNewGame();
        }

        private void RegisterGameModelEvents()
        {
            gameModel.OnLoseGame += gameModel_OnLoseGame;
            gameModel.OnWinGame += gameModel_OnWinGame;
        }
        private void StartNewGame()
        {
            timer.Stop();

            gameModel.CreateNewGame();
            gameModel.StartGame();
            mainView.UpdateView();

            timer.Start();
        }
        private void timer_OnElapsed(object o, EventArgs e)
        {
            gameModel.TimerTick();

            /*  This Try-Catch is a workaround.
                Issue: When the application is being closed and the timer calls this method at the same time,
                there's a chance that the 'mainView' is in an unstable state, causing runtime error */
            try
            {
                mainView.Invoke(new MethodInvoker(delegate
                {
                    mainView.UpdateTime();
                }));
            }
            catch { }
        }
        
        #region gameModel events
        private void gameModel_OnLoseGame()
        {
            mainView.UpdateView();
            mainView.LoseGame();
        }
        private void gameModel_OnWinGame()
        {
            mainView.UpdateView();
            mainView.WinGame();
        }
        #endregion

        #region mainView events
        private void mainView_OnWindowClosing()
        {
            timer.Stop();
            Application.Exit();
        }

        private void mainView_OnMarkField(int row, int col)
        {
            gameModel.MarkField(row, col);
            mainView.UpdateView();
        }

        private void mainView_OnRevealField(int row, int col)
        {
            gameModel.RevealField(row, col);
            mainView.UpdateView();
        }

        private void mainView_OnClearFieldsAround(int row, int col)
        {
            gameModel.ClearFieldsAround(row, col);
            mainView.UpdateView();
        }

        private void mainView_OnNewGame()
        {
            StartNewGame();
        }

        private void mainView_OnPauseGame()
        {
            SharedStructs.GameState gameState = gameModel.GetGameState();
            switch (gameState) {
                case SharedStructs.GameState.ONGOING: { 
                        gameModel.PauseGame(); 
                        timer.Stop(); 
                        break; 
                    }
                case SharedStructs.GameState.PAUSED: { 
                        gameModel.StartGame(); 
                        timer.Start(); 
                        break; 
                    }
            }
            mainView.UpdateView();
        }

        private void mainView_OnSaveGame(string path)
        {
            Model.IOManager.SaveIGameModel(path, gameModel);
        }

        private void mainView_OnLoadSavedGame(string path)
        {
            gameModel = Model.IOManager.LoadIGameModel(path);

            RegisterGameModelEvents();

            SharedStructs.GameState gameState = gameModel.GetGameState();
            switch (gameState) {
                case SharedStructs.GameState.STOPPED: { timer.Stop(); break; }
                case SharedStructs.GameState.PAUSED: { timer.Stop(); break; }
                case SharedStructs.GameState.ONGOING: { timer.Start(); break; }
            }

            mainView.SetGameModel(gameModel);
            mainView.UpdateView();
        }

        private void mainView_OnRestartGame()
        {
            gameModel.ResetGame();
            gameModel.StartGame();
            mainView.UpdateView();
            timer.Start();

        }

        private void mainView_OnChangeSettings(SharedStructs.Settings settings)
        {
            gameModel.ChangeSettings(settings);
            StartNewGame();
        }
        #endregion
    }
}

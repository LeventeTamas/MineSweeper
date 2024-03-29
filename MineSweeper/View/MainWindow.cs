﻿using MineSweeper.Model;
using MineSweeper.View.MineZone;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MineSweeper.View
{
    
    public partial class MainWindow : Form, IMainView
    {
        private Model.IGameModel gameModel;
        
        public event WindowClosingEventHandler OnWindowClosing; 
        public event MarkFieldEventHandler OnMarkField;
        public event RevealFieldEventHandler OnRevealField;
        public event ClearFieldsAroundEventHandler OnClearFieldsAround;
        public event NewGameEventHandler OnNewGame;
        public event PauseGameEventHandler OnPauseGame;
        public event SaveGameEventHandler OnSaveGame;
        public event LoadSavedGameEventHandler OnLoadSavedGame;
        public event RestartGameEventHandler OnRestartGame;
        public event ChangeSettingsEventHandler OnChangeSettings;

        public MainWindow(Model.IGameModel gameModel)
        {
            this.gameModel = gameModel;

            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            mineZone1.OnMarkField += mineZone1_MarkField;
            mineZone1.OnRevealField += mineZone1_RevealField;
            mineZone1.OnClearFieldsAround += mineZone1_ClearFieldsAround;
        }

        public void SetGameModel(IGameModel gameModel)
        {
            this.gameModel = gameModel;
        }

        public void UpdateView()
        {
            // Get fields from model
            SharedStructs.Field[,] fields = gameModel.GetFields();
            int rowNum = fields.GetLength(0);
            int colNum = fields.GetLength(1);

            // Set the form size according to the size and number of fields
            this.Width = colNum * MineZone.MineZone.FIELD_SIZE + 16;
            this.Height = rowNum * MineZone.MineZone.FIELD_SIZE + splitContainer1.Panel1.Height + menuStrip1.Height + (this.RectangleToScreen(this.ClientRectangle).Top - this.Top) + 12;
            

            // Create MineZoneFiled objects
            MineZone.MineZoneField[,] mineZoneFields = new MineZone.MineZoneField[rowNum, colNum];

            for (int r = 0; r < rowNum; r++)
            {
                for (int c = 0; c < colNum; c++)
                {
                    MineZone.MineZoneFieldType mineZoneFieldType = MineZone.MineZoneFieldType.COVERED;
                    switch (fields[r, c].State)
                    {
                        case SharedStructs.FieldState.MARKED: {
                                mineZoneFieldType = MineZone.MineZoneFieldType.MARKED; break;
                            }
                        case SharedStructs.FieldState.CLEARED: {
                                mineZoneFieldType = MineZone.MineZoneFieldType.REVEALED; break;
                            }
                        case SharedStructs.FieldState.MINE:{
                                mineZoneFieldType = MineZone.MineZoneFieldType.MINE; break;
                            }
                        default:{
                                mineZoneFieldType = MineZone.MineZoneFieldType.COVERED; break;
                            }
                    }
                    mineZoneFields[r, c] = new MineZone.MineZoneField(
                        mineZoneFieldType, 
                        fields[r, c].Value != 0 ? fields[r, c].Value.ToString() : ""
                        );
                }
            }

            mineZone1.SetFileds(mineZoneFields);

            // Update 'remaining mines' label
            lbRemainingMines.Text = gameModel.GetRemainingMines().ToString();

            // Update 'elapsed time' label
            UpdateTime();

            // Set UI according to the game status
            SharedStructs.GameState gameState = gameModel.GetGameState();
            switch(gameState)
            {
                case SharedStructs.GameState.ONGOING:
                    {
                        mineZone1.Enabled = true;
                        pauseToolStripMenuItem.Enabled = true;
                        pauseToolStripMenuItem.Checked = false;
                        break;
                    }
                case SharedStructs.GameState.PAUSED:
                    {
                        mineZone1.Enabled= false;
                        pauseToolStripMenuItem.Enabled = true;
                        pauseToolStripMenuItem.Checked = true;
                        break;
                    }
                case SharedStructs.GameState.STOPPED:
                    {
                        mineZone1.Enabled = false;
                        pauseToolStripMenuItem.Checked = false;
                        pauseToolStripMenuItem.Enabled = false;
                        break;
                    }
            }

        }    
        public void UpdateTime()
        {
            // Update 'elapsed time' label
            lbElapsedTime.Text = gameModel.GetElapsedTime();
        }
        public void LoseGame()
        {
            GameOverMessageBox gameOverMessageBox = new GameOverMessageBox("You have activated a mine. You lost!");
            DialogResult result = gameOverMessageBox.ShowDialog();

            if (result == DialogResult.Yes)
                OnNewGame();
            else if(result == DialogResult.Retry)
                OnRestartGame();
        }
        public void WinGame()
        {
            GameOverMessageBox gameOverMessageBox = new GameOverMessageBox("You have cleared all fields. You won!");
            DialogResult result = gameOverMessageBox.ShowDialog();

            if (result == DialogResult.Yes)
                OnNewGame();
            else if (result == DialogResult.Retry)
                OnRestartGame();
        }

        #region events
        private void mineZone1_MarkField(int row, int col)
        {
            OnMarkField(row, col);
        }

        private void mineZone1_RevealField(int row, int col)
        {
            OnRevealField(row, col);
        }
        
        private void mineZone1_ClearFieldsAround(int row, int col)
        {
            OnClearFieldsAround(row, col);
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            OnWindowClosing();
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OnNewGame();
        }
        
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OnPauseGame();
        }
        
        private void saveGameToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK){
                OnSaveGame(saveFileDialog1.FileName);
            }
        }

        private void loadGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK){
                OnLoadSavedGame(openFileDialog1.FileName);
            }
        }

        private void restartGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OnRestartGame();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SharedStructs.Settings settings = gameModel.GetSettings();
            SettingsWindow sw = new SettingsWindow(settings.NumOfRows, settings.NumOfColumns, settings.NumOfMines);
            if(sw.ShowDialog() == DialogResult.OK)
            {
                SharedStructs.Settings newSettings = new SharedStructs.Settings(sw.Rows, sw.Cols, sw.Mines);
                OnChangeSettings(newSettings);
            }
        }
        #endregion


    }
}

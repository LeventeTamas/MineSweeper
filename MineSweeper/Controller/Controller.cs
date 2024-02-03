using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MineSweeper.Controller
{
    public class Controller : ApplicationContext
    {
        private View.IMainView mainView;
        private Model.IGameModel gameModel;
        public Controller()
        {
            gameModel = new Model.GameModel();
            gameModel.OnTimeElapsed += gameModel_OnTimeElapsed;
            gameModel.NewGame();

            mainView = new View.MainWindow(gameModel);
            mainView.OnWindowClosing += mainView_OnWindowClosing;
            mainView.OnMarkField += mainView_OnMarkField;
            mainView.OnRevealField += mainView_OnRevealField;
            mainView.OnClearFieldsAround += mainView_OnClearFieldsAround;
            mainView.Show();
            mainView.UpdateView();
        }

        private void gameModel_OnTimeElapsed()
        {
            if (mainView == null) return;
            mainView.Invoke(new MethodInvoker(delegate
            {
                mainView.UpdateTime();
            }));
        }

        private void mainView_OnWindowClosing()
        {
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
    }
}

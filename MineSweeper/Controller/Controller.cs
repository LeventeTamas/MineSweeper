using System;
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

        public Controller()
        {
            mainView = new View.MainWindow();
            mainView.OnWindowClosing += CloseApplication;
            mainView.Show();
        }

        private void CloseApplication()
        {
            Application.Exit();
        }
    }
}

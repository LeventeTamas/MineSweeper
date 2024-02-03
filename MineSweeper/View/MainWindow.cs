using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MineSweeper.View
{
    
    public partial class MainWindow : Form, IMainView
    {
        public event WindowClosing OnWindowClosing; 

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            OnWindowClosing();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeaFight.Forms
{
    public partial class LogForm : Form
    {
        public LogForm()
        {
            InitializeComponent();
        }

        delegate void PrintTextCallback(string text);

        public void Message(string text)
        {
            if (InvokeRequired)
            {
                var callback = new PrintTextCallback(Message);
                Invoke(callback, text);
            }
            else
            {
                lvLog.Items.Add(text);
                lvLog.EnsureVisible(lvLog.Items.Count - 1);
                lvLog.Update();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        CancellationTokenSource tokenSource = new CancellationTokenSource();

        Task mainTask;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            mainTask = Test();
        }

        private async Task Test()
        {
            CancellationToken token = tokenSource.Token;
            await Task.Run(new Action(() =>
            {
                while (!token.IsCancellationRequested)
                {
                }
                Thread.Sleep(10000);
            }), token);
        }

        private async void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (mainTask.Status != TaskStatus.RanToCompletion)
            {
                e.Cancel = true;
                tokenSource.Cancel();
                await mainTask;
                Close();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace Loki.UI.Win.Views
{
    public partial class SplashView : DevExpress.XtraEditors.XtraForm
    {
        public SplashView()
        {
            InitializeComponent();
            this.FormClosing += SplashView_FormClosing;
            this.Deactivate += SplashView_Deactivate;
        }

        private void SplashView_Deactivate(object sender, EventArgs e)
        {
        }

        private void SplashView_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
    }
}
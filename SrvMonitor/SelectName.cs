using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ConMonitor
{
    public partial class SelectName : Form
    {
        public SelectName()
        {
            InitializeComponent();
        }

        #region Eventos
        internal event ClientTextEventHandler SelectedName;
        internal event UniversalHandler CancelConfig;

        #endregion


        internal List<string> ClientNames 
        {  
            set
            {
                this.lbNames.Items.Clear();
                foreach (var item in value)
                {
                    this.lbNames.Items.Add(item);
                }
            } 
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            if (CancelConfig != null)
                CancelConfig();
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            if (this.lbNames.SelectedItem == null)
            {
                MessageBox.Show("Se requiere seleccionar un nombre.");
                lbNames.Focus();
                return;
            }

            if (SelectedName != null)
            {
                string data = (string)lbNames.SelectedItem;
                ClientTextEventArgs ev = new ClientTextEventArgs(data);

                SelectedName(this, ev);
            }
                
        }

        
    }
}

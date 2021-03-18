using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Zyan.Communication;
using Zyan.Communication.Security;
using Zyan.Communication.Toolbox;
using Zyan.Communication.Protocols.Tcp;

using Monitor.Shared;


namespace Monitor.Supervisor
{
    public partial class Form1 : Form
    {
        #region Declaraciones

        private ZyanConnection _zConnection;

        public Action<RequestDataEvent> Out_GetAllData { get; set; }

        private ClientData _editClient;
        private bool _pending;

        #endregion



        public Form1()
        {
            InitializeComponent();
        }


        private bool CambiosPendientes
        {
            get { return _pending; }

            set
            {
                _pending = value;

                //this.btApply.Enabled = value;
                //this.cmbClientes.Enabled = !value;
                //this.btOk.Enabled = !value;
            }
        }
        private ClientData ClientData
        {
            get { return _editClient; }
            set
            {
                _editClient = value;

                // pasar valores a controles form
                this.tbName.Text = _editClient.Name;
                this.tbName.ReadOnly = true;
                this.tbAppPath.Text = _editClient.AppFilePath;
                this.tbLogPath.Text = _editClient.LogFilePath;
                this.tbPort.Text = _editClient.Port.ToString();
                this.tbTimeout.Text = _editClient.Timeout.ToString();
                this.chkEmail.Checked = _editClient.MailEnabled;

                CambiosPendientes = false;

                if (_editClient.Status == ClientStatus.Detenido)
                    this.btStatus.BackColor = Color.Red;

                Color btColor;

                switch (_editClient.Status)
                {
                    case ClientStatus.Inicial:
                        btColor = Color.Yellow;
                        break;

                    case ClientStatus.Operacional:
                        btColor = Color.Green;
                        break;

                    case ClientStatus.Reiniciado:
                        btColor = Color.Gold;
                        break;

                    case ClientStatus.Detenido:
                        btColor = Color.Red;
                        break;

                    case ClientStatus.Muerto:
                    default:
                        btColor = Color.Black;
                        break;
                }
                this.btStatus.BackColor = btColor;

                this.lbRestCount.Text = _editClient.RestartCount.ToString();
                this.lbUptime.Text = _editClient.Uptime.ToString();
                this.lbxHB.Items.Clear();
                this.lbxHB.Items.AddRange(_editClient.HeartBeatList);

            }
        }



        public void In_GetClient(ClientDataList cldat)
        {
           // MessageBox.Show("Hay " + cldat.List.Count.ToString());

            this.listBox1.Items.Clear();

            foreach (var item in cldat.List)
            {
                this.listBox1.Items.Add(item.Value);
            }

            this.listBox1.SelectedItem = this.listBox1.Items[0];
        }

        public void In_GetSys(SystemConfigData cldat)
        {
            
            this.tbUdpPort.Text = cldat.UdpServerPort.ToString();
            this.tbZyanComponent.Text = cldat.ZyanServerName;
            this.tbTcpPort.Text = cldat.ZyanServerPort.ToString();
        }

      
        
        private bool Connect(string arg)
        {
            
            try
            {
                if (_zConnection != null)
                    _zConnection.Dispose();

                TcpDuplexClientProtocolSetup protocol = new TcpDuplexClientProtocolSetup(true);

                Hashtable credentials = new Hashtable();
                credentials.Add(AuthRequestMessage.CREDENTIAL_USERNAME, "zc");
                credentials.Add(AuthRequestMessage.CREDENTIAL_PASSWORD, "zc");

                _zConnection = new ZyanConnection(arg, protocol, credentials, false, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                this.chkConect.Checked = false;
                return false;
            }

            this.chkConect.Checked = true;

            IMonitor relMonitor = _zConnection.CreateProxy<IMonitor>();

            //this.Out_GetClient = Asynchronizer<int>.WireUp(relMonitor.In_RequestClientDataList);
            //this.Out_GetSystem = Asynchronizer<int>.WireUp(relMonitor.In_RequestSystemConfigData);
            this.Out_GetAllData = Asynchronizer<RequestDataEvent>.WireUp(relMonitor.In_RequestData);

            relMonitor.Out_SendClientDataList = SyncContextSwitcher<ClientDataList>.WireUp(this.In_GetClient);
            relMonitor.Out_SendSystemConfigData = SyncContextSwitcher<SystemConfigData>.WireUp(this.In_GetSys);


            return true;

        }


        private void button1_Click(object sender, EventArgs e)
        {
            string conad;
            if (tbAddrs.Text.Length == 0)
                conad = Constants.ServerUrl;
            else
                conad = tbAddrs.Text;

            if (Connect(tbAddrs.Text))
            {
                GetAll();
                timer1.Start();
            }
        }

       
        private void GetAll()
        {
            //RequestDataEvent req = new RequestDataEvent(RequestDataEvent.RequestType.All);
            RequestDataEvent req = new RequestDataEvent(RequestType.All);


            Out_GetAllData(req);
        }

       

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                timer1.Stop();

                if (_zConnection != null)
                    _zConnection.Dispose();
            }
            catch 
            {
            }
            
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClientData = listBox1.SelectedItem as ClientData;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            GetAll();
        }

        
    }
}

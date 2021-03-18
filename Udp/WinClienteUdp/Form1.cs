using AMGS.Application.Utils.Serialization.Bin;
using Monitor.Shared.Heartbeat;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

/*  Este cliente realiza una implementación del emisor de Heartbeat reutilizando código compilado.
 *  El proyecto contiene una referencia a la dll Monitor.Shared. 
 *  Se hace referencia a la clase estática HbSenderUdp y HeartBeatGenerator.
 */

namespace WinClienteUdp
{
    public partial class Form1 : Form
    {
        // Declaraciones
        private bool nonNumberEntered = false;
        private bool valuesChanged = false;

        private string DatabaseName = Assembly.GetEntryAssembly().GetName().Name + ".bsr";

        private ConfigData _dbRoot;


        // Constructor
        public Form1()
        {
            InitializeComponent();
        }

        // Metodos
        private void StartHB()
        {
            // cargar config

            string ipad; 
            int remPort; 
            int locPort; 
            int interv;

            string id, format;
            bool useSerial;

            // Si se han cambiado los valores en los controles
            if (valuesChanged)
            {   // leer valores de controles
                ipad = tbIp.Text;
                remPort = int.Parse(tbSPort.Text);
                locPort = int.Parse(tbLocalPort.Text);
                interv = int.Parse(tbInterval.Text);
                id = tbID.Text;
                useSerial = chkSerial.Checked;
                format = tbFormat.Text;

                // actualizar config
                _dbRoot.IpServidor = ipad;
                _dbRoot.PuertoServidor = remPort;
                _dbRoot.PuertoLocal = locPort;
                _dbRoot.Intervalo = interv;
                _dbRoot.Format = format;
                _dbRoot.ClientID = tbID.Text;
                _dbRoot.UseSerial = chkSerial.Checked;

                if (rbKeyId.Checked)
                    _dbRoot.UseId = true;
                else
                    _dbRoot.UseId = false;

                SaveDatabase();
                valuesChanged = false;
            }
            else
            {
                // leer valores de DB
                ipad = _dbRoot.IpServidor ;
                remPort = _dbRoot.PuertoServidor;
                locPort = _dbRoot.PuertoLocal;
                interv = _dbRoot.Intervalo;
                format = _dbRoot.Format;
                id = _dbRoot.ClientID;
                useSerial = _dbRoot.UseSerial;

                if (_dbRoot.UseId)
                    rbKeyId.Checked = true;
                else
                    rbKeyPort.Checked = true;

            }

            if (rbKeyId.Checked)
                HeartBeatGenerator.ClientID = id;
            else
                HeartBeatGenerator.ClientID = null;

            HeartBeatGenerator.TimestampFormat = format;
            HeartBeatGenerator.UsarSerialHB = useSerial;
            
            HbSenderUdp.IniciarHeartbeat(ipad, remPort, locPort, interv);

  
        }

        private void StopHB()
        {
            HbSenderUdp.DetenerHeartbeat();
        }

        // Event Handlers

        private void rbKey_CheckedChanged(object sender, EventArgs e)
        {
            if (rbKeyId.Checked)
            {
                tbID.Enabled = true;
                tbLocalPort.Enabled = false;
                chkSerial.Enabled = true;
            }
            else
            {
                tbID.Enabled = false;
                tbLocalPort.Enabled = true;
                chkSerial.Checked = false;
                chkSerial.Enabled = false;
            }

            valuesChanged = true;
        }

        private void tbNumeric_KeyDown(object sender, KeyEventArgs e)
        {
            nonNumberEntered = false;

            // Determine whether the keystroke is a number from the top of the keyboard.
            if (e.KeyCode < Keys.D0 || e.KeyCode > Keys.D9)
            {
                // Determine whether the keystroke is a number from the keypad.
                if (e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9)
                {
                    // Determine whether the keystroke is a backspace.
                    if ((e.KeyCode != Keys.Back))
                    {
                        if ((e.KeyCode != Keys.Decimal))
                        {
                            if ((e.KeyCode != Keys.OemPeriod))
                            {
                                // A non-numerical keystroke was pressed.
                                // Set the flag to true and evaluate in KeyPress event.
                                nonNumberEntered = true;
                            }
                        }
                    }
                }
            }

        }

        private void tbNumeric_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nonNumberEntered == true)
            {
                // Impedir entrada del caracter no numerico, determinado en tbNumeric_KeyDown
                e.Handled = true;
            }

            valuesChanged = true;
        }

        private void tbID_KeyPress(object sender, KeyPressEventArgs e)
        {
            valuesChanged = true;
        }

        private void chkSerial_CheckedChanged(object sender, EventArgs e)
        {
            valuesChanged = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StartHB();
            lbMsg.Text = "Iniciado";
            button1.Enabled = false;
            button2.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            StopHB();
            lbMsg.Text = "Detenido";
            button1.Enabled = true;
            button2.Enabled = false;
       
        }

        // Iniciar automaticamente el envio  despues de cargar el formulario
        private void Form1_Load(object sender, EventArgs e)
        {
            // Cargar db
            if (OpenDatabase())
            {
                // leer valores de DB
                tbIp.Text = _dbRoot.IpServidor;
                tbSPort.Text = _dbRoot.PuertoServidor.ToString() ;
                tbLocalPort.Text = _dbRoot.PuertoLocal.ToString();
                tbInterval.Text = _dbRoot.Intervalo.ToString();
                tbFormat.Text = _dbRoot.Format;
                tbID.Text = _dbRoot.ClientID;
                chkSerial.Checked = _dbRoot.UseSerial;

                if (_dbRoot.UseId)
                    rbKeyId.Checked = true;
                else
                    rbKeyPort.Checked = true;
            }

                // Inicio automatico del emisor de HB
                //button1_Click(this, EventArgs.Empty);
           
        }



        #region Serializacion
        private bool OpenDatabase()
        {
            try
            {
                ConfigData tmpDB;

                if (File.Exists(DatabaseName))
                {
                    tmpDB = DeSerializarDeDisco(DatabaseName);
                }
                else
                {
                    tmpDB = new ConfigData();
                    SerializarADisco(tmpDB, DatabaseName);
                }
                //
                _dbRoot = tmpDB;
                return true;
            }
            catch (Exception )
            {
                return false;
            }
        }

        private void SaveDatabase()
        {
            SerializarADisco(_dbRoot, DatabaseName);
        }

        private void SerializarADisco(ConfigData obj, string file)
        {
            if (obj == null)
                return;

            Serializer.Serialize<ConfigData>(obj, file);
        }

        private ConfigData DeSerializarDeDisco(string file)
        {
            return Serializer.Deserialize<ConfigData>(file);
        }





        #endregion

       
    }
}

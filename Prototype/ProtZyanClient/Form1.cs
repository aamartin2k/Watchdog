using Monitor.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ProtZyanClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        // Acciones, salida de request
        public Action<RemReqClientData> Out_GetClientConfig { get; set; }
        public Action<RemReqSystemData> Out_GetSystemConfig { get; set; }
        public Action<RemReqQueueData> Out_GetQueueInfo { get; set; }
        public Action<RemReqLogFile> Out_GetLogFile { get; set; }
        public Action<RemReqConsoleText> Out_GetConsoleText { get; set; }

        // Request Handlers, entrada de request
        public void In_ClientDataList(RemReplyClientData cldat)
        {
            //ReceiveClient(cldat);
            string line1;
            rtbLog.SelectionColor = Color.White;
            line1 = "Recibida configuracion de Clientes.\n";
            this.rtbLog.AppendText(line1);
           
            rtbLog.SelectionColor = Color.Red;

            line1 = "Cantidad de clientes: " + cldat.Data.List.Length + "\n";
            this.rtbLog.AppendText(line1);
           
            rtbLog.SelectionColor = Color.Green;
            this.rtbLog.AppendText("Una linea mas de prueba...\n");
           

        }

        public void In_SystemConfigData(RemReplySystemData cldat)
        {
            rtbLog.SelectionColor = Color.White;
            this.rtbLog.AppendText("Recibida configuracion de Sistema.\n");
            this.rtbLog.AppendText("Zyan server: " + cldat.Data.ZyanServerName);
        }

        public void In_QueueInfo(RemReplyQueueInfo repl)
        {
            rtbLog.SelectionColor = Color.White;
            this.rtbLog.AppendText("Recibido estado de las colas.\n");
        }

        public void In_LogFile(RemReplyLogFile repl)
        {
            rtbLog.Clear();
            rtbLog.SelectionColor = Color.White;
           
            rtbLog.Clear();
            rtbLog.LoadFile(repl.File, RichTextBoxStreamType.PlainText);
        }

        public void RecibirEstado(EstadoConexion req)
        {
            this.checkBox1.Checked = req.Conectado;
        }

        public void In_Error(RemReplyConsoleText msg)
        {
            rtbLog.SelectionColor = Color.White;
            this.rtbLog.AppendText("Recibido mensaje de error:\n");
            rtbLog.SelectionColor = Color.Red;

            foreach (var item in msg.List)
            {
                this.rtbLog.AppendText(item.Text + "\n");
            }
        }

        public void In_ConsoleText(RemReplyConsoleText msg)
        {
            rtbLog.SelectionColor = Color.White;

            foreach (var item in msg.List)
            {
                this.rtbLog.AppendText(item.Text + "\n");
            }
            rtbLog.ScrollToCaret();
        }

        // controls event handlers
        private void btViewLog_Click(object sender, EventArgs e)
        {
            RemReqLogFile req = new RemReqLogFile("log.txt");

            Out_GetLogFile(req);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBus.Send(new Conectar());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Out_GetClientConfig(new RemReqClientData());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Out_GetSystemConfig(new RemReqSystemData());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Out_GetQueueInfo(new RemReqQueueData());
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Out_GetConsoleText(new RemReqConsoleText());
        }
    }
}

using System;
using System.Collections.Generic;


using Monitor.Shared;


namespace ProtDev
{
    static class Builder
    {
        static DbHandler _dbHandler;
        static GuiForm _guiForm;


        static internal void Build()
        {
            _dbHandler = new DbHandler();
            _guiForm = new GuiForm();
        }

        static internal void Close()
        {
            _dbHandler = null;

            if (_guiForm.Created)
                _guiForm.Close();

            _guiForm = null;
        }

        static internal void LaunchConsole()
        {
            Console.WriteLine("LaunchConsole");

            _dbHandler.OpenDatabase();

            // Mostrar contenido
            SystemConfigData scfg = _dbHandler._dbRoot.SystemConfig;
            Console.WriteLine("1- " + scfg.ZyanServerName + " " + scfg.ZyanServerPort);

            // Update externo config unica, sys
            scfg.ZyanServerName = "Modificado";
            scfg.ZyanServerPort = 20;
            _dbHandler.DbCommit();

            Console.WriteLine("2- " + scfg.ZyanServerName + " " + scfg.ZyanServerPort);


            // Update externo config multiple, client
            Dictionary<string, ClientData> clist = _dbHandler._dbRoot.ClientList;

            if (clist.Count == 0)
            {
                Console.WriteLine("No hay Clientes");
                UpdateClientes(clist);
                _dbHandler.DbCommit();
            }
            else
            {
                Console.WriteLine("Clientes: ");
                foreach (var item in clist.Values)
                {
                    Console.WriteLine(item.Name + "  " + item.Port);
                    Console.WriteLine("  HBs: ");

                    foreach (var hbs in item.HeartBeatList)
                    {
                        Console.WriteLine("       " + hbs);
                    }
                }
            }

            Console.WriteLine("Presione una tecla para terminar...");
            Console.ReadKey();
        }

        static internal void LaunchForm()
        {
            Console.WriteLine("LaunchForm");
            _dbHandler.OpenDatabase();
           

            System.Windows.Forms.Application.Run(new GuiForm());

            _dbHandler.DbClose();
            
        }


        static private void UpdateClientes(Dictionary<string, ClientData> list)
        {
            for (int i = 0; i < 5; i++)
            {
                ClientData cf = new ClientData();
                cf.Name = "Client" + i;
                cf.Port = 3530 + i;

                int qSize = 2;

                for (int j = 0; j < qSize; j++)
                {
                    cf.HeartBeat = DateTime.Now.ToString();
                }

               
                list.Add(cf.Name, cf);
                
            } 
        }
    }
}

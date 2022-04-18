using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Lib;

namespace Server
{
    public class ClientOperations
    {
        TcpClient? tcpClient;
        Thread? threadClient;

        public static event Action<string>? onRun;

        BinaryFormatter bf = new BinaryFormatter();
        //XmlSerializer serRequest = new XmlSerializer(typeof(Lib.Request));
        //XmlSerializer serResponse = new XmlSerializer(typeof(Lib.Responce));


        public ClientOperations(TcpClient client)
        {
            this.tcpClient = client;
            threadClient = new Thread(RunBin);
            threadClient.Start();
        }


        public void RunBin()
        {
            try
            {

                Lib.Request req = (Lib.Request)bf.Deserialize(tcpClient.GetStream());

                Lib.Responce response = new Lib.Responce();

                switch (req.command)
                {
                    case Lib.Command.Set:
                        Commands.Set s = new Commands.Set((Lib.EntityContext.Entity)req.data);
                        if (s.ChekVengor())
                        {
                            s.buildResponce(ref response);
                        }
                        else
                        {
                            response.succces = false;
                            response.code = Lib.ResponceCode.Error;
                            response.StatusTxt = "Vendor alredy exist";
                        }
                        break;

                    case Lib.Command.Get:
                        Commands.Get g = new Commands.Get((Lib.EntityContext.Entity)req.data);
                            g.buildResponce(ref response);
                        break;

                    default:
                        response.succces = false;
                        response.code = Lib.ResponceCode.Error;
                        response.StatusTxt = "Command not Found";
                        break;
                }
                bf.Serialize(tcpClient.GetStream(), response);
                Close();
            }
            catch (Exception ex)
            {
                onRun?.Invoke("Err: " + ex.Message);
                Close();
            }
        }
        void Close()
        {
            tcpClient?.Close();
            ClientConnect.clients.Remove(this);
        }
    }
}

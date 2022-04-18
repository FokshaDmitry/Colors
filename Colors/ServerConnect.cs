using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Lib;

namespace Colors
{
    public class ServerConnect
    {
        public IPAddress ip = IPAddress.Parse("127.0.0.1");
        public int port = 3456;

        public event Action<string>? onError;
        TcpClient tpcClient;
        Thread threadClient;
        NetworkStream stream;

        BinaryFormatter bf = new BinaryFormatter();
        //XmlSerializer serRequest = new XmlSerializer(typeof(Lib.Request));
        //XmlSerializer serResponse = new XmlSerializer(typeof(Lib.Responce));

        public void Connect()
        {
            try
            {
                tpcClient = new TcpClient();
                tpcClient.Connect(new IPEndPoint(ip, port));
                stream = tpcClient.GetStream();
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex.Message);
            }
        }
        public void newSearch(String name, string vendorName)
        {
            Lib.Request request = new Lib.Request();
            request.command = Lib.Command.Get;
            request.data = new Lib.EntityContext.Entity {id = new Guid(), Name = name, VendorName = vendorName, Image = null};
            try
            {
                bf.Serialize(stream, request);
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex.Message);
            }
        }
        public void newAdd(string name, string vendorName, byte[] image)
        {
            Lib.Request request = new Lib.Request();
            request.command = Lib.Command.Set;
            request.data = new Lib.EntityContext.Entity { id = Guid.NewGuid(), Name = name, VendorName = vendorName, Image = image };
            try
            {
                bf.Serialize(stream, request);
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex.Message);
            }
        }
        public void waitResponse(Action<Lib.Responce> onOk)
        {
            Lib.Responce response = (Lib.Responce)bf.Deserialize(stream);
            if (response.succces)
            {
                onOk(response);
            }
            else
            {
                onError?.Invoke(response.StatusTxt);
            }
        }
    }
}

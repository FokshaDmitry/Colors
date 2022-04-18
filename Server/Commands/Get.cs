using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Commands
{
    class Get
    {
        Lib.EntityContext.Entity data;
        AddDBcontext add;
        private byte[]? Image;
        public Get(Lib.EntityContext.Entity data)
        {
            this.data = data;
            add = new AddDBcontext();
        }

        public Lib.Responce buildResponce(ref Lib.Responce responce)
        {
            try
            {

                if (add.DBColors.Count() != 0)
                {
                    foreach (var item in add.DBColors)
                    {
                        if (item.VendorName == data.VendorName && item.Name == data.Name)
                        {
                            Image = File.ReadAllBytes($"D:\\Проекты\\Colors\\Image\\{item.id}.jpg");
                            responce.data = new Lib.EntityContext.Entity { id = item.id, Image = Image, Name = item.Name, VendorName = item.VendorName};
                            responce.succces = true;
                            responce.code = Lib.ResponceCode.Ok;
                            responce.StatusTxt = "Search Ok";
                            return responce;
                        }
                        else
                        {
                            responce.succces = false;
                            responce.code = Lib.ResponceCode.Error;
                            responce.StatusTxt = "Vendor dont find";
                        }
                    }
                }
                else
                {
                    responce.succces = false;
                    responce.code = Lib.ResponceCode.Error;
                    responce.StatusTxt = "Database is Empty";
                }
                return responce;

            }
            catch (Exception)
            {
                responce.succces = false;
                responce.code = Lib.ResponceCode.Error;
                responce.StatusTxt = "Add False";
                return responce;
            }

        }
    }
}

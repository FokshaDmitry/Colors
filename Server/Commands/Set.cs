using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib;

namespace Server.Commands
{
    class Set
    {
        Lib.EntityContext.Entity data;
        AddDBcontext add;

        public Set(Lib.EntityContext.Entity data)
        {
            this.data = data;
            add = new AddDBcontext();
        }

        public bool ChekVengor()
        {
            bool tmpName = false;
            bool tmpVName = false;
            if (add.DBColors.Count() != 0)
            {
                foreach (var item in add.DBColors)
                {
                    if (item.VendorName == data.VendorName)
                    {
                        tmpVName = true;
                    }
                    if (item.Name == data.Name)
                    {
                        tmpName = true;
                    }
                }
            }
            else
            {
                return true;
            }
            if (tmpName == true && tmpVName == true) return false;

            return true;
        }
        public Lib.Responce buildResponce(ref Lib.Responce responce)
        {
            try
            {
                add.DBColors.Add(data);
                add.SaveChanges();
                File.WriteAllBytes($"D:\\Проекты\\Colors\\Image\\{data.id}.jpg", data.Image);
                
                responce.succces = true;
                responce.code = Lib.ResponceCode.Ok;
                responce.StatusTxt = "Add Ok";
            }
            catch (Exception)
            {
                responce.succces = false;
                responce.code = Lib.ResponceCode.Error;
                responce.StatusTxt = "Add False";
            }
            return responce;
        }
    }
}

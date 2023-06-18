using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACS.Infrastructure
{
    public static class NoFoto
    {
        public  static byte[] NoFotoByte
        {
            get => File.ReadAllBytes($@"{Directory.GetCurrentDirectory()}\Resources\Image\No_Foto.png");
        }
    }
}

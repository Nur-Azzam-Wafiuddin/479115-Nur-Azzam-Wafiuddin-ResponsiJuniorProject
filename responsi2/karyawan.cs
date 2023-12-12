using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace responsi2
{
    internal class karyawan : departemen
    {
        private int id_karyawan;
        private string nama;

        public int Id_karyawan
        {
            get { return id_karyawan; }
            set { id_karyawan = value; }
        }

        public string Nama
        {
            get { return nama; }
            set { nama = value; }
        }
    }
}

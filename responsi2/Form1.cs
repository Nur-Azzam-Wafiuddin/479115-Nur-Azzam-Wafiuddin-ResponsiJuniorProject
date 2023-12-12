using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace responsi2
{
    public partial class Form1 : Form
    {
        private NpgsqlCommand cmd;
        string constring = "Server=localhost;Port=5432;Database=postgres;Username=postgres;Password=informatika";
        private NpgsqlConnection con = new NpgsqlConnection();
        private string sql = "";

        List<string> dropDown;

        public DataTable dt;
        private DataGridViewRow row;
        public Form1()
        {
            InitializeComponent();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                sql = "Insert into karyawan (nama, id_dep) values (@newNama, @newid_departemen)";
                cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("newNama", txt_Nama.Text);
                cmd.Parameters.AddWithValue("newid_departemen", int.Parse(txt_Dep.Text));

                cmd.ExecuteNonQuery();
                MessageBox.Show("Data Berhasil Ditambahkan");
                txt_Dep.Text = txt_Nama.Text = "";
                Form1_Load(sender, EventArgs.Empty);
                con.Close();

            }catch(Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message);

            }
            con.Close();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                sql = "Update karyawan set nama = @nama, id_dep = @id_dep where id_karyawan = @id_karyawan;";
                cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("nama", txt_Nama.Text);
                cmd.Parameters.AddWithValue("id_dep", int.Parse(txt_Dep.Text));
                cmd.Parameters.AddWithValue("id_karyawan", int.Parse(row.Cells["id_karyawan"].Value.ToString()));
                cmd.ExecuteNonQuery();
                MessageBox.Show("Berhasil Edit");
                txt_Dep.Text = txt_Nama.Text = "";
                Form1_Load(sender, EventArgs.Empty);
                con.Close();
            }catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            con.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try {
                con.Open();
                sql = "DELETE from karyawan where id_karyawan = @currentID";
                cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("currentID", int.Parse(row.Cells["id_karyawan"].Value.ToString()));
                MessageBox.Show("Berhasil Hapus");
                cmd.ExecuteNonQuery();
                con.Close();
                txt_Dep.Text = txt_Nama.Text = "";
                Form1_Load(sender, EventArgs.Empty);

            }
            catch(Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message);
            }
            con.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            con = new NpgsqlConnection(constring);
            try
            {
                con.Open();
                sql = "select nama_dep from departemen";
                cmd = new NpgsqlCommand(sql, con);
                string read = "";
                dropDown = new List<string>();
                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    read = reader.GetString(0);
                    dropDown.Add(read);
                }
                comboBox1.DataSource = dropDown;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message);
            }

            try
            {
                con.Open();
                sql = "SELECT k.id_karyawan, k.nama, k.id_dep, d.nama_dep from karyawan k inner join departemen d on k.id_dep = d.id_dep";
                cmd = new NpgsqlCommand(sql, con);
                dt = new DataTable();
                NpgsqlDataReader reader = cmd.ExecuteReader();
                dt.Load(reader);
                dataGridView2.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message);

            }
            con.Close();
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                row = dataGridView2.Rows[e.RowIndex];
                txt_Nama.Text = row.Cells["nama"].Value.ToString();
                txt_Dep.Text = row.Cells["id_dep"].Value.ToString();
                comboBox1.Text = dropDown[int.Parse(txt_Dep.Text) - 1];

            }
            catch(Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message);
            }
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            Trace.WriteLine(comboBox1.Text);
            Trace.WriteLine(comboBox1.SelectedIndex.ToString());
            int selectedDep = comboBox1.SelectedIndex + 1;
            txt_Dep.Text = selectedDep.ToString();
        }
    }
}

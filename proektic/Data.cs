using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace proektic
{
    public partial class Data : Form
    {
        private IConfigurationRoot _configuration;
        private System.Windows.Forms.Button lastButtonPressed;
        public Data()
        {
            InitializeComponent();
            _configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
            string connectionString = _configuration.GetConnectionString("MyConnectionString");
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO users (name_user, password_user) VALUES (@name, @password)", connection))
                {
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "select * from logi";
                    NpgsqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        dataGridView1.DataSource = dt;
                    }
                    cmd.Dispose();
                    connection.Close();
                }
            }


 

        }

        private void button1_Click(object sender, EventArgs e)
        {
            lastButtonPressed = (System.Windows.Forms.Button)sender;
            string connectionString = _configuration.GetConnectionString("MyConnectionString");
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                NpgsqlCommand cmd = new NpgsqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT adress, COUNT(adress) \r\nFROM logi\r\nGROUP BY adress;";
                NpgsqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    dataGridView1.DataSource = dt;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            lastButtonPressed = (System.Windows.Forms.Button)sender;
            string connectionString = _configuration.GetConnectionString("MyConnectionString");
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                NpgsqlCommand cmd = new NpgsqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT vrema, COUNT(vrema) \r\nFROM logi\r\nGROUP BY vrema;";
                NpgsqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    dataGridView1.DataSource = dt;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void Data_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string filePath = "logi.log";
            _configuration = new ConfigurationBuilder()
                           .AddJsonFile("appsettings.json")
                           .Build();
            string connectionString = _configuration.GetConnectionString("MyConnectionString");
            string[] lines = File.ReadAllLines(filePath);
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();
            foreach (string line in lines)
            {
                string[] parts = line.Split(' ');
                string ipAddress = parts[0];
                string timestamp = parts[1] + " " + parts[2];
                string request = parts[3] + " " + parts[4] + " " + parts[5];
                string statusCode = parts[6];
                NpgsqlCommand cmd = new NpgsqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"INSERT INTO logi (adress, vrema, http_method, http_status) VALUES ('{ipAddress}', '{timestamp}', '{request}', '{statusCode}');";
                cmd.ExecuteNonQuery();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            _configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               .Build();
            string connectionString = _configuration.GetConnectionString("MyConnectionString");
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                NpgsqlCommand cmd = new NpgsqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "TRUNCATE table logi";
                cmd.ExecuteNonQuery();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            lastButtonPressed = (System.Windows.Forms.Button)sender;
            _configuration = new ConfigurationBuilder()
                           .AddJsonFile("appsettings.json")
                           .Build();
            string connectionString = _configuration.GetConnectionString("MyConnectionString");
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();
            NpgsqlCommand comm = new NpgsqlCommand();
            comm.Connection = conn;
            comm.CommandType = CommandType.Text;
            comm.CommandText = $"SELECT * FROM logi\r\nWHERE vrema >= '{dateTimePicker1.Text}' AND vrema <= '{dateTimePicker2.Text}';";
            NpgsqlDataReader dr = comm.ExecuteReader();
            if (dr.HasRows)
            {
                DataTable dt = new DataTable();
                dt.Load(dr);
                dataGridView1.DataSource = dt;
                comm.ExecuteNonQuery();
            }

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            dateTimePicker1.CustomFormat = "yyyy/MM/dd";
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            dateTimePicker2.CustomFormat = "yyyy/MM/dd";
        }

        private void button7_Click(object sender, EventArgs e)
        {

            foreach (Control control in Controls)
            {
                if (control is System.Windows.Forms.Button button)
                {
                    button.Click += button1_Click;
                    button.Click += button2_Click;
                    button.Click += button5_Click;
                }
            }

            button7 = new System.Windows.Forms.Button();
            button7.Click += button7_Click;
            Controls.Add(button7);







            _configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
                .Build();
            string connectionString = _configuration.GetConnectionString("MyConnectionString");
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = conn;

            if (lastButtonPressed != null)
            {
                if (lastButtonPressed.Text == "Группировка по ip")
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT adress, COUNT(adress) \r\nFROM logi\r\nGROUP BY adress;";
                    File.WriteAllText("api.json", "");
                    using (var reader = cmd.ExecuteReader())
                    {
                        int a = 1;
                        while (reader.Read())
                        {
                            var jsonObject = new
                            {
                                ip = reader.GetString(0),
                                count = reader.GetInt32(1)
                            };
                            string json = JsonConvert.SerializeObject(jsonObject);
                            string data = $"{jsonObject}{Environment.NewLine}";
                            File.AppendAllText("api.json", data);
                            a++;
                        }
                    }
                }
                else if (lastButtonPressed.Text == "Группировка по дате")
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT vrema, COUNT(vrema) \r\nFROM logi\r\nGROUP BY vrema;";
                    File.WriteAllText("api.json", "");
                    using (var reader = cmd.ExecuteReader())
                    {
                        int a = 1;
                        while (reader.Read())
                        {
                            var jsonObject = new
                            {
                                vrema = reader.GetDateTime(0),
                                count = reader.GetInt32(1)
                            };
                            string json = JsonConvert.SerializeObject(jsonObject);
                            string data = $"{jsonObject}{Environment.NewLine}";
                            File.AppendAllText("api.json", data);
                            a++;
                        }
                    }
                }
                else if (lastButtonPressed.Text == "Выборка по дате")
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = $"SELECT * FROM logi\r\nWHERE vrema >= '{dateTimePicker1.Text}' AND vrema <= '{dateTimePicker2.Text}';";
                    File.WriteAllText("api.json", "");
                    using (var reader = cmd.ExecuteReader())
                    {
                        int a = 1;
                        while (reader.Read())
                        {
                            var jsonObject = new
                            {
                                ip = reader.GetString(1),
                                vrema = reader.GetDateTime(2),
                                http_method = reader.GetString(3),
                                http_status = reader.GetInt32(4)
                            };
                            string json = JsonConvert.SerializeObject(jsonObject);
                            string data = $"{jsonObject}{Environment.NewLine}";
                            File.AppendAllText("api.json", data);
                            a++;
                        }
                    }
                }
                else
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "cmd.CommandText = \"SELECT * from users\";";
                    File.WriteAllText("api.json", "");
                    using (var reader = cmd.ExecuteReader())
                    {
                        int a = 1;
                        while (reader.Read())
                        {
                            var jsonObject = new
                            {
                                ip = reader.GetString(1),
                                vrema = reader.GetDateTime(2),
                                http_method = reader.GetString(3),
                                http_status = reader.GetInt32(4)
                            };
                            string json = JsonConvert.SerializeObject(jsonObject);
                            string data = $"{jsonObject}{Environment.NewLine}";
                            File.AppendAllText("api.json", data);
                            a++;
                        }
                    }
                }
            }
        }
    }
}

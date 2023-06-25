using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace proektic
{
    public partial class Log_in : Form
    {
        private IConfigurationRoot _configuration;
        public Log_in()
        {
            InitializeComponent();
            _configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
                .Build();
            string connectionString = _configuration.GetConnectionString("MyConnectionString");
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT name_user from users";
            cmd.CommandText = "SELECT password_user from users";
            using (var reader = cmd.ExecuteReader())
            {
                int a = 1;
                while (reader.Read())
                {
                    string name_user = reader.GetString(0);
                    string password_user = reader.GetString(0);

                    
                    string data = $"{name_user}:{password_user}{Environment.NewLine}";
                    File.AppendAllText("users.txt", data);
                    a++;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Data f2 = new Data();
            f2.Owner = this;
            f2.Show();
            File.WriteAllText("users.txt", "");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string filePath = "users.txt";
            string fileContents = File.ReadAllText(filePath);

            string[] lines = fileContents.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            //foreach (string line in lines)
            //{
            //    string[] parts = line.Split(':');
            //    if (parts.Length >= 2)
            //    {
            //        string login = parts[0].Trim();
            //        string password = parts[1].Trim();

            //        if (login == textBox1.Text && password == textBox2.Text)
            //        {
            //            button1.Enabled = true;
            //            break;
            //        }

            //    }
            //}
            bool isAuthenticated = false;

            for (int i = 0; i < lines.Length; i++)
            {
                string[] parts = lines[i].Split(':');
                if (parts.Length >= 2)
                {
                    string login = parts[0].Trim();
                    string password = parts[1].Trim();

                    if (login == textBox1.Text && password == textBox2.Text)
                    {
                        isAuthenticated = true;
                        break;
                    }
                }
            }

            if (isAuthenticated)
            {
                button1.Enabled = true;
            }


        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

            if (textBox1.Text.Length <= 5 || textBox2.Text.Length <= 8)
            {
                    button1.Enabled = true;
            }
            
            string filePath = "users.txt";
            string fileContents = File.ReadAllText(filePath);

            string[] lines = fileContents.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in lines)
            {
                string[] parts = line.Split(':'); 
                if (parts.Length >= 2)
                {
                    string login = parts[0].Trim();
                    string password = parts[1].Trim();

                    if (login == textBox1.Text)
                    {
                        textBox2.Enabled = true;

                        if (password == textBox2.Text)
                        {
                            button1.Enabled = true;
                        }
                    }
                }
            }
        }

        private void Log_in_Load(object sender, EventArgs e)
        {
            button1.Enabled=false;
            
        }
    }
}

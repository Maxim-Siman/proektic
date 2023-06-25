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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace proektic
{
    public partial class Log_up : Form
    {
        private IConfigurationRoot _configuration;

        public Log_up()
        {
            InitializeComponent();
            button1.Enabled = false;
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connectionString = _configuration.GetConnectionString("MyConnectionString");

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                { 
                    connection.Open();

                    string name = textBox1.Text;
                    string password = textBox2.Text;

                    using (NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO users (name_user, password_user) VALUES (@name, @password)", connection))
                    {
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@password", password);
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                    }

                    

                    Data f2 = new Data();
                    f2.Owner = this;
                    f2.Show();
                }
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            
            if (textBox2.Text.Length < 8 || textBox1.Text.Length < 5)
            {
                    button1.Enabled = false;
            }
            else {
                button1.Enabled = true;
            }
        }

        private void Log_up_Load(object sender, EventArgs e)
        {

        }
    }
}

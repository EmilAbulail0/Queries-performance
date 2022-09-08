using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OracleClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cassandra;

namespace Train
{
    public partial class Sensor_form : Form
    {
        int current_id = 1;
        public Sensor_form()
        {
            InitializeComponent();
            this.comboBox1.Items.Add(true);
            this.comboBox1.Items.Add(false);
            var rs1 = GUI.session.Execute("select DISTINCT sensor_id from ptect_fdc.sensor");
            DataTable data1 = new DataTable("data1");
            //to create the column and schema
            DataColumn equipment_id = new DataColumn("sensor_id", typeof(Int32));
            data1.Columns.Add(equipment_id);
            foreach (var row in rs1)
            {
                Int32 Equipment_id = row.GetValue<Int32>("sensor_id");
                data1.Rows.Add(Equipment_id);
            }
            comboBox2.ValueMember = "sensor_id";
            comboBox2.DataSource = data1;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            OracleCommand cmd = new OracleCommand("select ptect_fdc.sensor_sequence.nextval from dual", GUI.conn);
            OracleDataAdapter adp = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            if (ds.Tables.Count > 0)
            {
                current_id = int.Parse(ds.Tables[0].Rows[0][0].ToString())+1;
            }

            //conn.Open();
            string isValid = comboBox1.Text;
            int isValidValue = 1;
            if (isValid == "True")
            {
                isValidValue = 1;
            }
            else if (isValid == "False")
            {
                isValidValue = 0;
            }
            //string id = textBox1.Text;
            string name = textBox4.Text;
            string alias = textBox3.Text;
            
            DialogResult dialogResult = MessageBox.Show("\tSensor Details: \n\t ID: " + current_id + "\n\t Name: " + name + "\n\t Alias: " + alias + "\n\t IsValid: " + isValid, "Sensor", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                var rs = GUI.session.Execute($"INSERT INTO ptect_fdc.sensor (sensor_id, sensor_alias,sensor_isvalid,sensor_name) VALUES ({current_id} , '{alias}', {isValidValue} , '{name}');");
                OracleCommand cmd1 = new OracleCommand($"INSERT INTO ptect_fdc.sensor (sensor_id, sensor_alias,sensor_isvalid,sensor_name) VALUES ({current_id} , '{alias}', {isValidValue} , '{name}')", GUI.conn);
                OracleDataReader reader = cmd1.ExecuteReader();
                updateData();
            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
            }

            // conn.Close();
        }

        private void updateData()
        {
            var rs1 = GUI.session.Execute("select DISTINCT sensor_id from ptect_fdc.sensor");
            DataTable data1 = new DataTable("data1");
            //to create the column and schema
            DataColumn equipment_id = new DataColumn("sensor_id", typeof(Int32));
            data1.Columns.Add(equipment_id);
            foreach (var row in rs1)
            {
                Int32 Equipment_id = row.GetValue<Int32>("sensor_id");
                data1.Rows.Add(Equipment_id);
            }
            comboBox2.ValueMember = "sensor_id";
            comboBox2.DataSource = data1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text == "")
            {
                MessageBox.Show("please enter a sensor id");
            }
            else
            {
                string sen_id = comboBox2.Text;
                OracleCommand cmd1 = new OracleCommand($"select * from ptect_fdc.sensor where sensor_id = {sen_id} ", GUI.conn);
                OracleDataAdapter adp1 = new OracleDataAdapter(cmd1);
                DataSet ds1 = new DataSet();
                adp1.Fill(ds1);
                //comboBox1.ValueMember = "eq_isvalid";
                if (ds1.Tables.Count > 0)
                {
                    dataGridView1.DataSource = ds1.Tables[0].DefaultView;
                    /*textBox4.Text = ds1.Tables[0].Rows[0][1].ToString();
                    textBox3.Text = ds1.Tables[0].Rows[0][2].ToString();
                    comboBox1.DataSource = ds1.Tables[0];*/
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text == "")
            {
                MessageBox.Show("please enter sensor id");
            }
            else
            {
                string sensor_id = comboBox2.Text;
                var rs = GUI.session.Execute($"delete from ptect_fdc.sensor where sensor_id={sensor_id};");
                OracleCommand cmd = new OracleCommand($"delete from ptect_fdc.sensor where sensor_id={sensor_id}", GUI.conn);
                OracleDataReader reader = cmd.ExecuteReader();
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
    }
    }


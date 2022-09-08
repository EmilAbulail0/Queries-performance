using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OracleClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;


namespace Train
{
    public partial class Data_form : Form
    {
        public Data_form()
        {
            InitializeComponent();
            /*OracleCommand cmd = new OracleCommand("select DISTINCT equipment_id from ptect_fdc.equipment order by equipment_id", GUI.conn);
            OracleDataAdapter adp = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            comboBox2.ValueMember = "equipment_id";
            if (ds.Tables.Count > 0)
            {
                comboBox2.DataSource = ds.Tables[0];
            }
            OracleCommand cmd1 = new OracleCommand("select DISTINCT sensor_id from ptect_fdc.sensor order by sensor_id", GUI.conn);
            OracleDataAdapter adp1 = new OracleDataAdapter(cmd1);
            DataSet ds1 = new DataSet();
            adp.Fill(ds1);
            comboBox3.ValueMember = "sensor_id";
            if (ds.Tables.Count > 0)
            {
                comboBox3.DataSource = ds.Tables[0];
            }*/
            var rs1 = GUI.session.Execute("select DISTINCT equipment_id from ptect_fdc.equipment");
            var rs2 = GUI.session.Execute("select DISTINCT sensor_id from ptect_fdc.sensor");

            DataTable data1 = new DataTable("data1");
            //to create the column and schema
            DataColumn equipment_id = new DataColumn("equipment_id", typeof(Int32));
            data1.Columns.Add(equipment_id);

            DataTable data2 = new DataTable("data2");
            //to create the column and schema
            DataColumn sensor_id = new DataColumn("sensor_id", typeof(Int32));
            data2.Columns.Add(sensor_id);

            foreach (var row in rs1)
            {
                Int32 Equipment_id = row.GetValue<Int32>("equipment_id");
                data1.Rows.Add(Equipment_id);
            }
            foreach (var row in rs2)
            {
                Int32 Sensor_id = row.GetValue<Int32>("sensor_id");
                data2.Rows.Add(Sensor_id);
            }
            comboBox2.ValueMember = "equipment_id";
            comboBox3.ValueMember = "sensor_id";
            comboBox2.DataSource = data1;
            comboBox3.DataSource = data2;

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string eq_id = comboBox2.Text;
            string sen_id = comboBox3.Text;
            string sen_val = textBox2.Text;
            string start_date =dateTimePicker2.Text;
            string end_date =dateTimePicker1.Text;

            

            DialogResult dialogResult = MessageBox.Show("\tData Details: \n\t Equipment ID : " + eq_id + "\n\t Sensor ID : " + sen_id + "\n\t Start Date : " + start_date + "\n\t End Date : " + end_date+"\n\t Sensor Value :"+sen_val, "Data", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                var rs = GUI.session.Execute($"INSERT INTO ptect_fdc.collected_data (Equipment_id,Sensor_id,Start_time,End_time,Sensor_value) VALUES ({eq_id} , {sen_id}, '{start_date}' , '{end_date}',{sen_val});");
                OracleCommand cmd = new OracleCommand($"INSERT INTO ptect_fdc.collected_data (Equipment_id,Sensor_id,Start_time,End_time,Sensor_value) VALUES ({eq_id}  ,{sen_id}, (TO_DATE('{start_date}', 'yyyy/mm/dd hh24:mi:ss')) , (TO_DATE('{end_date}', 'yyyy/mm/dd hh24:mi:ss')) , {sen_val})", GUI.conn);
                OracleDataReader reader = cmd.ExecuteReader();
            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedValue.ToString() == "" || comboBox3.SelectedValue.ToString() == "")
            {
                MessageBox.Show("please enter equipment id and sensor id");
            }
            else
            {
                string sen_id = comboBox3.Text;
                string equipment_id = comboBox2.Text;
                OracleCommand cmd1 = new OracleCommand($"select * from ptect_fdc.collected_data where sensor_id = {sen_id} AND equipment_id ={equipment_id} ", GUI.conn);
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
            if (comboBox2.SelectedValue.ToString() == "" || comboBox3.SelectedValue.ToString() == "")
            {
                MessageBox.Show("please enter equipment id and sensor id");
            }
            else
            {
                string sen_id = comboBox3.Text;
                string equipment_id = comboBox2.Text;
                var rs = GUI.session.Execute($"delete from ptect_fdc.sensor where sensor_id={sen_id} AND equipment_id={equipment_id};");
                OracleCommand cmd = new OracleCommand($"delete from ptect_fdc.sensor where sensor_id={sen_id} AND equipment_id={equipment_id}", GUI.conn);
                OracleDataReader reader = cmd.ExecuteReader();
            }
        
    }
    }
}

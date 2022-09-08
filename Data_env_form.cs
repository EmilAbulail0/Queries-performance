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
    public partial class Data_env_form : Form
    {
        int current_id = 1;

        public Data_env_form()
        {
            InitializeComponent();          
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

            var rs3 = GUI.session.Execute("select varenv_id from ptect_fdc.collected_data_env");
            DataTable data3 = new DataTable("data3");
            //to create the column and schema
            DataColumn varenv_id = new DataColumn("varenv_id", typeof(Int64));
            data3.Columns.Add(varenv_id);
            foreach (var row in rs3)
            {
                Int64 id = row.GetValue<Int64>("varenv_id");
                data3.Rows.Add(id);
            }
            comboBox1.ValueMember = "varenv_id";
            comboBox1.DataSource = data3;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OracleCommand cmd = new OracleCommand("select ptect_fdc.env_sequence.nextval from dual", GUI.conn);
            OracleDataAdapter adp = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            if (ds.Tables.Count > 0)
            {
                current_id = int.Parse(ds.Tables[0].Rows[0][0].ToString()) + 1;
            }

            string eq_id = comboBox2.Text;
            string sen_id = comboBox3.Text;
            string traceval = textBox1.Text;

            DialogResult dialogResult = MessageBox.Show("\tData env Details: \n\t Equipment id: " + eq_id + "\n\t Sensor id: " + sen_id + "\n\t VarEnv id: " + current_id + "\n\t Trace Value: " + traceval, "Sensor", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                var rs = GUI.session.Execute($"INSERT INTO ptect_fdc.collected_data_env (equipment_id, sensor_alias,varEnv_id,tracevalue) VALUES ({eq_id} , {sen_id}, {current_id} , '{traceval}');");
                OracleCommand cmd1 = new OracleCommand($"INSERT INTO ptect_fdc.collected_data_env (equipment_id, sensor_alias,varEnv_id,tracevalue)  VALUES ({eq_id} , {sen_id}, {current_id} , '{traceval}')", GUI.conn);
                OracleDataReader reader = cmd1.ExecuteReader();
            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
            }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            // open file dialog   
            OpenFileDialog open = new OpenFileDialog();
            // image filters  
            open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
            if (open.ShowDialog() == DialogResult.OK)
            {
                // image file path  
                textBox1.Text = open.FileName;
            }
        }
    }
}

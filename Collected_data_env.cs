using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OracleClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Train
{
    public partial class Collected_data_env : Form
    {
        public Collected_data_env()
        {
            InitializeComponent();
            /* var rs1 = GUI.session.Execute("select DISTINCT equipment_id from ptect_fdc.equipment");
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
            //using cassandra
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
               comboBox4.ValueMember = "varenv_id";
               comboBox4.DataSource = data3;
             */

            OracleCommand cmd = new OracleCommand($"select varenv_id from ptect_fdc.collected_data_env", GUI.conn);
            OracleDataAdapter adp = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            comboBox4.ValueMember = "varenv_id";
            if (ds.Tables.Count > 0)
            {
                comboBox4.DataSource = ds.Tables[0].DefaultView;

            }
            OracleCommand cmd0 = new OracleCommand($"select DISTINCT equipment_id from ptect_fdc.collected_data_env", GUI.conn);
            OracleDataAdapter adp0 = new OracleDataAdapter(cmd0);
            DataSet ds0 = new DataSet();
            adp0.Fill(ds0);
            comboBox2.ValueMember = "equipment_id";
            if (ds0.Tables.Count > 0)
            {
                comboBox2.DataSource = ds0.Tables[0].DefaultView;

            }
            OracleCommand cmd1 = new OracleCommand($"select DISTINCT sensor_id from ptect_fdc.collected_data_env", GUI.conn);
            OracleDataAdapter adp1 = new OracleDataAdapter(cmd1);
            DataSet ds1 = new DataSet();
            adp1.Fill(ds1);
            comboBox3.ValueMember = "sensor_id";
            if (ds1.Tables.Count > 0)
            {
                comboBox3.DataSource = ds1.Tables[0].DefaultView;

            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataTable data1 = new DataTable("data1");
            //to create the column and schema
            DataColumn tracevalue = new DataColumn("tracevalue", typeof(MemoryStream));
            data1.Columns.Add(tracevalue);
            var rs = GUI.session.Execute($"select tracevalue from ptect_fdc.collected_data_env where varenv_id  ={comboBox4.Text}  ALLOW FILTERING");

            foreach (var row in rs)
            {
                MemoryStream ms = new MemoryStream(row.GetValue<byte[]>("tracevalue"));
                pictureBox1.Image = new Bitmap(ms);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                
            }
           /* OracleCommand cmd = new OracleCommand($"select tracevalue from ptect_fdc.collected_data_env where varenv_id  ={comboBox4.Text}", GUI.conn);
            OracleDataAdapter adp = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            if (ds.Tables.Count > 0)
            {
                MemoryStream ms = new MemoryStream((byte[])ds.Tables[0].Rows[0]["tracevalue"]);               
                pictureBox1.Image = new Bitmap(ms);
                pictureBox1.SizeMode= PictureBoxSizeMode.StretchImage;  
            }*/
        }
    }
}

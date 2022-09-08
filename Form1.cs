using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OracleClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cassandra;

namespace Train
{
    public partial class GUI : Form
    {
        public static bool cassandra_button_click = false;
        public static bool oracle_button_click = false;
        public static bool insert_oracle = false;
        public static bool insert_cassandra = false;

        
        //List<Train.Record> dataList = new List<Train.Record>();
        public static ISession session;
        public static OracleConnection conn = new OracleConnection();
        public static string selectedTable;
        public static string current_equipment_id;
        public static string current_sensor_id;
        public static string current_var_id;
        private int numberOfBatchFiles =11;

        public GUI()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.AutoSize = true;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            cassandra_button_click = true;
            button1.Enabled = false;
            try
            {

                var cluster = Cluster.Builder()
                            .AddContactPoint("127.0.0.1")
                            .Build();
                //Create connections to the nodes using a keyspace
                session = cluster.Connect("ptect_fdc");
                MessageBox.Show("Connected to cassandra succssesfully");
                
            }
            catch (NoHostAvailableException ex)
            {
                MessageBox.Show("Please check cassandra connection settings");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (cassandra_button_click == false)
            {
                MessageBox.Show("Please connect to cassandra first");
            }
            else
            {
                DataTable data = new DataTable("data");
                //to create the column and schema
                DataColumn equipment_id = new DataColumn("EQUIPMENT_ID", typeof(Int64));
                data.Columns.Add(equipment_id);
                DataColumn sensor_id = new DataColumn("SENSOR_ID", typeof(Int64));
                data.Columns.Add(sensor_id);
                DataColumn start_time = new DataColumn("START_TIME", typeof(DateTime));
                data.Columns.Add(start_time);
                DataColumn end_time = new DataColumn("END_TIME", typeof(DateTime));
                data.Columns.Add(end_time);
                DataColumn sensor_value = new DataColumn("SENSOR_VALUE", typeof(float));
                data.Columns.Add(sensor_value);

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                var rs = session.Execute("select * from ptect_fdc.collected_data ");
                foreach (var row in rs)
                {
                    Int64 Equipment_id = row.GetValue<Int64>("equipment_id");
                    Int64 Sensor_id = row.GetValue<Int64>("sensor_id");
                    DateTime Start_date = row.GetValue<DateTime>("start_time");
                    DateTime? End_date = row.GetValue<DateTime?>("end_time");
                    float Sensor_value = row.GetValue<float>("sensor_value");
                    //do something with the value
                   // Record record = new Train.Record(Equipment_id.ToString(), Sensor_id.ToString(), Start_date.ToString(), End_date.ToString(), Sensor_value.ToString());
                    data.Rows.Add(Equipment_id, Sensor_id, Start_date, End_date, Sensor_value);
                    //dataList.Add(record);
                }
                dataGridView1.DataSource = data;
                stopwatch.Stop();
                label4.Text = (stopwatch.ElapsedMilliseconds.ToString() + " ms");

            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            insert_cassandra = true;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            //var process = System.Diagnostics.Process.Start(@"C:\Users\Emil Abulail\Desktop\Birzeit university\Training\Train\cassandra.bat");
            //process.WaitForExit();
            for (int i = 0; i < numberOfBatchFiles; i++)
            {
                string text = System.IO.File.ReadAllText($"C:\\Users\\Emil Abulail\\Desktop\\Birzeit university\\Training\\Train\\batch{i}.txt");
                var rs = session.Execute(text);
            }
            string text1 = System.IO.File.ReadAllText($"C:\\Users\\Emil Abulail\\Desktop\\Birzeit university\\Training\\Train\\eq_batch.txt");
            var rs1 = session.Execute(text1);
            string text2 = System.IO.File.ReadAllText($"C:\\Users\\Emil Abulail\\Desktop\\Birzeit university\\Training\\Train\\sen_batch.txt");
            var rs2 = session.Execute(text2);
            stopwatch.Stop();
            label7.Text = (stopwatch.ElapsedMilliseconds.ToString() + " ms");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (cassandra_button_click == false)
            {
                MessageBox.Show("Please connect to cassandra first");
            }
            else
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                var rs = session.Execute("TRUNCATE ptect_fdc.collected_data;");
                var rs1 = session.Execute("TRUNCATE ptect_fdc.equipment;");
                var rs2 = session.Execute("TRUNCATE ptect_fdc.sensor;");
                var rs3 = session.Execute("TRUNCATE ptect_fdc.collected_data_env;");

                stopwatch.Stop();
                label11.Text = (stopwatch.ElapsedMilliseconds.ToString() + " ms");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                oracle_button_click = true;
                button5.Enabled = false;
                string oradb = "Data Source=  (DESCRIPTION ="
       + "(ADDRESS = (PROTOCOL = TCP)(HOST = 127.0.0.1)(PORT = 1521))" +
        "(CONNECT_DATA =" +
          "(SERVER = DEDICATED)" +
          "(SERVICE_NAME = orcl3)" +
        ")" +
      ");User Id=ptect_fdc;password=x;";
                conn.ConnectionString = oradb;
                conn.Open();
                MessageBox.Show("connected to Oracle succssesfully");
            }
            catch (NoHostAvailableException ex)
            {
                MessageBox.Show("Please check oracle connection settings");

            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (oracle_button_click == false)
            {
                MessageBox.Show("Please connect to oracle first");
            }
            else
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                OracleCommand cmd = new OracleCommand("select * from ptect_fdc.collected_data", conn);
                OracleDataAdapter oda = new OracleDataAdapter(cmd);
                DataSet ds = new DataSet();
                oda.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    dataGridView2.DataSource = ds.Tables[0].DefaultView;
                }
                stopwatch.Stop();
                label5.Text = (stopwatch.ElapsedMilliseconds.ToString() + " ms");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            insert_oracle = true;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var process = System.Diagnostics.Process.Start(@"C:\Users\Emil Abulail\Desktop\Birzeit university\Training\Train\oracle.bat");
            process.WaitForExit();

           /* //insert manually
            string text = System.IO.File.ReadAllText($"C:\\Users\\Emil Abulail\\Desktop\\Birzeit university\\Training\\Train\\cassandra.txt");
            OracleCommand insertCmd = new OracleCommand(text, GUI.conn);
            OracleDataReader reader = insertCmd.ExecuteReader();
            */

            stopwatch.Stop();
            label9.Text = (stopwatch.ElapsedMilliseconds.ToString() + " ms");
            OracleCommand cmd0 = new OracleCommand($"select max(equipment_id) from equipment", GUI.conn);
            OracleCommand cmd1 = new OracleCommand($"select max(sensor_id) from sensor", GUI.conn);
            OracleCommand cmd2 = new OracleCommand($"select max(varEnv_id) from collected_data_env", GUI.conn);

            OracleDataAdapter adp = new OracleDataAdapter(cmd1);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            if (ds.Tables.Count > 0)
            {
                 current_sensor_id = ds.Tables[0].Rows[0][0].ToString();
            }
            OracleDataAdapter adp1 = new OracleDataAdapter(cmd0);
            DataSet ds1 = new DataSet();
            adp1.Fill(ds1);
            if (ds1.Tables.Count > 0)
            {
                 current_equipment_id = ds1.Tables[0].Rows[0][0].ToString();
            }
            OracleDataAdapter adp2 = new OracleDataAdapter(cmd2);
            DataSet ds2 = new DataSet();
            adp1.Fill(ds2);
            if (ds1.Tables.Count > 0)
            {
                current_var_id = ds1.Tables[0].Rows[0][0].ToString();
            }
            OracleCommand cmd3 = new OracleCommand($"drop sequence SENSOR_SEQUENCE", GUI.conn);
            OracleCommand cmd4 = new OracleCommand($"drop sequence EQ_SEQUENCE", GUI.conn);
            OracleCommand cmd7 = new OracleCommand($"drop sequence VAR_SEQUENCE", GUI.conn);
            OracleCommand cmd8 = new OracleCommand($"CREATE SEQUENCE VAR_SEQUENCE INCREMENT BY 1 START WITH {current_var_id}", GUI.conn);
            OracleCommand cmd5 = new OracleCommand($"CREATE SEQUENCE EQ_SEQUENCE INCREMENT BY 1 START WITH {current_equipment_id}", GUI.conn);
            OracleCommand cmd6 = new OracleCommand($"CREATE SEQUENCE SENSOR_SEQUENCE INCREMENT BY 1 START WITH {current_sensor_id}", GUI.conn);
            try
            {
                OracleDataReader reader1 = cmd3.ExecuteReader();
                OracleDataReader reader2 = cmd4.ExecuteReader();
                OracleDataReader reader5 = cmd7.ExecuteReader();

            }
            catch { }
            OracleDataReader reader3 = cmd5.ExecuteReader();
            OracleDataReader reader4 = cmd6.ExecuteReader();
            OracleDataReader reader6 = cmd8.ExecuteReader();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (oracle_button_click == false)
            {
                MessageBox.Show("Please connect to oracle first");
            }
            else
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                OracleCommand cmd = new OracleCommand("delete from ptect_fdc.collected_data", conn);
                OracleCommand cmd3 = new OracleCommand("delete from ptect_fdc.collected_data_env", conn);
                OracleCommand cmd1 = new OracleCommand("delete from ptect_fdc.equipment", conn);
                OracleCommand cmd2 = new OracleCommand("delete from ptect_fdc.sensor", conn);

                cmd.Connection = conn;
                cmd1.Connection = conn;
                cmd2.Connection = conn;
                OracleDataReader dr = cmd.ExecuteReader();
                OracleDataReader dr3 = cmd3.ExecuteReader();
                OracleDataReader dr1 = cmd1.ExecuteReader();
                OracleDataReader dr2 = cmd2.ExecuteReader();

                stopwatch.Stop();
                label13.Text = (stopwatch.ElapsedMilliseconds.ToString() + " ms");
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (oracle_button_click == false)
            {
                MessageBox.Show("Please connect to oracle first");
            }
            else
            {
                OracleCommand cmd = new OracleCommand(textBox1.Text.ToString(), conn);
                cmd.Connection = conn;
                OracleDataReader dr = cmd.ExecuteReader();
                
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (cassandra_button_click == false)
            {
                MessageBox.Show("Please connect to cassandra first");
            }
            else
            {               
                var rs = session.Execute(textBox1.Text.ToString());
            }
        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (oracle_button_click == false)
            {
                MessageBox.Show("Please connect to oracle");
            }
            else
            {
                Form2 f2 = new Form2();
                f2.ShowDialog(); // Shows Form2
            }
        }

        private void equipmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (oracle_button_click == false || cassandra_button_click == false)
            {
                MessageBox.Show("Please connect to databases");
            }
            else
            {
                Equipment_form f1 = new Equipment_form();
                f1.ShowDialog();
            }
        }

        private void sensorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (oracle_button_click == false || cassandra_button_click == false)
            {
                MessageBox.Show("Please connect to databases");
            }
            else
            {
                Sensor_form f1 = new Sensor_form();
                f1.ShowDialog();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void collecteddataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (oracle_button_click == false || cassandra_button_click == false)
            {
                MessageBox.Show("Please connect to databases");
            }
            else
            {
                Data_form f1 = new Data_form();
                f1.ShowDialog();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void exitToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void equipmentToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (oracle_button_click == false || cassandra_button_click == false)
            {
                MessageBox.Show("Please connect to databases");
            }
            else
            {
                selectedTable = "equipment";
                Table_Show_Form f1 = new Table_Show_Form();
                f1.ShowDialog();
            }
        }

        private void sensorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (oracle_button_click == false || cassandra_button_click == false)
            {
                MessageBox.Show("Please connect to databases");
            }
            else
            {
                selectedTable = "sensor";
                Table_Show_Form f1 = new Table_Show_Form();
                f1.ShowDialog();
            }
        }

        private void collectedDataToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (oracle_button_click == false || cassandra_button_click == false)
            {
                MessageBox.Show("Please connect to databases");
            }
            else
            {
                selectedTable = "collected_data";
                Table_Show_Form f1 = new Table_Show_Form();
                f1.ShowDialog();
            }
        }

        private void collectedDataenvToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (oracle_button_click == false || cassandra_button_click == false)
            {
                MessageBox.Show("Please connect to databases");
            }
            else
            {
                selectedTable = "collected_data_env";
                Collected_data_env f1 = new Collected_data_env();
                f1.ShowDialog();
            }
        }

        private void collectedDataenvToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (oracle_button_click == false || cassandra_button_click == false)
            {
                MessageBox.Show("Please connect to databases");
            }
            else
            {
                Data_env_form f1 = new Data_env_form();
                f1.ShowDialog();
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            //check cassandra connection
            if (cassandra_button_click == false)
            {
                MessageBox.Show("Please connect to Cassandra first");

            }
            else
            {
                //check that other tables are generated due to dependencies 
                if (insert_cassandra == false)
                {
                    MessageBox.Show("Please insert data to cassandra first");
                }
                else
                {
                    //start stop watch
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();

                    //read image in binary mode
                    byte[] rawData = File.ReadAllBytes(@"C:\Users\Emil Abulail\Desktop\Birzeit university\Training\Train\slider_puffin_jpegmini_mobile.jpg");

                    PreparedStatement ps = session.Prepare("BEGIN BATCH " +
                     "INSERT INTO collected_data_env (equipment_id,sensor_id,varenv_id, tracevalue) VALUES (?,?,?,?);" + "APPLY BATCH"
                     );
                    //inserting 10k records
                    for (int i = 0; i < 10; i++)
                    {
                        for (int j = 0; j < 20; j++)
                        {
                            for (int w = 0; w < 50; w++)
                            {
                                session.Execute(ps.Bind(long.Parse(i.ToString()), long.Parse(j.ToString()), long.Parse(w.ToString()), rawData));

                            }
                        }
                    }
                    //calculate execution time and display it
                    stopwatch.Stop();
                    label16.Text = (stopwatch.ElapsedMilliseconds.ToString() + " ms");
                }
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (insert_oracle == false)
            {
                MessageBox.Show("Please insert data to oracle first");
            }
            else { 
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var process = System.Diagnostics.Process.Start(@"C:\Users\Emil Abulail\Desktop\Birzeit university\Training\Train\oracleBlob.bat");
            process.WaitForExit();
            stopwatch.Stop();
            label18.Text = (stopwatch.ElapsedMilliseconds.ToString() + " ms");
            }
        }
    }

}

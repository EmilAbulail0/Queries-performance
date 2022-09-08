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
    public partial class Form2 : Form
    {
        string eq_selection;
        string sen_selection;
        public Form2()
        {
            InitializeComponent();
            OracleCommand cmd = new OracleCommand("select DISTINCT equipment_id from ptect_fdc.collected_data order by equipment_id", GUI.conn);
            OracleDataAdapter adp = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            comboBox1.ValueMember = "equipment_id";
            if (ds.Tables.Count > 0)
            {
                comboBox1.DataSource = ds.Tables[0];
            }          
            button3.Enabled = false;
            button1.Enabled = false;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OracleCommand cmd = new OracleCommand($"select start_time ,sensor_value from ptect_fdc.collected_data where equipment_id = {eq_selection} and sensor_id = {sen_selection}", GUI.conn);
            OracleDataAdapter adp = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            if (ds.Tables.Count > 0)
            {
                dataGridView1.DataSource = ds.Tables[0].DefaultView;
            }
            var list = new List<DateTime>();
            
            for (int i=0; i< dataGridView1.RowCount; i++) 
            {
                int selectedrowindex = dataGridView1.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = dataGridView1.Rows[selectedrowindex];
                string cellValue = Convert.ToString(selectedRow.Cells["start_time"].Value);
                DateTime dateTime = DateTime.Parse(cellValue);
                list.Add(dateTime);
            }
            
            

            //chart1.Size = new Size(800, 300); 
            chart1.BackColor = Color.LightBlue;
            chart1.BorderlineColor = Color.Red;
            chart1.ChartAreas[0].AxisY.Minimum = 0;
            chart1.ChartAreas[0].AxisY.Maximum = 5;
            chart1.ChartAreas[0].AxisY.Title = "Value";
            chart1.ChartAreas[0].AxisX.Title = "Time(s)";
            chart1.ChartAreas[0].AxisX.TitleForeColor = Color.Blue;
            chart1.ChartAreas[0].AxisY.TitleForeColor = Color.Blue;
            

            chart1.Series.Add(new Series());
            chart1.Series[0].ChartType = SeriesChartType.Line;
            int selectedrowindex1 = dataGridView1.SelectedCells[0].RowIndex;
            for (int i = 0; i < dataGridView1.RowCount-1; i++)
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[selectedrowindex1];
                string cellValue1 = Convert.ToString(selectedRow.Cells["sensor_value"].Value);
                if (cellValue1 == null) { }
                else
                {
                    chart1.Series[0].Points.AddXY(list[i].Millisecond, double.Parse(cellValue1));
                    selectedrowindex1++;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            eq_selection = comboBox1.SelectedValue.ToString();
            OracleCommand cmd1 = new OracleCommand($"select distinct sensor_id from ptect_fdc.collected_data where equipment_id = {eq_selection} order by sensor_id ", GUI.conn);
            OracleDataAdapter adp1 = new OracleDataAdapter(cmd1);
            DataSet ds1 = new DataSet();
            adp1.Fill(ds1);
            comboBox2.ValueMember = "sensor_id";
            if (ds1.Tables.Count > 0)
            {
                comboBox2.DataSource = ds1.Tables[0];
            }
            sen_selection = comboBox2.SelectedValue.ToString();
            button3.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }
    }
}

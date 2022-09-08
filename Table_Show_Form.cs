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
using System.Windows.Forms.DataVisualization.Charting;

namespace Train
{
    public partial class Table_Show_Form : Form
    {
        int columnIndex = 3;
     
        public Table_Show_Form()
        {
            InitializeComponent();
           
            label1.Text = GUI.selectedTable.ToUpper();
            OracleCommand cmd = new OracleCommand($"select * from ptect_fdc.{GUI.selectedTable}", GUI.conn);
            OracleDataAdapter adp = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            if (ds.Tables.Count > 0)
            {
                dataGridView1.DataSource = ds.Tables[0].DefaultView;
                
            }
       

        }

        private void Table_Show_Form_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}

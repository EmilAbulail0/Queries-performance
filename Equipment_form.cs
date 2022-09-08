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
    public partial class Equipment_form : Form
    {
        int current_id = 1;
        public Equipment_form()
        {
            InitializeComponent();         
            this.comboBox1.Items.Add(true);
            this.comboBox1.Items.Add(false);
            var rs1 = GUI.session.Execute("select DISTINCT equipment_id from ptect_fdc.equipment");
            DataTable data1 = new DataTable("data1");
            //to create the column and schema
            DataColumn equipment_id = new DataColumn("equipment_id", typeof(Int32));
            data1.Columns.Add(equipment_id);
            foreach (var row in rs1)
            {
                Int32 Equipment_id = row.GetValue<Int32>("equipment_id");
                data1.Rows.Add(Equipment_id);
            }
            comboBox2.ValueMember = "equipment_id";
            comboBox2.DataSource = data1;

           

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OracleCommand cmd = new OracleCommand("select ptect_fdc.eq_sequence.nextval from dual", GUI.conn);
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
            // string id = textBox1.Text;
            string name = textBox4.Text;
            string type = textBox3.Text;

            // conn.Close();
            DialogResult dialogResult = MessageBox.Show("\tEquipment Details: \n\t ID : " + current_id + "\n\t Name : " + name + "\n\t Type : " + type + "\n\t IsValid : " + isValid, "Equipment", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                var rs = GUI.session.Execute($"INSERT INTO ptect_fdc.equipment (equipment_id,eq_isvalid,eq_name,eq_type) VALUES ({current_id} , {isValidValue}, '{name}' , {type});");
                OracleCommand cmd1 = new OracleCommand($"INSERT INTO ptect_fdc.equipment (equipment_id,eq_isvalid,eq_name,eq_type) VALUES ({current_id} , {isValidValue}, '{name}' , {type})", GUI.conn);
                OracleDataReader reader = cmd1.ExecuteReader();
                updateData();
            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
            }
        }

        private void updateData()
        {
            var rs1 = GUI.session.Execute("select DISTINCT equipment_id from ptect_fdc.equipment");
            DataTable data1 = new DataTable("data1");
            //to create the column and schema
            DataColumn equipment_id = new DataColumn("equipment_id", typeof(Int32));
            data1.Columns.Add(equipment_id);
            foreach (var row in rs1)
            {
                Int32 Equipment_id = row.GetValue<Int32>("equipment_id");
                data1.Rows.Add(Equipment_id);
            }
            comboBox2.ValueMember = "equipment_id";
            comboBox2.DataSource = data1;
        }

        private void Equipment_form_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text == "")
            {
                MessageBox.Show("please enter equipment id");
            }
            else
            {
                string eq_id = comboBox2.Text;
                OracleCommand cmd1 = new OracleCommand($"select * from ptect_fdc.equipment where equipment_id = {eq_id} ", GUI.conn);
                OracleDataAdapter adp1 = new OracleDataAdapter(cmd1);
                DataSet ds1 = new DataSet();
                adp1.Fill(ds1);
                comboBox1.ValueMember = "eq_isvalid";
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
                MessageBox.Show("please enter equipment id");
            }
            else
            {
                string eq_id = comboBox2.Text;
                var rs = GUI.session.Execute($"delete from ptect_fdc.equipment where equipment_id={eq_id};");
                OracleCommand cmd = new OracleCommand($"delete from ptect_fdc.equipment where equipment_id={eq_id}", GUI.conn);
                OracleDataReader reader = cmd.ExecuteReader();
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

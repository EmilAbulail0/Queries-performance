using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Train
{
    internal class Record
    {
        public string Equipment_id;
        public string Sensor_id;
        public string Start_date;
        public string End_date;
        public string Sensor_value;

        public string tostring()
        {
            return "Data{" + "Equipment_id=" + Equipment_id + ", Sensor_id=" + Sensor_id + ", Start_date=" + Start_date + ", End_date=" + End_date + ", Sensor_value=" + Sensor_value + '}';
        }

        public Record()
        {
        }

        public Record(string Equipment_id, string Sensor_id, string Start_date, string End_date, string Sensor_value)
        {
            this.Equipment_id = Equipment_id;
            this.Sensor_id = Sensor_id;
            this.Start_date = Start_date;
            this.End_date =End_date;
            this.Sensor_value = Sensor_value;
        }
        
        public int MyProperty { get; set; }

    }
}

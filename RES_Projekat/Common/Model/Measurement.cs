using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
	public class Measurement
	{
		private int id;
		private DateTime date;
		private double value;

        public Measurement()
        {
            Date = DateTime.Now;
            Value = -1;
        }

        public Measurement(double value)
		{
			Date = DateTime.Now;
			Value = value;
        }

        public Measurement(double value, DateTime timestamp)
        {
            Date = timestamp;
            Value = value;
        }

        public int Id { get => id; set => id = value; }
		public DateTime Date { get => date; set => date = value; }
		public double Value { get => value; set => this.value = value; }
	}
}

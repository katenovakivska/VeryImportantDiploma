using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace EmployeeManagment.Methods
{
	[DataContract]
	public class TimePoint
    {
		public TimePoint(int amount, double value1, double value2, double value3, double value4)
		{
			Amount = amount;
			Time = Math.Round((value1 + value2 + value3 + value4) / 4, 2);
		}

		[DataMember(Name = "amount")]
		public int Amount;

		[DataMember(Name = "time")]
		public double Time;
	}
}

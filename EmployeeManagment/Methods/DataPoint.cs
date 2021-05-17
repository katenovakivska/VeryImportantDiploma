using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace EmployeeManagment.Methods
{
	[DataContract]
	public class DataPoint
	{
		public DataPoint(double amount, DateTime x, double avgComplexity)
		{
			Year = x.Year;
			Day = x.Day;
			Month = x.Month;
			Amount = amount;
			AvgComplexity = avgComplexity;
			Productivity = avgComplexity * amount;
		}

		[DataMember(Name = "year")]
		public int Year;

		[DataMember(Name = "day")]
		public int Day;

		[DataMember(Name = "month")]
		public int Month;

		[DataMember(Name = "amount")]
		public double Amount;

		[DataMember(Name = "complexity")]
		public double AvgComplexity;

		[DataMember(Name = "productivity")]
		public double Productivity;
	}

}

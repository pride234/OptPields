﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptField
{
	class Program
	{
		static void Main ( string[] args )
		{
			Polynomial a = new Polynomial("00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000011");
			Polynomial b = new Polynomial("10001010000000000101111110011011110000011011010110100010010010001000110111011011011101001101000101100110011110100000011111110111110010100001110011001101111110111010010100110");
			
			Console.WriteLine(a.ShiftRight(3).ToBit());
			//Console.WriteLine(a.MultiplyBy(b).ToBit());

			Console.ReadKey();
		}
	}
}

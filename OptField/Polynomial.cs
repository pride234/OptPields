using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptField
{
//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|

	public class Polynomial
	{
	
		private int[] number = null;
//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|

		public Polynomial()
		{

			number = new int[173];	// После вызова такого конструктора будет нулевой полином
		}
//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|

		public Polynomial(string bit)
		{
			if(bit.Length != 173) throw new DivideByZeroException();

			var dig = bit.Length;
			number = new int[dig];

			for(var i = 0; i < dig; i++)
			{
				number[dig-1-i] = Convert.ToByte(bit.Substring(bit.Length - (i + 1), 1), 2);		
			}
		}
//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|

		private static int[] Padding (int[] mas, int len) 		
		{ // Расширяет массив
		
			var padded = new int[len];

			for (var i = 0; i < mas.Length; i++)
			{ 
				padded[i] = mas[i];	
			}

			return padded;
		}
//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|

		public string ToBit()
		{ 
			
			var result = new StringBuilder();

			for (var i = 0; i < number.Length; i++)
			{ 
				result.Append(Convert.ToString(number[i], 2));
			}
			
			return result.ToString();
		}	
//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|

		public Polynomial GetOne()
		{
			
			return new Polynomial("11111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111");
		}
//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|

		public Polynomial Add (Polynomial num) 
		{

			int[] a = Padding(this.number, this.number.Length);	
			int[] b = Padding(num.number, num.number.Length);	
			var result = new int[173];	
			
			for (var i = 0; i < 173; i++)
			{
				result[i] = a[i] ^ b[i];
			}
			
			var Result = new Polynomial 
			{
				number = result,
			};

			return Result;
		}
//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|

		public Polynomial ShiftRight (int k) 
		{
			var Result = new Polynomial
			{
				number = Padding(this.number, this.number.Length)
			};

			int[] temp = new int[k];

			for (var i = k-1; i>=0; i--)
			{
				temp[i] = this.number[172-(k-1-i)];
			}

			for (var i = 172; i>=k; i--) Result.number[i] = Result.number[i-k];

			for (var i = 0; i<k; i++) Result.number[i] = temp[i];

			return Result;
		}
//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|

		public Polynomial ShiftLeft (int k) 
		{
			var Result = new Polynomial
			{
				number = Padding(this.number, this.number.Length)
			};

			int[] temp = new int[k];

			for (var i = 0; i<k; i++)
			{
				temp[i] = this.number[i];
			}

			for (var i = 0; i<173-k; i++) Result.number[i] = Result.number[i+k];

			for (var i = 0; i<k; i++) Result.number[172-(k-1-i)] = temp[i];

			return Result;
		}
//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|

		public Polynomial Square()
		{		
			return this.ShiftRight(1);
		}
//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|

		public int Trace()
		{
			
			var result = 0;

			for (var i = 0; i<173; i++)
			{
				result = result ^ this.number[i];
			}

			return result;
		}
//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|

		public Polynomial MultiplyBy(Polynomial num)
		{
			
			var p = 347;

			var Ushifts = new Polynomial[173];
			var Vshifts = new Polynomial[173];

			Ushifts[0] = new Polynomial{ number = Padding(this.number, this.number.Length) };
			Vshifts[0] = new Polynomial{ number = Padding(num.number, num.number.Length)   };

			for (var i = 1; i<173; i++) 
			{
				Ushifts[i] = this.ShiftLeft(i);
				Vshifts[i] = num.ShiftLeft(i);
			}

			var Umatrixs = new int[173][,];
			var Vmatrixs = new int[173][,];

			//Umatrixs[0] = new int[1,173];
			//Vmatrixs[0] = new int[173,1];

			//for (var i = 0; i<173; i++)
			//{
			//	Umatrixs[0][0,i] = Ushifts[0].number[i];
			//	Vmatrixs[0][i,0] = Ushifts[0].number[i];
			//}

			for (var i = 0; i<173; i++)
			{
				Umatrixs[i] = new int[1,173];
				Vmatrixs[i] = new int[173,1];

				for (var j = 0; j<173; j++) 
				{
					Umatrixs[i][0,j] = Ushifts[i].number[j];
					Vmatrixs[i][j,0] = Vshifts[i].number[j];
				} 				
			}	

			int[] powers_of_two = new int[173];

			powers_of_two[0] = 1;
			
			for(int i = 1; i < 173; i++) powers_of_two[i] = (powers_of_two[i-1]<<1) % p;

			int[,] Lyambda = new int[173,173];

	        int powi, powj;

		    for(int i = 0; i < 173; i++)
			{
	            powi = powers_of_two[i];
 
				for (int j = 0; j < 173; j++)
				{
	                powj = powers_of_two[j];

					if (((powi + powj) % p) == 1) Lyambda[i,j] = 1;
					else if (((powi - powj + p) % p) == 1) Lyambda[i,j] = 1;
                    else if (((powj - powi + p) % p) == 1) Lyambda[i,j] = 1; 
					else if ((((-powi - powj)+p) % p) == 1) Lyambda[i,j] = 1;
					else Lyambda[i,j] = 0;
		        }
			}

			var TempMatrix = new int[173][,];

			for (var d = 0; d<173; d++) 
			{ 				
				TempMatrix[d] = new int[1,173]; //Столько же строк, сколько в А; столько столбцов, сколько в B 

		        for (var i = 0; i < 1; ++i)
				{
	                for (var j = 0; j < 173; ++j)
					{
		                TempMatrix[d][i, j] = 0;
				        for(int k = 0; k < 173; ++k) //ТРЕТИЙ цикл, до A.m=B.n
						{ 
						    TempMatrix[d][i, j] ^= Umatrixs[d][i, k] & Lyambda[k, j]; //Собираем сумму произведений
						}
					}
				}			
			}

			var ResultMatrix = new int[173][,];

			for (var d = 0; d<173; d++) 
			{ 				
				ResultMatrix[d] = new int[1,1]; //Столько же строк, сколько в А; столько столбцов, сколько в B 

		        for (var i = 0; i < 1; ++i)
				{
	                for (var j = 0; j < 1; ++j)
					{
		                ResultMatrix[d][i, j] = 0;
				        for(int k = 0; k < 173; ++k) //ТРЕТИЙ цикл, до A.m=B.n
						{ 
						    ResultMatrix[d][i, j] ^= TempMatrix[d][i, k] & Vmatrixs[d][k, j]; //Собираем сумму произведений
						}
					}
				}			
			}
			
			var Result = new Polynomial();

			for (var i = 0; i<173; i++) Result.number[i] = ResultMatrix[i][0,0];

			return Result;
		}
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|

		public Polynomial GornerPower(Polynomial num) 
		{
		
			var C = this.GetOne();
			var A = new Polynomial
			{
				number = Padding(this.number, this.number.Length)
			};

			for (var i = num.number.Length-1; i>=0; i--) 
			{
				if (num.number[i] == 1)
				{ 
					C = C.MultiplyBy(A);
				}

				A = A.MultiplyBy(A);
			}
			
			return C;
		}
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
	
		public Polynomial Reverse()
		{
			
			var ferma = new Polynomial
			{
				number = new int[173]
			};

			ferma.number[172] = 0;

			for (var i = 171; i >=0; i--)
			{
				ferma.number[i] = 1;
			}
		
			return this.GornerPower(ferma);
		}
//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
	}
}

using System;

namespace Bau.Libraries.LibCsvHelper.Definitions
{
	/// <summary>
	///		Clase con los datos de un campo numérico
	/// </summary>
	public class CsvFieldNumeric : CsvFieldBase
	{
		public CsvFieldNumeric(string title, bool signAtRight = false, int decimals = 0, 
							   char decimalSeparator = '.', char? thousandsSeparator = null) : base(title)
		{
			SignAtRight = signAtRight;
			Decimals = decimals;
			DecimalSeparator = decimalSeparator;
			ThousandsSeparator = thousandsSeparator;
		}

		/// <summary>
		///		Indica si el signo se coloca a la derecha
		/// </summary>
		public bool SignAtRight { get; }

		/// <summary>
		///		Número de decimales
		/// </summary>
		public int Decimals { get; }

		/// <summary>
		///		Separador de decimales
		/// </summary>
		public char DecimalSeparator { get; }

		/// <summary>
		///		Separador de miles
		/// </summary>
		public char? ThousandsSeparator { get; }
	}
}

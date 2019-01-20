using System;

namespace Bau.Libraries.LibCsvHelper.Definitions
{
	/// <summary>
	///		Definición de un campo de tipo cadena
	/// </summary>
	public class CsvFieldString : CsvFieldBase
	{
		public CsvFieldString(string title, int maxLength = 0) : base(title)
		{
			MaxLength = maxLength;
		}

		/// <summary>
		///		Longitud máxima de la cadena
		/// </summary>
		public int MaxLength { get; }
	}
}

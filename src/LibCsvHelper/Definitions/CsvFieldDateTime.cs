using System;

namespace Bau.Libraries.LibCsvHelper.Definitions
{
	/// <summary>
	///		Definición de un campo de tipo fecha
	/// </summary>
	public class CsvFieldDateTime : CsvFieldBase
	{
		public CsvFieldDateTime(string title, string format = "yyyy-MM-dd HH:mm:ss") : base(title)
		{
			Format = format;
		}

		/// <summary>
		///		Formato de la fecha
		/// </summary>
		public string Format { get; }
	}
}

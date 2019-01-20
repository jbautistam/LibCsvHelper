using System;

namespace Bau.Libraries.LibCsvHelper.Definitions
{
	/// <summary>
	///		Separadores de filas / campos
	/// </summary>
	public class CsvDelimitiers
	{
		public CsvDelimitiers(string lines = null, char fields = ',', char quote = '"')
		{
			Lines = lines ?? Environment.NewLine;
			Fields = fields;
			Quote = quote;
		}

		/// <summary>
		///		Separador de líneas
		/// </summary>
		public string Lines { get; }

		/// <summary>
		///		Separador de campos
		/// </summary>
		public char Fields { get; }

		/// <summary>
		///		Carácter que se utiliza como discriminador en las cadenas
		/// </summary>
		public char Quote { get; }
	}
}

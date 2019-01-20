using System;

namespace Bau.Libraries.LibCsvHelper.Definitions
{
	/// <summary>
	///		Clase base para los campos
	/// </summary>
	public abstract class CsvFieldBase
	{
		public CsvFieldBase(string title)
		{
			Title = title;
		}

		/// <summary>
		///		Título del campo
		/// </summary>
		public string Title { get; }
	}
}

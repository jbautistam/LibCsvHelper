using System;
using System.Collections.Generic;

namespace Bau.Libraries.LibCsvHelper.Definitions
{
	/// <summary>
	///		Campos del archivo
	/// </summary>
	public class CsvFields : List<CsvFieldBase>
	{
		/// <summary>
		///		Obtiene el elemento que se corresponde con una clave
		/// </summary>
		internal CsvFieldBase Search(string key)
		{
			// Busca el elemento
			foreach (CsvFieldBase field in this)
				if (field.Title.Equals(key, StringComparison.CurrentCultureIgnoreCase))
					return field;
			// Si ha llegado hasta aquí es porque no ha encontrado nada
			return null;
		}
	}
}

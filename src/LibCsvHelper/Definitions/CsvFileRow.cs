using System;
using System.Collections.Generic;

namespace Bau.Libraries.LibCsvHelper.Definitions
{
	/// <summary>
	///		Datos de una fila de un archivo
	/// </summary>
	public class CsvFileRow
	{
		/// <summary>
		///		Añade un valor a la fila
		/// </summary>
		public void Add(string header, object value)
		{
			string key = NormalizeKey(header);

				// Añade / modifica el valor
				if (Values.ContainsKey(key))
					Values[key] = value;
				else
					Values.Add(key, value);
		}

		/// <summary>
		///		Busca un elemento por su clave
		/// </summary>
		internal object Search(string header)
		{
			string key = NormalizeKey(header);

				if (Values.ContainsKey(key))
					return Values[key];
				else
					return null;
		}

		/// <summary>
		///		Normaliza la clave
		/// </summary>
		private string NormalizeKey(string key)
		{
			return key.ToUpperInvariant();
		}

		/// <summary>
		///		Valores
		/// </summary>
		internal Dictionary<string, object> Values { get; } = new Dictionary<string, object>();
	}
}

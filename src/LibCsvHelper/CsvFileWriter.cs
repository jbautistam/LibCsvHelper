using System;
using System.Collections.Generic;

using Bau.Libraries.LibCsvHelper.Definitions;

namespace Bau.Libraries.LibCsvHelper
{
	/// <summary>
	///		Clase para escribir un archivo
	/// </summary>
	public class CsvFileWriter : IDisposable
	{
		public CsvFileWriter(CsvDelimitiers delimitiers, CsvFields fields)
		{
			Delimitiers = delimitiers ?? new CsvDelimitiers();
			Fields = fields;
		}

		/// <summary>
		///		Abre el archivo
		/// </summary>
		public void Open(string fileName, System.Text.Encoding encoding = null)
		{
			FileWriter = System.IO.File.AppendText(fileName);
		}

		/// <summary>
		///		Manda los datos restantes al stream
		/// </summary>
		public void Flush()
		{
			if (FileWriter != null)
				FileWriter.Flush();
		}

		/// <summary>
		///		Cierra el stream
		/// </summary>
		public void Close()
		{
			if (FileWriter != null)
			{
				// Envía los datos restantes al archivo
				Flush();
				// Cierra el stream
				FileWriter.Close();
				FileWriter = null;
			}
		}

		/// <summary>
		///		Escribe las cabeceras
		/// </summary>
		public void WriteHeaders()
		{
			List<string> columns = new List<string>();

				// Crea la lista de columnas con los títulos de los campos
				foreach (CsvFieldBase field in Fields)
					columns.Add(field.Title);
				// Escribe las cabeceras
				WriteLine(columns);
		}

		/// <summary>
		///		Escribe una línea
		/// </summary>
		public void WriteRow(CsvFileRow row)
		{
			List<string> columns = new List<string>();

				// Recorre los campos obteniendo los valores
				foreach (CsvFieldBase field in Fields)
				{
					object value = row.Search(field.Title);

						columns.Add(ConvertValue(field, value));
				}
		}

		/// <summary>
		///		Convierte un valor a cadena
		/// </summary>
		private string ConvertValue(CsvFieldBase fieldBase, object value)
		{
			if (value == null)
				return string.Empty;
			else
				switch (fieldBase)
				{
					case CsvFieldString field:
						return Left(value.ToString(), field.MaxLength);
					case CsvFieldBoolean field:
						if ((bool) value)
							return field.ValueTrue;
						else
							return field.ValueFalse;
					case CsvFieldNumeric field:
						return ConvertNumeric(value, field);
					case CsvFieldDateTime field:
						return ConvertDateTime(value as DateTime?, field);
					default:
						return string.Empty;
				}
		}

		/// <summary>
		///		Convierte un valor numérico a cadena
		/// </summary>
		private string ConvertNumeric(object value, CsvFieldNumeric field)
		{
			//TODO --> Le falta la conversión de puntos de miles, el número de decimales y el signo a izquierda / derecha
			switch (value)
			{
				case decimal converted:
					return converted.ToString(System.Globalization.CultureInfo.InvariantCulture).Replace('.', field.DecimalSeparator);
				case double converted:
					return converted.ToString(System.Globalization.CultureInfo.InvariantCulture).Replace('.', field.DecimalSeparator);
				default:
					return value.ToString();
			}
		}

		/// <summary>
		///		Convierte una fecha
		/// </summary>
		private string ConvertDateTime(DateTime? value, CsvFieldDateTime field)
		{
			if (value == null)
				return string.Empty;
			else
				return (value ?? DateTime.Now).ToString(field.Format);
		}

		/// <summary>
		///		Obtiene la parte izquierda de una cadena
		/// </summary>
		private string Left(string value, int maxLength)
		{
			if (string.IsNullOrEmpty(value) || maxLength == 0 || value.Length < maxLength)
				return value;
			else
				return value.Substring(0, maxLength);
		}

		/// <summary>
		///		Escribe una línea
		/// </summary>
		private void WriteLine(List<string> columns)
		{
			System.Text.StringBuilder builder = new System.Text.StringBuilder();

				// Obtiene los datos de la fila
				foreach (string column in columns)
				{
					// Añade el separador
					if (builder.Length > 0)
						builder.Append(Delimitiers.Fields);
					// Añade el valor normalizado
					if (!string.IsNullOrWhiteSpace(column))
						builder.Append(Normalize(column, Delimitiers.Fields));
				}
				// Escribe la cadena
				FileWriter.WriteLine(builder);
		}

		/// <summary>
		///		Normaliza el valor a escribir teniendo en cuenta el separador de campos
		/// </summary>
		private string Normalize(string value, char separator)
		{
			// Si en la cadena que se va a escribir, aparece el separador, hay que rodear la cadena por comillas pero
			// si la cadena ya tiene comillas hay que duplicar esas comillas
			if (!string.IsNullOrEmpty(value) && value.IndexOf(separator.ToString(), StringComparison.CurrentCultureIgnoreCase) >= 0)
			{ 
				// Duplica las "comillas" internas del campo
				value = value.Replace("\"", "\"\"");
				// Añade las "comillas" iniciales y finales
				value = $"\"{value}\"";
			}
			// Devuelve el valor normalizado
			return value;
		}

		/// <summary>
		///		Libera la memoria
		/// </summary>
		protected virtual void Dispose(bool disposing)
		{
			if (!Disposed)
			{
				// Cierra el archivo
				if (disposing)
					Close();
				// Indica que se ha liberado la memoria
				Disposed = true;
			}
		}

		/// <summary>
		///		Libera la memoria
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
		}

		/// <summary>
		///		Delimitadores para el archivo
		/// </summary>
		public CsvDelimitiers Delimitiers { get; }

		/// <summary>
		///		Campos
		/// </summary>
		public CsvFields Fields { get; }

		/// <summary>
		///		Handle de archivo
		/// </summary>
		private System.IO.StreamWriter FileWriter { get; set; }

		/// <summary>
		///		Indica si se ha liberado el archivo
		/// </summary>
		public bool Disposed { get; private set; }
	}
}

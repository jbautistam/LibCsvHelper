using System;
using System.Collections.Generic;

using Bau.Libraries.LibCsvHelper.Definitions;

namespace Bau.Libraries.LibCsvHelper
{
	/// <summary>
	///		Clase para leer de un archivo
	/// </summary>
	public class CsvFileParser : IDisposable
	{
		public CsvFileParser(CsvDelimitiers delimitiers, CsvFields fields)
		{
			Delimitiers = delimitiers ?? new CsvDelimitiers();
			Fields = fields;
		}

		/// <summary>
		///		Abre el archivo
		/// </summary>
		public void Open(string fileName, System.Text.Encoding encoding = null)
		{
			FileReader = System.IO.File.ReadLines(fileName);
		}

		/// <summary>
		///		Cierra el stream
		/// </summary>
		public void Close()
		{
			if (FileReader != null)
				FileReader = null;
		}

		/// <summary>
		///		Lee la cabecera del archivo
		/// </summary>
		public List<string> ReadHeader()
		{
			// Recorre las líneas buscando la primera línea que no esté vacía
			foreach (string line in FileReader)
			{
				string lineReaded = Normalize(line);

					if (!string.IsNullOrWhiteSpace(lineReaded))
						return ParseLine(lineReaded, Delimitiers.Fields, Delimitiers.Quote);
			}
			// Si ha llegado hasta aquí es porque no ha encontrado la cabecera
			return new List<string>();
		}

		/// <summary>
		///		Lee una línea
		/// </summary>
		public IEnumerable<CsvFileRow> ReadLine()
		{
			foreach (string line in FileReader)
			{ 
				string lineRead = Normalize(line);

					if (!string.IsNullOrWhiteSpace(lineRead)) // ... salta las líneas vacías
						yield return ConvertLine(ParseLine(lineRead, Delimitiers.Fields, Delimitiers.Quote));
			}
		}

		/// <summary>
		///		Normaliza una línea
		/// </summary>
		private string Normalize(string line)
		{
			string lineRead = string.Empty;

				// Salta las líneas vacías
				if (!string.IsNullOrWhiteSpace(line))
				{ 
					// Quita los espacios
					lineRead = line.Trim();
					// Comprueba que no sea un salto de línea
					if (string.IsNullOrWhiteSpace(lineRead) || lineRead.Equals("\r\n") || lineRead.Equals("\r") ||
							lineRead.Equals("\n") && !lineRead.Equals(char.ConvertFromUtf32(26))) // ... salta las líneas vacías
						lineRead = string.Empty;
				}
				// Devuelve la línea
				return lineRead;
		}

		/// <summary>
		///		Convierte los campos de una línea
		/// </summary>
		private CsvFileRow ConvertLine(List<string> cells)
		{
			CsvFileRow row = new CsvFileRow();

				// Convierte los registros
				foreach (CsvFieldBase field in Fields)
				{
					int index = Fields.IndexOf(field);

						if (index >= cells.Count)
							row.Add(field.Title, null);
						else
							row.Add(field.Title, ConvertCell(cells[index], field));
				}
				// Devuelve la fila leida
				return row;
		}

		/// <summary>
		///		Convierte el valor de un campo
		/// </summary>
		private object ConvertCell(string cell, CsvFieldBase fieldBase)
		{
			if (string.IsNullOrWhiteSpace(cell))
				return null;
			else
				switch (fieldBase)
				{
					case CsvFieldBoolean field:
						return cell.Equals(field.ValueTrue, StringComparison.CurrentCultureIgnoreCase);
					case CsvFieldNumeric field:
						return ConvertNumeric(cell, field);
					case CsvFieldDateTime field:
						return ConvertDateTime(cell, field);
					case CsvFieldString field:
						return cell;
					default:
						return cell;
				}
		}

		/// <summary>
		///		Convierte una cadena a fecha
		/// </summary>
		private DateTime? ConvertDateTime(string value, CsvFieldDateTime field)
		{
			if (string.IsNullOrEmpty(field.Format))
			{
				if (DateTime.TryParse(value, out DateTime converted))
					return converted;
				else 
					throw new ArgumentException($"Can't convert value to dateTime. Value: {value}");
			}
			else if (DateTime.TryParseExact(value, field.Format, System.Globalization.DateTimeFormatInfo.InvariantInfo,
										  System.Globalization.DateTimeStyles.AllowInnerWhite, out DateTime converted))
				return converted;
			else
				throw new ArgumentException($"Can't convert value to dateTime. Value: {value}");
		}

		/// <summary>
		///		Convierte una cadena a numérico
		/// </summary>
		private double ConvertNumeric(string value, CsvFieldNumeric field)
		{
			string sign = "+";

				// Quita el signo
				if (field.SignAtRight)
				{
					if (value.EndsWith("+") || value.EndsWith("-"))
					{
						// Obtiene el signo
						sign = value[value.Length - 1].ToString();
						// Quita el signo de la cadena
						if (value.Length > 1)
							value = value.Substring(0, value.Length - 1);
						else
							value = string.Empty;
					}
				}
				else if (value.StartsWith("+") || value.StartsWith("-"))
				{
					// Obtiene el signo
					sign = value.Substring(0);
					// Quita el signo de la cadena
					if (value.Length > 1)
						value = value.Substring(1);
					else
						value = string.Empty;
				}
				// Si está vacío, le pone un 0
				if (string.IsNullOrWhiteSpace(value))
					value = "0";
				// Comprueba el signo, lo primero
				if (sign != "+" && sign != "-")
					throw new ArgumentException($"Incorrect sign. Value: {value}");
				else 
				{
					// Quita los separadores de miles
					if (field.ThousandsSeparator != null)
						value = value.Replace(field.ThousandsSeparator.ToString(), "");
					// Cambia el separador decimal por el .
					value = value.Replace(field.DecimalSeparator.ToString(), ".");
					// Comprueba que sea numérico antes de continuar
					if (!IsNumeric(value))
						throw new ArgumentException($"The value is not numeric. Value: {value}");
					else if (!double.TryParse(sign + value, System.Globalization.NumberStyles.Number, 
											  System.Globalization.CultureInfo.InvariantCulture, out double numeric))
						throw new ArgumentException($"Can't convert to numeric. Value {value}");
					else
						return numeric;
				}
		}

		/// <summary>
		///		Comprueba si una cadena es numérica
		/// </summary>
		private bool IsNumeric(string value)
		{
			int points = 0;

				// Comprueba que todos sean dígitos o puntos
				foreach (char chr in value)
					if (chr == '.')
						points++;
					else if (!char.IsDigit(chr))
						return false;
				// Es numérico si sólo tiene un punto decimal
				return points <= 1;
		}

		/// <summary>
		///		Interpreta una línea de un archivo de longitud variable que normaliza sus campos con " o caracteres similares
		/// </summary>
		public List<string> ParseLine(string line, char fieldSeparator, char fieldQuotes = '"')
		{
			List<string> lineCsv = new List<string>();
			string field = "";
			bool isInQuotes = false;

				// Recorre la línea de texto buscando los campos
				foreach (char actual in line)
				{
					// Trata el carácter
					if (isInQuotes && actual == fieldQuotes)
						isInQuotes = false;
					else if (!isInQuotes) // ... trata el carácter dentro de las comillas
					{
						if (actual == fieldQuotes)
							isInQuotes = true;
						else if (actual == fieldSeparator)
						{ 
							// Añade el valor del campo
							lineCsv.Add(field);
							// Vacía la cadena intermedia
							field = string.Empty;
						}
						else
							field += actual;
					}
					else
						field += actual;
				}
				// Añade el último campo a la línea
				lineCsv.Add(field);
				// Devuelve la línea
				return lineCsv;
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
		private IEnumerable<string> FileReader { get; set; }

		/// <summary>
		///		Indica si se ha liberado el archivo
		/// </summary>
		public bool Disposed { get; private set; }
	}
}

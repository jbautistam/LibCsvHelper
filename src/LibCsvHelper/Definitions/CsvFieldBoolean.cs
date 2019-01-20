using System;
using System.Collections.Generic;
using System.Text;

namespace Bau.Libraries.LibCsvHelper.Definitions
{
	/// <summary>
	///		Campo de tipo lógico
	/// </summary>
	public class CsvFieldBoolean : CsvFieldBase
	{
		public CsvFieldBoolean(string title, string valueTrue = "1", string valueFalse = "0") : base(title) 
		{
			ValueTrue = valueTrue;
			ValueFalse = valueFalse;
		}

		/// <summary>
		///		Valor considerado verdadero
		/// </summary>
		public string ValueTrue { get; } 

		/// <summary>
		///		Valor considerado falso
		/// </summary>
		public string ValueFalse { get; }
	}
}

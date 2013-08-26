using System;
using umbraco.cms.businesslogic.datatype;
using Xilium.MarkdownDeepEditor4Umbraco.Extensions;

namespace Xilium.MarkdownDeepEditor4Umbraco.TextFormatter {

	public abstract class TextFormatterBase {

		protected internal DataEditor _dataEditor;

		protected internal Options _options;

		protected TextFormatterBase(DataEditor dataEditor, Options options) {
			this._dataEditor = dataEditor;
			this._options = options;
		}

		/// <summary>
		/// Returns DataEditor object that creates this textFormatter instance
		/// </summary>
		public DataEditor DataEditor {
			get { return this._dataEditor; }
		}

		/// <summary>
		/// Returns Options of DataType property
		/// </summary>
		public Options Options {
			get { return this._options; }
		}

		/// <summary>
		/// Converts string writte in Markdown format to HTML string
		/// </summary>
		/// <param name="value">string writte in Markdown format</param>
		/// <returns></returns>
		public abstract string Transform(string value);

		
	}
}

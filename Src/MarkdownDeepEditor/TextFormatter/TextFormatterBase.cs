namespace Xilium.MarkdownDeepEditor4Umbraco.TextFormatter {

	public abstract class TextFormatterBase {

		public TextFormatterBase() {
			
		}

		/// <summary>
		/// Converts string writte in Markdown format to HTML string
		/// </summary>
		/// <param name="value">string writte in Markdown format</param>
		/// <returns></returns>
		public abstract string Transform(string value);

		
	}
}

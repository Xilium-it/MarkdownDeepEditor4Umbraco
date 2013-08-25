namespace Xilium.MarkdownDeepEditor4Umbraco.MarkdownFormatter {
	public abstract class MarkdownFormatterBase {

		public MarkdownFormatterBase() {
			
		}

		/// <summary>
		/// Converts string writte in Markdown format to HTML string
		/// </summary>
		/// <param name="value">string writte in Markdown format</param>
		/// <returns></returns>
		public abstract string Transform(string value);

		
	}
}

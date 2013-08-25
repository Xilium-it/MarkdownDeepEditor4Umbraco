namespace Xilium.MarkdownDeepEditor4Umbraco.MarkdownFormatter {
	public class XiliumMarkdownDeepFormatter : MarkdownFormatterBase {
		private Xilium.MarkdownDeep.Markdown _instance = null;

		public XiliumMarkdownDeepFormatter() : base() {
			this._instance = new Xilium.MarkdownDeep.Markdown();
			this._instance.ExtraMode = true;
			this._instance.MarkdownInHtml = true;
			this._instance.SafeMode = false;
		}

		public override string Transform(string value) {
			return this._instance.Transform(value);
		}

	}
}

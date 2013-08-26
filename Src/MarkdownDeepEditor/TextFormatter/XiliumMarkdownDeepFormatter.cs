
namespace Xilium.MarkdownDeepEditor4Umbraco.TextFormatter {

	public class XiliumMarkdownDeepFormatter : TextFormatterBase {
		private Xilium.MarkdownDeep.Markdown _instance = null;

		public XiliumMarkdownDeepFormatter(DataEditor dataEditor, Options options)
			: base(dataEditor, options) {

			this._instance = new Xilium.MarkdownDeep.Markdown();
			this._instance.SafeMode = this.Options.SafeMode;
			this._instance.ExtraMode = this.Options.ExtraMode;
			this._instance.MarkdownInHtml = this.Options.MarkdownInHtml;
			this._instance.AutoHeadingIDs = this.Options.AutoHeadingIDs;
			this._instance.NewWindowForExternalLinks = this.Options.NewWindowForExternalLinks;
			this._instance.NewWindowForLocalLinks = this.Options.NewWindowForLocalLinks;
			this._instance.NoFollowLinks = this.Options.NoFollowLinks;
		}

		public override string Transform(string value) {
			return this._instance.Transform(value);
		}

	}
}

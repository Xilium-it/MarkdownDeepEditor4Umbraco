using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClientDependency.Core;
using Xilium.MarkdownDeepEditor4Umbraco.Extensions;

/*
[assembly: WebResource("Xilium.MarkdownDeepEditor4Umbraco.Resources.MarkEdit.showdown.js", "application/x-javascript")]
[assembly: WebResource("Xilium.MarkdownDeepEditor4Umbraco.Resources.MarkEdit.jquery.markedit.js", "application/x-javascript")]
[assembly: WebResource("Xilium.MarkdownDeepEditor4Umbraco.Resources.MarkEdit.jquery.markedit.css", "text/css", PerformSubstitution = true)]
[assembly: WebResource("Xilium.MarkdownDeepEditor4Umbraco.Resources.MarkEdit.images.wmd-buttons.png", "image/png")]
*/
[assembly: WebResource("Xilium.MarkdownDeepEditor4Umbraco.Resources.MDDEditor.MarkdownDeep.js", "application/x-javascript")]
[assembly: WebResource("Xilium.MarkdownDeepEditor4Umbraco.Resources.MDDEditor.MarkdownDeepEditor.js", "application/x-javascript")]
[assembly: WebResource("Xilium.MarkdownDeepEditor4Umbraco.Resources.MDDEditor.MarkdownDeepEditorUI.js", "application/x-javascript")]
[assembly: WebResource("Xilium.MarkdownDeepEditor4Umbraco.Resources.MDDEditor.mdd_styles.css", "text/css")]
[assembly: WebResource("Xilium.MarkdownDeepEditor4Umbraco.Resources.MDDEditor.mdd_ajax_loader.gif", "image/gif")]
[assembly: WebResource("Xilium.MarkdownDeepEditor4Umbraco.Resources.MDDEditor.mdd_gripper.png", "image/png")]
[assembly: WebResource("Xilium.MarkdownDeepEditor4Umbraco.Resources.MDDEditor.mdd_modal_background.png", "image/png")]
[assembly: WebResource("Xilium.MarkdownDeepEditor4Umbraco.Resources.MDDEditor.mdd_toolbar.png", "image/png")]
[assembly: WebResource("Xilium.MarkdownDeepEditor4Umbraco.Resources.MDDEditor.mdd_help.html", "text/html")]
[assembly: WebResource("Xilium.MarkdownDeepEditor4Umbraco.Resources.MDDEditor_custom.mdd_styles.css", "text/css", PerformSubstitution = true)]

namespace Xilium.MarkdownDeepEditor4Umbraco {
	/// <summary>
	/// The WMD control for the Markdown Editor.
	/// </summary>
	public class EditorUIControl : WebControl {

		/// <summary>
		/// ClassName of editor
		/// </summary>
		public const string EDITOR_CSS_CLASS_NAME = "Xilium_MarkdownDeepEditor";


		/// <summary>
		/// Initializes a new instance of the <see cref="EditorUIControl"/> class.
		/// </summary>
		public EditorUIControl() {
			this.TextBoxControl = new TextBox();
		}

		/// <summary>
		/// Gets or sets the options.
		/// </summary>
		/// <value>The options.</value>
		public Options Options { get; set; }

		/// <summary>
		/// Gets or sets the text.
		/// </summary>
		/// <value>The text for the TextBoxControl.</value>
		public string Text {
			get { return this.TextBoxControl.Text; }
			set { this.TextBoxControl.Text = value; }
		}

		/// <summary>
		/// Gets or sets the TextBox control.
		/// </summary>
		/// <value>The TextBox control.</value>
		protected TextBox TextBoxControl { get; set; }

		/// <summary>
		/// Initialize the control, make sure children are created
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
		protected override void OnInit(EventArgs e) {
			base.OnInit(e);

			this.EnsureChildControls();
		}

		/// <summary>
		/// Add the resources (sytles/scripts)
		/// </summary>
		/// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
		protected override void OnLoad(EventArgs e) {
			base.OnLoad(e);

			// check if WMD Editor has been enabled.
			if (this.Options.EnableEditorUI) {
				// adds the client dependencies.
				this.AddResourceToClientDependency("Xilium.MarkdownDeepEditor4Umbraco.Resources.MDDEditor.mdd_styles.css", ClientDependencyType.Css);
				this.AddResourceToClientDependency("Xilium.MarkdownDeepEditor4Umbraco.Resources.MDDEditor_custom.mdd_styles.css", ClientDependencyType.Css);
				this.AddResourceToClientDependency("Xilium.MarkdownDeepEditor4Umbraco.Resources.MDDEditor.MarkdownDeep.js", ClientDependencyType.Javascript);
				this.AddResourceToClientDependency("Xilium.MarkdownDeepEditor4Umbraco.Resources.MDDEditor.MarkdownDeepEditor.js", ClientDependencyType.Javascript);
				this.AddResourceToClientDependency("Xilium.MarkdownDeepEditor4Umbraco.Resources.MDDEditor.MarkdownDeepEditorUI.js", ClientDependencyType.Javascript);
			}
		}

		/// <summary>
		/// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
		/// </summary>
		protected override void CreateChildControls() {
			base.CreateChildControls();

			this.EnsureChildControls();

			// create the controls
			this.TextBoxControl.ID = this.TextBoxControl.ClientID;
			this.TextBoxControl.TextMode = TextBoxMode.MultiLine;
			this.TextBoxControl.Height = Unit.Pixel(this.Options.Height);
			this.TextBoxControl.Width = Unit.Pixel(this.Options.Width);
			this.TextBoxControl.CssClass = "umbEditorTextFieldMultiple";

			// add the controls
			this.Controls.Add(this.TextBoxControl);
		}

		/// <summary>
		/// Renders the contents of the control to the specified writer. This method is used primarily by control developers.
		/// </summary>
		/// <param name="writer">A <see cref="T:System.Web.UI.HtmlTextWriter"/> that represents the output stream to render HTML content on the client.</param>
		protected override void RenderContents(HtmlTextWriter writer) {
			writer.AddAttribute(HtmlTextWriterAttribute.Class, EDITOR_CSS_CLASS_NAME);
			writer.AddAttribute(HtmlTextWriterAttribute.Style, string.Concat("width: ", this.Options.Width + 6, "px;"));
			writer.RenderBeginTag(HtmlTextWriterTag.Div);

			this.TextBoxControl.RenderControl(writer);

			writer.RenderEndTag(); // div.<EDITOR_CSS_CLASS_NAME>

			// check if WMD Editor has been enabled.
			if (this.Options.EnableEditorUI) {

				var strJS = new System.Text.StringBuilder();
				strJS.Append("\n<script type=\"text/javascript\">");
				strJS.Append("\njQuery(window).load(function() {");
				strJS.Append("\n	var $ = jQuery;");

				// Set MarkdownDeep editor and options
				strJS.Append("\n	var $textbox = $('#" + this.TextBoxControl.ClientID + "').MarkdownDeep({");
				strJS.Append("\n		help_location: '" + this.GetWebResourceUrl("Xilium.MarkdownDeepEditor4Umbraco.Resources.MDDEditor.mdd_help.html") + "'");
				strJS.Append("\n		, SafeMode: " + this.Options.SafeMode.ToJson());
				strJS.Append("\n		, ExtraMode: " + this.Options.ExtraMode.ToJson());
				strJS.Append("\n		, MarkdownInHtml: " + this.Options.MarkdownInHtml.ToJson());
				strJS.Append("\n		, AutoHeadingIDs: " + this.Options.AutoHeadingIDs.ToJson());
				strJS.Append("\n		, NewWindowForExternalLinks: " + this.Options.NewWindowForExternalLinks.ToJson());
				strJS.Append("\n		, NewWindowForLocalLinks: " + this.Options.NewWindowForLocalLinks.ToJson());
				strJS.Append("\n		, NoFollowLinks: " + this.Options.NoFollowLinks.ToJson());
				strJS.Append("\n		, disableAutoIndent: " + this.Options.DisableAutoIndent.ToJson());
				strJS.Append("\n		, disableTabHandling: " + this.Options.DisableTabHandling.ToJson());
				strJS.Append("\n		, shopwPreview: '" + this.Options.ShowPreview.ToString().ToLower() + "'");
				strJS.Append("\n	});");

				strJS.Append("\n});");
				strJS.Append("\n</script>");

				writer.WriteLine(strJS);

				
			}
		}
	}
}

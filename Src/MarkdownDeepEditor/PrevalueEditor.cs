using System;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Xilium.MarkdownDeepEditor4Umbraco.Extensions;
using umbraco.BusinessLogic;
using umbraco.cms.businesslogic.datatype;
using umbraco.interfaces;

[assembly: WebResource("Xilium.MarkdownDeepEditor4Umbraco.Resources.PrevalueEditor.style.css", "text/css", PerformSubstitution = true)]

namespace Xilium.MarkdownDeepEditor4Umbraco {
	/// <summary>
	/// The Prevalue Editor for the Markdown data-type.
	/// </summary>
	public sealed class PrevalueEditor : WebControl, IDataPrevalue {
		/// <summary>
		/// The underlying base data-type.
		/// </summary>
		private readonly BaseDataType m_DataType;

		/// <summary>
		/// An object to temporarily lock writing to the database.
		/// </summary>
		private static readonly object m_Locker = new object();

		/// <summary>
		/// Current options
		/// </summary>
		private Options Options;

		/// <summary>
		/// The CheckBox for enabling the WMD Editor.
		/// </summary>
		private CheckBox EnableEditorUI;

		/// <summary>
		/// The RadioButtonList for the preview settings.
		/// </summary>
		private RadioButtonList ShowPreview;

		/// <summary>
		/// Includes default css style for preview rendering.
		/// </summary>
		private CheckBox ShowPreviewWithDefaultCssFile;

		/// <summary>
		/// Add ClassName to EditorUI control to customize preview style.
		/// </summary>
		private TextBox ShowPreviewWithCustomClassName;

		/// <summary>
		/// Specify url of local or remote file to include to customize preview style.
		/// </summary>
		private TextBox ShowPreviewWithCustomCssFile;

		/// <summary>
		/// The RadioButtonList for the output formats.
		/// </summary>
		private RadioButtonList OutputFormats;

		/// <summary>
		/// The TextBox control for the width of the data-type.
		/// </summary>
		private TextBox TextBoxWidth;

		/// <summary>
		/// The TextBox control for the height of the data-type.
		/// </summary>
		private TextBox TextBoxHeight;

		/// <summary>
		/// The CheckBox for enabling the SafeMode transform option.
		/// </summary>
		private CheckBox SafeMode;

		/// <summary>
		/// The CheckBox for enabling the ExtraMode transform option.
		/// </summary>
		private CheckBox ExtraMode;

		/// <summary>
		/// The CheckBox for enabling the MarkdownInHtml transform option.
		/// </summary>
		private CheckBox MarkdownInHtml;

		/// <summary>
		/// The CheckBox for enabling the AutoHeadingIDs transform option.
		/// </summary>
		private CheckBox AutoHeadingIDs;

		/// <summary>
		/// The CheckBox for enabling the NewWindowForExternalLinks transform option.
		/// </summary>
		private CheckBox NewWindowForExternalLinks;

		/// <summary>
		/// The CheckBox for enabling the NewWindowForLocalLinks transform option.
		/// </summary>
		private CheckBox NewWindowForLocalLinks;

		/// <summary>
		/// The CheckBox for enabling the NoFollowLinks transform option.
		/// </summary>
		private CheckBox NoFollowLinks;

		/// <summary>
		/// The CheckBox for enabling the DisableAutoIndent transform option.
		/// </summary>
		private CheckBox DisableAutoIndent;

		/// <summary>
		/// The CheckBox for enabling the disableTabHandling transform option.
		/// </summary>
		private CheckBox DisableTabHandling;


		/// <summary>
		/// Initializes a new instance of the <see cref="PrevalueEditor"/> class.
		/// </summary>
		/// <param name="dataType">Type of the data.</param>
		/// <param name="dbType">Type of the db.</param>
		public PrevalueEditor(BaseDataType dataType, DBTypes dbType)
			: base() {
			this.m_DataType = dataType;
			this.m_DataType.DBType = dbType;
		}

		/// <summary>
		/// Gets the editor.
		/// </summary>
		/// <value>The editor.</value>
		public Control Editor {
			get { return this; }
		}

		/// <summary>
		/// Gets the PreValue options for the data-type.
		/// </summary>
		/// <typeparam name="T">The type of the resulting object.</typeparam>
		/// <returns>
		/// Returns the options for the PreValue Editor
		/// </returns>
		public T GetPreValueOptions<T>() {
			var prevalues = PreValues.GetPreValues(this.m_DataType.DataTypeDefinitionId);
			if (prevalues.Count > 0) {
				var prevalue = (PreValue)prevalues[0];
				if (!String.IsNullOrEmpty(prevalue.Value)) {
					try {
						// deserialize the options
						var serializer = new JavaScriptSerializer();

						// return the options
						return serializer.Deserialize<T>(prevalue.Value);
					} catch (Exception ex) {
						Log.Add(LogTypes.Error, this.m_DataType.DataTypeDefinitionId, string.Concat("MarkdownDeep Editor: Execption thrown: ", ex.Message));
					}
				}
			}

			// if all else fails, return default options
			return default(T);
		}

		private void setControls() {
			// set the values
			this.TextBoxWidth.Text = this.Options.Width.ToString();
			this.TextBoxHeight.Text = this.Options.Height.ToString();
			this.OutputFormats.SelectedValue = this.Options.OutputFormat.ToString();

			this.SafeMode.Checked = this.Options.SafeMode;
			this.ExtraMode.Checked = this.Options.ExtraMode;
			this.MarkdownInHtml.Checked = this.Options.MarkdownInHtml;
			this.AutoHeadingIDs.Checked = this.Options.AutoHeadingIDs;
			this.NewWindowForExternalLinks.Checked = this.Options.NewWindowForExternalLinks;
			this.NewWindowForLocalLinks.Checked = this.Options.NewWindowForLocalLinks;
			this.NoFollowLinks.Checked = this.Options.NoFollowLinks;

			this.EnableEditorUI.Checked = this.Options.EnableEditorUI;
			this.DisableAutoIndent.Checked = this.Options.DisableAutoIndent;
			this.DisableTabHandling.Checked = this.Options.DisableTabHandling;
			this.ShowPreview.SelectedValue = this.Options.ShowPreview.ToString();
			this.ShowPreviewWithDefaultCssFile.Checked = this.Options.ShowPreviewWithDefaultCssFile;
			this.ShowPreviewWithCustomClassName.Text = this.Options.ShowPreviewWithCustomClassName;
			this.ShowPreviewWithCustomCssFile.Text = this.Options.ShowPreviewWithCustomCssFile;
		}

		/// <summary>
		/// Saves this instance.
		/// </summary>
		public void Save() {
			int valInt;

			// parse the width
			if (int.TryParse(this.TextBoxWidth.Text, out valInt) && valInt > 200) this.Options.Width = valInt;

			// parse the height
			if (int.TryParse(this.TextBoxHeight.Text, out valInt) && valInt > 20) this.Options.Height = valInt;
			
			// set the options
			this.Options.OutputFormat = this.OutputFormats.SelectedValue.ParseToEnum<Options.OutputFormats>(this.Options.OutputFormat);

			this.Options.SafeMode = this.SafeMode.Checked;
			this.Options.ExtraMode = this.ExtraMode.Checked;
			this.Options.MarkdownInHtml = this.MarkdownInHtml.Checked;
			this.Options.AutoHeadingIDs = this.AutoHeadingIDs.Checked;
			this.Options.NewWindowForExternalLinks = this.NewWindowForExternalLinks.Checked;
			this.Options.NewWindowForLocalLinks = this.NewWindowForLocalLinks.Checked;
			this.Options.NoFollowLinks = this.NoFollowLinks.Checked;

			this.Options.EnableEditorUI = this.EnableEditorUI.Checked;

			this.Options.DisableAutoIndent = this.DisableAutoIndent.Checked;
			this.Options.DisableTabHandling = this.DisableTabHandling.Checked;

			this.Options.ShowPreview = this.ShowPreview.SelectedValue.ParseToEnum<Options.ShowPreviewOptions>(this.Options.ShowPreview);
			this.Options.ShowPreviewWithDefaultCssFile = this.ShowPreviewWithDefaultCssFile.Checked;
			this.Options.ShowPreviewWithCustomClassName = this.ShowPreviewWithCustomClassName.Text.Replace('.', ' ').Replace("  ", " ").Trim();
			this.Options.ShowPreviewWithCustomCssFile = this.ShowPreviewWithCustomCssFile.Text;

			// save the options as JSON
			this.SaveAsJson();

			// set controls to loaded values
			setControls();
		}

		/// <summary>
		/// Saves the data-type PreValue options.
		/// </summary>
		public void SaveAsJson() {
			// serialize the options into JSON
			var serializer = new JavaScriptSerializer();
			var json = serializer.Serialize(this.Options);

			lock (m_Locker) {
				var prevalues = PreValues.GetPreValues(this.m_DataType.DataTypeDefinitionId);
				if (prevalues.Count > 0) {
					PreValue prevalue = (PreValue)prevalues[0];

					// update
					prevalue.Value = json;
					prevalue.Save();
				} else {
					// insert
					PreValue.MakeNew(this.m_DataType.DataTypeDefinitionId, json);
				}
			}
		}

		/// <summary>
		/// Renders the HTML opening tag of the control to the specified writer. This method is used primarily by control developers.
		/// </summary>
		/// <param name="writer">A <see cref="T:System.Web.UI.HtmlTextWriter"/> that represents the output stream to render HTML content on the client.</param>
		public override void RenderBeginTag(HtmlTextWriter writer) {
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "settings");
			writer.RenderBeginTag(HtmlTextWriterTag.Div); //// start 'settings'

			base.RenderBeginTag(writer);
		}

		/// <summary>
		/// Renders the HTML closing tag of the control into the specified writer. This method is used primarily by control developers.
		/// </summary>
		/// <param name="writer">A <see cref="T:System.Web.UI.HtmlTextWriter"/> that represents the output stream to render HTML content on the client.</param>
		public override void RenderEndTag(HtmlTextWriter writer) {
			base.RenderEndTag(writer);

			writer.RenderEndTag(); //// end 'settings'
		}

		/// <summary>
		/// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
		protected override void OnInit(EventArgs e) {
			base.OnInit(e);

			this.EnsureChildControls();

			// Adds the client dependencies.
			this.AddResourceToClientDependency("Xilium.MarkdownDeepEditor4Umbraco.Resources.PrevalueEditor.style.css", ClientDependency.Core.ClientDependencyType.Css);
		}

		/// <summary>
		/// Creates child controls for this control
		/// </summary>
		protected override void CreateChildControls() {
			base.CreateChildControls();

			// set-up child controls
			this.TextBoxWidth = new TextBox() { ID = "Width", CssClass = "guiInputText" };
			this.TextBoxHeight = new TextBox() { ID = "Height", CssClass = "guiInputText" };
			this.OutputFormats = new RadioButtonList() { ID = "OutputFormats", RepeatDirection = RepeatDirection.Vertical, RepeatLayout = RepeatLayout.Flow };

			this.SafeMode = new CheckBox() { ID = "SafeMode", Text = "SafeMode (only safe markup)" };
			this.ExtraMode = new CheckBox() { ID = "ExtraMode", Text = "ExtraMode (MarkdownExtra extensions)" };
			this.MarkdownInHtml = new CheckBox() { ID = "MarkdownInHtml", Text = "Markdown in Html (markdown in nested html)" };
			this.AutoHeadingIDs = new CheckBox() { ID = "AutoHeadingIDs", Text = "Auto Heading IDs (automatically generate IDs for headings)" };
			this.NewWindowForExternalLinks = new CheckBox() { ID = "NewWindowForExternalLinks", Text = "New window for external links" };
			this.NewWindowForLocalLinks = new CheckBox() { ID = "NewWindowForLocalLinks", Text = "New window for local links" };
			this.NoFollowLinks = new CheckBox() { ID = "NoFollowLinks", Text = "No-follow links (add rel=nofollow to all external links)" };
			this.DisableAutoIndent = new CheckBox() { ID = "DisableAutoIndent", Text = "Disable auto-indent (disables auto tab-indent on pressing enter)" };
			this.DisableTabHandling = new CheckBox() { ID = "DisableTabHandling", Text = "Disable tab-handling (disables tab key working in the editor)" };

			this.EnableEditorUI = new CheckBox() { ID = "EnableEditorUI", Text = "yes, enable EditorUI with toolbar" };
			this.ShowPreview = new RadioButtonList() { ID = "ShowPreview", RepeatDirection = RepeatDirection.Vertical, RepeatLayout = RepeatLayout.Flow };
			this.ShowPreviewWithDefaultCssFile = new CheckBox() { ID = "ShowPreviewWithDefaultCssFile", Text = "Load default style (load default css file to render preview area)" };
			this.ShowPreviewWithCustomClassName = new TextBox() { ID = "ShowPreviewWithCustomClassName", CssClass = "guiInputText" };
			this.ShowPreviewWithCustomCssFile = new TextBox() { ID = "ShowPreviewWithCustomCssFile", CssClass = "guiInputText guiInputStandardSize" };

			// populate the controls

			var items = new ListItem[]
			{
				new ListItem("HTML - Renders the Markdown text as HTML, stored as CDATA text.", Options.OutputFormats.HTML.ToString()),
				new ListItem("XML - Renders the Markdown text as HTML, stored as raw XML.", Options.OutputFormats.XML.ToString()),
				new ListItem("Markdown - Outputs the raw Markdown, stored as CDATA text.", Options.OutputFormats.Markdown.ToString())
			};
			this.OutputFormats.Items.AddRange(items);

			items = new ListItem[]
			{
				new ListItem("None - No preview will appear", Options.ShowPreviewOptions.None.ToString()),
				new ListItem("Toolbar - Adds Compose/Preview toggle buttons to the toolbar (default)", Options.ShowPreviewOptions.Toolbar.ToString()),
				new ListItem("Show - Auto-updated preview to appear below the editor", Options.ShowPreviewOptions.Show.ToString())
			};
			this.ShowPreview.Items.AddRange(items);

			// add the child controls
			this.Controls.Add(this.TextBoxWidth);
			this.Controls.Add(this.TextBoxHeight);
			this.Controls.Add(this.OutputFormats);

			this.Controls.Add(this.SafeMode);
			this.Controls.Add(this.ExtraMode);
			this.Controls.Add(this.MarkdownInHtml);
			this.Controls.Add(this.AutoHeadingIDs);
			this.Controls.Add(this.NewWindowForExternalLinks);
			this.Controls.Add(this.NewWindowForLocalLinks);
			this.Controls.Add(this.NoFollowLinks);
			this.Controls.Add(this.DisableAutoIndent);
			this.Controls.Add(this.DisableTabHandling);

			this.Controls.Add(this.EnableEditorUI);
			this.Controls.Add(this.ShowPreview);
			this.Controls.Add(this.ShowPreviewWithDefaultCssFile);
			this.Controls.Add(this.ShowPreviewWithCustomClassName);
			this.Controls.Add(this.ShowPreviewWithCustomCssFile);
		}

		/// <summary>
		/// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
		/// </summary>
		/// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
		protected override void OnLoad(EventArgs e) {
			base.OnLoad(e);

			// get PreValues, load them into the controls.
			var options = this.GetPreValueOptions<Options>();

			// no options? use the default ones.
			if (options == null) options = new Options(true);

			this.Options = options;

			// set controls to loaded values
			setControls();
		}


		/// <summary>
		/// Renders the contents of the control to the specified writer. This method is used primarily by control developers.
		/// </summary>
		/// <param name="writer">A <see cref="T:System.Web.UI.HtmlTextWriter"/> that represents the output stream to render HTML content on the client.</param>
		protected override void RenderContents(HtmlTextWriter writer) {
			// add property fields

			writer.OpenPrevalueGroup("General settings");
			writer.AddPrevalueRow("Editor width:", this.TextBoxWidth);
			writer.AddPrevalueRow("Editor height:", this.TextBoxHeight);
			writer.AddPrevalueRow("Output Format:", this.OutputFormats);
			writer.ClosePrevalueGroup();

			writer.OpenPrevalueGroup("Markdown");
			writer.AddPrevalueRow("Transform options:"
				, this.SafeMode
				, this.ExtraMode
				, this.MarkdownInHtml
				, this.AutoHeadingIDs
				, this.NewWindowForExternalLinks
				, this.NewWindowForLocalLinks
				, this.NoFollowLinks);
			writer.ClosePrevalueGroup();

			writer.OpenPrevalueGroup("EditorUI");
			writer.AddPrevalueRow("Enable EditorUI:", this.EnableEditorUI);
			writer.AddPrevalueRow("Options:"
				, this.DisableAutoIndent
				, this.DisableTabHandling);
			writer.ClosePrevalueGroup();

			writer.OpenPrevalueGroup("EditorUI preview");
			writer.AddPrevalueRow("Show preview:", this.ShowPreview);
			writer.AddPrevalueRow("CSS style", this.ShowPreviewWithDefaultCssFile);
			writer.AddPrevalueRow("Add ClassName", "it is usefull to customize preview style with custom css", this.ShowPreviewWithCustomClassName);
			writer.AddPrevalueRow("Add custom CSS", "custom css file to customize preview style; must be valid href value; example: \"/css/umbEditor/mdd_preview.css\"", this.ShowPreviewWithCustomCssFile);
			writer.ClosePrevalueGroup();
		}
	}
}

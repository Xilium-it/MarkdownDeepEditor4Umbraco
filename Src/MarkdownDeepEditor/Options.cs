using System;
using System.ComponentModel;

namespace Xilium.MarkdownDeepEditor4Umbraco {
	/// <summary>
	/// The options for the Markdown data-type.
	/// </summary>
	public class Options {
		/// <summary>
		/// Initializes a new instance of the <see cref="Options"/> class.
		/// </summary>
		public Options() {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Options"/> class.
		/// </summary>
		/// <param name="loadDefaults">if set to <c>true</c> [load defaults].</param>
		public Options(bool loadDefaults) {
			if (loadDefaults) {
				this.EnableUIEditor = true;
				this.Width = 600;
				this.Height = 400;
				this.OutputFormat = OutputFormats.HTML;
				this.ShowPreview = ShowPreviewOptions.Toolbar;

				this.SafeMode = false;
				this.ExtraMode = true;
				this.MarkdownInHtml = false;
				this.AutoHeadingIDs = false;
				this.NewWindowForExternalLinks = true;
				this.NewWindowForLocalLinks = false;
				this.NoFollowLinks = false;

				this.DisableAutoIndent = false;
				this.DisableTabHandling = false;
			}
		}


		/// <summary>
		/// Gets or sets a value indicating whether [enable UI Editor].
		/// </summary>
		/// <value><c>true</c> if [enable UI Editor]; otherwise, <c>false</c>.</value>
		[DefaultValue(true)]
		public bool EnableUIEditor { get; set; }

		/// <summary>
		/// Gets or sets the width.
		/// </summary>
		/// <value>The width.</value>
		[DefaultValue(600)]
		public int Width { get; set; }

		/// <summary>
		/// Gets or sets the height.
		/// </summary>
		/// <value>The height.</value>
		[DefaultValue(400)]
		public int Height { get; set; }
		
		/// <summary>
		/// Gets or sets the output format.
		/// </summary>
		/// <value>The output format.</value>
		[DefaultValue(OutputFormats.HTML)]
		public OutputFormats OutputFormat { get; set; }
		
		/// <summary>
		/// Gets or sets the selected preview.
		/// </summary>
		/// <value>The selected preview.</value>
		[DefaultValue(ShowPreviewOptions.Toolbar)]
		public ShowPreviewOptions ShowPreview { get; set; }

		/// <summary>
		/// enable only safe markup (default:false)
		/// </summary>
		[DefaultValue(false)]
		public bool SafeMode { get; set; }

		/// <summary>
		/// enable MarkdownExtra extensions (default:true)
		/// </summary>
		[DefaultValue(true)]
		public bool ExtraMode { get; set; }

		/// <summary>
		/// allow markdown in nested html (eg: divs) (default:false)
		/// </summary>
		[DefaultValue(false)]
		public bool MarkdownInHtml { get; set; }

		/// <summary>
		/// automatically generate IDs for headings (default:false)
		/// </summary>
		[DefaultValue(false)]
		public bool AutoHeadingIDs { get; set; }

		/// <summary>
		/// add target=_blank for links to urls starting with http:// (default:true)
		/// </summary>
		[DefaultValue(true)]
		public bool NewWindowForExternalLinks { get; set; }

		/// <summary>
		/// add target=_blank for local relative links (good for preview mode) (default:false)
		/// </summary>
		[DefaultValue(false)]
		public bool NewWindowForLocalLinks { get; set; }

		/// <summary>
		/// add rel=nofollow to all external links (default:false)
		/// </summary>
		[DefaultValue(false)]
		public bool NoFollowLinks { get; set; }

		/// <summary>
		/// disables auto tab-indent on pressing enter (default:false)
		/// </summary>
		[DefaultValue(false)]
		public bool DisableAutoIndent { get; set; }

		/// <summary>
		/// disables tab key working in the editor (default:false)
		/// </summary>
		[DefaultValue(false)]
		public bool DisableTabHandling { get; set; }


		/// <summary>
		/// The output formats for the data-type.
		/// </summary>
		public enum OutputFormats {
			/// <summary>
			/// Outputs as HTML.
			/// </summary>
			HTML = 0,

			/// <summary>
			/// Outputs as XML.
			/// </summary>
			XML = 1,

			/// <summary>
			/// Outputs as raw Markdown.
			/// </summary>
			Markdown = 2
		}


		public enum ShowPreviewOptions {
			
			/// <summary>
			/// Previow unavailable
			/// </summary>
			None = 0,

			/// <summary>
			/// Show/hide preview with buttons in toolbar
			/// </summary>
			Toolbar = 1,

			/// <summary>
			/// Show preview below editor
			/// </summary>
			Below = 2
		}

	}
}

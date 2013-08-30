MarkdownDeepEditor4Umbraco
==========================

The "MarkdownDeep Editor for Umbraco" is a data-type that allows you to write your content using MarkdownDeep syntax.

Configuration options are available to enable/disable the EditorUI.

This package is a fork of "Markdown Editor for Umbraco" developed by "leekelleher"
(visit [markdown4umb project](http://markdown4umb.codeplex.com/)).

**MarkdownDeep Editor for Umbraco** uses MarkdownDeep instead of MarkdownSharp, that adds table feature with alignment,
colspan and head-style sttributes to any table cell.

MarkdownDeep Editor allow detailed level of customization. You can enable/disable:

* **safeMode**: only safe markup
* **extraMode**: enables MarkdownExtra extensions
* **markdownInHtml**: allow markdown syntax in nested html
* **autoHeadingIDs**: automatically generate IDs for headings
* **newWindowForExternalLinks**: new window for external links (target=_blank)
* **newWindowForLocalLinks**: new window for local links (target=_blank)
* **noFollowLinks**: add rel=nofollow to all external links


It is possibile to utilize custom Markdown formatter developing a class in `App_Code` folder. This class will be
inherits from `TextFormatter.TextFormatterBase`, and with `TextFormatter.DefaultTextFormatter` attribute declaration.

Example:
	
```c#

	using Xilium.MarkdownDeepEditor4Umbraco.TextFormatter;

	[DefaultTextFormatter()]
	public class MyMDDFormatter : TextFormatterBase {
		private Xilium.MarkdownDeep.Markdown _instance = null;

		public MyMDDFormatter(DataEditor dataEditor, Options options)
			: base(dataEditor, options) {

			this._instance = new Xilium.MarkdownDeep.Markdown();
			this._instance.SafeMode = true;
			this._instance.ExtraMode = false;
			this._instance.MarkdownInHtml = this.Options.MarkdownInHtml;
		}

		public override string Transform(string value) {
			return this._instance.Transform(value);
		}
	}

```

## News in 2.1.0 revision:

### features

* add default css file to render Preview area. It defines titles, paragraph and tables tag.
* add setting option to define custom className for Editor control.
* add setting option to define custom css file to import for define custom Preview.

### issues

* preview don't works properly when there are more then 1 instance.
 
 
## MarkdownDeep syntax

For a reference to MarkdownDeep and its syntax please visit: [MarkdownDeep project](https://github.com/Xilium-it/MarkdownDeep)




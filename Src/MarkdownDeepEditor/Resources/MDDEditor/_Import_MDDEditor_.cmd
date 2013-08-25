rem Imports MarkdownDeep Editor from MarkdownDeep project into this folder
rem WARNING: this procedure updates existing files.

set MDDEditorPath=..\..\..\..\..\MarkdownDeep\MarkdownDeepJS

del MarkdownDeep*.js
del mdd_*.gif
del mdd_*.png
del mdd_*.css
del mdd_*.html

xcopy %MDDEditorPath%\MarkdownDeep.js .
xcopy %MDDEditorPath%\MarkdownDeepEditor.js .
xcopy %MDDEditorPath%\MarkdownDeepEditorUI.js .
xcopy %MDDEditorPath%\mdd_*.gif .
xcopy %MDDEditorPath%\mdd_*.png .
xcopy %MDDEditorPath%\mdd_*.css .
xcopy %MDDEditorPath%\mdd_*.html .

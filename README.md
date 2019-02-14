# Appchecker README
The Dynamics Appchecker is a set of technologies that allow customers to gain insight into their application code (i.e. source and metadata) in ways that have not been possible before. The tecnology is based on representing both source code and metadata as XML and providing rich search facilities by using the XQuery language to express declarative queries over the source code.

## Rules
The rules directory contains the set of rules that are will be checked when the user uploads their deployable package to LCS. If the submitted application code does not satisfy these requirements, the deployable package will not be accepted. The rules can be inspected and downloaded for any use, but Microsoft is the only agent that can modify and add new rules.

## Tools
The tools directory contains tools that the user can use to query over his source code that is extracted into an XML database.

## Using

* First, you will need to have a BaseX server installed on a server that can be reached from the computer on which you are running this extension. BaseX is open source and can be installed from here: www.basex.org.
* You then need to extract the ASTs for the source code that you are looking at.

## Known Issues

None at this time

## Contributing

This project welcomes contributions and suggestions. Most contributions require you to
agree to a Contributor License Agreement (CLA) declaring that you have the right to,
and actually do, grant us the rights to use your contribution. For details, visit
https://cla.microsoft.com.

Please note that u

When you submit a pull request, a CLA-bot will automatically determine whether you need
to provide a CLA and decorate the PR appropriately (e.g., label, comment). Simply follow the
instructions provided by the bot. You will only need to do this once across all repositories using our CLA.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/)
or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

## Release Notes

## Working with Markdown

**Note:** You can author your README using Visual Studio Code.  Here are some useful editor keyboard shortcuts:

* Split the editor (`Cmd+\` on OSX or `Ctrl+\` on Windows and Linux)
* Toggle preview (`Shift+CMD+V` on OSX or `Shift+Ctrl+V` on Windows and Linux)
* Press `Ctrl+Space` (Windows, Linux) or `Cmd+Space` (OSX) to see a list of Markdown snippets

### For more information

* [Visual Studio Code's Markdown Support](http://code.visualstudio.com/docs/languages/markdown)
* [Markdown Syntax Reference](https://help.github.com/articles/markdown-basics/)

**Enjoy!**
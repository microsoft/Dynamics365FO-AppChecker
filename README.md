# Application checker README
The Dynamics Application checker is a set of technologies that allow customers to gain insight into their application code (i.e. source and metadata) in ways that have not been possible before. The technology is based on representing both source code and metadata in XML and providing rich search facilities by using the XQuery language to express declarative queries over the source code. The current implementation runs inside a BaseX repository running locally on the developer's box. You can find more information about installing BaseX here: https://docs.microsoft.com/en-us/dynamics365/unified-operations/dev-itpro/dev-tools/install-basex  

## Rules
The rules directory contains the set of rules that are will be checked when the user uploads their deployable package to LCS. If the submitted application code does not satisfy these requirements, the deployable package will not be accepted. The rules can be inspected and downloaded for any use, but Microsoft is the only agent that can modify and add new rules.

## Tools
The tools directory contains tools that the user can use to query over his source code that is extracted into an XML database.

## Using

* First, you will need to have a BaseX server installed on a server that can be reached from the computer on which you are running this extension. BaseX is open source and can be installed from here: www.basex.org. You will be given instructions the first time you use the application checker.
* You then need to extract the ASTs for the source code that you are looking at. This happens automatically during building of the deployable backage. The X++ compiler now supports generating the XML artifacts as a by product of compilation.

## Known Issues

None at this time

## Notes and Caveats
As discussed above, the compiler generates an enriched, structured XML representation of the source code. This is stored by BaseX in a directory with the name of the package being compiled inside the root directory called data. You can run your own queries over this content by using the BaseX GUI tool. If you want to delete this representation of the source, use the BaseX GUI to drop the database with the appropriate name  (Database | Open and manage... | Drop...)

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

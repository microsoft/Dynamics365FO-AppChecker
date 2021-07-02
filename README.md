
| Project | Status |
| --- | --- |
| Explorer .NET Framework | ![Build.Socratex](https://github.com/microsoft/Dynamics365FO-AppChecker/workflows/Build.Socratex/badge.svg)|
| Explorer .NET Core | ![Build.Explorer.netcore](https://github.com/microsoft/Dynamics365FO-AppChecker/workflows/Build.Explorer.netcore/badge.svg) |
| Ast Visualizer| ![Build.AstVisualizer](https://github.com/microsoft/Dynamics365FO-AppChecker/workflows/Build.AstVisualizer/badge.svg) |
| Python Extractor | ![Python Extractor](https://github.com/microsoft/Dynamics365FO-AppChecker/workflows/Python%20Extractor/badge.svg)|
| Go Extractor| ![Go Extractor](https://github.com/microsoft/Dynamics365FO-AppChecker/workflows/Go%20Extractor/badge.svg) |
| Bulk Query tool | ![Build.BulkQuery.netcore](https://github.com/microsoft/Dynamics365FO-AppChecker/workflows/Build.BulkQuery.netcore/badge.svg) |
| Graph Explorer .NET Core | ![Build.GraphExplorer.netcore](https://github.com/microsoft/Dynamics365FO-AppChecker/workflows/Build.GraphExplorer.netcore/badge.svg) |

This is the non-microsoft version
# Application checker
The Dynamics Application checker is a set of technologies that allow customers to gain insight into their application code (i.e. source and metadata) in ways that have not been possible before. The technology is based on representing both source code and metadata in XML and providing rich search facilities by using the XQuery language to express declarative queries over the source code. The current implementation runs inside a BaseX repository running locally on the developer's box. You can find more information about installing BaseX here: https://docs.microsoft.com/en-us/dynamics365/unified-operations/dev-itpro/dev-tools/install-basex

## Rules
The rules directory contains the set of rules that are currently available and will be checked when the user uploads their deployable package to LCS. If the submitted application code does not satisfy these requirements, the deployable package will not be accepted. The rules can be inspected and downloaded for any use, but Microsoft is the only agent that can modify and add new rules.

## Tools
The tools directory contains tools that the user can use to query over his source code that is extracted into an XML database.

## Using

* First, you will need to have the BaseX functionality installed on the computer running this extension. BaseX is open source and can be installed from here: www.basex.org. You will be given installation instructions the first time you use the application checker.
* You then need to extract the ASTs for the source code that you are looking at. This happens automatically during the compilation process of a full package and/or building of the deployable package. The X++ compiler now supports generating the XML artifacts as a _by product_ of compilation.

## Known Issues

The Java virtual machine needs to know how much RAM it is allowed to allocate. If the default supplied is too high for you to start the VM, you will get an error message. To work around that, create an environment variable called BASEX_JVM and set it to the following value:

set BASEX_JVM=-Xmx3G

where the specification after the Xmx specifies the number of gigabytes to allocate.

## Notes and Caveats
As discussed above, the compiler generates an enriched, structured XML representation of the source code. This is stored by BaseX in a directory with the name of the package being compiled inside the root directory called data. You can run your own queries over this content by using the BaseX GUI tool. If you want to delete this representation of the source, use the BaseX GUI to drop the database with the appropriate name  (Database | Open and manage... | Drop...)

## Contributing

This project welcomes contributions and suggestions. Most contributions require you to
agree to a Contributor License Agreement (CLA) declaring that you have the right to,
and actually do, grant us the rights to use your contribution. For details, visit
https://cla.microsoft.com.

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

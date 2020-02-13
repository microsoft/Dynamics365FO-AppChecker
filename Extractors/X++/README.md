# How to extract XML from X++ source code and metdata
There is no extractor per se for the X++ language, because the functionality is already built into the compiler for that language. Please use the following flags to instruct the X++ compiler to create the XML files in the designated directory:

$ xppc.exe -writeAsts -astOutputPath=<OutputDirectory>

If the designated directory does not exist, it will be created. The direcotory will contain a nested directory carrying the name of the model in which the artifacts are found.


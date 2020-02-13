# Copyright (c) Microsoft Corporation.
# Licensed under the MIT license.

import os.path
import sys
import argparse
import pathlib
import xmlnodegenerator
import generator

# This is the entry point for the python Xml extractor.
def extractor() -> None:
    '''
    Main entry point. It receives the parsed command line options
    and dispatches control to the extractor with the files to extract
    '''

    parser = argparse.ArgumentParser(description='Extract XML from python code')
    parser.add_argument('source', type=str, help='The root of the python codebase to extract')
    parser.add_argument('target', type=str, help='The directory storing the generated XML')
    parser.add_argument('--verbose', type=bool, default=False, help='provide more diagnostic output')
    parser.add_argument('--formatxml', type=bool, default=False, help='format the output')

    args = parser.parse_args()

    if args.verbose == True:
        isverbose = True
    else:
        isverbose = False

    if args.formatxml == True:
        format = True
    else:
        format = False

    if isverbose:
        print("Source folder   : " + args.source)
        print("Target folder   : " + args.target)

    # Check that both arguments denote directories. The root directory
    # must exist, while the out directory is created if it does not exist.
    if not os.path.isdir(args.source):
        print(args.source + " is not an existing directory", file=sys.stderr)
        return

    if not os.path.isdir(args.target):
        if isverbose:
            print(args.target + " does not exist. Creating...")

        # This recursively creates the directory and does not raise an exception
        # if the directory already exists.
        pathlib.Path(args.target).mkdir(parents=True, exist_ok=True)

    # Iterate over the the python files in the root directory and generate for each
    path = pathlib.Path(args.source)
    pathlist = list(path.glob('**/*.py'))
    for path in pathlist:
        # because path is object, not string
        path_in_str = str(path)
        generator.generator(args.source, path_in_str, args.target, isverbose, format)

if __name__ == '__main__':
    extractor()

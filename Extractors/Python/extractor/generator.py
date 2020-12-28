# Copyright (c) Microsoft Corporation.
# Licensed under the MIT license.

import ast
import xmlnodegenerator
import xml.etree as ET
import pathlib

def generator(source_root: 'The toplevel directory',
              source_path: 'The actual file name, transitively under the root',
              target_path: 'The place where the file should be written',
              verbose: bool,
              format: 'Whether or not the XML generated should be readable by humans') -> None:
    '''
    Generate the XML representation from the specified path into the directory
    passed in the out parameter. Issue interesting messages if the verbose flag
    is set
    '''

    if verbose:
        print("Generating " + source_path + " into " + target_path)

    try:
        source = open(source_path, 'r', errors='ignore', encoding="utf-8").read()
        tree = ast.parse(source)
        # s = ast.dump(tree)
        xml = _generate(tree, source, verbose, format)
        xml.set("Name", str(pathlib.Path(source_path).name))
        xml.set("Artifact", str(source_path))
        xml.set("Language", "Python")
        xml.set("Source", source)

        save(xml, source_root, source_path, target_path)
    except IOError as io:
        print("IO Exception handled: " + repr(io.message.filename))
    except SyntaxError as su:
        print("Syntax error in source: " + source_path + "(" + str(su.lineno), ", " + str(su.offset) + "):\n" + str(su))
    except Exception as e:
        print("General error in source: " + source_path + ": " + str(e))

def save(xml, source_root, source_path, target_path):
    sp = pathlib.Path(source_path)
    root_removed = sp.relative_to(source_root)
    target = target_path /  root_removed
    target = target.with_suffix('.xml')

    # Create the directory structure if it does not already exist
    pathlib.Path(target.parent).mkdir(parents=True, exist_ok=True)

    ET.ElementTree.ElementTree(xml).write(target)

    #with open(target, 'w') as file:
    #    file.write(t.tostring()  )

def class_name(obj):
    """ Returns the class name of an object"""
    return obj.__class__.__name__

def indent(elem: ET.ElementTree, level=0):
    ''' Indents XML so it can be pretty printed '''
    i = "\n" + level*"  "
    if len(elem):
        if not elem.text or not elem.text.strip():
            elem.text = i + "  "
        if not elem.tail or not elem.tail.strip():
            elem.tail = i
        for elem in elem:
            indent(elem, level+1)
        if not elem.tail or not elem.tail.strip():
            elem.tail = i
    else:
        if level and (not elem.tail or not elem.tail.strip()):
            elem.tail = i

def _generate(syntax_tree, source_code: str, verbose, format):
    generator = xmlnodegenerator.XmlNodeGenerator(source_code)
    xml = generator.visitModule(syntax_tree)

    if format:
        indent(xml)

    if verbose:
        ET.ElementTree.dump(xml)

    return xml


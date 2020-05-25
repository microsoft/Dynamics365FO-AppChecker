// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;

namespace WorkflowLibrary
{
    public static class TranslateExtensions
    {
        /// <summary>
        /// Do XSLT transformation from the script provided as the file name.
        /// </summary>
        /// <param name="document">The incoming document</param>
        /// <param name="filename">The file containing the XSLT script</param>
        /// <returns>The transformed document.</returns>
        public static XDocument TranslateFromFile(this XDocument document, string filename)
        {
            if (document == null)
                return null;

            if (string.IsNullOrEmpty(filename))
                throw new ArgumentNullException(nameof(filename));

            if (!File.Exists(filename))
                throw new ArgumentException("No such file: " + filename, nameof(filename));

            // Create and load the transform with script execution enabled.
            XslCompiledTransform transform = new XslCompiledTransform();

            XsltSettings settings = new XsltSettings() { EnableScript = true };

            transform.Load(filename, settings, null);

            var outputStore = new StringBuilder();
            using (XmlWriter output = XmlWriter.Create(outputStore))
            {
                // Execute the transformation.
                transform.Transform(document.CreateReader(ReaderOptions.None), output);
            }

            // Now the results are in the output store.
            return XDocument.Parse(outputStore.ToString());
        }

        /// <summary>
        /// Do an XSLT transformation with the script provided as the source parameter
        /// </summary>
        /// <param name="document">The incoming document</param>
        /// <param name="xslt">The string containing the XSLT script that transforms the incoming document
        /// to the returned value. An exception is thrown if this parameter is not provided.</param>
        /// <returns>The transformed document. </returns>
        public static XDocument TranslateFromSource(this XDocument document, string xslt)
        {
            if (document == null)
                return null;

            if (string.IsNullOrEmpty(xslt))
                throw new ArgumentNullException(nameof(xslt));

            // Create and load the transform with script execution enabled.
            XslCompiledTransform transform = new XslCompiledTransform();

            XsltSettings settings = new XsltSettings() { EnableScript = true };

            using (var reader = XmlReader.Create(new StringReader(xslt)))
            {
                transform.Load(reader, settings, null);

                var outputStore = new StringBuilder();
                using (XmlWriter output = XmlWriter.Create(outputStore))
                {
                    // Execute the transformation.
                    transform.Transform(document.CreateReader(ReaderOptions.None), output);
                }

                // Now the results are in the output store.
                return XDocument.Parse(outputStore.ToString());
            }
        }

        /// <summary>
        /// Do a transformation given a function/
        /// </summary>
        /// <param name="document">The incoming document.</param>
        /// <param name="transformation">The function that provides the transformation from the
        /// incoming document to the returned value. If no function is provided, the identity
        /// function is assumed.</param>
        /// <returns></returns>
        public static XDocument Transform(this XDocument document, Func<XDocument, XDocument> transformation)
        {
            if (document == null)
                return null;

            // By definition the null function is interpreted as the identity function.
            if (transformation == null)
                return document;

            return transformation(document);
        }

        public static XDocument SaveAsFile(this XDocument document, string filename)
        {
            if (document == null)
                return null;

            if (string.IsNullOrEmpty(filename))
                throw new ArgumentNullException(nameof(filename));

            try
            {
                document.Save(filename);
            }
            catch (Exception )
            {
                return null;
            }
            return document;
        }

        public static string SaveAsFile(this string document, string filename)
        {
            if (document == null)
                return null;

            if (string.IsNullOrEmpty(filename))
                throw new ArgumentNullException(nameof(filename));

            try
            {
                System.IO.File.WriteAllText(filename, document);
            }
            catch (Exception)
            {
                return null;
            }
            return document;
        }

        // TODO: Put in a transformation that writes the information back into the database.
    }
}

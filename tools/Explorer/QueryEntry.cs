// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace XppReasoningWpf
{
    public class QueryEntry
    {
        public string Path { get; set; }
        public string Description { get; set; }

        public string Language { get; set; }

        public string Name {  get { return System.IO.Path.GetFileNameWithoutExtension(this.Path);  } }
    }
}

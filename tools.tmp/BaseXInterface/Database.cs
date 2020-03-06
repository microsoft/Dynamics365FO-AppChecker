// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.using System;

using System;
namespace BaseXInterface
{
    /// <summary>
    /// Serialization format for information coming back from BaseX
    /// </summary>
    public class Database
    {
        public string Name { get; set; }
        public int Resources { get; set; }
        public Int64 Size { get; set; }
        public bool IsCurrent { get; set; }
    }
}

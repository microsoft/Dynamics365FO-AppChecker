// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.
using System;

namespace BaseXInterface
{
    public class DatabaseSession : Session, IDisposable
    {
        public bool InUse { get; set; }
        public string Database { get; private set; }

        public DatabaseSession (string database, string host, int port, string username, string pw)
            : base(host, port, username, pw)
        {
            this.Database = database;
        }

        public void Dispose()
        {
            // Put this back into the pool of sessions to be reused for this database.
            BaseXInterface.BaseXServer.ReturnSession(this);
        }
    }
}

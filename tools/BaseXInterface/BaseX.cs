// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
// ReSharper disable All

namespace BaseXInterface
{
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;

    public delegate void DatabaseOpeningDelegate(string databaseName);
    public delegate void DatabaseOpenedDelegate(string databaseName);

    /// <summary>
    /// Interface to the BaseX service
    /// </summary>
    public class BaseXServer
    {
        /// <summary>
        /// List of database sessions. The sessions will be reused.
        /// </summary>
        private List<DatabaseSession> sessions = new List<DatabaseSession>();
        private readonly static SemaphoreSlim locker = new SemaphoreSlim(1, 1);
        private readonly string host;
        private readonly int port;

        private string Username { get; }

        private string Password { get; }

        /// <summary>
        /// Signals that the provided database is being opened.
        /// </summary>
        public DatabaseOpeningDelegate DatabaseOpening;

        /// <summary>
        /// Signals that the provided database has been opened.
        /// </summary>
        public DatabaseOpenedDelegate DatabaseOpened;

        public BaseXServer(string server, int port, string username, string password)
        {
            this.host = server;
            this.port = port;
            this.Username = username;
            this.Password = password;
        }

        /// <summary>
        /// Call this method when the connection to the server is no longer wanted.
        /// </summary>
        public async Task CloseConnectionAsync()
        {
            await locker.WaitAsync();
            try
            {
                foreach (var session in this.sessions)
                {
                    session.Close();
                }
                sessions = null;
            }
            finally
            {
                locker.Release();
            }
        }

        private DatabaseSession CreateSession(string database)
        {
            return new DatabaseSession(database, this.host, this.port, this.Username, this.Password);
        }

        private DatabaseSession CreateSession(string database, object tag)
        {
            var retval = new DatabaseSession(database, this.host, this.port, this.Username, this.Password)
            {
                Tag = tag
            };
            return retval;
        }

        public async Task<DatabaseSession> GetSessionAsync(string database)
        {
            await locker.WaitAsync();
            try
            {
                var existingUnused = sessions.Where(s => !s.InUse && string.Compare(s.Database, database, true) == 0).FirstOrDefault();
                if (existingUnused == null)
                {
                    // there were no sessions ready to be used. Create one and add to list.
                    var session = this.CreateSession(database);
                    session.InUse = true;
                    sessions.Add(session);
                    if (database.Length != 0)
                    {
                        this.DatabaseOpening?.Invoke(database);
                        await session.OpenDatabaseAsync(database);
                        this.DatabaseOpened?.Invoke(database);
                    }
                    return session;
                }
                else
                {
                    // The session is no longer in use, so go ahead and reuse.
                    existingUnused.InUse = true;
                    return existingUnused;
                }
            }
            finally
            {
                locker.Release();
            }
        }

        public DatabaseSession GetSession(string database)
        {
            var existingUnused = sessions.Where(s => !s.InUse && string.Compare(s.Database, database, true) == 0).FirstOrDefault();
            if (existingUnused == null)
            {
                // there were no sessions ready to be used. Create one and add to list.
                var session = this.CreateSession(database);
                session.InUse = true;
                sessions.Add(session);
                if (database.Length != 0)
                {
                    this.DatabaseOpening?.Invoke(database);
                    session.OpenDatabase(database);
                    this.DatabaseOpened?.Invoke(database);
                }
                return session;
            }
            else
            {
                // The session is no longer in use, so go ahead and reuse.
                existingUnused.InUse = true;
                return existingUnused;
            }
        }

        // TODO: Put in a timer that will make sure that running sessions that have
        // not been used for a time are closed.

        static internal void ReturnSession(DatabaseSession session)
        {
            locker.WaitAsync();
            session.InUse = false;
            locker.Release();
        }

        public async Task<ObservableCollection<Database>> GetDatabasesAsync()
        {
            var res = new ObservableCollection<Database>();

            string databases;
            using (var session = await this.GetSessionAsync(""))
            {
                databases = await session.ExecuteAsync("list");
            }

            string line;
            using (StringReader r = new StringReader(databases))
            {
                r.ReadLine(); // Name Resource, Size ...
                r.ReadLine(); // --------
                while ((line = r.ReadLine()) != null)
                {
                    if (string.IsNullOrEmpty(line))
                        break;
                    var re = new Regex(@"^(?'name'.+)\s+(?'resources'\d+)\s+(?'size'\d+)", RegexOptions.IgnoreCase);
                    var matches = re.Match(line);

                    res.Add(new Database()
                    {
                        Name = matches.Groups["name"].Value.Trim(),
                        Resources = int.Parse(matches.Groups["resources"].Value),
                        Size = Int64.Parse(matches.Groups["size"].Value),
                        IsCurrent = false,
                    });
                }
            }
            return res;
        }

    }
}

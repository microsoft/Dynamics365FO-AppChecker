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

    /// <summary>
    /// Interface to the BaseX service
    /// </summary>
    public class BaseXServer
    {
        /// <summary>
        /// List of database sessions. The sessions will be reused.
        /// </summary>
        private List<DatabaseSession> sessions = new List<DatabaseSession>();
        private readonly static object locker = new object();
        private readonly string host;
        private readonly int port;

        private string Username { get; }

        private string Password { get; }

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
        public void CloseConnection()
        {
            lock (locker)
            {
                foreach (var session in this.sessions)
                {
                    session.Close();
                }
                sessions = null;
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

        public DatabaseSession GetSession(string database)
        {
            lock (locker)
            {
                var existingUnused = sessions.Where(s => !s.InUse && string.Compare(s.Database, database, true) == 0).FirstOrDefault();
                if (existingUnused == null)
                {
                    // there were no sessions ready to be used. Create one and add to list.
                    var session = this.CreateSession(database);
                    session.InUse = true;
                    sessions.Add(session);
                    if (database.Length != 0)
                        session.OpenDatabase(database);
                    return session;
                }
                else
                {
                    // The session is no longer in use, so go ahead and reuse.
                    existingUnused.InUse = true;
                    return existingUnused;
                }
            }
        }

        // TODO: Put in a timer that will make sure that running sessions that have
        // not been used for a time are closed.

        static internal void ReturnSession(DatabaseSession session)
        {
            lock (locker)
            {
                session.InUse = false;
            }
        }

        public ObservableCollection<Database> GetDatabases()
        {
            var res = new ObservableCollection<Database>();

            try
            {
                string databases;
                using (var session = this.GetSession(""))
                {
                    databases = session.Execute("list");
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
                        var re = new Regex(@"^(?'name'.+)\s+(?'resources'\d+)\s+(?'size'\d+)" , RegexOptions.IgnoreCase);
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
            }
            catch (Exception e)
            {
                string s = ("Exception " + e.Message);
                throw;
            }

            return res;
        }

    }
}

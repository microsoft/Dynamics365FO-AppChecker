/*
 * Language Binding for BaseX.
 * Works with BaseX 7.0 and later
 *
 * Documentation: http://docs.basex.org/wiki/Clients
 *
 * (C) BaseX Team 2005-12, BSD License
 */
// Added async support
using System;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BaseXInterface
{
    public class Session 
    {
        private readonly byte[] cache = new byte[4096];
        private NetworkStream stream;
        private TcpClient socket;
        private string info = "";
        private int bpos;
        private int bsize;

        /// <summary>
        /// For identification purposes.
        /// </summary>
        public object Tag { get; set; }

        public Session(string host, int port, string username, string pw)
        {
            socket = new TcpClient(host, port);

            stream = socket.GetStream();
            string[] response = Receive().Split(':');

            string nonce;
            string code;
            if (response.Length > 1)
            {
                code = username + ":" + response[0] + ":" + pw;
                nonce = response[1];
            }
            else
            {
                code = pw;
                nonce = response[0];
            }

            Send(username);
            Send(MD5(MD5(code) + nonce));
            if (stream.ReadByte() != 0)
            {
                throw new IOException("Access denied.");
            }
        }

        private void Execute(string com, Stream ms)
        {
            Send(com);
            Init();
            Receive(ms);
            info = Receive();
            if (!Ok())
            {
                throw new IOException(info);
            }
        }

        public virtual string Execute(string com)
        {
            MemoryStream ms = new MemoryStream();
            Execute(com, ms);
            return System.Text.Encoding.UTF8.GetString(ms.ToArray());
        }

        private async Task ExecuteAsync(string com, Stream ms)
        {
            await SendAsync(com);
            this.Init();
            await ReceiveAsync(ms);
            info = await ReceiveAsync();
            if (!(await OkAsync()))
            {
                throw new IOException(info);
            }
        }

        public virtual async Task<string> ExecuteAsync(string com)
        {
            MemoryStream ms = new MemoryStream();
            await ExecuteAsync(com, ms);
            return System.Text.Encoding.UTF8.GetString(ms.ToArray());
        }

        public async void CloseAsync()
        {
            await SendAsync("exit");
            socket.Close();
        }

        public void Close()
        {
            Send("exit");
            // socket.Close();
        }

        private void Init()
        {
            bpos = 0;
            bsize = 0;
        }

        private async Task<byte> ReadAsync()
        {
            if (bpos == bsize)
            {
                bsize = await stream.ReadAsync(cache, 0, 4096);
                bpos = 0;
            }
            return cache[bpos++];
        }

        private byte Read()
        {
            if (bpos == bsize)
            {
                bsize = stream.Read(cache, 0, 4096);
                bpos = 0;
            }
            return cache[bpos++];
        }

        private void Receive(Stream ms)
        {
            while (true)
            {
                byte b = Read();
                if (b == 0) break;
                // read next byte if 0xFF is received
                ms.WriteByte(b == 0xFF ? Read() : b);
            }
        }

        private async Task ReceiveAsync(Stream ms)
        {
            while (true)
            {
                byte b = await ReadAsync();
                if (b == 0)
                    break;
                // read next byte if 0xFF is received
                ms.WriteByte(b == 0xFF ? await ReadAsync() : b);
            }
        }

        private async Task<string> ReceiveAsync()
        {
            MemoryStream ms = new MemoryStream();
            await this.ReceiveAsync(ms);
            return Encoding.UTF8.GetString(ms.ToArray());
        }

        private string Receive()
        {
            MemoryStream ms = new MemoryStream();
            Receive(ms);
            return Encoding.UTF8.GetString(ms.ToArray());
        }

        private async Task SendAsync(string message)
        {
            byte[] msg = System.Text.Encoding.UTF8.GetBytes(message);
            await stream.WriteAsync(msg, 0, msg.Length);
            stream.WriteByte(0);
        }

        private void Send(string message)
        {
            byte[] msg = System.Text.Encoding.UTF8.GetBytes(message);
            stream.Write(msg, 0, msg.Length);
            stream.WriteByte(0);
        }

        private async Task SendAsync(Stream s)
        {
            while (true)
            {
                int t = s.ReadByte();
                if (t == -1) break;
                if (t == 0x00 || t == 0xFF)
                    stream.WriteByte(Convert.ToByte(0xFF));

                stream.WriteByte(Convert.ToByte(t));
            }
            stream.WriteByte(0);
            info = await ReceiveAsync();
            if (!(await this.OkAsync()))
            {
                throw new IOException(info);
            }
        }

        private void Send(Stream s)
        {
            while (true)
            {
                int t = s.ReadByte();
                if (t == -1) break;
                if (t == 0x00 || t == 0xFF) stream.WriteByte(Convert.ToByte(0xFF));
                stream.WriteByte(Convert.ToByte(t));
            }
            stream.WriteByte(0);
            info = Receive();
            if (!Ok())
            {
                throw new IOException(info);
            }
        }

        private async Task<bool> OkAsync()
        {
            return await ReadAsync() == 0;
        }

        private bool Ok()
        {
            return Read() == 0;
        }

        private string MD5(string input)
        {
            MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();
            byte[] hash = MD5.ComputeHash(Encoding.UTF8.GetBytes(input));

            StringBuilder sb = new StringBuilder();
            foreach (byte h in hash)
            {
                sb.Append(h.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}

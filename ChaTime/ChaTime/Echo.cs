using System;
using System.Collections.Generic;

namespace Lesson002_02_DataTransfer
{
    public class Echo : IDisposable
    {
        private Queue<byte> storage;

        public Echo(string host, int port)
        {
            this.storage = new Queue<byte>();
        }

        public int Send(byte[] data)
        {
            var cnt = 0;
            if (data != null)
            {
                foreach (var d in data)
                {
                    this.storage.Enqueue(d);
                }

                cnt = data.Length;
            }

            return cnt;
        }

        public int Available
        {
            get
            {
                return this.storage.Count;
            }
        }

        public int Receive(byte[] buffer)
        {
            var cnt = 0;
            if (buffer != null)
            {
                cnt = Math.Min(this.storage.Count, buffer.Length);
                if (cnt > 0)
                {
                    for (var i = 0; i < cnt; i++)
                    {
                        var d = this.storage.Dequeue();
                        buffer[i] = d;
                    }
                }
            }

            return cnt;
        }

        public void Dispose()
        {
            if (this.storage != null)
            {
                this.storage.Clear();
                this.storage = null;
            }
        }
    }
}


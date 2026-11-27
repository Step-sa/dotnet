using System;
using System.IO;

namespace ArtGalleryApp
{
    internal class GalleryLogger
    {
        public event Action<string>? EntryCreated;

        public void Log<T>(string message, T value)
        {
            string entry = $"[{DateTimeOffset.Now:yyyy-MM-dd HH:mm:ss zzz}] {message}: {value}";
            EntryCreated?.Invoke(entry);
        }
    }

    internal class ConsoleLogWriter
    {
        public void Write(string entry)
        {
            Console.WriteLine(entry);
        }
    }

    internal class FileLogWriter
    {
        private readonly string _path;

        public FileLogWriter(string path)
        {
            _path = path;
        }

        public void Write(string entry)
        {
            File.AppendAllText(_path, entry + Environment.NewLine, System.Text.Encoding.UTF8);
        }
    }
}

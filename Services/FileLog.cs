using System.Text;

namespace Tp_TODO.Services
{
    public class FileLog : IFileLog
    {
        private readonly string _filePath;
        private static readonly SemaphoreSlim _lock = new(1, 1);

        public FileLog(IWebHostEnvironment env)
        {
            var dir = Path.Combine(env.ContentRootPath, "Logs");
            Directory.CreateDirectory(dir);
            _filePath = Path.Combine(dir, "actions.log");
        }

        public async Task Write(string line)
        {
            await _lock.WaitAsync();
            try
            {
                await File.AppendAllTextAsync(_filePath, line + Environment.NewLine, Encoding.UTF8);
            }
            finally
            {
                _lock.Release();
            }
        }
    }
}

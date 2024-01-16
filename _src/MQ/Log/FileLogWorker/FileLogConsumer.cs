using RabbitMQ.Abstracts;
using RabbitMQ.Concretes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileLog.WORKER
{
    public class FileLogConsumer(ILogger<FileLogConsumer> logger, IRabbitMqConsumerService rabbitMqConsumerService) : BackgroundService
    {
        private readonly ILogger<FileLogConsumer> _logger = logger;
        private readonly IRabbitMqConsumerService _rabbitMqConsumerService = rabbitMqConsumerService;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //_rabbitMqConsumerService.ConsumeAll(async (message) => await Test1(message));
            //await _rabbitMqConsumerService.ConsumeAllAsync(AsyncExample1);
            _rabbitMqConsumerService.ConsumeAllSync(SyncExample1);
        }
        private void SyncExample1(string messageFromQueue)
        {
            _logger.LogInformation("SYNC: {message}", messageFromQueue);
        }
        private async Task AsyncExample1(string messageFromQueue)
        {
            _logger.LogInformation("ASYNC: {message}", messageFromQueue);
        }
        private static StreamWriter CreateFileStream()
        {
            var currentDate = DateOnly.FromDateTime(DateTime.UtcNow);

            string currentMonth = currentDate.Month < 10 ? "0" + currentDate.Month : currentDate.Month.ToString();
            string currentDay = currentDate.Day < 10 ? "0" + currentDate.Day : currentDate.Day.ToString();

            return new StreamWriter(path: Path.Combine(Directory.GetCurrentDirectory(), "LogFiles", $"{currentDay}_{currentMonth}_{currentDate.Year}_logs.txt"), append: true, encoding: Encoding.UTF8, bufferSize: 1024);
        }
        private static void FlushStreamAndClose(StreamWriter stream)
        {
            stream.Flush();
            stream.Dispose();
            stream.Close();
        }
        private static string GenerateErrorMessage(string fileName, string errorMessage)
        {
            return $"ERROR => DATETIME: {DateTime.UtcNow} - FILE: {fileName}.cs - CONTENT: {errorMessage}";
        }
        private static string GenerateInfoMessage(string fileName, string infoMessage)
        {
            return $"INFO => DATETIME: {DateTime.UtcNow} - FILE: {fileName}.cs - CONTENT: {infoMessage}";
        }
    }
}

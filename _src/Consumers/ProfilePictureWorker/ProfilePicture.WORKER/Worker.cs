using CoreService.Application.Repositories.ProfilePictureRepository;
using MassTransit;
using ProfilePicture.WORKER.Consumers;
using RabbitMQ.Abstracts;

namespace ProfilePicture.WORKER
{
    public class Worker(ILogger<Worker> logger, IProfilePictureWriteRepository profilePictureWriteRepository) : BackgroundService
    {
        private readonly ILogger<Worker> _logger = logger;
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("WORKER STARTED RUNNING");
            /*
            var configuredEventBus = _consumerEventBusProvider.ConfigureBus(configuration =>
            {
                configuration.ReceiveEndpoint(
                    queueName: "core-service-queue",
                    configureEndpoint1 =>
                    {
                        configureEndpoint1.Consumer<CreateProfilePictureMQConsumer>();
                    });
            });
            await configuredEventBus.StartAsync(stoppingToken);
            if(stoppingToken.IsCancellationRequested)
            {
                await configuredEventBus.StopAsync(stoppingToken);
            }
            */
        }
        private async Task AsyncExample1(string messageFromQueue)
        {
            _logger.LogInformation("ASYNC: {message}", messageFromQueue);
        }
        /* RABBITMQ EXAMPLES
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _rabbitMqConsumerService.ConsumeAllAsync(AsyncExample1);
            }
        }
        private void SyncExample1(string messageFromQueue)
        {
            _logger.LogInformation("SYNC: {message}", messageFromQueue);
        }
        private async Task AsyncExample1(string messageFromQueue)
        {
            _logger.LogInformation("ASYNC: {message}", messageFromQueue);
        }
        */
        /*
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
         
         */
    }
}

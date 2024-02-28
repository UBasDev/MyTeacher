using CoreService.Application.MQContracts.Commands.Abstracts;
using CoreService.Application.Repositories.ProfilePictureRepository;
using CoreService.Domain.Entities.ProfilePicture;
using MassTransit;

namespace ProfilePicture.WORKER.Consumers
{
    public class CreateProfilePictureMQConsumer : IConsumer<ICreateProfilePictureMQCommand>
    {
        public CreateProfilePictureMQConsumer(ILogger<CreateProfilePictureMQConsumer> logger, IProfilePictureWriteRepository profilePictureWriteRepository) {
            _logger = logger;
            _profilePictureWriteRepository = profilePictureWriteRepository;
        }
        private readonly ILogger<CreateProfilePictureMQConsumer> _logger;
        private readonly IProfilePictureWriteRepository _profilePictureWriteRepository;
        public async Task Consume(ConsumeContext<ICreateProfilePictureMQCommand> context)
        {
            _logger.LogInformation("Consumer is running");
            var messageFromQueue = context.Message;
            try
            {
                var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                var fullFilePathToSave = Path.Combine(folderPath, $"profilePicx{new Random().Next(1, 10000)}.jpg");
                var streamWriter = new StreamWriter(fullFilePathToSave, append: true);

                await streamWriter.BaseStream.WriteAsync(messageFromQueue.ProfilePicture, context.CancellationToken);
                //await streamWriter.BaseStream.WriteAsync(messageFromQueue.ProfilePicture, 0, messageFromQueue.ProfilePicture.Length, context.CancellationToken);

                var (isSucessful, errorMessage) = await _profilePictureWriteRepository.CreateSingleDocumentAsync(ProfilePictureEntity.CreateNewProfilePicture(messageFromQueue.UserId, messageFromQueue.UserProfileId, fullFilePathToSave, ".jpg", (uint)messageFromQueue.ProfilePicture.Length));
                
                if (!isSucessful) _logger.LogError("This profile picture file couldn't uploaded. TraceId: {@CorrelationId} UserId: {@UserId} UserProfileId: {@UserProfileId} ErrorMessage: {@ErrorMessage}", context.CorrelationId, messageFromQueue.UserId, messageFromQueue.UserProfileId, errorMessage);
                await streamWriter.DisposeAsync();
                streamWriter.Close();

            }
            catch (Exception ex)
            {
                _logger.LogError("An unexpected error has been occurred while uploading profile picture. TraceId: {@CorrelationId} UserId: {@UserId} UserProfileId: {@UserProfileId} ErrorMessage: {@ErrorMessage}", context.CorrelationId, messageFromQueue.UserId, messageFromQueue.UserProfileId, ex.Message);
            }
        }
    }
}

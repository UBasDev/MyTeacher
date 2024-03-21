using CoreService.Application.MQContracts.Commands.Abstracts;
using CoreService.Application.Repositories.ProfilePicture;
using CoreService.Application.Repositories.ProfilePictureRepository;
using CoreService.Domain.Entities.ProfilePicture;
using MassTransit;
using MongoDB.Driver;

namespace ProfilePicture.WORKER.Consumers
{
    public class CreateProfilePictureMQConsumer : IConsumer<ICreateProfilePictureMQCommand>
    {
        public CreateProfilePictureMQConsumer(ILogger<CreateProfilePictureMQConsumer> logger, IProfilePictureWriteRepository profilePictureWriteRepository)
        {
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
                if (messageFromQueue == null) return;

                var fullFilePathToSave = CreateFullPathForFile(messageFromQueue.ProfilePictureName, messageFromQueue.ProfilePictureExtension);

                var streamWriter = new StreamWriter(fullFilePathToSave, append: true);

                await streamWriter.BaseStream.WriteAsync(messageFromQueue.ProfilePictureData, context.CancellationToken);
                //await streamWriter.BaseStream.WriteAsync(messageFromQueue.ProfilePicture, 0, messageFromQueue.ProfilePicture.Length, context.CancellationToken);

                /*
                var currentUserActiveProfilePhotos = await _profilePictureReadRepository.GetDocumentsByConditionAsync(p => p.UserId == messageFromQueue.UserId && p.UserProfileId == messageFromQueue.UserProfileId && p.IsActive);

                if(currentUserActiveProfilePhotos.Count() > 0)
                {
                    await _profilePictureWriteRepository.UpdateSingleDocumentAsync(Builders<ProfilePictureEntity>.Filter.Where(p => p.UserId == messageFromQueue.UserId && p.UserProfileId == messageFromQueue.UserProfileId && p.IsActive), Builders<ProfilePictureEntity>.Update.Set(p => p.IsActive, false));
                }
                */

                var (isSucessful, errorMessage) = await _profilePictureWriteRepository.CreateSingleDocumentAsync(ProfilePictureEntity.CreateNewProfilePicture(messageFromQueue.UserId, messageFromQueue.UserProfileId, fullFilePathToSave, messageFromQueue.ProfilePictureExtension, (uint)messageFromQueue.ProfilePictureData.Length));

                if (!isSucessful) _logger.LogError("This profile picture file couldn't uploaded. TraceId: {@CorrelationId} UserId: {@UserId} UserProfileId: {@UserProfileId} Error: {@Error}", context.CorrelationId, messageFromQueue.UserId, messageFromQueue.UserProfileId, errorMessage);
                await streamWriter.DisposeAsync();
                //streamWriter.Close();

            }
            catch (Exception ex)
            {
                _logger.LogError("An unexpected error has been occurred while uploading profile picture. TraceId: {@CorrelationId} UserId: {@UserId} UserProfileId: {@UserProfileId} Error: {@Error}", context.CorrelationId, messageFromQueue.UserId, messageFromQueue.UserProfileId, ex.Message);
            }
        }
        private static string CreateFullPathForFile(string profilePictureName, string profilePictureExtension)
        {
            var currentDateTime = DateTime.Now;
            string year = currentDateTime.Year.ToString();
            string month = currentDateTime.Month.ToString().Length == 1 ? $"0{currentDateTime.Month}" : currentDateTime.Month.ToString();
            string date = currentDateTime.Day.ToString().Length == 1 ? $"0{currentDateTime.Day}" : currentDateTime.Day.ToString();
            string hour = currentDateTime.Hour.ToString().Length == 1 ? $"0{currentDateTime.Hour}" : currentDateTime.Hour.ToString();
            string minute = currentDateTime.Minute.ToString().Length == 1 ? $"0{currentDateTime.Minute}" : currentDateTime.Minute.ToString();
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), $"{year}_{month}_{date}_{hour}_{minute}_{profilePictureName}_{Guid.NewGuid().ToString().AsSpan(0, 5)}{profilePictureExtension}");
        }
    }
}

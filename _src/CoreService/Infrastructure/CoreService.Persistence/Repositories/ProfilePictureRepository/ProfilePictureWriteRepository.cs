using CoreService.Application.Repositories.ProfilePicture;
using CoreService.Domain.Entities.ProfilePicture;
using CoreService.Persistence.Repositories.MongoGenericRepository;
using MongoDB.Bson;
using MongoDb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreService.Application.Repositories.ProfilePictureRepository;

namespace CoreService.Persistence.Repositories.ProfilePictureRepository
{
    public class ProfilePictureWriteRepository(MongoDbSettings mongoDbSettings) : MongoGenericWriteRepository<ProfilePictureEntity, ObjectId>(mongoDbSettings, _collectionName), IProfilePictureWriteRepository
    {
        private const string _collectionName = "ProfilePictures";
    }
}

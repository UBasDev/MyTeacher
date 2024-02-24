using CoreService.Application.Repositories.ProfilePicture;
using CoreService.Domain.Entities.ProfilePicture;
using CoreService.Persistence.Repositories.MongoGenericRepository;
using MongoDb.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Persistence.Repositories.ProfilePictureRepository
{
    public class ProfilePictureReadRepository(MongoDbSettings mongoDbSettings) : MongoGenericReadRepository<ProfilePictureEntity, ObjectId>(mongoDbSettings, _collectionName), IProfilePictureReadRepository
    {
        private const string _collectionName = "ProfilePictures";
    }
}

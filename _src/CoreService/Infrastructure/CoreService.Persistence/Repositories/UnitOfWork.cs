﻿using CoreService.Application.Contexts;
using CoreService.Application.Repositories;
using CoreService.Application.Repositories.CompanyRepository;
using CoreService.Application.Repositories.ProfilePicture;
using CoreService.Application.Repositories.ProfilePictureRepository;
using CoreService.Application.Repositories.ProfileRepository;
using CoreService.Application.Repositories.RoleRepository;
using CoreService.Application.Repositories.UserRepository;
using CoreService.Domain.Entities.Profile;
using CoreService.Domain.Entities.User;
using CoreService.Persistence.Repositories.CompanyRepository;
using CoreService.Persistence.Repositories.ProfilePictureRepository;
using CoreService.Persistence.Repositories.ProfileRepository;
using CoreService.Persistence.Repositories.RoleRepository;
using CoreService.Persistence.Repositories.UserRepository;
using Microsoft.EntityFrameworkCore;
using MongoDb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Persistence.Repositories
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly MongoDbSettings _mongoDbSettings;
        private IProfileReadRepository _profileReadRepository;
        private IProfileWriteRepository _profileWriteRepository;
        private IUserReadRepository _userReadRepository;
        private IUserWriteRepository _userWriteRepository;
        private IProfilePictureReadRepository _profilePictureReadRepository;
        private IProfilePictureWriteRepository _profilePictureWriteRepository;
        private ICompanyReadRepository _companyReadRepository;
        private ICompanyWriteRepository _companyWriteRepository;
        private IRoleReadRepository _roleReadRepository;
        private IRoleWriteRepository _roleWriteRepository;

        public UnitOfWork(ApplicationDbContext dbContext, MongoDbSettings mongoDbSettings)
        {
            _dbContext = dbContext;
            _mongoDbSettings = mongoDbSettings;
        }

        public IProfileReadRepository ProfileReadRepository
        {
            get
            {
                _profileReadRepository = new ProfileReadRepository(_dbContext);
                return _profileReadRepository;
            }
        }

        public IProfileWriteRepository ProfileWriteRepository
        {
            get
            {
                _profileWriteRepository = new ProfileWriteRepository(_dbContext);
                return _profileWriteRepository;
            }
        }

        public IUserReadRepository UserReadRepository
        {
            get
            {
                _userReadRepository = new UserReadRepository(_dbContext);
                return _userReadRepository;
            }
        }

        public IUserWriteRepository UserWriteRepository
        {
            get
            {
                _userWriteRepository = new UserWriteRepository(_dbContext);
                return _userWriteRepository;
            }
        }

        public IProfilePictureReadRepository ProfilePictureReadRepository
        {
            get
            {
                _profilePictureReadRepository = new ProfilePictureReadRepository(_mongoDbSettings);
                return _profilePictureReadRepository;
            }
        }

        public IProfilePictureWriteRepository ProfilePictureWriteRepository
        {
            get
            {
                _profilePictureWriteRepository = new ProfilePictureWriteRepository(_mongoDbSettings);
                return _profilePictureWriteRepository;
            }
        }

        public ICompanyReadRepository CompanyReadRepository
        {
            get
            {
                _companyReadRepository = new CompanyReadRepository(_dbContext);
                return _companyReadRepository;
            }
        }

        public ICompanyWriteRepository CompanyWriteRepository
        {
            get
            {
                _companyWriteRepository = new CompanyWriteRepository(_dbContext);
                return _companyWriteRepository;
            }
        }

        public IRoleReadRepository RoleReadRepository
        {
            get
            {
                _roleReadRepository = new RoleReadRepository(_dbContext);
                return _roleReadRepository;
            }
        }

        public IRoleWriteRepository RoleWriteRepository
        {
            get
            {
                _roleWriteRepository = new RoleWriteRepository(_dbContext);
                return _roleWriteRepository;
            }
        }

        public void SaveChanges() => _dbContext.SaveChanges();

        public async Task SaveChangesAsync(CancellationToken cancellationToken) => await _dbContext.SaveChangesAsync(cancellationToken);

        private bool _disposed = false;

        private void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
                _dbContext.Dispose();
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
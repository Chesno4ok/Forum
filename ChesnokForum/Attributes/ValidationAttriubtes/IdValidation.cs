﻿using Forum.Application.DatabaseService;
using Forum.Logic.Models;
using Forum.Logic.Repository;
using Forum.Persistance;
using Forum.Persistence.Repository;
using Microsoft.Extensions.Caching.Distributed;
using Repository.Models;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Remoting;

namespace Forum.API.Attributes.ValidationAttriubtes
{
    public class IdValidation<R, O> : ValidationAttribute 
        where R : IRepository<O>
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {

            var repository = Activator.CreateInstance(typeof(R), validationContext.GetService<ForumContext>(),
                validationContext.GetService<IDistributedCache>()) as IRepository<O>;

            if (repository is null)
                throw new ArgumentException("Couldn't create repository");

            Guid? guid = (Guid?)value;

            if (guid is null)
                return new ValidationResult("Id is not guid", new string[] { "Id" });

            var item = repository.Get((Guid)guid).Result;

            if (item is not null)
                return ValidationResult.Success;
            return new ValidationResult("Item not found", new string[] {"Id"});
        }
    }
}

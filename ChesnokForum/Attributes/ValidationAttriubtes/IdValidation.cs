using Forum.Application.DatabaseService;
using Forum.Logic.Models;
using Forum.Logic.Repository;
using Forum.Persistance;
using Forum.Persistence.Repository;
using Repository.Models;
using System.ComponentModel.DataAnnotations;

namespace Forum.API.Attributes.ValidationAttriubtes
{
    public class IdValidation<T>(IRepository<T> repository) : ValidationAttribute
    {
        IRepository<T> repository = repository;
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            Guid? guid = (Guid?)value;

            if (guid is null)
                throw new ArgumentException("Value is not GUID");

            var item = repository.Get((Guid)guid);

            if (item is not null)
                return ValidationResult.Success;
            return new ValidationResult("Such user doesn't exist");

            new IdValidation<Post>(new PostRepository());

        }
    }
}

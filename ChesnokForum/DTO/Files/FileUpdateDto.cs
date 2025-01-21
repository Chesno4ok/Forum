using Forum.Application.Auth;
using Forum.Application.DatabaseService;
using Microsoft.CodeAnalysis.Host;
using System.ComponentModel.DataAnnotations;

namespace Forum.API.DTO.Files
{
    public class FileUpdateDto : IValidatableObject
    {
        public Guid Id { get; set; }
        public byte[]? Binary { get; set; } = null!;
        public string? FileExtension { get; set; } = null!;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var fileService = validationContext.GetService<FileService>();


            if(fileService.GetFile(Id).Result is null)
            {
                yield return new ValidationResult("File doesn't exist", new string[] { "Id" });
                yield break;
            }

            var authService = validationContext.GetService<AuthService>();
            var httpContextAccessor = (IHttpContextAccessor)validationContext.GetService(typeof(IHttpContextAccessor));
            var request = httpContextAccessor.HttpContext.Request;

            var userId = Guid.Parse(authService.GetId(request.Headers.First(i => i.Key == "Authorization").Value));
            if (fileService.GetFile(Id).Result.UserId != userId)
                yield return new ValidationResult("File doesn't belong to user", new string[] { "Id" });
        }
    }
}

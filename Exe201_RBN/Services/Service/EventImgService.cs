using BusinessObject;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Repositories.IRepositories;
using Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
	public class EventImgService : IEventImgService
	{
		private readonly IEventImgRepository _eventImgRepository;
		private readonly Cloudinary _cloudinary;

		public EventImgService(IEventImgRepository eventImgRepository, Cloudinary cloudinary)
		{
			_eventImgRepository = eventImgRepository;
			_cloudinary = cloudinary;
		}

		public async Task UploadFile(IFormFile file)
		{
			if (file.Length > 0)
			{
				// Convert the IFormFile into a stream to upload to Cloudinary
				await using var stream = file.OpenReadStream();
				var uploadParams = new ImageUploadParams()
				{
					File = new FileDescription(file.FileName, stream),
					PublicId = $"event_imgs/{Path.GetFileNameWithoutExtension(file.FileName)}"
				};

				var uploadResult = await _cloudinary.UploadAsync(uploadParams);

				if (uploadResult.StatusCode == HttpStatusCode.OK)
				{
					// After uploading to Cloudinary, save the image URL in the database
					var eventImg = new EventImg
					{
						ImageUrl = uploadResult.SecureUrl.ToString(),
						DateUpLoad = DateTime.Now
					};
					await _eventImgRepository.UploadFile(eventImg);
				}
				else
				{
					throw new Exception("Failed to upload image to Cloudinary");
				}
			}
		}
	}

}

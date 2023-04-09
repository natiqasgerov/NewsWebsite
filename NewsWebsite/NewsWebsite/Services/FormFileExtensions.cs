namespace NewsWebsite.Services
{
    public static class FormFileExtensions
    {
        public async static Task<byte[]> GetBytes(IFormFile formFile)
        {
            using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
}

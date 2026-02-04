using PRN232.FUNewsManagement.Models.Request.Tag;
using PRN232.FUNewsManagement.Models.Response.NewsArticle;
using PRN232.FUNewsManagement.Repo.Interfaces;
using PRN232.FUNewsManagement.Services.Interfaces;
using PRN232.FUNewsManagement.Services.Mappers;

namespace PRN232.FUNewsManagement.Services.Services
{
    public class TagService : ITagService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TagService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TagResponse?> GetByIdAsync(int id)
        {
            var tag = await _unitOfWork.Tags.GetByIdAsync(id);
            if (tag == null)
            {
                return null;
            }

            return TagMapper.ToResponse(tag);
        }

        public async Task<List<TagResponse>> GetAllAsync()
        {
            var tags = await _unitOfWork.Tags.GetAllTagsAsync();
            return tags.Select(TagMapper.ToResponse).ToList();
        }

        public async Task<TagResponse> CreateAsync(CreateTagRequest request)
        {
            // Check if tag name exists
            if (await _unitOfWork.Tags.NameExistsAsync(request.TagName))
            {
                throw new InvalidOperationException("Tag name already exists");
            }

            var tag = TagMapper.ToEntity(request);

            await _unitOfWork.Tags.AddAsync(tag);
            await _unitOfWork.SaveChangesAsync();

            return TagMapper.ToResponse(tag);
        }

        public async Task<TagResponse> UpdateAsync(int id, UpdateTagRequest request)
        {
            var tag = await _unitOfWork.Tags.GetByIdAsync(id);
            if (tag == null)
            {
                throw new InvalidOperationException("Tag not found");
            }

            // Check if tag name exists (excluding current tag)
            if (await _unitOfWork.Tags.NameExistsAsync(request.TagName, id))
            {
                throw new InvalidOperationException("Tag name already exists");
            }

            TagMapper.UpdateEntity(tag, request);

            _unitOfWork.Tags.Update(tag);
            await _unitOfWork.SaveChangesAsync();

            return TagMapper.ToResponse(tag);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var tag = await _unitOfWork.Tags.GetByIdAsync(id);
            if (tag == null)
            {
                return false;
            }

            // Check if tag has news articles
            if (await _unitOfWork.Tags.HasNewsArticlesAsync(id))
            {
                throw new InvalidOperationException("Cannot delete tag with linked news articles");
            }

            _unitOfWork.Tags.Delete(tag);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRN232.FUNewsManagementSystem.Repo.Models;
using PRN232.FUNewsManagementSystem.Repo.UnitOfWork;
using PRN232.FUNewsManagementSystem.Services.Services.IService;

namespace PRN232.FUNewsManagementSystem.Services.Services
{
    public class TagService : ITagService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TagService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Tag>> GetAllTagsAsync()
        {
            return await _unitOfWork.TagRepository.GetAllAsync();
        }

        public async Task<Tag> GetTagByIdAsync(int id)
        {
            return await _unitOfWork.TagRepository.GetByIdAsync(id);
        }

        public async Task<Tag> GetTagByNameAsync(string tagName)
        {
            return await _unitOfWork.TagRepository.GetTagByNameAsync(tagName);
        }

        public async Task<List<Tag>> GetTagsByNewsArticleIdAsync(int newsArticleId)
        {
            return await _unitOfWork.TagRepository.GetTagsByNewsArticleIdAsync(newsArticleId);
        }

        public async Task<bool> CreateTagAsync(Tag tag)
        {
            try
            {
                if (await _unitOfWork.TagRepository.IsTagNameExistAsync(tag.TagName))
                    return false;

                await _unitOfWork.TagRepository.CreateAsync(tag);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateTagAsync(Tag tag)
        {
            try
            {
                await _unitOfWork.TagRepository.UpdateAsync(tag);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteTagAsync(int id)
        {
            try
            {
                var tag = await _unitOfWork.TagRepository.GetByIdAsync(id);
                if (tag == null)
                    return false;

                await _unitOfWork.TagRepository.RemoveAsync(tag);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> IsTagNameExistAsync(string tagName)
        {
            return await _unitOfWork.TagRepository.IsTagNameExistAsync(tagName);
        }
    }
}

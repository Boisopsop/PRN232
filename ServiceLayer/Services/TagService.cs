using RepositoryLayer.Entities;
using RepositoryLayer.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace ServiceLayer.Services
{
    public interface ITagService
    {
        IQueryable<Tag> GetAllTags();
        Tag? GetTagById(int id);
        Tag? GetTagByName(string name);
        void CreateTag(Tag tag);
        void UpdateTag(Tag tag);
        void DeleteTag(int id);
    }

    public class TagService : ITagService
    {
        private readonly ITagRepository _repository;

        public TagService(ITagRepository repository)
        {
            _repository = repository;
        }

        public IQueryable<Tag> GetAllTags()
        {
            return _repository.GetAll();
        }

        public Tag? GetTagById(int id)
        {
            return _repository.GetById(id);
        }

        public Tag? GetTagByName(string name)
        {
            return _repository.GetByName(name);
        }

        public void CreateTag(Tag tag)
        {
            _repository.Add(tag);
            _repository.SaveChanges();
        }

        public void UpdateTag(Tag tag)
        {
            _repository.Update(tag);
            _repository.SaveChanges();
        }

        public void DeleteTag(int id)
        {
            var tag = _repository.GetById(id);
            if (tag != null)
            {
                _repository.Delete(tag);
                _repository.SaveChanges();
            }
        }
    }
}

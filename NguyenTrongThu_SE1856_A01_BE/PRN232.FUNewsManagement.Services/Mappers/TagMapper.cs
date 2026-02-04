using PRN232.FUNewsManagement.Models.Entities;
using PRN232.FUNewsManagement.Models.Request.Tag;
using PRN232.FUNewsManagement.Models.Response.NewsArticle;

namespace PRN232.FUNewsManagement.Services.Mappers
{
    public static class TagMapper
    {
        public static TagResponse ToResponse(Tag entity)
        {
            return new TagResponse
            {
                TagID = entity.TagID,
                TagName = entity.TagName,
                Note = entity.Note
            };
        }

        public static Tag ToEntity(CreateTagRequest request)
        {
            return new Tag
            {
                TagName = request.TagName,
                Note = request.Note
            };
        }

        public static void UpdateEntity(Tag entity, UpdateTagRequest request)
        {
            entity.TagName = request.TagName;
            entity.Note = request.Note;
        }
    }
}

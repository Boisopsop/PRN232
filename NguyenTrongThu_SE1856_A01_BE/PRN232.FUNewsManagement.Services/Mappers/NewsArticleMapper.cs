using PRN232.FUNewsManagement.Models.Entities;
using PRN232.FUNewsManagement.Models.Request.NewsArticle;
using PRN232.FUNewsManagement.Models.Response.NewsArticle;

namespace PRN232.FUNewsManagement.Services.Mappers
{
    public static class NewsArticleMapper
    {
        public static NewsArticleResponse ToResponse(NewsArticle entity)
        {
            return new NewsArticleResponse
            {
                NewsArticleID = entity.NewsArticleID,
                NewsTitle = entity.NewsTitle,
                Headline = entity.Headline,
                CreatedDate = entity.CreatedDate,
                CategoryID = entity.CategoryID,
                CategoryName = entity.Category?.CategoryName ?? string.Empty,
                NewsStatus = entity.NewsStatus,
                StatusText = entity.NewsStatus ? "Active" : "Inactive",
                CreatedByID = entity.CreatedByID,
                CreatedByName = entity.CreatedBy?.AccountName ?? string.Empty,
                ModifiedDate = entity.ModifiedDate
            };
        }

        public static NewsArticleDetailResponse ToDetailResponse(NewsArticle entity)
        {
            return new NewsArticleDetailResponse
            {
                NewsArticleID = entity.NewsArticleID,
                NewsTitle = entity.NewsTitle,
                Headline = entity.Headline,
                CreatedDate = entity.CreatedDate,
                NewsContent = entity.NewsContent,
                NewsSource = entity.NewsSource,
                CategoryID = entity.CategoryID,
                CategoryName = entity.Category?.CategoryName ?? string.Empty,
                NewsStatus = entity.NewsStatus,
                StatusText = entity.NewsStatus ? "Active" : "Inactive",
                CreatedByID = entity.CreatedByID,
                CreatedByName = entity.CreatedBy?.AccountName ?? string.Empty,
                ModifiedDate = entity.ModifiedDate,
                Tags = entity.NewsTags?.Select(nt => new TagResponse
                {
                    TagID = nt.Tag.TagID,
                    TagName = nt.Tag.TagName,
                    Note = nt.Tag.Note
                }).ToList() ?? new List<TagResponse>()
            };
        }

        public static NewsArticle ToEntity(
            CreateNewsArticleRequest request,
            string newsId,
            short createdById)
        {
            return new NewsArticle
            {
                NewsArticleID = newsId,
                NewsTitle = request.NewsTitle,
                Headline = request.Headline,
                NewsContent = request.NewsContent,
                NewsSource = request.NewsSource,
                CategoryID = request.CategoryID,
                NewsStatus = request.NewsStatus,
                CreatedByID = createdById,
                CreatedDate = DateTime.Now,
                NewsTags = request.TagIDs.Select(tagId => new NewsTag
                {
                    NewsArticleID = newsId,
                    TagID = tagId
                }).ToList()
            };
        }

        public static void UpdateEntity(
            NewsArticle entity,
            UpdateNewsArticleRequest request,
            short updatedById)
        {
            entity.NewsTitle = request.NewsTitle;
            entity.Headline = request.Headline;
            entity.NewsContent = request.NewsContent;
            entity.NewsSource = request.NewsSource;
            entity.CategoryID = request.CategoryID;
            entity.NewsStatus = request.NewsStatus;
            entity.UpdatedByID = updatedById;
            entity.ModifiedDate = DateTime.Now;

            // Update tags
            entity.NewsTags.Clear();
            foreach (var tagId in request.TagIDs)
            {
                entity.NewsTags.Add(new NewsTag
                {
                    NewsArticleID = entity.NewsArticleID,
                    TagID = tagId
                });
            }
        }
    }
}

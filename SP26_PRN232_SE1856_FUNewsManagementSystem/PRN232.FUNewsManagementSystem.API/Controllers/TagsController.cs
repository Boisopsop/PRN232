using Microsoft.AspNetCore.Mvc;
using PRN232.FUNewsManagementSystem.Repo.Models;
using PRN232.FUNewsManagementSystem.Services.Services.IService;

namespace PRN232.FUNewsManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagsController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tag>>> GetTags()
        {
            var tags = await _tagService.GetAllTagsAsync();
            return Ok(tags);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Tag>> GetTag(int id)
        {
            var tag = await _tagService.GetTagByIdAsync(id);

            if (tag == null)
            {
                return NotFound(new { message = $"Tag with ID {id} not found." });
            }

            return Ok(tag);
        }

        [HttpPost]
        public async Task<ActionResult<Tag>> CreateTag(Tag tag)
        {
            var result = await _tagService.CreateTagAsync(tag);

            if (!result)
            {
                return BadRequest(new { message = "Failed to create tag. Tag name may already exist." });
            }

            return CreatedAtAction(nameof(GetTag), new { id = tag.TagId }, tag);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTag(int id, Tag tag)
        {
            if (id != tag.TagId)
            {
                return BadRequest(new { message = "Tag ID mismatch." });
            }

            var result = await _tagService.UpdateTagAsync(tag);

            if (!result)
            {
                return NotFound(new { message = $"Tag with ID {id} not found." });
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTag(int id)
        {
            var result = await _tagService.DeleteTagAsync(id);

            if (!result)
            {
                return NotFound(new { message = $"Tag with ID {id} not found." });
            }

            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Tag>>> SearchTags(
            [FromQuery] string? keyword = null)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                var allTags = await _tagService.GetAllTagsAsync();
                return Ok(allTags);
            }

            var tags = await _tagService.GetAllTagsAsync();
            var filteredTags = tags.Where(t =>
                (t.TagName != null && t.TagName.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
                (t.Note != null && t.Note.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            ).ToList();

            return Ok(filteredTags);
        }
    }
}

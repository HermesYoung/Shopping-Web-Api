using DatabaseContext.Entities;
using ManagementSite.Controllers.Models;
using ManagementSite.Controllers.Models.Promotion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Abstracts;
using Repositories.Common;
using Repositories.Repositories.PromotionRepository.Models;

namespace ManagementSite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionController : ControllerBase
    {
        private readonly IPromotionRepository _promotionRepository;

        public PromotionController(IPromotionRepository promotionRepository)
        {
            _promotionRepository = promotionRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePromotion(PromotionCreateRequest promotionCreate)
        {
            var contentCreateResult = CreateContent(promotionCreate);

            if (!contentCreateResult.IsSuccess)
            {
                return BadRequest(contentCreateResult.Error);
            }
            
            var result = await _promotionRepository.CreatePromotionAsync(contentCreateResult.Value!);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }
            
            return NoContent();
        }

        private Result<PromotionContent> CreateContent(PromotionCreateRequest promotionCreate)
        {
            var providers = new List<IPromotionProvider>();
            if (promotionCreate.SpecialOffer != null)
            {
                providers.Add(promotionCreate.SpecialOffer);
            }

            if (promotionCreate.Discount != null)
            {
                providers.Add(promotionCreate.Discount);
            }
            
            if (providers.Count == 0)
            {
                return Result<PromotionContent>.Failure(Error.Create("Promotion provider not found", new ErrorMessage(ErrorCode.NoPromotionProvider)));
            }

            var promotionContent = new PromotionContent
            {
                StartDate = promotionCreate.StartDate,
                EndDate = promotionCreate.EndDate,
                Title = promotionCreate.Title,
                DisplayContent = promotionCreate.DisplayContent,
                Content = providers
            };
            
            return Result<PromotionContent>.Success(promotionContent);
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePromotion(PromotionUpdateRequest promotionUpdate)
        {
            var contentUpdateResult = CreateContent(promotionUpdate);
            if (!contentUpdateResult.IsSuccess)
            {
                return BadRequest(contentUpdateResult.Error);
            }
            
            var result = await _promotionRepository.UpdatePromotionAsync(promotionUpdate.Id ,contentUpdateResult.Value!);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }
            
            return Ok(promotionUpdate.Id);
        }

        [HttpGet]
        public async Task<IActionResult> GetPromotions([FromQuery] PromotionQuery query)
        {
            var result = await _promotionRepository.GetPromotionCalenderAsync(query.StartDate, query.EndDate);
          
            return Ok(result);
        }

        [HttpDelete("/{id}")]
        public async Task<IActionResult> DeletePromotion(Guid id)
        {
            var result = await _promotionRepository.DeletePromotionAsync(id);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }
            
            return Ok(id);
        }
        
    }
}
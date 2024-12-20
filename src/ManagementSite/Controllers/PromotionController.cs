using DatabaseContext.Entities;
using ManagementSite.Controllers.Models;
using ManagementSite.Controllers.Models.Promotion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Abstracts;
using Repositories.Common;
using Repositories.Repositories.PromotionRepository.Models;
using Repositories.Repositories.PromotionRepository.Models.PromotionProviders;

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
        public async Task<IActionResult> CreatePromotion(PromotionRequest promotion)
        {
            var contentCreateResult = CreateContent(promotion);

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

        private Result<PromotionContent> CreateContent(PromotionRequest promotion)
        {
            var providers = new List<PromotionProviderBase>();
            if (promotion.SpecialOffer != null)
            {
                providers.Add(promotion.SpecialOffer);
            }

            if (promotion.Discount != null)
            {
                providers.Add(promotion.Discount);
            }
            
            if (providers.Count == 0)
            {
                return Result<PromotionContent>.Failure(Error.Create("Promotion provider not found", new ErrorMessage(ErrorCode.NoPromotionProvider)));
            }

            var promotionContent = new PromotionContent
            {
                StartDate = promotion.StartDate,
                EndDate = promotion.EndDate,
                Title = promotion.Title,
                DisplayContent = promotion.DisplayContent,
                Content = providers
            };
            
            return Result<PromotionContent>.Success(promotionContent);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePromotion(Guid id ,[FromBody]PromotionRequest promotionUpdate)
        {
            var contentUpdateResult = CreateContent(promotionUpdate);
            if (!contentUpdateResult.IsSuccess)
            {
                return BadRequest(contentUpdateResult.Error);
            }
            
            var result = await _promotionRepository.UpdatePromotionAsync(id,contentUpdateResult.Value!);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }
            
            return Ok(id);
        }

        [HttpGet]
        public async Task<IActionResult> GetPromotions([FromQuery] PromotionQuery query)
        {
            var result = await _promotionRepository.GetPromotionCalenderAsync(query.StartDate, query.EndDate);
          
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPromotionAsync(Guid id)
        {
            var result = await _promotionRepository.GetPromotionDetailAsync(id);
            if (!result.IsSuccess)
            {
                return NotFound(result.Error);
            }
            
            return Ok(result.Value);
        }

        [HttpDelete("{id}")]
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
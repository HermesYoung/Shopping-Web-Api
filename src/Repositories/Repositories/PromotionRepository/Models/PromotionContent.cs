﻿using Repositories.Repositories.PromotionRepository.Models.PromotionProviders;

namespace Repositories.Repositories.PromotionRepository.Models;

public class PromotionContent
{
    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
    public required string Title { get; set; }

    public string? DisplayContent { get; set; }

    public required IEnumerable<PromotionProviderBase> Content { get; set; }
}
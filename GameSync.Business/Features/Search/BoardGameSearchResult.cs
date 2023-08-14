﻿namespace GameSync.Business.Features.Search;

public class BoardGameSearchResult
{
    public required int YearPublished { get; init; }
    public required string Name { get; init; }
    public bool IsExpansion { get; init; }
    public required int Id { get; init; }
}

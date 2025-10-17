using Application.Commands.PublisherCommands;
using Application.Shared;
using Shared;

namespace Application.Queries.PublisherQueries;

public class GetAllPublishersQuery : Query<List<PublisherCommandResult>>
{
    public override async Task<QueryExecutionResult<List<PublisherCommandResult>>> Execute()
    {
        var publishers = await _publisherRepository.GetAllAsync();
        if (publishers == null || !publishers.Any())
            return await Fail("No publishers found");

        var result = publishers.Select(p => new PublisherCommandResult
        {
            Id = p.Id,
            Name = p.Name
        }).ToList();

        return await Ok(result);
    }
}

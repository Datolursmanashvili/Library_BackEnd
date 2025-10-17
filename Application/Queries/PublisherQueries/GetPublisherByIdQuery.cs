using Application.Commands.PublisherCommands;
using Application.Shared;
using Shared;

namespace Application.Queries.PublisherQueries;

public class GetPublisherByIdQuery : Query<PublisherCommandResult>
{
    public int Id { get; set; }

    public override async Task<QueryExecutionResult<PublisherCommandResult>> Execute()
    {
        var publisher = await _publisherRepository.GetByIdAsync(Id);
        if (publisher == null)
            return await Fail("Publisher not found");

        return await Ok(new PublisherCommandResult
        {
            Id = publisher.Id,
            Name = publisher.Name
        });
    }
}

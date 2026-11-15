using MediatR;
using ModularCore.Application.Common.Interfaces;
using ModularCore.Domain.Entities;
using ModularCore.Domain.Exceptions;
namespace ModularCore.Application.ServiceRequests.Commands.AddComment;

public record AddCommentCommand(Guid ServiceRequestId, string Content, string AuthorName, bool IsInternal) : IRequest<Guid>;

public class AddCommentCommandHandler : IRequestHandler<AddCommentCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    public AddCommentCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task<Guid> Handle(AddCommentCommand request, CancellationToken cancellationToken)
    {
        var sr = await _context.ServiceRequests.FindAsync(new object[] { request.ServiceRequestId }, cancellationToken);
        if (sr == null) throw new NotFoundException("Service request not found.");

        var comment = new Comment
        {
            ServiceRequestId = request.ServiceRequestId,
            Content = request.Content,
            AuthorName = request.AuthorName,
            IsInternal = request.IsInternal
        };

        _context.Comments.Add(comment);
        await _context.SaveChangesAsync(cancellationToken);
        return comment.Id;
    }
}
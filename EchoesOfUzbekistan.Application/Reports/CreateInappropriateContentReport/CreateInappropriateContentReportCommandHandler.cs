using EchoesOfUzbekistan.Application.Abstractions.Messages;
using EchoesOfUzbekistan.Application.Users.Services;
using EchoesOfUzbekistan.Domain.Abstractions;
using EchoesOfUzbekistan.Domain.Guides;
using EchoesOfUzbekistan.Domain.Guides.Repositories;
using EchoesOfUzbekistan.Domain.Reports;
using FluentValidation;

namespace EchoesOfUzbekistan.Application.Reports.CreateInappropriateContentReport;
internal class CreateInappropriateContentReportCommandHandler : ICommandHandler<CreateInappropriateContentReportCommand, Guid>
{
    private readonly IGuideRepository _guideRepository;
    private readonly IInappropriateContentReportRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContextService _userContextService;
    private readonly IValidator<CreateInappropriateContentReportCommand> _validator;

    public CreateInappropriateContentReportCommandHandler(
        IInappropriateContentReportRepository repository,
        IUnitOfWork unitOfWork, IGuideRepository guideRepository, IValidator<CreateInappropriateContentReportCommand> validator, IUserContextService userContextService)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _guideRepository = guideRepository;
        _validator = validator;
        _userContextService = userContextService;
    }

    public async Task<Result<Guid>> Handle(CreateInappropriateContentReportCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        if (request.UserId != _userContextService.UserId)
            return Result.Failure<Guid>(Error.CannotPostForOthers);

        var guide = await _guideRepository.GetByIdAsync(request.AudioGuideId, cancellationToken);
        if (guide == null)
            return Result.Failure<Guid>(AudioGuideErrors.NotFound);

        var report = new InappropriateContentReport(
            request.UserId,
            request.AudioGuideId,
            request.Reason);

        _repository.Add(report);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(report.Id);
    }
}

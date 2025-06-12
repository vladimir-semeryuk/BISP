using EchoesOfUzbekistan.Application.Abstractions.FileHandling;
using EchoesOfUzbekistan.Application.Abstractions.Messages;
using EchoesOfUzbekistan.Application.Files.Services.FileNamingStrategies;
using EchoesOfUzbekistan.Application.TextToSpeech.Interfaces;
using EchoesOfUzbekistan.Application.Users.Services;
using EchoesOfUzbekistan.Domain.Abstractions;
using FluentValidation;

namespace EchoesOfUzbekistan.Application.TextToSpeech.SynthesizeAudio;
internal class SynthesizeAudioCommandHandler : ICommandHandler<SynthesizeAudioCommand, SynthesizeAudioResult>
{
    private readonly IFileService _fileService;
    private readonly IUserContextService _userContext;
    private readonly ITextToSpeechService _textToSpeechService;
    private readonly IValidator<SynthesizeAudioCommand> _validator;

    public SynthesizeAudioCommandHandler(IUserContextService userContext, IFileService fileService, ITextToSpeechService textToSpeechService, IValidator<SynthesizeAudioCommand> validator)
    {
        _userContext = userContext;
        _fileService = fileService;
        _textToSpeechService = textToSpeechService;
        _validator = validator;
    }

    public async Task<Result<SynthesizeAudioResult>> Handle(SynthesizeAudioCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var userId = _userContext.UserId;

        var isLanguageSupported = _textToSpeechService.GetVoiceNameByLanguage(request.LanguageCode);
        if (string.IsNullOrEmpty(isLanguageSupported))
            return Result.Failure<SynthesizeAudioResult>(Error.UnsupportedTTSLanguage);

        var file = await _textToSpeechService.SynthesizeSpeechAsync(request.Text, request.LanguageCode);
        if (file == null)
            return Result.Failure<SynthesizeAudioResult>(Error.TextToSpeechGenerationError);


        Guid fileId = Guid.NewGuid();
        var fileName = $"tts-audio-{fileId}";
        var strategy = FileNamingStrategyFactory.GetStrategy(request.EntityType);
        var filePath = strategy.GetFilePath(userId.ToString(), fileId.ToString(), _textToSpeechService.ContentType);

        await _fileService.UploadAsync(file, fileName, filePath, _textToSpeechService.ContentType, cancellationToken);

        var getUrl = await _fileService.GetPresignedUrlForGetAsync(filePath, cancellationToken);
        return new SynthesizeAudioResult{AudioUrl = getUrl, FileKey = filePath};
    }
}

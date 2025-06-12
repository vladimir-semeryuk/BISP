using FluentValidation;

namespace EchoesOfUzbekistan.Application.TextToSpeech.SynthesizeAudio;
public class SynthesizeAudioCommandValidator : AbstractValidator<SynthesizeAudioCommand>
{
    public SynthesizeAudioCommandValidator()
    {
        RuleFor(x => x.Text)
            .NotEmpty().WithMessage("Text must be provided.")
            .MaximumLength(5000).WithMessage("Text should not exceed 5000 characters.");
    }
}
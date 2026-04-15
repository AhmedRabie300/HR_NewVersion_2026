using System.Text.RegularExpressions;
using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using FluentValidation;

namespace Infrastructure.Services
{
    public class CodeGenerationService : ICodeGenerationService
    {
        private readonly ICompanyRepository _companyRepo;
        private readonly ILocalizationService _localizer;

        public CodeGenerationService(ICompanyRepository companyRepo, ILocalizationService localizer)
        {
            _companyRepo = companyRepo;
            _localizer = localizer;
        }

        public async Task<string> GenerateCodeAsync(
            int companyId,
            string? providedCode,
            Func<int, CancellationToken, Task<string?>> getMaxCodeAsync,
            Func<string, CancellationToken, Task<bool>> codeExistsAsync,
            CancellationToken cancellationToken = default)
        {
            var company = await _companyRepo.GetByIdAsync(companyId);
            if (company == null)
                throw new NotFoundException("Company", companyId);

             if (company.HasSequence == true)
            {
                if (!string.IsNullOrWhiteSpace(providedCode))
                {
                    throw new ValidationException("Cannot provide manual code when auto-generation is enabled");
                }

                var separator = company.Separator ?? "-";
                var sequenceLength = company.SequenceLength ?? 5;
                var prefix = company.Prefix?.ToString() ?? string.Empty;

                var maxCode = await getMaxCodeAsync(companyId, cancellationToken);
                int lastNumber = ExtractNumber(maxCode);

                var newNumber = lastNumber + 1;
                var formattedNumber = newNumber.ToString($"D{sequenceLength}");

                return string.IsNullOrEmpty(prefix)
                    ? formattedNumber
                    : $"{prefix}{separator}{formattedNumber}";
            }
            else
            {
                if (string.IsNullOrWhiteSpace(providedCode))
                    throw new ValidationException(_localizer.GetMessage("CodeRequired", 1));

                if (await codeExistsAsync(providedCode, cancellationToken))
                    throw new ConflictException("Entity", "Code", providedCode);

                return providedCode;
            }
        }

        private int ExtractNumber(string? code)
        {
            if (string.IsNullOrEmpty(code)) return 0;

            var match = Regex.Match(code, @"\d+$");
            if (match.Success && int.TryParse(match.Value, out int number))
                return number;

            if (int.TryParse(code, out int directNumber))
                return directNumber;

            return 0;
        }
    }
}
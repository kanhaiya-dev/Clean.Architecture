using Clean.Architecture.Core.Common.Interfaces.Authentication;
using Clean.Architecture.Core.Common.Mapper;
using Clean.Architecture.Core.Interfaces;
using MediatR;

namespace Clean.Architecture.Core.Accounts.Commands.Create
{
    public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public CreateAccountCommandHandler(IUnitOfWork unitOfWork, IJwtTokenGenerator jwtTokenGenerator)
        {
            _unitOfWork = unitOfWork;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task Handle(CreateAccountCommand command, CancellationToken cancellationToken)
        {
            var account = AccountMapper.MapToAccount(command.AccountRequest);
            var token = _jwtTokenGenerator.GenerateToken(Guid.NewGuid(), "John", "Doe");
            await _unitOfWork.AccountRepository.AddAsync(account);
        }
    }
}

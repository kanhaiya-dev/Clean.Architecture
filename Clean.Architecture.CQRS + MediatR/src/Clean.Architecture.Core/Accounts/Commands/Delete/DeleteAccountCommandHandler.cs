using Clean.Architecture.Core.Accounts.Commands.Create;
using Clean.Architecture.Core.Interfaces;
using MediatR;

namespace Clean.Architecture.Core.Accounts.Commands.Delete
{
    public class DeleteAccountCommandHandler : IRequestHandler<DeleteAccountCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteAccountCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteAccountCommand command, CancellationToken cancellationToken)
        {
            return await _unitOfWork.AccountRepository.DeleteAsync(command.AccountNumber);
        }
    }
}

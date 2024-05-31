using Clean.Architecture.Core.Common.Mapper;
using Clean.Architecture.Core.Common.Response;
using Clean.Architecture.Core.Interfaces;
using MediatR;

namespace Clean.Architecture.Core.Accounts.Queries.Get
{
    public class GetAccountByNumberQueryHandler : IRequestHandler<GetAccountByNumberQuery, AccountResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAccountByNumberQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AccountResponse?> Handle(GetAccountByNumberQuery query, CancellationToken cancellationToken)
        {
            var response = await _unitOfWork.AccountRepository.GetByIdAsync(query.AccountNumber);
            return response != null ? AccountMapper.MapToAccountResponse(response) : null;
        }
    }
}

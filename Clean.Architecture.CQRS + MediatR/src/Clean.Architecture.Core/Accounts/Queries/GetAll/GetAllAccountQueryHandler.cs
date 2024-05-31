using Clean.Architecture.Core.Common.Mapper;
using Clean.Architecture.Core.Common.Response;
using Clean.Architecture.Core.Interfaces;
using MediatR;

namespace Clean.Architecture.Core.Accounts.Queries.GetAll
{
    public class GetAllAccountQueryHandler : IRequestHandler<GetAllAccountQuery, IEnumerable<AccountResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllAccountQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<AccountResponse>> Handle(GetAllAccountQuery query, CancellationToken cancellationToken)
        {
            var accounts = await _unitOfWork.AccountRepository.GetAllAsync();
            return accounts.Select(AccountMapper.MapToAccountResponse).ToList();
        }
    }
}

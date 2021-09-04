using System.Collections.Generic;
using MediatR;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using Application.Common.Queries;
using Application.MailTemplates.Dtos;
using System.Threading.Tasks;
using System.Threading;
using Domain.Interfaces.Read;
using Application.Interfaces;

namespace Application.MailTemplates.Queries
{
    public class GetMailTemplatesListForThisUserQuery : IRequest<IEnumerable<MailTemplateTableDto>>
    {
        public GetMailTemplatesListForThisUserQuery() { }
    }
    public class GetMailTemplatesListForThisUserQueryHandler : IRequestHandler<GetMailTemplatesListForThisUserQuery, IEnumerable<MailTemplateTableDto>>
    {
        protected readonly IMailTemplateReadRepository _mailTemplateReadRepository;
        protected readonly ICurrentUserContext _currentUserContext;
        protected readonly IMapper _mapper;
        public GetMailTemplatesListForThisUserQueryHandler(IMailTemplateReadRepository mailTemplateReadRepository,
             ICurrentUserContext currentUserContext,
             IMapper mapper)
        {
            _mailTemplateReadRepository = mailTemplateReadRepository;
            _currentUserContext = currentUserContext;
            _mapper = mapper;
        }
        public async Task<IEnumerable<MailTemplateTableDto>> Handle(GetMailTemplatesListForThisUserQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<MailTemplateTableDto>>(
                await _mailTemplateReadRepository
                    .GetMailTemplatesForThisUser((await _currentUserContext.GetCurrentUser()).Id));
        }
    }
}

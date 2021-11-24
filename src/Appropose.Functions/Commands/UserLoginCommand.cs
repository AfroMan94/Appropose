using System;
using Appropose.Core.Interfaces;
using AutoMapper;
using DevOne.Security.Cryptography.BCrypt;
using FluentResults;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Appropose.Functions.FluentErrors;
using Microsoft.Extensions.Logging;

namespace Appropose.Functions.Commands
{
   public class UserLoginCommand : IRequest<Result<UserLoginCommandResponse>>
    {
        public UserLoginCommand(string login)
        {
            Login = login;
        }
        
        public string Login { get; set; }
        public string Password { get; set; }
    }

    public class UserLoginCommandResponse {
        public string Id { get; set; }
        public string Login { get; set; }
        public string AzureKey { get; set; }
    }

    public class UserLoginHandler : IRequestHandler<UserLoginCommand, Result<UserLoginCommandResponse>>
    {
        private readonly IUserRepository _user;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public UserLoginHandler(IUserRepository userRepository, IMapper mapper, ILogger logger)
        {
            _user = userRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<Result<UserLoginCommandResponse>> Handle(UserLoginCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Password))
            {
                return Result.Fail(new ValidationError("HashedPassword must be specified"));
            }
            if (string.IsNullOrEmpty(request.Login))
            {
                return Result.Fail(new ValidationError("Login must be specified"));
            }

            try
            {
                var user = await _user.GetUserByLogin(request.Login);
                if (user is null)
                {
                    return Result.Fail(new NotFoundError("Login or password is not correct"));
                }

                var result = BCryptHelper.CheckPassword(request.Password, user?.Password);

                if (!result)
                {
                    return Result.Fail(new NotFoundError("Login or password is not correct"));
                }

                var response = _mapper.Map<UserLoginCommandResponse>(user);

                return Result.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong during adding post");
                return Result.Fail(new RuntimeError("Something went wrong during logging"));
            }
        }
    }
}

using Appropose.Core.Interfaces;
using AutoMapper;
using DevOne.Security.Cryptography.BCrypt;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ToDoList.Functions.FluentErrors;

namespace Appropose.Functions.Commands
{
   public class UserLoginCommand : IRequest<Result<UserLoginCommandResponse>>
    {
        public UserLoginCommand(string login)
        {
            Login = login;
        }
        
        public string Login { get; set; }
        public string Id { get; set; }
        public string Password { get; set; }
        public string[] Posts { get; set; }

    }

    public class UserLoginCommandResponse {
        public string Id { get; set; }
        public string Login { get; set; }
        public string AzureKey { get; set; }
        public string [] Posts { get; set; }
    }

    public class UserLoginHandler : IRequestHandler<UserLoginCommand, Result<UserLoginCommandResponse>>
    {
        private readonly ILogger _logger;
        private readonly IUserRepository _user;
        private readonly IMapper _mapper;
        public UserLoginHandler(ILogger logger, IUserRepository userRepository, IMapper mapper)
        {
            _logger = logger;
            _user = userRepository;
            _mapper = mapper;
        }
        public async Task<Result<UserLoginCommandResponse>> Handle(UserLoginCommand request, CancellationToken cancellationToken)
        {

            //hashowanie 
            if (string.IsNullOrEmpty(request.Password))
            {
                return Result.Fail(new ValidationError("HashedPassword must be specified"));
            }
            //
            if (string.IsNullOrEmpty(request.Login))
            {
                return Result.Fail(new ValidationError("Login must be specified"));
            }

           var user = await _user.GetUserByLogin(request.Login);
            var result = BCryptHelper.CheckPassword(request.Password, user.Password);

            if (!result)
            {
                return Result.Fail(new ValidationError("Incorrect Password"));
            }

            var newResponse = new UserLoginCommandResponse();
            newResponse.Id = user.Id;
            newResponse.Login = user.Login;
            newResponse.Posts = user.Posts;
            newResponse.AzureKey = user.AzureToken;
            

            return Result.Ok(newResponse) ;
        }
    }
}

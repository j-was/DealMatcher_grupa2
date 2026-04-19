using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealMatcher.Backend.UseCases.Features.User.Update;

public sealed record UpdateUserCommand(int UserId, string Name, string Surname) : IRequest<Result<UserDTO>>;

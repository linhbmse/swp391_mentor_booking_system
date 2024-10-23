﻿using Microsoft.AspNetCore.Authorization;
using SwpMentorBooking.Application.Common.Interfaces;
using SwpMentorBooking.Domain.Entities;
using System.Security.Claims;

namespace SwpMentorBooking.Web.Authorization
{ // Custom authorization handler and requirement
    public class GroupLeaderRequirement : IAuthorizationRequirement { }

    public class GroupLeaderHandler : AuthorizationHandler<GroupLeaderRequirement>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GroupLeaderHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, GroupLeaderRequirement requirement)
        {
            var userEmail = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _unitOfWork.User.Get(u => u.Email == userEmail, includeProperties: nameof(StudentDetail));

            if (user != null && user.StudentDetail != null && user.StudentDetail.IsLeader)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

}

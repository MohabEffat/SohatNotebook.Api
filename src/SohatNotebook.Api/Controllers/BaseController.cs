﻿using AutoMapper;
using DataService.IConfiguration;
using Entities.Dtos.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace SohatNotebook.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected IUnitOfWork _unitOfWork;
        protected UserManager<IdentityUser> _userManager;
        protected IMapper _mapper;


        public BaseController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
        }
        internal Error PopulateError(int code, string Message, string Type)
        {
            return new Error { Code = code, Message = Message, Type = Type };
        }
    }
}

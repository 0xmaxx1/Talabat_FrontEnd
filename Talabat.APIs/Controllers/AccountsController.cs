using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.Core.DTOs;
using Talabat.Core.Models.Identity;
using Talabat.Core.Services;

namespace Talabat.APIs.Controllers
{
    public class AccountsController : ApiBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountsController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITokenService tokenService,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }



        #region Login
        [HttpPost("login")] // POST: api/accounts/login
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            AppUser? user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user is null)
                return Unauthorized(new ApiResponse(401));

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded)
                return Unauthorized(new ApiResponse(401));

            return new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            };
        }
        #endregion


        #region Register
        [HttpPost("register")] // POST : api/accounts/register
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            // Check Email Existence 
            if (CheckEmailExists(registerDto.Email).Result.Value)
                return BadRequest(new ApiValidationErrorResponse() { Errors = new string[] { "This Email is already in Use" } });


            AppUser user = new AppUser()
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.Email.Split('@')[0],
                PhoneNumber = registerDto.PhoneNumber
            };

            IdentityResult result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
                return BadRequest(new ApiResponse(400));

            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            });
        }
        #endregion


        #region Get Current User
        [HttpGet] // GET: api/accounts
        [Authorize]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            string? email = User.FindFirstValue(ClaimTypes.Email);

            AppUser? user = await _userManager.FindByEmailAsync(email);

            if (user is null)
                return Unauthorized(new ApiResponse(401));

            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            });
        }
        #endregion


        #region Get User Address
        [HttpGet("address")] // GET : api/accounts/address
        [Authorize]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            AppUser? user = await _userManager.GetUserWithAddressAsync(User);

            if (user is null)
                return Unauthorized(new ApiResponse(401));


            AddressDto address = _mapper.Map<Address, AddressDto>(user.Address);

            return Ok(address);
        }
        #endregion


        #region Update User Address
        [HttpPut("address")] // PUT : api/accounts/address
        [Authorize]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto updatedAddress)
        {
            //var address = _mapper.Map<AddressDto, Address>(updatedAddress);

            AppUser? user = await _userManager.GetUserWithAddressAsync(User);

            //address.Id = user.Address.Id;

            user.Address.FirstName = updatedAddress.FirstName;
            user.Address.LastName = updatedAddress.LastName;
            user.Address.Street = updatedAddress.Street;
            user.Address.City = updatedAddress.City;
            user.Address.Country = updatedAddress.Country;


            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest(new ApiResponse(400));


            return Ok(updatedAddress);
        }
        #endregion


        [HttpGet("emailExists")] // GET : api/accounts/emailexists?email=xxx.yyy@gmail.com
        public async Task<ActionResult<bool>> CheckEmailExists(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
        }
    }
}

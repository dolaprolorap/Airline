﻿using backend.Controllers;
using backend.DataAccess.Repository;
using backend.Models.API;
using backend.Models.DB;
using backend.ServerResponse;
using backend.ServerResponse.AuthService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using CustomUserManager = backend.Custom.UserManager;

namespace backend.Services
{
    public class AuthService : IAuthService
    {
        private PasswordHasher<User> _hasher = new PasswordHasher<User>();
        private IUnitOfWork _unit;
        private ITokenManagerService _tokenManagerService;
        private ITokenCreatorService _tokenCreatorService;
        private ITrackerService _tracker;

        public AuthService(IUnitOfWork unit, ITokenManagerService tokenManagerService, ITokenCreatorService tokenCreatorService, ITrackerService tracker)
        {
            _unit = unit;
            _tokenManagerService = tokenManagerService;
            _tokenCreatorService = tokenCreatorService;
            _tracker = tracker;
        }

        public StatusResponse Login(LoginModel loginData)
        {
            var queryableUsers = _unit.UserRepo.ReadWhere((user) => user.Email == loginData.Email);
            if (!queryableUsers.Any())
            {
                _tracker.UnsuccessTryLogin(loginData.Email, "User not found");
                return new LoginResponse(LoginResponseType.UserWasNotFound, userEmail: loginData.Email);
            }

            var user = queryableUsers.First();
            if (_hasher.VerifyHashedPassword(user, user.Password, loginData.Password) != PasswordVerificationResult.Success)
            {
                _tracker.UnsuccessTryLogin(loginData.Email, "Invalid password");
                return new LoginResponse(LoginResponseType.InvalidPassword, password: loginData.Password);
            }

            Tuple<string, string>? tokens;

            if (!_tokenManagerService.UpdateTokensByEmail(loginData.Email, out tokens, out _))
            {
                _tracker.UnsuccessTryLogin(loginData.Email, "Unsucces update tokens");
                return new LoginResponse(LoginResponseType.TokensNotUpdated);
            }

            _tracker.SuccessLogin(loginData.Email);
            return new LoginResponse(LoginResponseType.Ok, data: new TokenPair()
            {
                accessToken = tokens!.Item1,
                refreshToken = tokens!.Item2,
            }, userEmail: loginData.Email);
        }

        public StatusResponse Refresh(TokenPair tokenPair)
        {
            var claims = _tokenCreatorService.GetPrincipal(tokenPair.accessToken);
            var nameClaim = claims.FindFirst(ClaimTypes.Name);
            if (nameClaim == null)
            {
                return new RefreshResponse(RefreshResponseType.NoNameClaim, oldAccessToken: tokenPair.accessToken);
            }

            var email = nameClaim.Value;
            if (email == null)
            {
                return new RefreshResponse(RefreshResponseType.NameClaimIsNull, oldAccessToken: tokenPair.accessToken);
            }

            if (!_tokenManagerService.IsRefreshTokenExpired(email))
            {
                Tuple<string, string>? newTokens;

                if (_tokenManagerService.UpdateTokensByRefreshToken(email, tokenPair.refreshToken,
                    out newTokens, out _))
                {
                    return new RefreshResponse(RefreshResponseType.Ok, new TokenPair()
                    {
                        accessToken = newTokens!.Item1,
                        refreshToken = newTokens!.Item2,
                    });
                }
                else
                {
                    return new RefreshResponse(RefreshResponseType.FailUpdateToken);
                }
            }
            else
            {
                return new RefreshResponse(RefreshResponseType.RefreshTokenExpired);
            }
        }

        public StatusResponse Register(RegisterUser registerUser)
        {
            var queryableUser = _unit.UserRepo.ReadWhere(user => user.Email == registerUser.Email);
            if (queryableUser.Any())
            {
                return new RegisterResponse(RegisterResponseType.UserAlreadyExists, userEmail: registerUser.Email);
            }

            var queryableRole = _unit.RoleRepo.ReadWhere(role => role.Id == registerUser.RoleID);
            if (!queryableRole.Any())
            {
                return new RegisterResponse(RegisterResponseType.RoleIdWasNotFound, roleId: registerUser.RoleID);
            }

            var queryableOffice = _unit.OfficeRepo.ReadWhere(office => office.Id == registerUser.OfficeID);
            if (!queryableOffice.Any())
            {
                return new RegisterResponse(RegisterResponseType.OfficeIdWasNotFound, officeId: registerUser.OfficeID);
            }

            DateOnly birthdate;

            try
            {
                birthdate = DateOnly.ParseExact(registerUser.Birthdate, "yyyy-MM-dd",
                    System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                return new RegisterResponse(RegisterResponseType.InvalidDateTimeFormat, dateTimeStr: registerUser.Birthdate);
            }

            //BIG KOSTIL
            int lastMaxId = _unit.UserRepo.Max(user => user.Id);

            User newUser = new User()
            {
                Id = lastMaxId + 1,
                RoleId = registerUser.RoleID,
                Email = registerUser.Email,
                Password = _hasher.HashPassword(new User(), registerUser.Password),
                FirstName = registerUser.Firstname,
                LastName = registerUser.Lastname,
                OfficeId = registerUser.OfficeID,
                Birthdate = birthdate,
                Active = true
            };

            _unit.UserRepo.Add(newUser);
            _unit.Save();

            Token newToken = new Token()
            {
                UserId = lastMaxId + 1,
            };

            _unit.TokenRepo.Add(newToken);
            _unit.Save();

            return Login(new LoginModel()
            {
                Email = registerUser.Email,
                Password = registerUser.Password
            });
        }

        public GetMyselfResponse GetMyself(string email)
        {
            var user = _unit.UserRepo.ReadFirst(user => user.Email == email);
            if (user == null)
            {
                return new GetMyselfResponse(
                    GetMyselfResponseType.UserNotFound,
                    email: email);
            }

            var userManager = new CustomUserManager(_unit);
            var status = userManager.ConvertUser(user);

            if (status.ResponseType != ServerResponse.Custom.UserManager.ConvertUserResponseType.Ok)
            {
                return new GetMyselfResponse(GetMyselfResponseType.ConvertError);
            }

            return new GetMyselfResponse(
                GetMyselfResponseType.Ok,
                email: email,
                userData: status.User);
        }
    }
}

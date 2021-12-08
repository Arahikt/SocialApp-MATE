using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class LikesController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly ILikesRepository _likesRepostory;
        public LikesController(IUserRepository userRepository, ILikesRepository likesRepostory)
        {
            _likesRepostory = likesRepostory;
            _userRepository = userRepository;

        }
        [HttpPost("{username}")]
        public async Task<ActionResult> AddLike(string username)
        {
            var sourceUserId = User.GetUserId();
            var likedUser = await _userRepository.GetUserByUsernameAsync(username);
            var sourceUser = await _likesRepostory.GetUserWithLikes(sourceUserId);

            if (likedUser == null) return NotFound();
            if (sourceUser.UserName == username) return BadRequest("Invalid");

            var userLike = await _likesRepostory.GetUserLike(sourceUserId, likedUser.Id);


            if (userLike != null)
            {
                sourceUser.LikedOthers.Remove(userLike);
            }
            else
            {
                userLike = new AppUserLike
                {
                    SourceUserId = sourceUserId,
                    LikedUserId = likedUser.Id
                };
                sourceUser.LikedOthers.Add(userLike);
            }

            if (await _userRepository.SaveAllAsync()) return Ok();
            return BadRequest("Failed");
        }



        [HttpGet]
        public async Task<ActionResult<IEnumerable<LikeDto>>> GetUserLikes([FromQuery]LikesParams likesParams)
        {
            likesParams.UserId = User.GetUserId();
            var users = await _likesRepostory.GetUserLikes(likesParams);
            Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);
            return Ok(users);
        }
    }
}
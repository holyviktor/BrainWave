using AutoMapper;
using BrainWave.Core.Entities;
using BrainWave.Infrastructure.Data;
using BrainWave.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BrainWave.WebUI.Controllers;

public class UsersController : Controller
{
    private readonly BrainWaveDbContext _dbContext;
    private readonly IMapper _mapper;

    public UsersController(BrainWaveDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    [Route("users/{tag}")]
    public IActionResult Index(string tag)
    {
        var userTag = (HttpContext.User.Identity as ClaimsIdentity)?.FindFirst("Name")?.Value;
        if (userTag == null)
        {
            throw new ArgumentException();
        }
        var authorisedUser = _dbContext.Users.Include(x=>x.Followings)
            
            .FirstOrDefault(x => x.Tag == userTag);
        var user = _dbContext.Users.Include(x=>x.Articles)
            .Include(x=>x.Followings)
            .FirstOrDefault(x => x.Tag == tag);
        
        if (user is null || authorisedUser is null)
        {
            return NotFound();
        }
        if (authorisedUser == user)
        {
            return RedirectToAction("Index", "Profile");
        }
        ViewBag.isFollowed = authorisedUser.Followings?.Any(x => x.FollowingUser == user);
        var articlesViewModel = _mapper.Map<List<ArticleViewModel>>(user.Articles);
        var followers = _dbContext.Followings.Count(x => x.FollowingUserId == user.Id);
        var followings = _dbContext.Followings.Count(x => x.UserId == user.Id);
        var profileViewModel = new ProfileViewModel
        {
            User = user,
            Articles = articlesViewModel,
            Followers = followers,
            Followings = followings
        };
        return View(profileViewModel);
    }
    [Route("users/{tag}")]
    [HttpPost]
    public IActionResult Follow(string tag)
    {
        var userTag = (HttpContext.User.Identity as ClaimsIdentity)?.FindFirst("Name")?.Value;
        if (userTag == null)
        {
            throw new ArgumentException();
        }
        var authorisedUser = _dbContext.Users.FirstOrDefault(x => x.Tag == userTag);
        var user = _dbContext.Users.Include(x=>x.Articles)
            .FirstOrDefault(x => x.Tag == tag);
        
        if (user is null || authorisedUser is null)
        {
            return NotFound();
        }
        if (authorisedUser == user)
        {
            return RedirectToAction("Index", "Profile");
        }
        var following = _dbContext.Followings
            .FirstOrDefault(x => x.User == authorisedUser && x.FollowingUser == user);
        if (following is null)
        {
            _dbContext.Followings.Add(new Following
            {
                FollowingUser = user,
                User = authorisedUser
            });
        }
        else
        {
            _dbContext.Remove(following);
        }
        _dbContext.SaveChanges();
        return RedirectToAction("Index", new { tag });
    }
}
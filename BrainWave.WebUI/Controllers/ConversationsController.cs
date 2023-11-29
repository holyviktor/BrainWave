using AutoMapper;
using BrainWave.Core.Entities;
using BrainWave.Infrastructure.Data;
using BrainWave.WebUI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
namespace BrainWave.WebUI.Controllers
{
    [Authorize]
    public class ConversationsController : Controller
    {
        private readonly BrainWaveDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly string FolderPath = "/media/conversations/";
        public ConversationsController(BrainWaveDbContext dbContext, IMapper mapper, IWebHostEnvironment appEnvironment)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _appEnvironment = appEnvironment;
        }
        public ActionResult Index()
        {
            var userTag = (HttpContext.User.Identity as ClaimsIdentity)?.FindFirst("Name")?.Value;
            if (userTag == null)
            {
                throw new ArgumentException();
            }
            var userAuthorised = _dbContext.Users.FirstOrDefault(x => x.Tag == userTag);
            if (userAuthorised == null)
            {
                throw new ArgumentException();
            }
            var userConversations = _dbContext.Conversations
                .Join(_dbContext.Participants,
                    c => c.Id,
                    p => p.ConversationId,
                    (c, p) => new { Conversation = c, Participant = p })
                .Where(cp => cp.Participant.UserId == userAuthorised.Id)
                .Select(cp => cp.Conversation)
                .ToList();
            var conversationsViewModel = new List<ConversationsViewModel>();
            foreach (var conversation in userConversations)
            {
                var message = _dbContext.Messages
                    .Where(x => x.ConversationId == conversation.Id)
                    .OrderByDescending(x => x.DateTimeCreated)
                    .Include(x => x.User)
                    .FirstOrDefault();
                MessageViewModel? messageViewModel = null;
                if (message != null)
                {
                    var user = _mapper.Map<UserViewModel>(message.User);
                    messageViewModel = new MessageViewModel
                    {
                        Text = message.Text,
                        DateTimeCreated = message.DateTimeCreated,
                        User = user,
                    };
                }
                conversationsViewModel.Add(new()
                {
                    Id = conversation.Id,
                    Name = conversation.Name,
                    Photo = conversation.Photo,
                    Message = messageViewModel
                });
            }

            return View(conversationsViewModel);
        }

        public ActionResult Create() {
            var userTag = (HttpContext.User.Identity as ClaimsIdentity)?.FindFirst("Name")?.Value;
            if (userTag == null)
            {
                throw new ArgumentException();
            }
            var user = _dbContext.Users.Include(x => x.Participants).FirstOrDefault(x => x.Tag == userTag);
            if (user == null)
            {
                throw new ArgumentException();
            }
            var followings = _dbContext.Followings.Include(x=>x.FollowingUser).Where(x => x.UserId == user.Id).ToList();
            ViewBag.Users = followings;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ConversationInputViewModel conversationInputViewModel, IFormFile photoFile)
        {
            var userTag = (HttpContext.User.Identity as ClaimsIdentity)?.FindFirst("Name")?.Value;
            if (userTag == null)
            {
                throw new ArgumentException();
            }
            var userAuthorised = _dbContext.Users.FirstOrDefault(x => x.Tag == userTag);
            if (userAuthorised == null)
            {
                throw new ArgumentException();
            }

            if (!ModelState.IsValid || photoFile == null)
            {
                var followings = _dbContext.Followings.Include(x => x.FollowingUser).Where(x => x.UserId == userAuthorised.Id).ToList();
                ViewBag.Users = followings;
                return View("Create");
            }
            var conversation = new Conversation
            {
                Name = conversationInputViewModel.Name,
                Photo = ""
            };
            _dbContext.Conversations.Add(conversation);
            _dbContext.SaveChanges();

            var type = photoFile.FileName.Split('.').Last();
            var filename = conversation.Id.ToString() + '.' + type;
            var path = FolderPath + filename;
            using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
            {
                await photoFile.CopyToAsync(fileStream);
            }
            conversation.Photo = filename;


            _dbContext.Participants.Add(new Participant
            {
                User = userAuthorised,
                Conversation = conversation
            });

            foreach (var participantId in conversationInputViewModel.Participants)
            {
                var user = _dbContext.Users.Find(participantId);
                if (user == null)
                {
                    throw new InvalidOperationException();
                }
                var participant = new Participant
                {
                    User = user,
                    Conversation = conversation,
                };
                _dbContext.Participants.Add(participant);

            }

            
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }
        [Route("conversations/{conversationId:int}")]
        public ActionResult Conversation(int conversationId)
        {
            
            var userTag = (HttpContext.User.Identity as ClaimsIdentity)?.FindFirst("Name")?.Value;
            if (userTag == null)
            {
                throw new ArgumentException();
            }
            var user = _dbContext.Users.Include(x => x.Participants).FirstOrDefault(x => x.Tag == userTag);
            if (user == null)
            {
                throw new ArgumentException();
            }
            var userConversation = _dbContext.Conversations
                .Include(x => x.Messages)
                .Include(x => x.Participants)
                .ThenInclude(x => x.User)
                .FirstOrDefault(x => x.Id == conversationId);
            if (userConversation == null)
            {
                return NotFound();
            }
            var conversationViewModel = new ConversationViewModel
            {
                Id = userConversation.Id,
                Name = userConversation.Name,
                Photo = userConversation.Photo,
                ParticipantsCount = userConversation.Participants.Select(p => p.User).Count(),
                Messages = _mapper.Map<List<MessageViewModel>>(userConversation.Messages.OrderBy(x => x.DateTimeCreated).ToList()),
                User = _mapper.Map<UserViewModel>(user)
            };
            return View(conversationViewModel);
        }
    }
}

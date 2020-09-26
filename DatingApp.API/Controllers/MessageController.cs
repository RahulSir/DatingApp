using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.DTOs;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/users/{userId}/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IDatingRepository _repo;

        public MessageController(IMapper mapper, IDatingRepository repo)
        {
            _mapper = mapper;
            _repo = repo;
        }
        [HttpGet("{id}", Name = "GetMessage")]
        public async Task<IActionResult> GetMessage(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)){
                return Unauthorized();
            }
            var msgfomrepo = await _repo.GetMessage(id);
            if(msgfomrepo == null){
                return NotFound();
                
            }
            return Ok(msgfomrepo);
        }

        [HttpGet]
        public async Task<IActionResult> GetMessageForUser(int userId ,[FromQuery]MessageParams messageParams){
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)){
                return Unauthorized();
            }
            messageParams.UserId = userId;
            var messagesfromrepo = await _repo.GetMessagesForUser(messageParams);
            var messagetoreturn =  _mapper.Map<IEnumerable<MessageToReturnDTO>>(messagesfromrepo);
            Response.AddPagination(messagesfromrepo.CurrentPage,
                        messagesfromrepo.PageSize, messagesfromrepo.TotalCount, messagesfromrepo.TotalPages);
            return Ok(messagetoreturn);
        }

        [HttpGet("thread/{recepientid}")]
        public async Task<IActionResult> GetMessageThread(int userId , int recepientid){
           if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)){
                return Unauthorized();
            } 
            var messagesfromrepo = await _repo.GetMessageThread(userId , recepientid);
            var messagethread = _mapper.Map<IEnumerable<MessageToReturnDTO>>(messagesfromrepo);
            return Ok(messagethread);
        }
        [HttpPost("{id}")]
        public async Task<IActionResult> DeleteMessage(int id , int userId){
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)){
                return Unauthorized();
            } 
            var msgfomrepo = await _repo.GetMessage(id);
            if(msgfomrepo == null){
                return NotFound();                
            }
            if(msgfomrepo.SenderId == userId){
                msgfomrepo.SenderDeleted = true;
            }
             if(msgfomrepo.RecepientId == userId){
                msgfomrepo.RecepientDeleted = true;
            }
            if( msgfomrepo.SenderDeleted ==true && msgfomrepo.RecepientDeleted ==true){
                 _repo.Delete(msgfomrepo);
            }
            if(await _repo.SaveAll()){
                return NoContent();
            }
            throw new Exception("Problem in deleting message");

        } 

        [HttpPost("{id}/read")]
        public async Task<IActionResult> markMessageAsRead(int id , int userId){
        if (userId!= int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)){
                return Unauthorized();
            }
            var message= await _repo.GetMessage(id);
            if(message.RecepientId!=userId){
                return Unauthorized();
            }
            message.isRead = true;
            message.DateRead = DateTime.Now;
            await _repo.SaveAll();
                return NoContent();
            
            

        }
        [HttpPost]
        public async Task<IActionResult> CreateMessage(int userid , MessageForCreationDTO messageForCreationDTO){
            var sender = await _repo.GetUser(userid);
            if (sender.id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)){
                return Unauthorized();
            }
            //  I think this is less to do with automapper magic, and more to do with the fact
            //  that both Recipient and Sender entities are also in the DbContext from the 
            // previous _repo.GetUser() calls.
            // Now, when the "message" entity is added to the DbContext
            // _repo.Add(message);
            // the Recipient/Sender navigation properties in the message entity 
            // are somehow linked to the Recipient/Sender entities in DbContext, and as if by magic, 
            // in the debug window we see the data from these entities added to in the message entity.
            // Update:
            // Ah, I see from a more recent question that you plan to get this updated anyway! Thanks
            messageForCreationDTO.SenderId = userid;
            var recepient = await _repo.GetUser(messageForCreationDTO.RecepientId);
            if(recepient == null){
                return BadRequest("Recpient Not found");
            }
            var message = _mapper.Map<Message>(messageForCreationDTO);
            // message.Sender = sender ; 
            _repo.Add(message);
    
            if(await _repo.SaveAll()){
                // if we wright below line above upper line then it will not save 
                // and therefore we will get a random / large negative id of 
                // messagetoreturn because message is not saved in sql yet
                var messagetoReturn = _mapper.Map<MessageToReturnDTO>(message);
                // IN .net core 3.0 we need to pass all route paramaters in CreatedAtRoute
                // in this case like
                //return CreatedAtRoute("GetMessage" , new {userId = userid , id =message.id },messagetoreturn)
                return CreatedAtRoute("GetMessage" , new{id = message.Id} , messagetoReturn);
            }
            throw new Exception("Creating the message failed on Save");
        }
    }
}
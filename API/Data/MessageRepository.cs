using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public MessageRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public void AddMessage(Message message)
        {
            _context.Messages!.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            _context.Messages!.Remove(message);
        }

        public async Task<Message?> GetMessage(int id)
        {
            return await _context.Messages!.FindAsync(id);
        }

        public async Task<PagedList<MessageDto?>?> GetMessagesForUser(MessageParams messageParams)
        {
            var query = _context.Messages?.OrderByDescending(m => m.MessageSent).AsQueryable();

            // Вот такую короткую запись switch надо запомнить
            query = messageParams.Container switch
            {
                "Inbox" => query?.Where(u => u.Recipient.Username == messageParams.Username),
                "Outbox" => query?.Where(u => u.Sender.Username == messageParams.Username),
                _ => query?.Where(u => u.Recipient.Username == messageParams.Username && u.DateRead == null)
            };

            #region эта короткая запись switch заменяет то что в комменте ниже:
            // switch (messageParams.Container)
            // {
            //     case "Inbox": 
            //         query = query?.Where(u => u.Recipient.Username == messageParams.Username);
            //         break;
            //     case "Outbox": 
            //         query = query?.Where(u => u.Sender.Username == messageParams.Username);
            //         break;
            //     default:
            //         query = query?.Where(u => u.Recipient.Username == messageParams.Username && u.DateRead == null);
            //         break;
            // }            
            #endregion

            var messages = query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider); // Маппинг IQueryable<Message> => IQueryable<MessageDto> делаем через ProjectTo.

            return await PagedList<MessageDto?>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentUsername, string recipientUsername)
        {
            var messages = await _context.Messages!
                                    .Include(u => u.Sender).ThenInclude(p => p.Photos) // Мы хотим показывать фотки пользователей в списке сообщений
                                    .Include(u => u.Recipient).ThenInclude(p => p.Photos)
                                    .Where(m => m.Recipient.Username == currentUsername && m.Sender.Username == recipientUsername
                                                || // Здесь скобки не нужны, в шарпе && стоит выше по порядку выполнения, чем ||
                                                m.Recipient.Username == recipientUsername && m.Sender.Username == currentUsername)
                                    .OrderBy(m => m.MessageSent)
                                    .ToListAsync();

            var unreadMessages = messages.Where(m => m.DateRead == null && m.RecipientUsername == currentUsername);

            if (unreadMessages.Any())
            {
                foreach (var message in unreadMessages)
                    message.DateRead = DateTime.Now;

                await _context.SaveChangesAsync();
            }
            
            return _mapper.Map<IEnumerable<MessageDto>>(messages);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
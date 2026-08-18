// DashboardController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupportTicketSystem.Data;
using SupportTicketSystem.Models;

namespace SupportTicketSystem.Api
{
    [Route("api/dashboard")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly INotificationService _notificationService;

        public DashboardController(AppDbContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllTickets()
        {
            var itUsers = await _context.Users
                .Where(u => u.Role == "IT")
                .Select(u => new { u.Id, u.FullName })
                .ToListAsync();

            var tickets = await _context.Tickets
                .Include(t => t.CreatedByUser)
                .Include(t => t.AssignedToUser)
                .OrderByDescending(t=>t.CreatedAt)
                .ToListAsync();

            

            var response = new
            {

                tickets = tickets.Select(t => new
                {
                    
                    t.Id,
                    t.Title,
                    UserName = t.CreatedByUser!.FullName,
                    Status = t.Status ?? "در انتظار بررسی",
                    AssignedTo = t.AssignedToUser != null ? t.AssignedToUser.FullName : "هنوز ارجاع نشده",
                    AssignedToId = t.AssignedToUserId,
                    EV = t.EmployeeVerif ?? false,
                    

                })
                ,
                itUsers,
                summary = new
                {
                    pending = tickets.Count(t => t.Status == null || t.Status == "در انتظار بررسی"),
                    inprogress = tickets.Count(t => t.Status == "در حال انجام"),
                    done = tickets.Count(t => t.Status == "انجام شده"),
                    canceled = tickets.Count(t => t.Status == "باطل شده")
                }
            };

            return Ok(response);
        }

        [HttpPost("assign/{ticketId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignTicket(int ticketId, [FromBody] AssignDto dto)
        {
            var ticket = await _context.Tickets
                .Include(t => t.CreatedByUser)
                .Include(t => t.AssignedToUser)
                .FirstOrDefaultAsync(t => t.Id == ticketId);

            if (ticket == null)
                return NotFound("تیکت پیدا نشد");

            User _user = _context.Users.FirstOrDefault(u => u.Id == dto.UserId);
            ticket.AssignedToUserId = dto.UserId;
            ticket.AssignedAt = DateTime.Now;
            ticket.Status = "در حال انجام";
            ticket.StatusAt = DateTime.Now;

            AssignHistory assignHistory = new AssignHistory()
            {
                TicketId= ticketId,
                AssignedAt= ticket.AssignedAt,
                AssignedToUserId= ticket.AssignedToUserId
            };
            _context.AssignHistories.Add(assignHistory);

            

            await _notificationService.NotifyAsync(
            _user!.Username,
            "تیکت جدید",
            $"تیکت {ticket.Id} به شما اختصاص داده شد.",
            ticket.Id);

            await _context.SaveChangesAsync();

            var updated = new
            {
                ticket.Id,
                ticket.Title,
                UserName = ticket.CreatedByUser.FullName,
                Status = ticket.Status,
                AssignedToId = ticket.AssignedToUserId,
                AssignedTo = _context.Users.First(u => u.Id == ticket.AssignedToUserId).FullName
            };

            return Ok(updated);
        }

        [HttpGet("it")]
        [Authorize(Roles = "IT")]
        public async Task<IActionResult> GetITTickets()
        {
            var userId = int.Parse(User.FindFirst("UserId")!.Value);

            var tickets = await _context.Tickets
                .Include(t => t.CreatedByUser)
                .Where(t => t.AssignedToUserId == userId)
                .OrderByDescending(t=>t.AssignedAt)
                .ToListAsync();

            var response = tickets.Select(t => new
            {
                t.Id,
                t.Title,
                t.Description,
                t.Priority,
                t.Status,
                UserName = t.CreatedByUser.FullName,
                FileUrl = !string.IsNullOrEmpty(t.AttachmentPath) ? $"{t.AttachmentPath}"/*$"/uploads/{t.AttachmentPath}"*/ : null
            });

            return Ok(response);
        }

        [HttpPut("/api/tickets/{id}/status")]
        [Authorize(Roles = "IT")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] StatusDto dto)
        {
            var userId = int.Parse(User.FindFirst("UserId")!.Value);

            var ticket = await _context.Tickets
                .Include(t => t.CreatedByUser)
                .Include(t => t.AssignedToUser)
                .FirstOrDefaultAsync(t => t.AssignedToUserId == userId && t.Id == id);

            User _user = _context.Users.FirstOrDefault(u => u.Id == ticket.CreatedByUserId);
            await _notificationService.NotifyAsync(
            _user!.Username,
            ticket.Status,
             $"تیکت شما با عنوان «{ticket.Title}» توسط کارشناس «{ticket.AssignedToUser?.FullName}» انجام شد." + " لطفا دکمه تایید را در کنار ردیف مورد نظر کلیک کنید. ",
            ticket.Id);


            if (ticket == null)
                return NotFound("تیکت یافت نشد یا اجازه دسترسی ندارید.");

            bool wasDoneBefore = ticket.Status == "انجام شده";

            ticket.Status = dto.Status;
            ticket.StatusAt = DateTime.Now;

            StatusHistory statusHistory = new StatusHistory()
            {
                StatusAt=ticket.StatusAt,
                Status=ticket.Status,
                StatusUserId=userId,
                TicketId=ticket.Id
            };
            _context.StatusHistories.Add(statusHistory);


            if (dto.Status == "انجام شده" && !wasDoneBefore)
            {
                var notif = new Notification
                {
                    TicketId = ticket.Id,
                    UserId = ticket.CreatedByUserId,
                    Message = $"تیکت شما با عنوان «{ticket.Title}» توسط کارشناس «{ticket.AssignedToUser?.FullName}» انجام شد."+" لطفا دکمه تایید را در کنار ردیف مورد نظر کلیک کنید. ",
                    SenderName = ticket.AssignedToUser?.FullName ?? "کارشناس",
                    CreatedAt = DateTime.Now,
                    IsRead = false
                };

                _context.Notifications.Add(notif);
            }

            await _context.SaveChangesAsync();

            return Ok(new { message = "وضعیت با موفقیت به‌روزرسانی شد." });
        }

        [HttpGet("admin/charts")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetChartData()
        {
            var itUsers = await _context.Users.Where(u => u.Role == "IT").ToListAsync();

            var tickets = await _context.Tickets
                .Include(t => t.AssignedToUser)
                .Where(t => t.AssignedToUserId != null)
                .ToListAsync();

            var donutData = tickets
                .GroupBy(t => t.AssignedToUser!.FullName)
                .Select(g => new { user = g.Key, count = g.Count() })
                .ToList();

            var barData = itUsers.Select(user =>
            {
                var userTickets = tickets.Where(t => t.AssignedToUserId == user.Id);
                return new
                {
                    user = user.FullName,
                    pending = userTickets.Count(t => t.Status == null || t.Status == "در انتظار بررسی"),
                    inprogress = userTickets.Count(t => t.Status == "در حال انجام"),
                    done = userTickets.Count(t => t.Status == "انجام شده"),
                    canceled = userTickets.Count(t => t.Status == "باطل شده")
                };
            }).ToList();

            return Ok(new { donut = donutData, bar = barData });
        }

        [HttpGet("/api/notifications")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> GetMyNotifications()
        {
            var userId = int.Parse(User.FindFirst("UserId")!.Value);
            User _user = _context.Users.FirstOrDefault(u => u.Id == userId);
            var notifications = await _context.PushNotifications
                .Where(n => n.UserId == _user.Username && !n.IsRead)
                .OrderByDescending(n => n.CreatedAt)
                .Take(10)
                .ToListAsync();

            notifications.ForEach(n => n.IsRead = true);
            await _context.SaveChangesAsync();

            return Ok(notifications);
        }

        public class AssignDto
        {
            public int UserId { get; set; }
        }

        public class StatusDto
        {
            public string Status { get; set; } = string.Empty;
        }
    }
}

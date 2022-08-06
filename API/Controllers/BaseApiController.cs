using API.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))] // Включаем ActionFilter для сохранения времени LastActive нашего пользователя для всех контролеров
    [ApiController]
    [Route("api/[controller]")] 
    public class BaseApiController : ControllerBase
    {
        
    }
}
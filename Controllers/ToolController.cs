using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppTool.Dtos;
using AppTool.Model;
using AppTool.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppTool.Controllers
{
    [Authorize]
    public class ToolController : Controller
    {
        private readonly ToolService _toolService;
        public ToolController(ToolService toolService)
        {
            _toolService = toolService;
        }
        [HttpGet]
        [Route("api/tool/get")]
        public async Task<List<Tool>> GetAll()
        {
            return await _toolService.GetAll();
        }

        [HttpGet]
        [Route("api/tool/search")]
        public async Task<List<Tool>> Search(FilterDto dto)
        {
            var result = await _toolService.Search(dto);
            return result;
        }
    }
}
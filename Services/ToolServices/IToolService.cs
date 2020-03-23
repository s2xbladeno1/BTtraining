using AppTool.Dtos;
using AppTool.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppTool.Services.ToolServices
{
    public interface IToolService
    {
        Task<List<Tool>> GetAll();
        Task<List<Tool>> Search(FilterDto dto);
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using api_pim.Entities;
using api_pim.Models;
using api_pim.Exceptions;

using System.Net;

using AutoMapper;

namespace api_pim.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PagamentoController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    private readonly IMapper _mapper;

    private readonly ILogger<PagamentoController> _logger;

    public PagamentoController(ApplicationDbContext context, IMapper mapper, ILogger<PagamentoController> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }
}
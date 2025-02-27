﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebStore9.Interfaces.Services;
using WebStore9Domain.Entities.Orders;

namespace WebStore9.WebAPI.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersApiController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersApiController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("user/{userName}")]
        public async Task<IActionResult> GetUserOrders(string userName)
        {
            var orders = await _orderService.GetUserOrders(userName);
            return Ok(orders); 
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderById(id);
            if (order is null)
                return NotFound();

            return Ok(order);
        }

        [HttpPost("{userName}")]
        public async Task<IActionResult> CreateOrder(string userName)
        {
            var order = await _orderService.CreateOrder(userName,);
            return Ok(order);
        }

    }
}

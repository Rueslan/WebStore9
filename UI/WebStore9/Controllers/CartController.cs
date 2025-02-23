﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore9.Interfaces.Services;
using WebStore9Domain.ViewModels;

namespace WebStore9.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _сartService;

        public CartController(ICartService сartService) => _сartService = сartService;

        public async Task<IActionResult> Index() => View(new CartOrderViewModel{Cart = await _сartService.GetViewModelAsync()});

        public IActionResult Add(int Id)
        {
            _сartService.Add(Id);
            return RedirectToAction("Index", "Cart");
        }

        public IActionResult Decrement(int Id)
        {
            _сartService.Decrement(Id);
            return RedirectToAction("Index", "Cart");
        }

        public IActionResult Remove(int Id)
        {
            _сartService.Remove(Id);
            return RedirectToAction("Index", "Cart");
        }

        [Authorize]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckOut(OrderViewModel orderViewModel, [FromServices] IOrderService orderService)
        {
            if (!ModelState.IsValid)
                return View(nameof(Index), new CartOrderViewModel
                {
                    Cart = await _сartService.GetViewModelAsync(),
                    Order = orderViewModel,
                });

            var order = await orderService.CreateOrder(
                User.Identity!.Name,
                await _сartService.GetViewModelAsync(),
                orderViewModel);

            _сartService.Clear();

            return RedirectToAction(nameof(OrderConfirmed), new { order.Id });
        }

        public IActionResult OrderConfirmed(int id)
        {
            ViewBag.OrderId = id;
            return View();
        }
    }
}

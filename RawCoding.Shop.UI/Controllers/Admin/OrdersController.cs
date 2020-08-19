﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RawCoding.Shop.Application.Admin.Orders;

namespace RawCoding.Shop.UI.Controllers.Admin
{
    public class OrdersController : AdminBaseController
    {
        [HttpGet]
        public IActionResult GetOrders(int status, [FromServices] GetOrders getOrders) =>
            Ok(getOrders.Do(status));

        [HttpGet("{id}")]
        public IActionResult GetOrder(int id, [FromServices] GetOrder getOrder) =>
            Ok(getOrder.Do(id));

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromServices] UpdateOrder updateOrder)
        {
            var success = await updateOrder.DoAsync(id) > 0;
            if (success)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
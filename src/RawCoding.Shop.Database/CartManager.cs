﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RawCoding.Shop.Domain.Interfaces;
using RawCoding.Shop.Domain.Models;

namespace RawCoding.Shop.Database
{
    public class CartManager : ICartManager
    {
        private readonly ApplicationDbContext _ctx;

        public CartManager(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<Cart> CreateCart(string cartId)
        {
            var cart = new Cart
            {
                UserId = cartId,
            };
            _ctx.Add(cart);
            await _ctx.SaveChangesAsync();
            return cart;
        }

        public Task<int> UpdateCart(Cart cart)
        {
            _ctx.Carts.Update(cart);
            return _ctx.SaveChangesAsync();
        }

        public async Task<int> RemoveStock(int stockId, string userId)
        {
            var cart = _ctx.Carts.AsNoTracking().FirstOrDefault(x => x.UserId == userId);

            if (cart == null)
            {
                return -1;
            }

            var stock = _ctx.CartProducts
                .FirstOrDefault(x => x.StockId == stockId && x.CartId == cart.Id);

            if (stock == null)
            {
                return -1;
            }

            _ctx.CartProducts.Remove(stock);
            await _ctx.SaveChangesAsync();

            return stock.Qty;
        }

        public async Task<int> GetCartId(string userId)
        {
            var cart = _ctx.Carts?.AsNoTracking()
                           .FirstOrDefault(x => x.UserId == userId && !x.Closed)
                       ?? await CreateCart(userId);

            return cart.Id;
        }

        public Task<Cart> GetCart(string userId)
        {
            var cart = _ctx.Carts
                .Include(x => x.Products)
                .FirstOrDefault(x => x.UserId == userId && !x.Closed);

            return cart == null ? CreateCart(userId) : Task.FromResult(cart);
        }

        public Cart GetCartFull(string userId)
        {
            return _ctx.Carts
                .Include(x => x.Products)
                .ThenInclude(x => x.Stock)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.Images)
                .AsNoTracking()
                .FirstOrDefault(x => x.UserId == userId && !x.Closed);
        }

        public IList<CartProduct> GetCartProducts(int cartId)
        {
            return _ctx.CartProducts
                .Where(x => x.CartId == cartId)
                .Include(x => x.Stock)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.Images)
                .AsNoTracking()
                .ToList();
        }


        public void ClearCart()
        {
            throw new NotImplementedException();
        }

        public void AddCustomerInformation(CustomerInformation customer)
        {
            throw new NotImplementedException();
        }

        public CustomerInformation GetCustomerInformation()
        {
            throw new NotImplementedException();
        }
    }
}
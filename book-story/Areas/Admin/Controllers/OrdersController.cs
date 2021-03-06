using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookOnlineShop.Data;
using BookOnlineShop.Models;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;

namespace BookOnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "ADMIN")]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Orders
        public async Task<IActionResult> Index(string telephone,int status,int paymenttype,string paydate)
        {
               
            var orders = await _context.Orders.OrderByDescending(b => b.Status == 1).OrderByDescending(b => b.CreateAt).ToListAsync();
        /*    if (!string.IsNullOrEmpty(telephone) || status != 0 || paymenttype != 0 || !string.IsNullOrEmpty(paydate))
            {
                var date1 = paydate.Substring(0, 10);
                var date2 = paydate.Substring(13);
                orders = await _context.Orders.Where(b => b.Telephone.Contains(telephone) && b.Status == status && b.paymenttype == paymenttype && b.CreateAt > DateTime.ParseExact(date1, "dd/MM/yyyy", null) && b.CreateAt < DateTime.ParseExact(date2, "dd/MM/yyyy", null)).ToListAsync();
            }*/
            if (!string.IsNullOrEmpty(telephone))
            {
                orders = await _context.Orders.Where(b => b.Telephone.Contains(telephone)).ToListAsync();
            }
            if (status != 0)
            {
                orders = await _context.Orders.Where(b => b.Status == status).ToListAsync();
            }
              if (paymenttype != 0)
            {
                orders = await _context.Orders.Where(b => b.paymenttype == paymenttype).ToListAsync();
            }
          /*  if (!string.IsNullOrEmpty(paydate))
            {

                var date1 = paydate.Substring(0, 10);
                var date2 = paydate.Substring(13);
               
                  orders = await _context.Orders.Where(b => b.CreateAt > DateTime.ParseExact(date1, "dd/MM/yyyy", null) && b.CreateAt < DateTime.ParseExact(date2, "dd/MM/yyyy", null)).ToListAsync();
     
            }*/



            return View(orders);
        }

        // GET: Admin/Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

         
            var orders = await _context.Orders
                  .Include(od => od.OrderProducts)
                .ThenInclude(p => p.Products)
                .FirstOrDefaultAsync(m => m.ID == id);
           
            if (orders == null)
            {
                return NotFound();
            }

            return View(orders);
        }

        // GET: Admin/Orders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Address,Telephone,OrderNote,CreateAt,UpdateAt,paymenttype,Status,GrandTotal,UserID")] Orders orders)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orders);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(orders);
        }

        // GET: Admin/Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orders = await _context.Orders.FindAsync(id);
            if (orders == null)
            {
                return NotFound();
            }
            return View(orders);
        }

        // POST: Admin/Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Address,Telephone,OrderNote,CreateAt,UpdateAt,paymenttype,Status,GrandTotal,UserID,CustomerName")] Orders orders)
        {
            if (id != orders.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orders);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrdersExists(orders.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(orders);
        }

        // GET: Admin/Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orders = await _context.Orders
                .FirstOrDefaultAsync(m => m.ID == id);
            if (orders == null)
            {
                return NotFound();
            }

            return View(orders);
        }

        // POST: Admin/Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orders = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(orders);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrdersExists(int id)
        {
            return _context.Orders.Any(e => e.ID == id);
        }
    }
}

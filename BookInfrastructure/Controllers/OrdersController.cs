    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using BookDomain.Model;
    using BookInfrastructure;
    using Microsoft.AspNetCore.Authorization;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc.Rendering;

    namespace BookInfrastructure.Controllers
    {
        public class OrdersController : Controller
        {
            private readonly BooksShopdatabaseContext _context;
            private readonly UserManager<Customer> _userManager; // Змінено IdentityUser на Customer

            public OrdersController(BooksShopdatabaseContext context, UserManager<Customer> userManager)
            {
                _context = context;
                _userManager = userManager;
            }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var booksShopdatabaseContext = _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Book);
            return View(await booksShopdatabaseContext.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var order = await _context.Orders
                    .Include(o => o.Customer)
                    .FirstOrDefaultAsync(m => m.OrderId == id);
                if (order == null)
                {
                    return NotFound();
                }

                return View(order);
            }

            // GET: Orders/Create
            public IActionResult Create()
            {
                ViewData["CustomerEmail"] = new SelectList(_context.Customers, "Email", "Email");
                return View();
            }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerEmail,OrderDate,TotalPrice,DeliveryAddress,OrderStatus,ShoppingBasketId")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();

                Console.WriteLine($"Створено замовлення: {order.OrderId}");

                var shoppingBasket = await _context.ShoppingBaskets
                    .Include(sb => sb.ShoppingBasketBooks)
                    .ThenInclude(sbb => sbb.Book)
                    .FirstOrDefaultAsync(sb => sb.Id == order.ShoppingBasketId);

                if (shoppingBasket != null && shoppingBasket.ShoppingBasketBooks.Any())
                {
                    // Очищаємо старі OrderDetails, якщо вони є (для безпеки)
                    var existingOrderDetails = _context.OrderDetails.Where(od => od.OrderId == order.OrderId);
                    if (existingOrderDetails.Any())
                    {
                        _context.OrderDetails.RemoveRange(existingOrderDetails);
                        Console.WriteLine("Очищено старі OrderDetails перед додаванням нових.");
                    }

                    foreach (var item in shoppingBasket.ShoppingBasketBooks)
                    {
                        if (item.Count > 0 && item.Book != null)
                        {
                            var orderDetail = new OrderDetail
                            {
                                OrderId = order.OrderId,
                                BookId = item.Book.Id,
                                Quantity = item.Count,
                                TotalPrice = item.Count * item.Book.Price,
                                Status = "Новий"
                            };
                            _context.OrderDetails.Add(orderDetail); // Додаємо до контексту
                            Console.WriteLine($"Додано OrderDetail: OrderId={orderDetail.OrderId}, BookId={orderDetail.BookId}, Quantity={orderDetail.Quantity}");
                        }
                    }

                    try
                    {
                        await _context.SaveChangesAsync();
                        Console.WriteLine("Успішно збережено OrderDetails.");
                    }
                    catch (DbUpdateException ex)
                    {
                        Console.WriteLine($"Помилка при збереженні: {ex.InnerException?.Message}");
                        return View(order);
                    }
                }
                else
                {
                    Console.WriteLine($"Кошик порожній або не знайдено для ShoppingBasketId: {order.ShoppingBasketId}");
                }

                return RedirectToAction(nameof(Index));
            }

            return View(order);
        }




        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var order = await _context.Orders.FindAsync(id);
                if (order == null)
                {
                    return NotFound();
                }

                ViewData["CustomerEmail"] = new SelectList(_context.Customers, "Email", "Email", order.CustomerEmail);
                return View(order);
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Edit(int id, [Bind("OrderId,CustomerEmail,OrderDate,TotalPrice,DeliveryAddress,OrderStatus,ShoppingBasketId")] Order order, string PaymentMethod, string CardName, string CardNumber, string Expiration, string CVV)
            {
                if (id != order.OrderId)
                {
                    Console.WriteLine($"ID не збігається: передане {order.OrderId}, очікуване {id}");
                    return NotFound();
                }

                // Отримуємо email автентифікованого користувача
                var userEmail = User.Identity?.Name;
                if (string.IsNullOrEmpty(userEmail))
                {
                    ModelState.AddModelError("", "Користувач не автентифікований.");
                    Console.WriteLine("Користувач не автентифікований.");
                    return View(order);
                }

                // Знаходимо користувача за email і отримуємо його Id
                var user = await _userManager.FindByEmailAsync(userEmail);
                if (user == null)
                {
                    ModelState.AddModelError("", "Користувача не знайдено.");
                    Console.WriteLine("Користувача не знайдено для email: " + userEmail);
                    return View(order);
                }

                // Встановлюємо правильний Id користувача
                order.CustomerEmail = user.Id;
                Console.WriteLine($"Встановлено CustomerEmail: {order.CustomerEmail}");

                // Перевіряємо і встановлюємо DeliveryAddress, якщо він null
                if (string.IsNullOrEmpty(order.DeliveryAddress))
                {
                    ModelState.AddModelError("DeliveryAddress", "Адреса доставки є обов'язковою.");
                    Console.WriteLine("Адреса доставки не вказана.");
                    return View(order);
                }

                // Завантажуємо існуюче замовлення для оновлення
                var existingOrder = await _context.Orders
                    .Include(o => o.OrderDetails) // Завантажуємо існуючі деталі
                    .ThenInclude(od => od.Book)
                    .FirstOrDefaultAsync(o => o.OrderId == id);

                if (existingOrder == null)
                {
                    Console.WriteLine($"Замовлення з ID {id} не знайдено.");
                    return NotFound();
                }
                Console.WriteLine($"Знайдено замовлення: OrderId={existingOrder.OrderId}");

                // Оновлюємо поля існуючого замовлення
                existingOrder.CustomerEmail = order.CustomerEmail;
                existingOrder.OrderDate = order.OrderDate ?? DateTime.Now;
                existingOrder.DeliveryAddress = order.DeliveryAddress;
                existingOrder.OrderStatus = order.OrderStatus;
                existingOrder.ShoppingBasketId = order.ShoppingBasketId;
                existingOrder.TotalPrice = order.TotalPrice;
                if (order.TotalPrice > 0)
                {
                    existingOrder.TotalPrice = order.TotalPrice;
                }
                else if (order.ShoppingBasketId != null)
                {
                    var shoppingBasket = await _context.ShoppingBaskets
                        .Include(sb => sb.ShoppingBasketBooks)
                        .ThenInclude(i => i.Book)
                        .FirstOrDefaultAsync(sb => sb.Id == order.ShoppingBasketId);

                    if (shoppingBasket != null)
                    {
                        existingOrder.TotalPrice = shoppingBasket.ShoppingBasketBooks.Sum(i => i.Count * (i.Book?.Price ?? 0m));
                        Console.WriteLine($"Обчислено TotalPrice: {existingOrder.TotalPrice}, Кількість книг: {shoppingBasket.ShoppingBasketBooks.Count}");
                    }
                    else
                    {
                        Console.WriteLine($"Koшик не знайдено для ShoppingBasketId: {order.ShoppingBasketId}");
                    }
                }

                // Логіка для оновлення або створення OrderDetails
                if (order.ShoppingBasketId != null)
                {
                    var shoppingBasket = await _context.ShoppingBaskets
                        .Include(sb => sb.ShoppingBasketBooks)
                        .ThenInclude(sbb => sbb.Book)
                        .FirstOrDefaultAsync(sb => sb.Id == order.ShoppingBasketId);

                    if (shoppingBasket != null)
                    {
                        Console.WriteLine($"Знайдено кошик з {shoppingBasket.ShoppingBasketBooks.Count} елементами.");

                        // Очищаємо існуючі деталі
                        _context.OrderDetails.RemoveRange(existingOrder.OrderDetails);
                        Console.WriteLine($"Видалено {existingOrder.OrderDetails.Count} старих деталей.");

                        // Створюємо нові деталі
                        foreach (var item in shoppingBasket.ShoppingBasketBooks)
                        {
                            if (item.Count > 0 && item.Book != null && item.Book.Id > 0)
                            {
                                var orderDetail = new OrderDetail
                                {
                                    OrderId = existingOrder.OrderId,
                                    BookId = item.Book.Id,
                                    Quantity = item.Count,
                                    TotalPrice = item.Count * item.Book.Price,
                                    Status = "Новий"
                                };
                                _context.OrderDetails.Add(orderDetail);
                                Console.WriteLine($"Додано OrderDetail: OrderId={orderDetail.OrderId}, BookId={orderDetail.BookId}, Quantity={orderDetail.Quantity}, TotalPrice={orderDetail.TotalPrice}");
                            }
                            else
                            {
                                Console.WriteLine($"Пропущено елемент: Count={item.Count}, BookId={item.Book?.Id}");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Koшик не знайдено для ShoppingBasketId: {order.ShoppingBasketId}");
                    }
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        await _context.SaveChangesAsync();
                        Console.WriteLine("Успішно збережено зміни, включаючи OrderDetails.");
                    }
                    catch (DbUpdateException ex)
                    {
                        Console.WriteLine($"Помилка при збереженні: {ex.InnerException?.Message}");
                        return View(order); // Повертаємо вигляд із помилкою
                    }
                    return RedirectToAction("OrderConfirmation", new { id = existingOrder.OrderId });
                }

                ViewData["CustomerEmail"] = new SelectList(_context.Customers, "Email", "Email", order.CustomerEmail);
                return View(order);
            }

            // GET: Order Confirmation
            public IActionResult OrderConfirmation(int id)
            {
                var order = _context.Orders
                    .Include(o => o.OrderDetails)
                    .ThenInclude(oi => oi.Book)
                    .FirstOrDefault(o => o.OrderId == id);

                if (order == null)
                {
                    return NotFound();
                }

                return View(order);
            }

            // GET: Orders/Delete/5
            public async Task<IActionResult> Delete(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var order = await _context.Orders
                    .Include(o => o.Customer)
                    .FirstOrDefaultAsync(m => m.OrderId == id);
                if (order == null)
                {
                    return NotFound();
                }

                return View(order);
            }

        [Authorize(Roles = "Admin")]
        public IActionResult Orders()
        {
            var orders = _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Book)
                .Include(o => o.Customer) // Додаємо Customer
                .ToList();

            foreach (var order in orders)
            {
                if (order.OrderDetails == null)
                {
                    order.OrderDetails = _context.OrderDetails
                        .Where(od => od.OrderId == order.OrderId)
                        .Include(od => od.Book)
                        .ToList();
                }
            }

            return View(orders);
        }

        public async Task<IActionResult> Checkout(int id)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            // Перевіряємо і створюємо OrderDetails лише якщо їх немає
            if (!await _context.OrderDetails.AnyAsync(od => od.OrderId == order.OrderId))
            {
                if (order.ShoppingBasketId != null)
                {
                    var shoppingBasket = await _context.ShoppingBaskets
                        .Include(sb => sb.ShoppingBasketBooks)
                        .ThenInclude(sbb => sbb.Book)
                        .FirstOrDefaultAsync(sb => sb.Id == order.ShoppingBasketId);

                    if (shoppingBasket?.ShoppingBasketBooks.Any() == true)
                    {
                        foreach (var item in shoppingBasket.ShoppingBasketBooks)
                        {
                            if (item.Count > 0 && item.Book != null)
                            {
                                var orderDetail = new OrderDetail
                                {
                                    OrderId = order.OrderId,
                                    BookId = item.Book.Id,
                                    Quantity = item.Count,
                                    TotalPrice = item.Count * item.Book.Price,
                                    Status = "Новий"
                                };
                                _context.OrderDetails.Add(orderDetail);
                                Console.WriteLine($"Додано OrderDetail: OrderId={orderDetail.OrderId}, BookId={orderDetail.BookId}");
                            }
                        }
                        await _context.SaveChangesAsync();
                        Console.WriteLine("Успішно збережено OrderDetails.");
                    }
                    else
                    {
                        Console.WriteLine($"Кошик порожній для ShoppingBasketId: {order.ShoppingBasketId}");
                    }
                }
                else
                {
                    Console.WriteLine("ShoppingBasketId не задано.");
                }
            }

            // Завантажуємо OrderDetails для відображення
            order.OrderDetails = await _context.OrderDetails
                .Include(od => od.Book)
                .Where(od => od.OrderId == order.OrderId)
                .ToListAsync();

            // Оновлюємо TotalPrice
            order.TotalPrice = order.OrderDetails?.Sum(od => od.TotalPrice) ?? 0;

            if (order.OrderDetails != null)
            {
                foreach (var detail in order.OrderDetails)
                {
                    if (detail.Quantity == 0) detail.Quantity = 1;
                    detail.TotalPrice = (detail.Book?.Price ?? 0) * detail.Quantity;
                }
                await _context.SaveChangesAsync();
                Console.WriteLine("Успішно оновлено TotalPrice.");
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeleteConfirmed(int id)
            {
                var order = await _context.Orders.FindAsync(id);
                if (order != null)
                {
                    _context.Orders.Remove(order);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            private bool OrderExists(int id)
            {
                return _context.Orders.Any(e => e.OrderId == id);
            }
        }
    }
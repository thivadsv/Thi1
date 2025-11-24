using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTTAPI.Data;
using VTTAPI.Models;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    // Dependency Injection: Nhận DbContext qua Constructor
    public ProductsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET /api/products
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        // Trả về tất cả sản phẩm từ Database
        return await _context.Products.ToListAsync();
    }

    // --- CHỨC NĂNG CƠ BẢN: UPDATE (PUT) ---
    // PUT /api/products/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, Product product)
    {
        if (id != product.Id)
        {
            return BadRequest(); // Trả về lỗi 400
        }

        // Đánh dấu đối tượng là đã thay đổi
        _context.Entry(product).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync(); // Lưu thay đổi vào Database
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Products.Any(e => e.Id == id))
            {
                return NotFound(); // Trả về lỗi 404 nếu không tìm thấy ID
            }
            else
            {
                throw;
            }
        }

        return NoContent(); // Trả về 204 No Content (Thành công nhưng không cần trả dữ liệu)
    }

    // (Thêm POST, GET by Id, và DELETE tương tự)
}
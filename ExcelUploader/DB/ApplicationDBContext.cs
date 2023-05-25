using ExcelUploader.Models;
using Microsoft.EntityFrameworkCore;

namespace ExcelUploader.DB
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }

        public DbSet<ExcelFileDTO> ExcelFileDTOs { get; set; }
    }
}

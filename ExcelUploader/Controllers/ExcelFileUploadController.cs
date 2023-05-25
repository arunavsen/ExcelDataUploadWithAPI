using ExcelUploader.DB;
using ExcelUploader.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace ExcelUploader.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExcelFileUploadController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public ExcelFileUploadController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpPost("PopulateData")]
        public IActionResult PopulateData(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest("No file uploaded.");

                using var stream = new MemoryStream();
                file.CopyTo(stream);

                using var package = new ExcelPackage(stream);
                var worksheet = package.Workbook.Worksheets[0]; // Assuming the data is in the first sheet


                var rows = worksheet.Cells
                                    .Where(cell => cell.Start.Row > 1) // Skip header row
                                    .Select(cell => cell.Start.Row)
                                    .Distinct();

                var excelData = new List<ExcelFileDTO>();

                foreach (var row in rows)
                {
                    var rowData = worksheet.Cells[row, 1, row, worksheet.Dimension.Columns].Select(cell => cell.Value?.ToString()).ToList();

                    var excelRow = new ExcelFileDTO();
                    excelRow.Month = rowData[0];
                    excelRow.Year = int.Parse(rowData[1]);
                    excelRow.CountryCode = rowData[2];
                    excelRow.BriefType = rowData[3];
                    excelRow.Title = rowData[4];
                    excelRow.ShortStory = rowData[5];
                    excelRow.LongStory = rowData[6];
                    excelRow.Link = rowData[7];
                    excelRow.PubDate = DateTime.Parse(rowData[8]);

                    excelData.Add(excelRow);
                }

                _context.ExcelFileDTOs.AddRange(excelData);
                _context.SaveChanges();

                return Ok(excelData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}

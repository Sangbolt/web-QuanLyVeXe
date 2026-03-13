using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace VEXE.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly Models.VEXE db = new Models.VEXE();

        public ActionResult GenerateInvoice(int ticketId)
        {
            var ticket = db.tickets
                .Include("city")
                .Include("city1")
                .FirstOrDefault(t => t.id == ticketId);

            if (ticket == null)
                return HttpNotFound("Không tìm thấy vé này.");

            using (MemoryStream ms = new MemoryStream())
            {
                Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                document.Open();

                // Đảm bảo rằng Times New Roman được sử dụng
                string fontPath = "C:\\Windows\\Fonts\\times.ttf"; // Đường dẫn tới phông Times New Roman trên hệ thống
                BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

                // Màu cam chủ đạo
                BaseColor primaryOrange = new BaseColor(255, 165, 0); // Màu cam
                BaseColor redColor = new BaseColor(255, 0, 0); // Màu đỏ
                BaseColor blueColor = new BaseColor(0, 0, 255); // Màu xanh

                try
                {
                    PdfPTable table1 = new PdfPTable(2);
                    table1.WidthPercentage = 100f;
                    float[] widths = new float[] { 1f, 1f };
                    table1.SetWidths(widths);

                    // Thêm logo đầu tiên vào cột 1
                    string logoPath1 = "C:\\Users\\tinhy\\OneDrive\\Desktop\\PhuongTrang_Admin\\XeKhachPhuongTrang\\XeKhachPhuongTrang\\Content\\Images\\logoText.png";
                    Image logo1 = Image.GetInstance(logoPath1);
                    logo1.ScaleToFit(150f, 150f);
                    logo1.Alignment = Element.ALIGN_CENTER;
                    PdfPCell cell1 = new PdfPCell(logo1) { Border = 0, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE };
                    table1.AddCell(cell1);

                    // Thêm logo thứ hai vào cột 2
                    string logoPath2 = "C:\\Users\\tinhy\\OneDrive\\Desktop\\PhuongTrang_Admin\\XeKhachPhuongTrang\\XeKhachPhuongTrang\\Content\\Images\\chatluong.jpg";
                    Image logo2 = Image.GetInstance(logoPath2);
                    logo2.ScaleToFit(150f, 150f);
                    logo2.Alignment = Element.ALIGN_CENTER;
                    PdfPCell cell2 = new PdfPCell(logo2) { Border = 0, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE };
                    table1.AddCell(cell2);
                    document.Add(table1);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Không thể tải logo: {ex.Message}");
                }

                // Title
                var titleFont = new Font(baseFont, 22, Font.BOLD, BaseColor.ORANGE);
                var textFont = new Font(baseFont, 18, Font.NORMAL, BaseColor.BLACK);
                document.Add(new Paragraph("Hóa Đơn Đặt Vé", titleFont) { Alignment = Element.ALIGN_CENTER });
                document.Add(new Paragraph($"Ngày: {DateTime.Now:dd/MM/yyyy}", textFont));
                document.Add(new Paragraph("\n"));

                // Ticket Details
                PdfPTable table = new PdfPTable(2) { WidthPercentage = 100 };
                table.AddCell(new PdfPCell(new Phrase("Thông Tin", textFont))
                {
                    Colspan = 2,
                    HorizontalAlignment = Element.ALIGN_CENTER
                });

                table.AddCell(new Phrase("Tổng tiền:", textFont));
                table.AddCell(new Phrase($"{ticket.price?.ToString("N0") ?? "N/A"} VND", textFont));

                table.AddCell(new Phrase("Ngày khởi hành:", textFont));
                table.AddCell(new Phrase(ticket.departure_date?.ToString("dd/MM/yyyy") ?? "N/A", textFont));

                table.AddCell(new Phrase("Điểm đi:", textFont));
                table.AddCell(new Phrase(ticket.city?.cityName ?? "N/A", textFont));

                table.AddCell(new Phrase("Điểm đến:", textFont));
                table.AddCell(new Phrase(ticket.city1?.cityName ?? "N/A", textFont));

                table.AddCell(new Phrase("Giờ khởi hành:", textFont));
                table.AddCell(new Phrase(ticket.departure_time?.ToString(@"hh\:mm") ?? "N/A", textFont));

                table.AddCell(new Phrase("Giờ đến:", textFont));
                table.AddCell(new Phrase(ticket.arrival_time?.ToString(@"hh\:mm") ?? "N/A", textFont));

                document.Add(table);
                document.Add(new Paragraph("\nCảm ơn quý khách đã sử dụng dịch vụ của chúng tôi.", textFont));
                document.Close();
                writer.Close();
                byte[] file = ms.ToArray();
                return File(file, "application/pdf", $"Invoice_{ticketId}.pdf");
            }
        }
    }
}
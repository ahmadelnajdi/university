using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using University.Areas.Identity.Data;
using University.Data;
using University.Models;


namespace University.Controllers
{
    public class PapersController : Controller
    {
        private readonly UniversityContext _context;


        public PapersController(UniversityContext context)
        {
           
            
            _context = context;
        }

        [Authorize]
        // GET: Papers
        public async Task<IActionResult> Index()
        {


          

            List<Paper> lists = new List<Paper>();
            List<Paper> lists2 = new List<Paper>();
            lists = await _context.papers.ToListAsync();
            for (int i = 0; i < lists.Count(); i++)
            {
                if (lists[i].Email == Name.logClient)
                    lists2.Add(lists[i]);

            }

            return View(lists2);
        }


        public async Task<IActionResult> ViewAll()
        {

            return View(await _context.papers.ToListAsync());
        }
        public async Task<IActionResult> CreatePaper(int? id) {


            var paper = await _context.papers
                .FirstOrDefaultAsync(m => m.id == id);


            StudentData student = new StudentData(); 
            List<StudentData> lists = new List<StudentData>();
            List<StudentData> notes = new List<StudentData>();
            lists = await _context.studentDatas.ToListAsync();

            for (int i = 0; i < lists.Count(); i++) {

                if (paper.StudentID == lists[i].Faculty)
                    student.Studentid = lists[i].Studentid;
                student.Faculty = lists[i].Faculty; 
                    student.Course = lists[i].Course;
                    student.Note = lists[i].Note;

                notes.Add(student);
           }


            PdfDocument document = new PdfDocument();
            PdfPage page = document.Pages.Add();


            return View();
        }


        public async Task<IActionResult> Admin()
        {


            List<Paper> papers = new List<Paper>();
            List<Paper> lists = new List<Paper>();

            papers = await _context.papers.ToListAsync();

            for (int i = 0; i < papers.Count(); i++) {

                if (papers[i].Status == "In progress")
                    lists.Add(papers[i]);
            
            }

 
            return View(lists);
        }



        public async Task<FileStreamResult> CompleteAsync(int? id) {


            var paper = await _context.papers
               .FirstOrDefaultAsync(m => m.id == id);


            List<StudentData> students = new List<StudentData>();
            List<StudentData> print = new List<StudentData>();
            students = await _context.studentDatas.ToListAsync();

            StudentData student = null;

            for (int i = 0; i < students.Count(); i++) {

                if ((students[i].Studentid).Equals(paper.StudentID))

                {
                    student = new StudentData();
                    student.Faculty = students[i].Faculty;
                    student.Course = students[i].Course;
                    student.Note = students[i].Note;
                    print.Add(student);
                }
            }



           


            PdfDocument document = new PdfDocument();

            //Add a page to the document
            PdfPage page = document.Pages.Add();

            //Create PDF graphics for the page
            PdfGraphics graphics = page.Graphics;

            //Set the standard font
            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);

            

            FileStream imageStream = new FileStream("ua.jpg", FileMode.Open, FileAccess.Read);
            PdfBitmap image = new PdfBitmap(imageStream);
            //Draw the image
            graphics.DrawImage(image,30, 0);


          

         
            

            if (paper.type == "Notes")
            {


                DataTable table = new DataTable("INFO");

                // Declare DataColumn and DataRow variables.
                DataColumn column;
                DataColumn column2;
                
              
                // Create column.
                column = new DataColumn();
                column.DataType = Type.GetType("System.String");
                column.ColumnName = "Course";
                table.Columns.Add(column);
                
                column2 = new DataColumn();
                column2.DataType = Type.GetType("System.Int32");
                column2.ColumnName = "Note";
                table.Columns.Add(column2);

                foreach (StudentData item in print)
                {
                    table.Rows.Add(item.Course,item.Note);

                }


                

                PdfBrush solidBrush = new PdfSolidBrush(new PdfColor(126, 151, 173));
               Rectangle bounds = new RectangleF(0, 200, graphics.ClientSize.Width, 30);
                //Draws a rectangle to place the heading in that region.
                graphics.DrawRectangle(solidBrush, bounds);
                //Creates a font for adding the heading in the page
                PdfFont subHeadingFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 14);
                //Creates a text element to add the invoice number
                PdfTextElement element = new PdfTextElement("Student " + paper.Firstname +" "+ paper.lastname, subHeadingFont); 
                element.Brush = PdfBrushes.White;

                //Draws the heading on the page
                PdfLayoutResult result = element.Draw(page, new PointF(10, bounds.Top + 8));
                string currentDate = "DATE " + DateTime.Now.ToString("MM/dd/yyyy");
                //Measures the width of the text to place it in the correct location
                SizeF textSize = subHeadingFont.MeasureString(currentDate);
                PointF textPosition = new PointF(graphics.ClientSize.Width - textSize.Width - 10, result.Bounds.Y);
                //Draws the date by using DrawString method
                graphics.DrawString(currentDate, subHeadingFont, element.Brush, textPosition);
                PdfFont timesRoman = new PdfStandardFont(PdfFontFamily.TimesRoman, 10);
                //Creates text elements to add the address and draw it to the page.
                element = new PdfTextElement("Student ID: " + paper.StudentID, timesRoman);
                element.Brush = new PdfSolidBrush(new PdfColor(126, 155, 203));
                result = element.Draw(page, new PointF(10, result.Bounds.Bottom + 25));
                PdfPen linePen = new PdfPen(new PdfColor(126, 151, 173), 0.70f);
                PointF startPoint = new PointF(0, result.Bounds.Bottom + 3);
                PointF endPoint = new PointF(graphics.ClientSize.Width, result.Bounds.Bottom + 3);
                //Draws a line at the bottom of the address
                graphics.DrawLine(linePen, startPoint, endPoint);


                //Creates the datasource for the table
                
                //Creates a PDF grid
                PdfGrid grid = new PdfGrid();
                //Adds the data source
                grid.DataSource = table;
                //Creates the grid cell styles
                PdfGridCellStyle cellStyle = new PdfGridCellStyle();
                cellStyle.Borders.All = PdfPens.White;
                PdfGridRow header = grid.Headers[0];
                //Creates the header style
                PdfGridCellStyle headerStyle = new PdfGridCellStyle();
                headerStyle.Borders.All = new PdfPen(new PdfColor(126, 151, 173));
                headerStyle.BackgroundBrush = new PdfSolidBrush(new PdfColor(126, 151, 173));
                headerStyle.TextBrush = PdfBrushes.White;
                headerStyle.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 14f, PdfFontStyle.Regular);

                //Adds cell customizations
                for (int i = 0; i < header.Cells.Count; i++)
                {
                    if (i == 0 || i == 1)
                        header.Cells[i].StringFormat = new PdfStringFormat(PdfTextAlignment.Left, PdfVerticalAlignment.Middle);
                    else
                        header.Cells[i].StringFormat = new PdfStringFormat(PdfTextAlignment.Right, PdfVerticalAlignment.Middle);
                }

                //Applies the header style
                header.ApplyStyle(headerStyle);
                cellStyle.Borders.Bottom = new PdfPen(new PdfColor(217, 217, 217), 0.70f);
                cellStyle.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 12f);
                cellStyle.TextBrush = new PdfSolidBrush(new PdfColor(131, 130, 136));
                //Creates the layout format for grid
                PdfGridLayoutFormat layoutFormat = new PdfGridLayoutFormat();
                // Creates layout format settings to allow the table pagination
                layoutFormat.Layout = PdfLayoutType.Paginate;
                //Draws the grid to the PDF page.
                PdfGridLayoutResult gridResult = grid.Draw(page, new RectangleF(new PointF(0, result.Bounds.Bottom + 40), new SizeF(graphics.ClientSize.Width, graphics.ClientSize.Height - 100)), layoutFormat);




            }
             
            if (paper.type == "Continue Education") {
               
                int now = DateTime.Now.Year;
                int y = now - 1;
                String text = "The student " + paper.Firstname +" "+paper.lastname +" continu his/her education for the year: ";
                String text2 = y + "-" + now;
                graphics.DrawString(text, font, PdfBrushes.Black, new PointF(0, 180));
                graphics.DrawString(text2, font, PdfBrushes.Red, new PointF(0, 210));
            }


            //Saving the PDF to the MemoryStream
            MemoryStream stream = new MemoryStream();

            document.Save(stream);

            //Set the position as '0'.
            stream.Position = 0;

            //Download the PDF document in the browser
            FileStreamResult fileStreamResult = new FileStreamResult(stream, "application/pdf");

            fileStreamResult.FileDownloadName = paper.StudentID+"_"+paper.type +".pdf";

            paper.Status = "Complete";

            _context.Update(paper);
            await _context.SaveChangesAsync();

            RedirectToAction("Admin");

            return fileStreamResult;









        }

        // GET: Papers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paper = await _context.papers
                .FirstOrDefaultAsync(m => m.id == id);
            if (paper == null)
            {
                return NotFound();
            }
            String faculty = " ";
            List<StudentData> lists = new List<StudentData>();
           
            lists = await _context.studentDatas.ToListAsync();
            for (int i = 0; i < lists.Count(); i++)
            {
                if (lists[i].Studentid == paper.StudentID)
                {
                    faculty = lists[i].Faculty;
                    break;
                }
            }

            if (paper.Status == "In progress")
            {

                if (faculty == "genie")
                {

                    ViewBag.path = "genie";
                    ViewBag.description = "Engineering faculty";
                }
                else
                    if (faculty == "business")
                {

                    ViewBag.path = "business";
                    ViewBag.description = "Business faculty" ;
                }
                else
                {
                    ViewBag.path = "eps";

                    ViewBag.description = "Faculty of Sports Sciences";
                }



            }

            else
            {
               
                
                ViewBag.description = "General Secretariat";
            }




            return View(paper);
        }

        // GET: Papers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Papers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Firstname,lastname,StudentID,Year,type,Copy,Status,Email")] Paper paper)
        {
            if (ModelState.IsValid)
            {
                paper.Status = "In progress";
                _context.Add(paper);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(paper);
        }

        // GET: Papers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paper = await _context.papers.FindAsync(id);
            if (paper == null)
            {
                return NotFound();
            }
            return View(paper);
        }

        // POST: Papers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Firstname,lastname,StudentID,Year,type,Copy,Status,Email")] Paper paper)
        {
            if (id != paper.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(paper);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaperExists(paper.id))
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
            return View(paper);
        }

        // GET: Papers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paper = await _context.papers
                .FirstOrDefaultAsync(m => m.id == id);
            if (paper == null)
            {
                return NotFound();
            }

            return View(paper);
        }

        // POST: Papers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var paper = await _context.papers.FindAsync(id);
            _context.papers.Remove(paper);
            await _context.SaveChangesAsync();

            if (Name.logClient == "admin@admin.com")

                return RedirectToAction("Admin");
            return RedirectToAction(nameof(Index));
        }

        private bool PaperExists(int id)
        {
            return _context.papers.Any(e => e.id == id);
        }
    }
}

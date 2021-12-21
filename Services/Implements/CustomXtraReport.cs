using DevExpress.XtraPrinting;
using DevExpress.XtraPrinting.Drawing;
using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicReport.Services.Implements
{
    public class CustomXtraReport
    {
        public void InitBands(XtraReport rep)
        {
            // Create bands
            var detail = new DetailBand();
            var pageHeader = new PageHeaderBand();
            var reportHeader = new ReportHeaderBand();
            var reportFooter = new ReportFooterBand();
            reportHeader.Height = 40;
           detail.HeightF = 20;
            reportFooter.Height = 100;
            pageHeader.Height = 20;
            // Place the bands onto a report
            rep.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] { reportHeader, detail, pageHeader, reportFooter });
        }
        public void InitDetailsBasedonXRTable(XtraReport rep)
        {
            DataSet ds = (DataSet)rep.DataSource;
            int colCount = ds.Tables[0].Columns.Count;
            int colWidth = (rep.PageWidth - (rep.Margins.Left + rep.Margins.Right)) / colCount;
            rep.Margins = new System.Drawing.Printing.Margins(20, 20, 20, 20);
            var tieude = new XRLabel();
            tieude.Text = "DYNAMIC REPORT";
            tieude.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            tieude.ForeColor = Color.Orange;
            tieude.Font = new Font("Tahoma", 20, FontStyle.Bold, GraphicsUnit.Pixel);
            tieude.Width = Convert.ToInt32(rep.PageWidth - 50);
            // Create a table to represent headers
            var tableHeader = new XRTable();
            //   tableHeader.SnapLineMargin = new PaddingInfo(0, 0, 0, 0);
             tableHeader.HeightF = 40;
           
            tableHeader.BackColor = Color.Gray;
            tableHeader.ForeColor = Color.White;
            tableHeader.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            tableHeader.Font = new Font("Tahoma", 12, FontStyle.Bold, GraphicsUnit.Pixel);
            tableHeader.Width = rep.PageWidth - (rep.Margins.Left + rep.Margins.Right);
           tableHeader.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 100.0f);
            var headerRow = new XRTableRow();
            headerRow.Width = tableHeader.Width;
            tableHeader.Rows.Add(headerRow);
            tableHeader.BeginInit();
            // Create a table to display data
            var tableDetail = new XRTable();
           // tableDetail.HeightF = 20;
             tableDetail.HeightF = rep.Band.HeightF;
            tableDetail.Padding = new DevExpress.XtraPrinting.PaddingInfo(100,100,100,100);
            tableDetail.Width = rep.PageWidth - (rep.Margins.Left + rep.Margins.Right);
            tableDetail.Font = new Font("Tahoma", 12, FontStyle.Regular, GraphicsUnit.Pixel);
            var detailRow = new XRTableRow();
          detailRow.HeightF = 20;
            //  detailRow.TopF = -10;
            
            detailRow.Width = tableDetail.Width;
            tableDetail.Rows.Add(detailRow);
            tableDetail.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 100.0f);
            tableDetail.BeginInit();
            // Create table cells, fill the header cells with text, bind the cells to data
            for (int i = 0, loopTo = colCount - 1; i <= loopTo; i++)
            {
                var headerCell = new XRTableCell();
                headerCell.Text = ds.Tables[0].Columns[i].Caption;
                var detailCell = new XRTableCell();
                detailCell.DataBindings.Add("Text", default, ds.Tables[0].Columns[i].Caption);
                if (i == 0)
                {
                    headerCell.Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom;
                    detailCell.Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom;
                }
                else
                {
                    headerCell.Borders = DevExpress.XtraPrinting.BorderSide.All;
                    detailCell.Borders = DevExpress.XtraPrinting.BorderSide.All;
                }

                if (i == 0)
                {
                    headerCell.Width = 50;
                    detailCell.Width = 50;
                   detailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
                }
                else if (i == 1)
                {
                    headerCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                    headerCell.Width = 130;
                    detailCell.Width = 130;
                }
                else if (i == 2)
                {
                    headerCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                    headerCell.Width = 70;
                    detailCell.Width = 70;
                }
                else if (i == 4)
                {
                    headerCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                    headerCell.Width = 145;
                    detailCell.Width = 145;
                }
                else
                {
                    headerCell.Width = colWidth;
                    detailCell.Width = colWidth;
                }

                detailCell.Borders = DevExpress.XtraPrinting.BorderSide.Bottom | DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right;

                // Place the cells into the corresponding tables
                headerRow.Cells.Add(headerCell);
                detailRow.Cells.Add(detailCell);
            }

            tableHeader.EndInit();
            tableDetail.EndInit();
            // Place the table onto a report's Detail band
            rep.Bands[BandKind.ReportHeader].Controls.Add(tieude);
            rep.Bands[BandKind.PageHeader].Controls.Add(tableHeader);
            rep.Bands[BandKind.Detail].Controls.Add(tableDetail);
        }

        public void SetTextWatermark(XtraReport ps)
        {
            // Create the text watermark.
            var textWatermark = new Watermark();

            // Set watermark options.
            textWatermark.Text = "Dynamic Report";
            textWatermark.TextDirection = DirectionMode.ForwardDiagonal;
            textWatermark.Font = new Font(textWatermark.Font.FontFamily, 40);
            textWatermark.ForeColor = Color.FromArgb(0x7000e5ff);
            textWatermark.TextTransparency = 10;
            textWatermark.ShowBehind = false;
            textWatermark.PageRange = "1,3-5";

            // Add the watermark to a document.
            ps.Watermark.CopyFrom(textWatermark);
        }

        public void SetPictureWatermark(XtraReport ps)
        {
            // Create the picture watermark.
            var pictureWatermark = new Watermark();

            // Set watermark options.
            pictureWatermark.Image = Bitmap.FromFile("logo.png");
            pictureWatermark.ImageAlign = ContentAlignment.MiddleCenter;
            pictureWatermark.ImageTiling = false;
            pictureWatermark.ImageViewMode = ImageViewMode.Zoom;
            pictureWatermark.ImageTransparency = 150;
            pictureWatermark.ShowBehind = true;
            // pictureWatermark.PageRange = "2,4"

            // Add the watermark to a document.
            ps.Watermark.CopyFrom(pictureWatermark);
        }
        private string folderRootPdf = "./wwwroot/Pdfs";
        //private string folderRootPdf = "E://";

        public string CreatePdf(DataSet dataSet)
        {
            // Create XtraReport instance
            var rep = new XtraReport();
            rep.PaperKind = System.Drawing.Printing.PaperKind.A4;
            //if (grpPagesetup.Text == 0)
            //{
            //    rep.PaperKind = System.Drawing.Printing.PaperKind.A4;
            //}
            //else
            //{
            //    rep.PaperKind = System.Drawing.Printing.PaperKind.A4Rotated;
            //}

            SetTextWatermark(rep);
            //if (grpWaterMark.Text == 0)
            //{
            //    SetTextWatermark(rep);
            //}
            //else
            //{
            //    SetPictureWatermark(rep);
            //}

            rep.DataSource = dataSet;
            rep.DataMember = dataSet.Tables[0].TableName;
            InitBands(rep);
            InitDetailsBasedonXRTable(rep);
            PdfExportOptions pdfExportOptions = new PdfExportOptions()
            {
                PdfACompatibility = PdfACompatibility.PdfA1b
            };
            var pathFile = Path.ChangeExtension(dataSet.Tables[0].TableName+Guid.NewGuid().ToString(), ".pdf");
            rep.ExportToPdf(Path.Combine(folderRootPdf,pathFile), pdfExportOptions);
            return pathFile;
        }

    }
}

using DEPI_Donation.Data;
using DEPI_Donation.Models;
using Microsoft.EntityFrameworkCore;

namespace DEPI_Donation.Models.ModelsBL
{
    public class ReportBL
    {
        private readonly AppDbcontext _context;

        public ReportBL(AppDbcontext context)
        {
            _context = context;
        }

        public List<Report> GetAllReports()
        {
            return _context.Reports.Include(r => r.Activities).ToList();
        }

        public Report GetReportById(int id)
        {
            var report = _context.Reports.Include(r => r.Activities)
                                         .FirstOrDefault(r => r.ReportId == id);
            if (report == null)
                throw new Exception("Report not found.");
            return report;
        }

        public void AddReport(Report newReport)
        {
            _context.Reports.Add(newReport);
            _context.SaveChanges();
        }

        public void UpdateReport(int id, Report updatedReport)
        {
            var report = _context.Reports.FirstOrDefault(r => r.ReportId == id);
            if (report == null)
                throw new Exception("Report not found.");

            report.Title = updatedReport.Title;
            report.Description = updatedReport.Description;
            _context.SaveChanges();
        }

        public void DeleteReport(int id)
        {
            var report = _context.Reports.Include(r => r.Activities)
                                         .FirstOrDefault(r => r.ReportId == id);
            if (report == null)
                throw new Exception("Report not found.");

            if (report.Activities.Any())
                _context.Activities.RemoveRange(report.Activities);

            _context.Reports.Remove(report);
            _context.SaveChanges();
        }
    }
}

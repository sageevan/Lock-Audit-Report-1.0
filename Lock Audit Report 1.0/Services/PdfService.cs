using Lock_Audit_Report_1._0.Models;
using System;
using System.Collections.ObjectModel;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.IO;
using System.Windows;

namespace Lock_Audit_Report_1._0.Services
{
    public static class PdfService
    {
        public static void ExportEventsToPdf(ObservableCollection<OnlineEvent> events)
        {
            if (events.Count == 0) throw new Exception("No events to export.");

            string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Report_{DateTime.Now:yyyyMMdd_HHmm}.pdf");

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(20);
                    page.Header().Text("Room Events Report").SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);
                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(120);
                            columns.RelativeColumn();
                            columns.ConstantColumn(80);
                            columns.ConstantColumn(80);
                            columns.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("Date/Time");
                            header.Cell().Text("Guest Name");
                            header.Cell().Text("Status");
                            header.Cell().Text("Room");
                            header.Cell().Text("Credential Class");
                        });

                        foreach (var e in events)
                        {
                            table.Cell().Text(e.Timestamp.ToString("dd/MM/yyyy HH:mm"));
                            table.Cell().Text(e.GuestName);
                            table.Cell().Text(e.KeyStatus);
                            table.Cell().Text(e.RoomNumber);
                            table.Cell().Text(e.CredentialClass);
                        }
                    });
                });
            }).GeneratePdf(fileName);
        }
    }
}
using ClosedXML.Excel;

namespace ASM.Application.Infrastructure.Excel;

public class ExcelWriter<T> : IExcelWriter<T> where T : class
{
    public void Write(List<T> data, string ignoreColumn, Stream stream)
    {
        using var workbook = new XLWorkbook();

        var worksheet = workbook.Worksheets.Add("Report");

        worksheet.Row(1).Style.Font.Bold = true;

        var headers = typeof(T).GetProperties()
            .Where(p => p.Name != ignoreColumn)
            .Select(p => p.Name)
            .ToList();

        for (var i = 0; i < headers.Count; i++)
        {
            worksheet.Cell(1, i + 1).Value = headers[i];
        }

        for (var i = 0; i < data.Count; i++)
        {
            var item = data[i];

            for (var j = 0; j < headers.Count; j++)
            {
                var value = item.GetType().GetProperty(headers[j])?.GetValue(item);
                worksheet.Cell(i + 2, j + 1).Value = value is not null ? value.ToString() : string.Empty;
            }
        }

        worksheet.Columns().AdjustToContents();

        workbook.SaveAs(stream);
    }
}

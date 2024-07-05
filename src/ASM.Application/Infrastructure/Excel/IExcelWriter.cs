namespace ASM.Application.Infrastructure.Excel;

public interface IExcelWriter<T> where T : class
{
    void Write(List<T> data, string ignoreColumn, Stream stream);
}

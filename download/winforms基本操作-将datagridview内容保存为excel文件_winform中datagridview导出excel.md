





---


### title: winforms基本操作\-将datagridview内容保存为[excel](https://so.csdn.net/so/search?q=excel&spm=1001.2101.3001.7020)文件 tags: \[winforms, windows, datagridview] categories: \[客户端, windows, winforms]


这里记录一下将winforms展示的datagridview，导出或保存为excel文件。


这里说一下环境、版本信息：  
 win系统：[win11](https://so.csdn.net/so/search?q=win11&spm=1001.2101.3001.7020)  
 框架：winforms  
 依赖：Microsoft.Office.Interop.Excel  
 .net：8\.0\.401  
 .net framework: 4\.8


DataGridView对象为dataGridView1，然后添加一个按钮，绑定事件btnConfirm即可。



```
private void btnConfirm(object sender, EventArgs e)
{
    Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
    if (excelApp == null)
    {
        MessageBox.Show("无法创建Excel，您可能需要安装Excel");
        return;
    }

    // 创建excel工作薄
    Workbook workBook = excelApp.Workbooks.Add(Type.Missing);
    Worksheet workSheet = null;

    // 创建工作表
    workSheet = workBook.Sheets["Sheet1"];
    workSheet = workBook.ActiveSheet;

    // 表头
    Range headerRow = workSheet.Rows[1];
    headerRow.Cells[1, 1] = "文件路径";
    headerRow.Cells[1, 2] = "文件名";
    // 表头格式
    headerRow.Font.Bold = true;
    headerRow.Interior.Color = ColorTranslator.ToOle(Color.LightBlue);


    // 将DataGridView表格内数据复制到excel工作表
    for (int i = 0; i < dataGridView1.Rows.Count; i++)
    {
        for (int j = 0; j < dataGridView1.Columns.Count; j++)
        {
            workSheet.Cells[i + 2, j + 1] = dataGridView1.Rows[i].Cells[j].Value;
        }
    }

    // 导出到excel文件
    SaveFileDialog saveFileDialog = new SaveFileDialog();
    saveFileDialog.Filter = "Excel Workbook (*.xlsx)|*.xlsx|Excel 97-2003 Workbook (*.xls)|*.xls";
    saveFileDialog.Title = "保存文件名称到Excel";
    saveFileDialog.ShowDialog();

    if (saveFileDialog.FileName != "")
    {
        try
        {
            workBook.SaveAs(saveFileDialog.FileName);
            workBook.Close(false);
            excelApp.Quit();

            System.Runtime.InteropServices.Marshal.ReleaseComObject(workSheet);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(workBook);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);

            workSheet = null;
            workBook = null;
            excelApp = null;

            MessageBox.Show("Excel文件已保存到：" + saveFileDialog.FileName);

        }
        catch (Exception ex)
        {
            MessageBox.Show("保存文件名称到Excel失败，请稍后重试。" + ex.Message);
        }
    }
}
![](https://csdnimg.cn/release/blogv2/dist/pc/img/newCodeMoreWhite.png)* 1
* 2
* 3
* 4
* 5
* 6
* 7
* 8
* 9
* 10
* 11
* 12
* 13
* 14
* 15
* 16
* 17
* 18
* 19
* 20
* 21
* 22
* 23
* 24
* 25
* 26
* 27
* 28
* 29
* 30
* 31
* 32
* 33
* 34
* 35
* 36
* 37
* 38
* 39
* 40
* 41
* 42
* 43
* 44
* 45
* 46
* 47
* 48
* 49
* 50
* 51
* 52
* 53
* 54
* 55
* 56
* 57
* 58
* 59
* 60
* 61
* 62
* 63
* 64
* 65
* 66

```

好了，记录到这里，如有问题，欢迎大家联系我讨论。


![修行](https://i-blog.csdnimg.cn/direct/e958412e8ace47a5a27b41240273c0b6.png)




---


**书山有路勤为径，学海无涯苦作舟。**


欢迎关注 公众号：【乐知付加密平台】，您的网络资源可变现


一起学习，一起进步。  
 



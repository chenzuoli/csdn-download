using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WinFormWaitDialog;
using System.Diagnostics;

namespace csdn_download
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void exportBtn_Click(object sender, EventArgs e)
        {
            // 选择导出文件夹
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            folderDialog.Description = "请选择一个文件夹保存博客文章";
            DialogResult dialogResult = folderDialog.ShowDialog();
            string selectedFolder = "";
            if (dialogResult == DialogResult.OK)
            {
                selectedFolder = folderDialog.SelectedPath;
                Console.WriteLine("选择的文件夹为：" + selectedFolder);
            } else
            {
                MessageBox.Show("请选择文件夹保存博客文章");
                return;
            }

            // 获取选择的博客链接
            var ifSeleted = false;
            var selectedUrls = new List<string>();
            for(int i = 0;i < dataGridView1.Rows.Count;i++)
            {
                if (Convert.ToBoolean(dataGridView1.Rows[i].Cells[0].Value)  == true) {
                    ifSeleted = true;
                    selectedUrls.Add(dataGridView1.Rows[i].Cells[2].Value.ToString());
                };
            }
            if (!ifSeleted) {
                MessageBox.Show("未选择任务博客文章，请选择后再导出。");
            }

            // 爬取文章并导出
            get_articles(selectedFolder, selectedUrls);
        }

        private async void get_articles(string selectedFolder, List<string> article_urls)
        {
            var waitDialog = new WaitDialog((IProgress<string> progress) =>
            {
                LongRunningProcess(progress);
            });
            waitDialog.Show();

            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(35);

            // http请求超时时间10min
            var watch = Stopwatch.StartNew();
            try
            {
                using (var tokenSource = new CancellationTokenSource(TimeSpan.FromMinutes(30)))
                {
                    for(int i=0; i < article_urls.Count; i++)
                    {
                        string article_url = article_urls[i];
                        string get_article_content_url = "http://localhost:5000/csdn/get_article?csdn_url=" + article_url;
                        HttpResponseMessage response = await client.GetAsync(get_article_content_url, tokenSource.Token);
                        if (response.IsSuccessStatusCode)
                        {
                            string content = await response.Content.ReadAsStringAsync();
                            Console.WriteLine(content);

                            // 导出到文件夹中
                            Dictionary<string, string> article_info = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
                            write_markdown(selectedFolder, article_info);
                            Thread.Sleep(1000); // sleep 1s
                        }
                        else
                        {
                            Console.WriteLine($"Error: {response.StatusCode}, csdn download url: {article_url}");
                        }
                    }
                }
            }
            catch (TaskCanceledException ex)
            {
                MessageBox.Show($"{watch.Elapsed} s 任务超时");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"请求错误：{ex.ToString()}");
            }
            finally
            {
                if (waitDialog != null)
                {
                    // 关闭【加载中】进度条
                    waitDialog.Close();
                }
            }
        }

        private void write_markdown(string selectedFolder, Dictionary<string, string>  article_info )
        {
            // 写markdown文件
            string content = article_info["content"];
            string title = article_info["title"].Replace("-CSDN博客", "");
            File.WriteAllText(Path.Combine(selectedFolder, title + ".md"),  content);
        }

        private  void viewBlogBtn_Click(object sender, EventArgs e)
        {
            // 加载中
            var waitDialog = new WaitDialog((IProgress<string> progress) =>
            {
                LongRunningProcess(progress);
            });
            waitDialog.Show();
            load_articles(waitDialog);
            //waitDialog.Close();

        }

        private async void load_articles(WaitDialog waitDialog)
        {
            string user_id = textBox1.Text;
            if (string.IsNullOrEmpty(user_id)) {
                waitDialog.Close();
                MessageBox.Show("请输入用户ID");
                return; }

            string csdn_url = "http://localhost:5000/csdn/get_articles/" + user_id;
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(35);
            
            // http请求超时时间10min
            var watch= Stopwatch.StartNew();
            try
            {
                using (var tokenSource = new CancellationTokenSource(TimeSpan.FromMinutes(30)))
                {
                    HttpResponseMessage response = await client.GetAsync(csdn_url, tokenSource.Token);
                    if (response.IsSuccessStatusCode)
                    {

                        string content = await response.Content.ReadAsStringAsync();
                        Console.WriteLine(content);

                        // 填充表格内容
                        List<Dictionary<string, string>> articles = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(content);
                        fillGridView(articles);
                        Thread.Sleep(1000); // sleep 1s
                    }
                    else
                    {
                        Console.WriteLine($"Error: {response.StatusCode}, csdn download url: {csdn_url}");
                    }
                }
            }
            catch (TaskCanceledException ex)
            {
                MessageBox.Show($"{watch.Elapsed} s 任务超时");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"请求错误：{ex.ToString()}");
            }
            finally
            {
                if (waitDialog != null)
                {
                    // 关闭【加载中】进度条
                    waitDialog.Close();
                }
            }
        }


        private async void LongRunningProcess(IProgress<string> progress)
        {
            for (int i = 0; i < 100; i++)
            {
                progress.Report($"csdn downloading...");
                Thread.Sleep(10000);
            }
         }

        private void fillGridView(List<Dictionary<string, string>> data)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("文章标题", typeof(string));
            dt.Columns.Add("文章链接", typeof(string));
            dt.Columns.Add("发表时间", typeof(string));
            for (int i = 0; i < data.Count; i++)
            {
                Dictionary<string, string> article_info = data[i];
                dt.Rows.Add(article_info["title"], article_info["url"], article_info["post_time"]);
            }
            // 将DataTable数据绑定到DataGridView
            dataGridView1.RowHeadersVisible = true;
            dataGridView1.DataSource = dt;
            // 让datatable自动填满整个DataGridView
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // 在每行的第一列添加复选框
            DataGridViewCheckBoxColumn checkboxColumn = new DataGridViewCheckBoxColumn();
            checkboxColumn.HeaderText = "";
            checkboxColumn.Width = 10;
            checkboxColumn.Name = "checkBoxColumn";
            dataGridView1.Columns.Insert(0, checkboxColumn);

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == -1)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);
                using (Brush brush = new SolidBrush(e.CellStyle.ForeColor))
                {
                    e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.CellStyle.Font, brush, e.CellBounds.Location.X + 14, e.CellBounds.Location.Y + 8);
                }
                e.Handled = true;
            }
        }

        private void loadingLabel(object sender, DoWorkEventArgs e)
        {
            MessageBox.Show("加载中...");
        }

        private void selectAll_Click(object sender, EventArgs e)
        {
            // 遍历datagridview每一行，判断是否已经选中，若为选中，则选中
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dataGridView1.Rows[i].Cells[0].Value) == false)
                {
                    dataGridView1.Rows[i].Cells[0].Value = true;
                } else
                    continue;
            }
        }

        private void cancelSelectAll_Click(object sender, EventArgs e)
        {
            // 取消全选
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dataGridView1.Rows[i].Cells[0].Value) == true)
                {
                    dataGridView1.Rows[i].Cells[0].Value = false;
                }
                else continue;
            }
        }

        private void reverseSelect_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dataGridView1.Rows[i].Cells[0].Value) ==  false)
                {
                    dataGridView1.Rows[i].Cells[0].Value=true;
                } else {
                    dataGridView1.Rows[i].Cells[0].Value = false;
                }
            }
        }
    }
}

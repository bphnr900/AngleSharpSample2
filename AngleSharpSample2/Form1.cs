using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AngleSharp.Dom.Html;
using AngleSharp.Extensions;
using AngleSharp;

namespace AngleSharpSample2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var keyword = txtKeyword.Text;

            var titles = await searchByGoogle(keyword);
            foreach (var title in titles)
            {
                textBox1.AppendText(title + Environment.NewLine);
            }
        }
        private async Task<System.Collections.Generic.IEnumerable<string>> searchByGoogle(string searchKeyword)
        {
            //セットアップ
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);

            //検索ページを開く
            await context.OpenAsync("https://www.google.co.jp/");

            //指定したキーワード(test)で検索を行う
            await context.Active.QuerySelector<IHtmlFormElement>("form").SubmitAsync(new
            {
                q = searchKeyword,
            });

            //検索結果のタイトル一覧を取得する
            var tags = context.Active.QuerySelectorAll("h3");
            var titles = tags.Select(m => m.TextContent);

            return titles;
        }
    }

}

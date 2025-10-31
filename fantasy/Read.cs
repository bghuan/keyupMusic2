using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace keyupMusic2.fantasy
{
    public partial class Read : Form
    {
        private System.Timers.Timer timer_Every100ms;
        public Read()
        {
            InitializeComponent();

            Size = new Size(2000, 1200); // 恢复默认大小
            StartPosition = FormStartPosition.WindowsDefaultBounds;
            Location = new Point(220, 220); // 恢复默认位置
            TransparencyKey = Color.Red;
            BackColor = Color.Red;
            textBox1.BackColor = Color.Red;
            textBox1.BorderStyle = BorderStyle.None;
            Text = "Read";

            Shown += (s, e) => webView21.Focus();
            InitializeAsync();
        }
        private string url = "https://www.qidian.com/book/1035351824/";
        // 设定你想要的缩放比例
        private double myZoom = 1.75;
        private async Task InitializeAsync()
        {
            await webView21.EnsureCoreWebView2Async(null);
            webView21.DefaultBackgroundColor = Color.Transparent;

            url = Common.ConfigValue("aaa");
            webView21.CoreWebView2.Navigate(url);

            webView21.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.F3 || e.KeyCode == Keys.F9)
                {
                    e.Handled = true; // 阻止继续传递
                }
                if (e.KeyCode == Keys.F2)
                {
                    myZoom = 1.75;
                    //Common.LossScale();
                    e.Handled = true; // 阻止继续传递
                }
                if (e.KeyCode == Keys.F1)
                {
                    myZoom = 1.25;
                    //TopMost = !TopMost;
                    e.Handled = true; // 阻止继续传递
                }
                if (e.KeyCode == Keys.F11)
                {
                    if (FormBorderStyle != FormBorderStyle.None)
                    {
                        FormBorderStyle = FormBorderStyle.None;
                        TopMost = true;
                        //TransparencyKey = Color.Fuchsia;
                        //BackColor = Color.Fuchsia;
                        WindowState = FormWindowState.Normal;
                        Bounds = Screen.FromControl(this).Bounds;
                        textBox1.Hide();
                    }
                    else
                    {
                        FormBorderStyle = FormBorderStyle.Sizable;
                        TopMost = false;
                        //TransparencyKey = Color.Red;
                        //BackColor = Color.Red;
                        WindowState = FormWindowState.Normal;
                        Size = new Size(2000, 1200); // 恢复默认大小
                        StartPosition = FormStartPosition.WindowsDefaultBounds;
                        Location = new Point(220, 220); // 恢复默认位置
                        textBox1.Show();
                    }
                    e.Handled = true; // 阻止继续传递
                }
            };

            webView21.CoreWebView2.NavigationStarting += (s, e) =>
            {
                //webView21.Visible = false; // 跳转时先隐藏
            };
            webView21.CoreWebView2.NavigationCompleted += async (s, e) =>
            {
                if (webView21.CoreWebView2.Source.IndexOf("qidia") > 0)
                    webView21.ZoomFactor = 1.25;
                else
                    webView21.ZoomFactor = myZoom;
                //webView21.Visible = true;      // 再显示，避免缩放跳变
                string currentUrl = webView21.CoreWebView2.Source;
                Common.ConfigValue("aaa", currentUrl);
                string aaaa = @"
function setBgTransparent(node, depth) {
    if (!node || depth > 10) return;
    if (node.style) node.style.backgroundColor = 'transparent';
    if (node.style) node.style.color = '#f3f3f3';
    if (node.style && localStorage.textcolor) node.style.color = localStorage.textcolor;
    if (node.children) {
        for (let i = 0; i < node.children.length; i++) {
            setBgTransparent(node.children[i], depth + 1);
        }
    }
}
function bottomLine() {
    const topp = (document.body.scrollHeight - window.innerHeight) + 'px';
    const line = document.createElement('div');
    Object.assign(line.style, {
        position: 'absolute',
        top: topp,
        width: '100%',
        height: '1px',
        borderTop: '1px dashed #eee',
        zIndex: 999999,
    });
    document.body.appendChild(line);
}

setBgTransparent(document.documentElement, 1);
setInterval(()=>{setBgTransparent(document.documentElement, 1)}, 1000)

const style = document.createElement('style');
style.textContent = `::-webkit-scrollbar { display: none !important; }`;
document.head.appendChild(style);

document.body.addEventListener(""keyup"", event => { if (event.key === '.') (localStorage.textcolor = localStorage.textcolor == '#f3f3f3' ? '#000' : '#f3f3f3') })

if (location.href.indexOf('qidian') > 0) {
    var asddsadas = () => {
        setBgTransparent(document.body, 1);
        try { for (let obj of document.getElementsByClassName('page-ops')) { obj.style.display = 'none' } } catch { }
        try { document.getElementById('j-topOpBox').style.display = 'none' } catch { }
        try { document.getElementById('left-container').style.display = 'none' } catch { }
        document.querySelectorAll('.noise-bg').forEach(o => { o.style.backgroundImage = ""none"" })
        try { document.querySelector('#r-menu').style.display = 'none' } catch { }
        document.querySelector('#reader-content').style.marginLeft = '50px'
    }
    setInterval(asddsadas, 1000)
}

if (location.href.indexOf('dingdiange') > 0) {
    apprecom1.style.display = 'none'; apprecom2.style.display = 'none'; document.querySelector('.reader_mark1').style.display = 'none'; document.querySelector('.reader_mark0').style.display = 'none'; box_con.style.border = 'none';
    box_con.children[4].style.display = 'none'
    bottomLine()
    var asdddd = (event) => {
        if (Math.ceil(window.pageYOffset) + Math.ceil(window.innerHeight) + 100 >= document.body.scrollHeight) {
            document.querySelectorAll('.bottem a')[3].click()
            event.stopPropagation();
        }
    }
    document.body.addEventListener(""keydown"", event => { if (event.key == ' ' || event.key == 'PageDown') { asdddd(event) } })
}


if (location.href.indexOf('biqugewenx') > 0) {
    document.body.addEventListener(""keyup"", event => { console.log(event.key); if (event.key === 'ArrowRight') { A3.click() } })

    if (content) content.removeChild(content.lastChild)
    if (content) content.removeChild(content.lastChild)

    if (document.querySelectorAll('.pc, .hotbook, .tuibook, .mobile, .footer')) document.querySelectorAll('.pc, .hotbook, .tuibook, .mobile, .footer').forEach(o => o.style.display = 'none')
    if (document.querySelector('.box_con')) document.querySelector('.box_con').style.border = 'none'
}

if (location.href.indexOf('92yanqing') > 0) {
    var asdddd = (event) => {
        if (Math.ceil(window.pageYOffset) + Math.ceil(window.innerHeight) + 100 >= document.body.scrollHeight) {
            next_url.click()
            event.stopPropagation();
        }
    }
    document.body.addEventListener(""keydown"", event => { if (event.key == ' ' || event.key == 'PageDown') { asdddd(event) } })
    document.querySelector('.read').style.padding='20px'
var list1=['justify','ad-second','audio']
    booktxt.childNodes.forEach(o=>{list1.forEach(zzz=>{if(o.innerText.indexOf(zzz)>=0)booktxt.removeChild(o);});o.style.margin = '0';})
document.querySelectorAll('.readvote')[0].style.display='none'
document.querySelectorAll('.readvote')[1].style.display='none'
scroll(0,300)
    bottomLine()
}


var pagedownnext = (event,ele) => {
        if (Math.ceil(window.pageYOffset) + Math.ceil(window.innerHeight) + 100 >= document.body.scrollHeight) {
            ele.click()
            event.stopPropagation();
        }
    }

if (location.href.indexOf('x36sfw1') > 0) {
content.innerText=content.innerText.substr(0,content.innerText.indexOf('无尽的昏迷过后'))
content.innerText = content.innerText.replace(/谷.*?<\/span>/g, '');
document.querySelector('.box_con').style.border='none'
document.body.addEventListener(""keydown"", event => { if (event.key == ' ' || event.key == 'PageDown') { pagedownnext(event,document.querySelector('.next')) } })
}
bottomLine()
";
                await webView21.ExecuteScriptAsync(aaaa);
                if (!webView21.Focused) webView21.Focus();
            };
            textBox1.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    if (!string.IsNullOrWhiteSpace(textBox1.Text))
                    {
                        webView21.CoreWebView2?.Navigate(textBox1.Text);
                    }
                }
            };
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            webView21.Focus();
        }

        protected override void Dispose(bool disposing)
        {
            timer_Every100ms.Stop();
            base.Dispose(disposing);
        }

        private void Read_Load(object sender, EventArgs e)
        {
            webView21.Focus();
            Every100ms();
        }
        public void Every100ms()
        {
            timer_Every100ms = new(1000);
            timer_Every100ms.Elapsed += (sender, e) =>
            {
                if (Common.Position.Y == 0) return;
                if (Common.Position.X == 0 && webView21.Visible)
                {
                    webView21.Invoke(() => Hide());
                }
                if (Common.Position.X != 0 && !webView21.Visible)
                {
                    webView21.Invoke(() => Show());
                }
            };
            timer_Every100ms.AutoReset = true;
            timer_Every100ms.Start();
        }
    }
}

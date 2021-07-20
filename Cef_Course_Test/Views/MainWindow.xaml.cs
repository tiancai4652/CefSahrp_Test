using Cef_Course_Test.Base;
using CefSharp;
using System.Windows;

namespace Cef_Course_Test.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string address = "http://localhost/preview/index.html?fileJson=https://mv.xesimg.com/XESlides/slidev2/slide_238629/1626168865100.json&env=3";

        public MainWindow()
        {
            InitializeComponent();
            browser.BrowserSettings.DefaultEncoding = "utf8";
            browser.Address = address;
            browser.LoadingStateChanged += (sender, args) =>
            {
                if (args.IsLoading == false)
                {
                  
                }
            };
            browser.FrameLoadEnd += (sender, args) =>
            {
                if (args.Frame.IsMain)
                {
                  
                }
            };
            InitJS();
        }


        private void Window_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key.Equals(System.Windows.Input.Key.F12))
            {
                browser.ShowDevTools();
            }
        }

        void InitJS()
        {
            TestClass t = new TestClass();
            t.jsInvok = new TestClass.jsInvokeEventHandle(this.ON_JsInvok);
            browser.JavascriptObjectRepository.Register("TestClass", t, BindingOptions.DefaultBinder);
        }

        bool isCanComunicate = false;
        int currentPageIndex = 0;
        bool NextRunAnimationOrPage = false;
        bool PreviousRunAnimationOrPage = false;


        private void ON_JsInvok(string msg)
        {
            if (msg == "") return;
            serialize<jsInvokeData> ser = new serialize<jsInvokeData>();
            jsInvokeData invokData = ser.getJsonContract(msg);
            if (invokData.type == "tplLoadComplete")
            {
                isCanComunicate = true;
            }
            else if (invokData.type == "currentPageIndexChanged")
            {
                serialize<jsonData<currentPageIndexChange>> ser2 = new serialize<jsonData<currentPageIndexChange>>();
                jsonData<currentPageIndexChange> jsData = ser2.getJsonContract(msg);
                if (currentPageIndex != jsData.data.currentPageIndex)
                {
                    currentPageIndex = jsData.data.currentPageIndex;
                    //d_currentPageIndexChanged(jsData.data.pageIndex);
                }
            }
            //else if (invokData.type == "webScrollPositionChanged")
            //{
            //    serialize<jsonData<WebPositionC>> ser2 = new serialize<jsonData<WebPositionC>>();
            //    jsonData<WebPositionC> jsData = ser2.getJsonContract(msg);
            //    d_webScrollPositionChanged(jsData.data.x, jsData.data.y);
            //}
            //else if (invokData.type == "h5PageTestAnswer")
            //{
            //    serialize<jsonData<H5PageAnswerC>> ser2 = new serialize<jsonData<H5PageAnswerC>>();
            //    jsonData<H5PageAnswerC> jsData = ser2.getJsonContract(msg);
            //    d_slideGetPageAnswer(jsData.data.index, jsData.data.answer);
            //}
            //else if (invokData.type == "getSlideInfo")
            //{
            //    serialize<jsonData<SlideInfoC>> ser2 = new serialize<jsonData<SlideInfoC>>();
            //    jsonData<SlideInfoC> jsData = ser2.getJsonContract(msg);
            //    m_slideInfo = jsData.data;
            //    Config.getInstance().g_slideIsReady = true;
            //    createEmptyPage(m_slideInfo.totalPageNum);
            //}
            else if (invokData.type == "isLastAniamtion")
            {
                serialize<jsonData<isLastAnimationC>> ser2 = new serialize<jsonData<isLastAnimationC>>();
                jsonData<isLastAnimationC> jsData = ser2.getJsonContract(msg);
                NextRunAnimationOrPage = !jsData.data.isLastAniamtion;
            }
            else if (invokData.type == "isFirstAniamtion")
            {
                serialize<jsonData<isLastAnimationC>> ser2 = new serialize<jsonData<isLastAnimationC>>();
                jsonData<isLastAnimationC> jsData = ser2.getJsonContract(msg);
                PreviousRunAnimationOrPage = !jsData.data.isFirstAniamtion;
            }
        }

        private void Up_Click(object sender, RoutedEventArgs e)
        {
            if (currentPageIndex > 0 && isCanComunicate)
            {
                if (PreviousRunAnimationOrPage)
                {
                    PreviousAnimation();
                }
                else
                {
                    PreviousPage();
                }
            }
        }

        void NextAnimation()
        {
            browser.ExecuteScriptAsync("playNextAction()");
        }

        void NextPage()
        {
            browser.ExecuteScriptAsync($"setCurrentPage({currentPageIndex + 1})");
        }

        void PreviousAnimation()
        {
            browser.ExecuteScriptAsync("playPreviousAction()");
        }

        void PreviousPage()
        {
            if (currentPageIndex > 0)
            {
                browser.ExecuteScriptAsync($"setCurrentPage({currentPageIndex - 1})");
            }
        }

        private void Down_Click(object sender, RoutedEventArgs e)
        {
            if (isCanComunicate)
            {
                if (NextRunAnimationOrPage)
                {
                    NextAnimation();
                }
                else
                {
                    NextPage();
                }
            }
        }
    }
}

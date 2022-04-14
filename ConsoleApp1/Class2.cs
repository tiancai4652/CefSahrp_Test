using CefSharp;
using CefSharp.DevTools.Page;
using CefSharp.Internals;
using CefSharp.OffScreen;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Class2
    {
        public Class2(ChromiumWebBrowser chromiumWebBrowser)
        {
            browser = chromiumWebBrowser;
        }

        ChromiumWebBrowser browser;
        /// <summary>
        /// Capture page screenshot.
        /// </summary>
        /// <param name="format">Image compression format (defaults to png).</param>
        /// <param name="quality">Compression quality from range [0..100] (jpeg only).</param>
        /// <param name="viewport">view port to capture, if not null the browser will be resized to match the width/height.</param>
        /// <returns>A task that can be awaited to obtain the screenshot as a byte[].</returns>
        public async Task<byte[]> CaptureScreenshotAsync( int? quality = null, Viewport viewport = null)
        {
            //ThrowExceptionIfDisposed();
            //ThrowExceptionIfBrowserNotInitialized();

            using (var devToolsClient = browser.GetDevToolsClient())
            {
                if (viewport != null)
                {
                    await ResizeAsync((int)viewport.Width, (int)viewport.Height, (float)viewport.Scale).ConfigureAwait(continueOnCapturedContext: false);
                }

                //https://bitbucket.org/chromiumembedded/cef/issues/3103/offscreen-capture-screenshot-with-devtools
                //CEF OSR mode doesn't set the size internally when CaptureScreenShot is called with a clip param specified, so
                //we must manually resize our view.
                var response = await devToolsClient.Page.CaptureScreenshotAsync(null, quality, fromSurface: true).ConfigureAwait(continueOnCapturedContext: false);

                return response.Data;
            }
        }

        private Size size = new Size(1366, 768);
        /// <summary>
        /// Get/set the size of the Chromium viewport, in pixels.
        /// This also changes the size of the next rendered bitmap.
        /// </summary>
        /// <value>The size.</value>
        public Size Size
        {
            get { return size; }
            set
            {
                if (size != value)
                {
                    size = value;

                    if (browser.IsBrowserInitialized)
                    {
                        browser.GetBrowserHost().WasResized();
                    }
                }
            }
        }

        private event EventHandler<OnPaintEventArgs> AfterPaint;
        /// <summary>
        /// Resize the browser
        /// </summary>
        /// <param name="width">width</param>
        /// <param name="height">height</param>
        /// <param name="deviceScaleFactor">device scale factor</param>
        /// <returns>A task that can be awaited and will resolve when the desired size is achieved.</returns>
        /// <remarks>
        /// The current implementation is fairly symplistic, it simply resizes the browser
        /// and resolves the task when the browser starts painting at the desired size.
        /// </remarks>
        public Task ResizeAsync(int width, int height, float? deviceScaleFactor = null)
        {


            if (size.Width == width && size.Height == height && deviceScaleFactor == null)
            {
                return Task.FromResult(true);
            }

            var tcs = new TaskCompletionSource<bool>();
            EventHandler<OnPaintEventArgs> handler = null;

            handler = (s, e) =>
            {
                if (e.Width == width && e.Height == height)
                {
                    AfterPaint -= handler;

                    tcs.TrySetResultAsync(true);
                }
            };

            AfterPaint += handler;

            //Only set the value if not null otherwise
            //a call to NotifyScreenInfoChanged will be made.
            if (deviceScaleFactor.HasValue)
            {
                DeviceScaleFactor = deviceScaleFactor.Value;
            }
            Size = new Size(width, height);

            return tcs.Task;
        }

        private float deviceScaleFactor = 1.0f;
        /// <summary>
        /// Device scale factor. Specifies the ratio between physical and logical pixels.
        /// </summary>
        public float DeviceScaleFactor
        {
            get { return deviceScaleFactor; }
            set
            {
                deviceScaleFactor = value;

                if (browser.IsBrowserInitialized)
                {
                    browser.GetBrowserHost().NotifyScreenInfoChanged();
                }
            }
        }
    }
}

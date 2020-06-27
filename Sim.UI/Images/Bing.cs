using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Sim.UI.Images
{

    using Mvvm.Observers;

    public static class BingImage
    {
        public static readonly DependencyProperty UseBingImageProperty = DependencyProperty.RegisterAttached("UseBingImage", typeof(bool), typeof(BingImage), new PropertyMetadata(OnUseBingImageChanged));

        private static BitmapImage cachedBingImage;

        //public static Uri UriBing { get; set; }
        //public static Task<Uri> UriBing => GetCurrentBingImageUrl();
        public static Uri UriBing()
        {
            var tas_url = Task.Run(() => GetCurrentBingImageUrl()); 
            return tas_url.Result;  
        }
        
        private static async void OnUseBingImageChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var newValue = (bool)e.NewValue;
            var image = o as Image;
            var imageBrush = o as ImageBrush;

            if (!newValue || (image == null && imageBrush == null))
            {
                return;
            }

            if (cachedBingImage == null)
            {
                var url = await GetCurrentBingImageUrl();
                if (url != null)
                {
                    cachedBingImage = new BitmapImage(url);
                }
            }

            if (cachedBingImage != null)
            {
                if (image != null)
                {
                    image.Source = cachedBingImage;
                }
                else if (imageBrush != null)
                {
                    imageBrush.ImageSource = cachedBingImage;
                }
            }
        }

        private static async Task<Uri> GetCurrentBingImageUrl()
        {
            var client = new HttpClient();
            var result = await client.GetAsync("http://www.bing.com/hpimagearchive.aspx?format=xml&idx=0&n=2&mbl=1&mkt=en-ww");
            if (result.IsSuccessStatusCode)
            {
                using (var stream = await result.Content.ReadAsStreamAsync())
                {
                    var doc = XDocument.Load(stream);

                    var url = (string)doc.XPathSelectElement("/images/image/url");

                    //UriBing = new Uri(string.Format(CultureInfo.InvariantCulture, "http://bing.com{0}", url), UriKind.Absolute);

                    return new Uri(string.Format(CultureInfo.InvariantCulture, "http://bing.com{0}", url), UriKind.Absolute);
                }
            }

            return null;
        }


        public static bool GetUseBingImage(DependencyObject o)
        {
            return (bool)o.GetValue(UseBingImageProperty);
        }

        public static void SetUseBingImage(DependencyObject o, bool value)
        {
            o.SetValue(UseBingImageProperty, value);
        }
    }
}

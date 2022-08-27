using Apps.Models;
using System;
using System.ComponentModel;
using System.IO;
using System.Net;


[assembly: Xamarin.Forms.Dependency(typeof(AndroidDownloader))]
public class AndroidDownloader : IDownloader
{
    public event EventHandler<DownloadEventArgs> OnFileDownloaded;

    [Obsolete]
    public void DownloadFile(string url, string folder)
    {
        string pathToNewFolder = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, folder);
        Directory.CreateDirectory(pathToNewFolder);

        try
        {
            WebClient webClient = new WebClient();
            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
            string pathToNewFile = Path.Combine(pathToNewFolder, Path.GetFileName(url));
            webClient.DownloadFileAsync(new Uri(url), pathToNewFile);
        }
        catch (Exception)
        {
            if (OnFileDownloaded != null)
                OnFileDownloaded.Invoke(this, new DownloadEventArgs(false));
        }
    }

    private void Completed(object sender, AsyncCompletedEventArgs e)
    {
        if (e.Error != null)
        {
            if (OnFileDownloaded != null)
                OnFileDownloaded.Invoke(this, new DownloadEventArgs(false));
        }
        else
        {
            if (OnFileDownloaded != null)
                OnFileDownloaded.Invoke(this, new DownloadEventArgs(true));
        }
    }
}
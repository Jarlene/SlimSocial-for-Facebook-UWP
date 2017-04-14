﻿using System;
using System.IO;
using Windows.ApplicationModel.Resources;
using Windows.Graphics.Display;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace SlimSocial_for_Facebook
{
    public sealed partial class MainPage
    {
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        ApplicationView view = ApplicationView.GetForCurrentView();
        
        public MainPage()
        {
            InitializeComponent();

            // if full screen mode is activated, try to enter in full screen mode
            if (localSettings.Values.ContainsKey("fullScreen"))
                if ((bool)localSettings.Values["fullScreen"])
                    view.TryEnterFullScreenMode(); // Full screen mode activate
                else
                    view.ExitFullScreenMode();

            GoHome(); // Loads the main page

            // Handle the back button request (go to the previous page)
            SystemNavigationManager.GetForCurrentView().BackRequested += (s, a) =>
            {
                if (facebookWebView.CanGoBack)
                {
                    facebookWebView.GoBack();
                    a.Handled = true; // This block the app closing
                }
            };
        }




        ////////////// METHODS
        private async void FacebookWebView_LoadCompleted(object sender, NavigationEventArgs e)
        {
            string cssToApply = "";

            // Check what to apply
            if (localSettings.Values.ContainsKey("blockTopBar"))
                if ((bool)localSettings.Values["blockTopBar"])
                {
                    cssToApply += "#header {position: fixed; z-index: 12; top: 0px;} #root {padding-top: 44px;} .item.more {position:fixed; bottom: 0px; text-align: center !important;}"; // .item.more show "show all message"
                    
                    var h = ApplicationView.GetForCurrentView().VisibleBounds.Height - 44;
                    float density = DisplayInformation.GetForCurrentView().LogicalDpi;
                    int barHeight = (int)(h / density);

                    cssToApply += ".flyout {max-height:" + barHeight + "px; overflow-y:scroll;}"; // Without this doesn't scroll
                }
            if (localSettings.Values.ContainsKey("hideAdsAndPeopleYouMayKnow"))
                if ((bool)localSettings.Values["hideAdsAndPeopleYouMayKnow"]) cssToApply += "#m_newsfeed_stream article[data-ft*=\"\\\\\"ei\\\\\":\\\\\"\"] {display:none !important;}";
            if (localSettings.Values.ContainsKey("centerTextPosts"))
                if ((bool)localSettings.Values["centerTextPosts"]) cssToApply += "._5rgt._5msi {text-align: center;}";
            if (localSettings.Values.ContainsKey("addSpaceBetweenPosts"))
                if ((bool)localSettings.Values["addSpaceBetweenPosts"]) cssToApply += "article {margin-top: 50px !important;}";
            if (localSettings.Values.ContainsKey("darkTheme"))
                if ((bool)localSettings.Values["darkTheme"])
                {
                    commandBar.Background.SetValue(SolidColorBrush.ColorProperty, Colors.Black);
                    cssToApply += "body, #root, .storyStream, ._2v9s, ._4nmh, ._4u3j, ._35aq, ._146a, ._4g34, ._5pxb, ._55wq, ._53_-, ._55ws, ._u42, .jx-result, .jx-typeahead-results, ._56bt, ._52x7, ._vqv, ._4g33, ._5rgt, .popover_flyout, .flyout, #m_newsfeed_stream, ._55wo, ._3iln, .mentions-suggest, #header, ._xy, ._bgx, .acb, .acg, .aclb, .touch ._4g34, ._59e9, .nontouch ._5ui0, input[type=text], .acw, ._5up8, ._5kgn, .tlLinkContainer, .aps, .jewel .flyout .header, .appCenterCategorySelectorButton, .tlBody, #timelineBody, .timelineX, .timeline .feed, .timeline .tlPrelude, .timeline .tlFeedPlaceholder, .touch ._5c9u, .touch ._5ca9, .innerLink, ._5dy4, ._52x3, #m_group_stories_container, .albums, .subpage, ._uwu, ._uww, .scrollAreaBody, .al, .apl, .structuredPublisher, .groupChromeView, ._djv, ._bjg, ._5kgn, ._3f50, ._55wm, ._58f0 { background: #000 !important; /* the background */ } ._50cg._2ss { background: #000 !important; } .composerLinkText, .fcg { color: #d2d2d2 !important; } /* white text */ body, .touch ._2ya3, .composerTextSelected, .composerInput, .mentions-input, input[type=text], ._5001, .timeline .cover .profileName, .appListTitle, ._52jd, ._52jb, ._52jg, ._5qc3, .tlActorText, .tlLinkTitle, ._5379, ._5cqn, ._592p, ._3c9l, ._4yrh, .name, .btn, .upText, .tlLinkTitleOnly, ._5rgt, ._52x2, ._52jh, ._52ja, ._56bz, ._2tbu, ._1mwn, ._55sr, ._5t6r, ._1_oe, ._52lz, ._2l5v, .inputtext, .inputpassword, .touch, .touch tr, .touch input, .touch textarea, .touch .mfsm { color: #d2d2d2 !important; } .touch ._2ya3 { border-radius: 5px; padding: 5px; } /* blue link text */ a, .actor, .mfsl, .fcw, .title, .blueName, ._5aw4, ._vqv, ._5yll, ._5qc3, ._52lz, ._4nwe, ._27vp, ._ir4, ._5wsv, ._46pa { color: #DFEFF0 !important; } /* dark important */ .acy, .nontouch ._55mb .actor-link, .nontouch a.btnD, .inlineMedia.storyAttachment { background: #304702 !important; } .statusBox, ._5whq, ._56bt, .composerInput, .mentions-input, ._1svy, ._bji { background: #323232 !important; } .ufiBorder, ._5as0, ._5ef_, ._35aq { border-color: #555 !important; } /* buttons */ .button>a.touchable, .btn, .touch ._5c9u, ._2l5v, ._52x1, ._tn0, ._52ja, ._5lm6 { background: #323232 !important; } .flyout { border: 1px solid #fff !important; } /* context menu */ ._5c0e, ._5bn_ { background: #4e4e4e !important; } article, ._4o50, .acw, ._53_-, ._3wjp, ._usq, ._55wq, ._400s { border: 3px solid #383838 !important; border-radius: 4px; } ._1_oa, ._bmx, ._52x1 { border-bottom: 1px solid #383838 !important; } article h3 { color: #999 !important; } /* no border */ .aclb, ._53_-, ._52x6, ._52x1, ._2l5v, ._tn0, ._52ja, ._5lm6 { border-top: 0px; border-bottom: 0px; } ._59te.popoverOpen, ._59te.isActivem, ._59te { background: #000;/*topbar*/ border-bottom: 1px solid #444; border-right: 1px solid #444; }";
                }

            // Apply
            await facebookWebView.InvokeScriptAsync("eval", new[] { "javascript:function addStyleString(str) { var node = document.createElement('style'); node.innerHTML = " +
                "str; document.body.appendChild(node); } addStyleString('" + cssToApply + "');" });

            iconRotation.Stop(); // Stop rotation icon
        }
        
        private void FacebookWebView_NewWindowRequested(WebView sender, WebViewNewWindowRequestedEventArgs e)
        {
            if (e.Uri.AbsoluteUri.Contains(".gif") || e.Uri.AbsoluteUri.Contains("video")) // Is it a .gif or a video?
            { // Yes
                iconRotation.Begin(); // Start rotation icon
                facebookWebView.Navigate(e.Uri); // Open gif/video into a facebookWebView
                e.Handled = true; // This block stock browser
            }
        }
        
        private void FacebookWebView_NavigationFailed(object sender, WebViewNavigationFailedEventArgs e)
        {
            var loader = new ResourceLoader();
            string noConnection = loader.GetString("noConnection");
            facebookWebView.NavigateToString(noConnection);
        }










        ////////////// PERSONAL METHODS

        // To go to fb homepage
        private void GoHome()
        {
            iconRotation.Begin(); // Start rotation icon

            if (!localSettings.Values.ContainsKey("showRecentNews"))
                facebookWebView.Navigate(facebookWebView.Source);
            else
                if ((bool)localSettings.Values["showRecentNews"])
                facebookWebView.Navigate(new Uri(facebookWebView.Source + "?sk=h_chr")); // Load .facebook.com/home.php
            else
                facebookWebView.Navigate(new Uri(facebookWebView.Source + "?sk=h_nor")); // Load m.facebook.com
        }

        // To block the app closing
        private async void AppClosing()
        {
            var loader = new ResourceLoader();
            string exitDialog = loader.GetString("exitDialog");
            string exitTitleDialog = loader.GetString("exitTitleDialog");
            string yesCommand = loader.GetString("yesCommand");
            string noCommand = loader.GetString("noCommand");
            MessageDialog appClosing = new MessageDialog(exitDialog, exitTitleDialog);
            appClosing.Commands.Add(new UICommand(yesCommand) { Id = 0 });
            appClosing.Commands.Add(new UICommand(noCommand) { Id = 1 });

            appClosing.DefaultCommandIndex = 0;
            appClosing.CancelCommandIndex = 1;

            var result = await appClosing.ShowAsync();
            if ((int)result.Id == 0)
                Application.Current.Exit(); // Exit
        }








        //////////////EVENTS

        // Primary COMMAND BAR COMMAND
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            iconRotation.Begin(); // Start rotation icon
            facebookWebView.Refresh();
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        { GoHome(); }

        private async void TopButton_Click(object sender, RoutedEventArgs e)
        { await facebookWebView.InvokeScriptAsync("eval", new[] { "window.scrollTo(0,0);" }); }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (facebookWebView.CanGoBack)
                facebookWebView.GoBack(); // Go back
            else
                AppClosing();
        }

        // Secondary COMMAND BAR COMMAND
        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Uri url = facebookWebView.Source;
            var fileName = Path.GetFileName(url.LocalPath);
            var thumbnail = RandomAccessStreamReference.CreateFromUri(url);

            var remoteFile = await StorageFile.CreateStreamedFileFromUriAsync(fileName, url, thumbnail);
            await remoteFile.CopyAsync(KnownFolders.SavedPictures, fileName, NameCollisionOption.GenerateUniqueName);
            
            var loader = new ResourceLoader();
            string saveDialog = loader.GetString("saveDialog");
            string saveTitleDialog = loader.GetString("saveTitleDialog");
            string okCommand = loader.GetString("okCommand");
            MessageDialog savedPicture = new MessageDialog(saveDialog, saveTitleDialog);
            savedPicture.Commands.Add(new UICommand(okCommand));

            savedPicture.DefaultCommandIndex = 0;
            var result = await savedPicture.ShowAsync();
        }

        private async void BrowserButton_Click(object sender, RoutedEventArgs e)
        { await Launcher.LaunchUriAsync(facebookWebView.Source); }

        private async void ClearCacheButton_Click(object sender, RoutedEventArgs e)
        { await WebView.ClearTemporaryWebDataAsync(); }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        { Frame.Navigate(typeof(SettingPage)); }
    }
}
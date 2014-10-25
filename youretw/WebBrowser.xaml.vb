Imports System.Runtime.InteropServices
Public Class WebBrowser
    <DllImport("wininet.dll", CharSet:=CharSet.Auto, SetLastError:=True)> _
    Public Shared Function InternetSetCookie(ByVal lpszUrlName As String, ByVal lbszCookieName As String, ByVal lpszCookieData As String) As Boolean
    End Function
    Public Aspsession As String
    Public Ecpacid As String
    Dim _url As String
    Public Lesson As String
    Protected Overrides Sub OnSourceInitialized(ByVal e As EventArgs)
        InternetSetCookie("http://cn.myet.com/ElizaWeb/PersonalizedPage.aspx", "TargetServerKey", "CN1-LLabs; path=/")
        InternetSetCookie("http://cn.myet.com/ElizaWeb/PersonalizedPage.aspx", "APVersion", "5708; path=/")
        InternetSetCookie("http://cn.myet.com/ElizaWeb/PersonalizedPage.aspx", "LastSessionID", Aspsession + "; path=/")
        InternetSetCookie("http://cn.myet.com/ElizaWeb/PersonalizedPage.aspx", "ASP.NET_SessionId", Aspsession + "; path=/")
        InternetSetCookie("http://cn.myet.com/ElizaWeb/PersonalizedPage.aspx", "ContentProvider", "; path=/")
        InternetSetCookie("http://cn.myet.com/ElizaWeb/PersonalizedPage.aspx", "IsCookieSupported", "Y; path=/")
        InternetSetCookie("http://cn.myet.com/ElizaWeb/PersonalizedPage.aspx", "IsAllianceAccount", "N; path=/")
        InternetSetCookie("http://cn.myet.com/ElizaWeb/PersonalizedPage.aspx", "TmpRememberMyAccount", "N; path=/")
        InternetSetCookie("http://cn.myet.com/ElizaWeb/PersonalizedPage.aspx", "WPort", "49156; path=/")
        InternetSetCookie("http://cn.myet.com/ElizaWeb/PersonalizedPage.aspx", "RememberMyAccount", "N; path=/")
        InternetSetCookie("http://cn.myet.com/ElizaWeb/PersonalizedPage.aspx", "EcpACID", Ecpacid + "; path=/")
        InternetSetCookie("http://cn.myet.com/ElizaWeb/PersonalizedPage.aspx", "IsWebVersion", "true; path=/")
        Browser.Navigate("http://cn.myet.com/ElizaWeb/PersonalizedPage.aspx")
    End Sub

    Private Sub Browser_Navigated(sender As Object, e As NavigationEventArgs) Handles Browser.Navigated
        Try
            _url = Browser.Source.AbsoluteUri
            Omnibox.Text = _url
            If _url.Contains("LessonMainPageContainer.aspx") Then
                Lesson = _url.Substring(104, 16)
                Browser.Navigate("about:blank")
                Browser.Dispose()
                Close()
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub ButtonBack_Click(sender As Object, e As RoutedEventArgs) Handles ButtonBack.Click
        Try
            Browser.GoBack()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub ButtonForward_Click(sender As Object, e As RoutedEventArgs) Handles ButtonForward.Click
        Try
            Browser.GoForward()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub ButtonHome_Click(sender As Object, e As RoutedEventArgs) Handles ButtonHome.Click
        Browser.Navigate("http://cn.myet.com/ElizaWeb/PersonalizedPage.aspx")
    End Sub

    Private Sub ButtonRefresh_Click(sender As Object, e As RoutedEventArgs) Handles ButtonRefresh.Click
        Browser.Navigate(Omnibox.Text)
    End Sub

    Private Sub OmniboxEnter(sender As Object, e As KeyEventArgs) Handles Omnibox.KeyUp
        If e.Key = Key.Enter Then
            Browser.Navigate(Omnibox.Text)
        End If
    End Sub
End Class


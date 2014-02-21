Imports System.Net
Imports System.Runtime.InteropServices

Public Class WebBrowser
    <DllImport("wininet.dll", CharSet:=CharSet.Auto, SetLastError:=True)> _
    Public Shared Function InternetSetCookie(ByVal lpszUrlName As String, ByVal lbszCookieName As String, ByVal lpszCookieData As String) As Boolean
    End Function
    Public aspsession As String
    Public ecpacid As String
    Dim url As String
    Public lesson As String
    Protected Overrides Sub OnSourceInitialized(ByVal e As System.EventArgs)
        InternetSetCookie("http://cn.myet.com/ElizaWeb/PersonalizedPage.aspx", "TargetServerKey", "CN1-LLabs; path=/")
        InternetSetCookie("http://cn.myet.com/ElizaWeb/PersonalizedPage.aspx", "APVersion", "5705; path=/")
        InternetSetCookie("http://cn.myet.com/ElizaWeb/PersonalizedPage.aspx", "LastSessionID", aspsession + "; path=/")
        InternetSetCookie("http://cn.myet.com/ElizaWeb/PersonalizedPage.aspx", "ASP.NET_SessionId", aspsession + "; path=/")
        InternetSetCookie("http://cn.myet.com/ElizaWeb/PersonalizedPage.aspx", "ContentProvider", "; path=/")
        InternetSetCookie("http://cn.myet.com/ElizaWeb/PersonalizedPage.aspx", "IsCookieSupported", "Y; path=/")
        InternetSetCookie("http://cn.myet.com/ElizaWeb/PersonalizedPage.aspx", "IsAllianceAccount", "N; path=/")
        InternetSetCookie("http://cn.myet.com/ElizaWeb/PersonalizedPage.aspx", "TmpRememberMyAccount", "N; path=/")
        InternetSetCookie("http://cn.myet.com/ElizaWeb/PersonalizedPage.aspx", "WPort", "49156; path=/")
        InternetSetCookie("http://cn.myet.com/ElizaWeb/PersonalizedPage.aspx", "RememberMyAccount", "N; path=/")
        InternetSetCookie("http://cn.myet.com/ElizaWeb/PersonalizedPage.aspx", "EcpACID", ecpacid + "; path=/")
        InternetSetCookie("http://cn.myet.com/ElizaWeb/PersonalizedPage.aspx", "IsWebVersion", "true; path=/")
        Webb1.Navigate("http://cn.myet.com/ElizaWeb/PersonalizedPage.aspx")
    End Sub

    Private Sub Browser_Navigated(sender As Object, e As NavigationEventArgs) Handles Webb1.Navigated
        Try
            url = Webb1.Source.AbsoluteUri
            omnibox.Text = url
            If url.Contains("LessonMainPageContainer.aspx") Then
                lesson = url.Substring(109, 16)
                Webb1.Dispose()
                Me.Close()
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub buttonback_Click(sender As Object, e As RoutedEventArgs) Handles buttonback.Click
        Try
            Webb1.GoBack()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub buttonforward_Click(sender As Object, e As RoutedEventArgs) Handles buttonforward.Click
        Try
            Webb1.GoForward()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub buttonhome_Click(sender As Object, e As RoutedEventArgs) Handles buttonhome.Click
        Webb1.Navigate("http://cn.myet.com/ElizaWeb/PersonalizedPage.aspx")
    End Sub

    Private Sub buttonrefresh_Click(sender As Object, e As RoutedEventArgs) Handles buttonrefresh.Click
        Webb1.Navigate(omnibox.Text)
    End Sub

    Private Sub omniboxenter(sender As Object, e As KeyEventArgs) Handles omnibox.KeyUp
        If e.Key = Key.Enter Then
            Webb1.Navigate(omnibox.Text)
        End If
    End Sub
End Class


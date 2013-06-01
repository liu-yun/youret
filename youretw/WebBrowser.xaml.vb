Imports System.Net
Imports System.Runtime.InteropServices
Imports System.Windows.Threading

Public Class WebBrowser
    <DllImport("wininet.dll", CharSet:=CharSet.Auto, SetLastError:=True)> _
    Public Shared Function InternetSetCookie(ByVal lpszUrlName As String, ByVal lbszCookieName As String, ByVal lpszCookieData As String) As Boolean
    End Function
    Public aspsession As String
    Public ecpacid As String
    Dim url As String
    Public lesson As String
    WithEvents Timer As New DispatcherTimer()
    Protected Overrides Sub OnSourceInitialized(ByVal e As System.EventArgs)
        ' InternetSetCookie("http://cn.myet.com/ElizaWeb/PersonalizedPage.aspx", "LLang", "EN; path=/")
        InternetSetCookie("http://cn.myet.com/ElizaWeb/PersonalizedPage.aspx", "TargetServerKey", "CN1-LLabs; path=/")
        InternetSetCookie("http://cn.myet.com/ElizaWeb/PersonalizedPage.aspx", "APVersion", "5509; path=/")
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

        Timer.Interval = TimeSpan.FromMilliseconds(1000)
        Timer.Start()
    End Sub
    Private Sub dispatcherTimer_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles Timer.tick
        Try
            url = Webb1.Source.AbsoluteUri
            If url.Contains("LessonMainPageContainer.aspx") Then
                lesson = url.Substring(109, 16)
                Webb1.Navigate("about:blank")
                Me.Close()
            End If
        Catch ex As Exception
        End Try

    End Sub
End Class


Imports System.Windows.Forms

Module AppStart
    Sub Main()
        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)
        Application.Run(New ConnectSettingsForm())
    End Sub
End Module
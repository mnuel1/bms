<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ConnectSettingsForm
    Inherits System.Windows.Forms.Form

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Private components As System.ComponentModel.IContainer

    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.lblServer = New System.Windows.Forms.Label()
        Me.txtServer = New System.Windows.Forms.TextBox()
        Me.lblUser = New System.Windows.Forms.Label()
        Me.txtUser = New System.Windows.Forms.TextBox()
        Me.lblPassword = New System.Windows.Forms.Label()
        Me.txtPassword = New System.Windows.Forms.TextBox()
        Me.lblDatabase = New System.Windows.Forms.Label()
        Me.txtDatabase = New System.Windows.Forms.TextBox()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.pnlContainer = New System.Windows.Forms.Panel()
        Me.pnlContainer.SuspendLayout()
        Me.SuspendLayout()

        ' lblTitle
        Me.lblTitle.AutoSize = False
        Me.lblTitle.Font = New System.Drawing.Font("Comic Sans MS", 20.0!, System.Drawing.FontStyle.Bold)
        Me.lblTitle.ForeColor = System.Drawing.Color.FromArgb(51, 51, 51)
        Me.lblTitle.Location = New System.Drawing.Point(30, 30)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(440, 40)
        Me.lblTitle.Text = "Database Connection"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter

        ' lblServer
        Me.lblServer.AutoSize = True
        Me.lblServer.Font = New System.Drawing.Font("Comic Sans MS", 12.0!)
        Me.lblServer.ForeColor = System.Drawing.Color.FromArgb(51, 51, 51)
        Me.lblServer.Location = New System.Drawing.Point(30, 90)
        Me.lblServer.Name = "lblServer"
        Me.lblServer.Size = New System.Drawing.Size(60, 21)
        Me.lblServer.Text = "Server:"

        ' txtServer
        Me.txtServer.Location = New System.Drawing.Point(30, 120)
        Me.txtServer.Name = "txtServer"
        Me.txtServer.Size = New System.Drawing.Size(400, 30)
        Me.txtServer.Text = "127.0.0.1"
        Me.txtServer.Font = New System.Drawing.Font("Comic Sans MS", 12.0!)

        ' lblUser
        Me.lblUser.AutoSize = True
        Me.lblUser.Font = New System.Drawing.Font("Comic Sans MS", 12.0!)
        Me.lblUser.ForeColor = System.Drawing.Color.FromArgb(51, 51, 51)
        Me.lblUser.Location = New System.Drawing.Point(30, 170)
        Me.lblUser.Name = "lblUser"
        Me.lblUser.Size = New System.Drawing.Size(80, 21)
        Me.lblUser.Text = "User ID:"

        ' txtUser
        Me.txtUser.Location = New System.Drawing.Point(30, 200)
        Me.txtUser.Name = "txtUser"
        Me.txtUser.Size = New System.Drawing.Size(400, 30)
        Me.txtUser.Text = "root"
        Me.txtUser.Font = New System.Drawing.Font("Comic Sans MS", 12.0!)

        ' lblPassword
        Me.lblPassword.AutoSize = True
        Me.lblPassword.Font = New System.Drawing.Font("Comic Sans MS", 12.0!)
        Me.lblPassword.ForeColor = System.Drawing.Color.FromArgb(51, 51, 51)
        Me.lblPassword.Location = New System.Drawing.Point(30, 250)
        Me.lblPassword.Name = "lblPassword"
        Me.lblPassword.Size = New System.Drawing.Size(80, 21)
        Me.lblPassword.Text = "Password:"

        ' txtPassword
        Me.txtPassword.Location = New System.Drawing.Point(30, 280)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.Size = New System.Drawing.Size(400, 30)
        Me.txtPassword.UseSystemPasswordChar = True
        Me.txtPassword.Font = New System.Drawing.Font("Comic Sans MS", 12.0!)

        ' lblDatabase
        Me.lblDatabase.AutoSize = True
        Me.lblDatabase.Font = New System.Drawing.Font("Comic Sans MS", 12.0!)
        Me.lblDatabase.ForeColor = System.Drawing.Color.FromArgb(51, 51, 51)
        Me.lblDatabase.Location = New System.Drawing.Point(30, 330)
        Me.lblDatabase.Name = "lblDatabase"
        Me.lblDatabase.Size = New System.Drawing.Size(80, 21)
        Me.lblDatabase.Text = "Database:"

        ' txtDatabase
        Me.txtDatabase.Location = New System.Drawing.Point(30, 360)
        Me.txtDatabase.Name = "txtDatabase"
        Me.txtDatabase.Size = New System.Drawing.Size(400, 30)
        Me.txtDatabase.Text = "bms"
        Me.txtDatabase.Font = New System.Drawing.Font("Comic Sans MS", 12.0!)

        ' btnSave
        Me.btnSave.Location = New System.Drawing.Point(150, 420)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(120, 40)
        Me.btnSave.Text = "Connect"
        Me.btnSave.Font = New System.Drawing.Font("Comic Sans MS", 12.0!, System.Drawing.FontStyle.Bold)

        ' btnCancel
        Me.btnCancel.Location = New System.Drawing.Point(310, 420)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(120, 40)
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.Font = New System.Drawing.Font("Comic Sans MS", 12.0!, System.Drawing.FontStyle.Bold)

        ' lblStatus
        Me.lblStatus.AutoSize = False
        Me.lblStatus.Font = New System.Drawing.Font("Comic Sans MS", 12.0!)
        Me.lblStatus.ForeColor = System.Drawing.Color.FromArgb(51, 51, 51)
        Me.lblStatus.Location = New System.Drawing.Point(30, 480)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(440, 40)
        Me.lblStatus.Text = ""
        Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.TopLeft
        Me.lblStatus.AutoEllipsis = True

        ' pnlContainer
        Me.pnlContainer.BackColor = System.Drawing.Color.White
        Me.pnlContainer.Controls.Add(Me.lblTitle)
        Me.pnlContainer.Controls.Add(Me.lblServer)
        Me.pnlContainer.Controls.Add(Me.txtServer)
        Me.pnlContainer.Controls.Add(Me.lblUser)
        Me.pnlContainer.Controls.Add(Me.txtUser)
        Me.pnlContainer.Controls.Add(Me.lblPassword)
        Me.pnlContainer.Controls.Add(Me.txtPassword)
        Me.pnlContainer.Controls.Add(Me.lblDatabase)
        Me.pnlContainer.Controls.Add(Me.txtDatabase)
        Me.pnlContainer.Controls.Add(Me.btnSave)
        Me.pnlContainer.Controls.Add(Me.btnCancel)
        Me.pnlContainer.Controls.Add(Me.lblStatus)
        Me.pnlContainer.Location = New System.Drawing.Point(0, 0)
        Me.pnlContainer.Name = "pnlContainer"
        Me.pnlContainer.Size = New System.Drawing.Size(500, 600)
        Me.pnlContainer.Padding = New System.Windows.Forms.Padding(10)
        Me.pnlContainer.Anchor = AnchorStyles.None

        ' ConnectSettingsForm
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(245, 246, 245)
        Me.Controls.Add(Me.pnlContainer)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = True
        Me.MinimizeBox = True
        Me.Name = "ConnectSettingsForm"
        Me.Text = "Database Connection"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.pnlContainer.ResumeLayout(False)
        Me.pnlContainer.PerformLayout()
        Me.ResumeLayout(False)
    End Sub

    Friend WithEvents lblTitle As Label
    Friend WithEvents lblServer As Label
    Friend WithEvents txtServer As TextBox
    Friend WithEvents lblUser As Label
    Friend WithEvents txtUser As TextBox
    Friend WithEvents lblPassword As Label
    Friend WithEvents txtPassword As TextBox
    Friend WithEvents lblDatabase As Label
    Friend WithEvents txtDatabase As TextBox
    Friend WithEvents btnSave As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents lblStatus As Label
    Friend WithEvents pnlContainer As Panel
End Class
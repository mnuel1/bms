<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Login
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
        Me.lblUsername = New System.Windows.Forms.Label()
        Me.usernameTextBox = New System.Windows.Forms.TextBox()
        Me.lblPassword = New System.Windows.Forms.Label()
        Me.passwordTextbox = New System.Windows.Forms.TextBox()
        Me.loginBtn = New System.Windows.Forms.Button()
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
        Me.lblTitle.Text = "Login"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter

        ' lblUsername
        Me.lblUsername.AutoSize = True
        Me.lblUsername.Font = New System.Drawing.Font("Comic Sans MS", 12.0!)
        Me.lblUsername.ForeColor = System.Drawing.Color.FromArgb(51, 51, 51)
        Me.lblUsername.Location = New System.Drawing.Point(30, 90)
        Me.lblUsername.Name = "lblUsername"
        Me.lblUsername.Size = New System.Drawing.Size(80, 21)
        Me.lblUsername.Text = "Username:"

        ' usernameTextBox
        Me.usernameTextBox.Location = New System.Drawing.Point(30, 120)
        Me.usernameTextBox.Name = "usernameTextBox"
        Me.usernameTextBox.Size = New System.Drawing.Size(400, 30)
        Me.usernameTextBox.Font = New System.Drawing.Font("Comic Sans MS", 12.0!)

        ' lblPassword
        Me.lblPassword.AutoSize = True
        Me.lblPassword.Font = New System.Drawing.Font("Comic Sans MS", 12.0!)
        Me.lblPassword.ForeColor = System.Drawing.Color.FromArgb(51, 51, 51)
        Me.lblPassword.Location = New System.Drawing.Point(30, 170)
        Me.lblPassword.Name = "lblPassword"
        Me.lblPassword.Size = New System.Drawing.Size(80, 21)
        Me.lblPassword.Text = "Password:"

        ' passwordTextbox
        Me.passwordTextbox.Location = New System.Drawing.Point(30, 200)
        Me.passwordTextbox.Name = "passwordTextbox"
        Me.passwordTextbox.Size = New System.Drawing.Size(400, 30)
        Me.passwordTextbox.UseSystemPasswordChar = True
        Me.passwordTextbox.Font = New System.Drawing.Font("Comic Sans MS", 12.0!)

        ' loginBtn
        Me.loginBtn.Location = New System.Drawing.Point(150, 260)
        Me.loginBtn.Name = "loginBtn"
        Me.loginBtn.Size = New System.Drawing.Size(120, 40)
        Me.loginBtn.Text = "Login"
        Me.loginBtn.Font = New System.Drawing.Font("Comic Sans MS", 12.0!, System.Drawing.FontStyle.Bold)

        ' btnCancel
        Me.btnCancel.Location = New System.Drawing.Point(310, 260)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(120, 40)
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.Font = New System.Drawing.Font("Comic Sans MS", 12.0!, System.Drawing.FontStyle.Bold)

        ' lblStatus
        Me.lblStatus.AutoSize = False
        Me.lblStatus.Font = New System.Drawing.Font("Comic Sans MS", 12.0!)
        Me.lblStatus.ForeColor = System.Drawing.Color.FromArgb(51, 51, 51)
        Me.lblStatus.Location = New System.Drawing.Point(30, 320)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(440, 40)
        Me.lblStatus.Text = ""
        Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.TopLeft
        Me.lblStatus.AutoEllipsis = True

        ' pnlContainer
        Me.pnlContainer.BackColor = System.Drawing.Color.White
        Me.pnlContainer.Controls.Add(Me.lblTitle)
        Me.pnlContainer.Controls.Add(Me.lblUsername)
        Me.pnlContainer.Controls.Add(Me.usernameTextBox)
        Me.pnlContainer.Controls.Add(Me.lblPassword)
        Me.pnlContainer.Controls.Add(Me.passwordTextbox)
        Me.pnlContainer.Controls.Add(Me.loginBtn)
        Me.pnlContainer.Controls.Add(Me.btnCancel)
        Me.pnlContainer.Controls.Add(Me.lblStatus)
        Me.pnlContainer.Location = New System.Drawing.Point(0, 0)
        Me.pnlContainer.Name = "pnlContainer"
        Me.pnlContainer.Size = New System.Drawing.Size(500, 600)
        Me.pnlContainer.Padding = New System.Windows.Forms.Padding(10)
        Me.pnlContainer.Anchor = AnchorStyles.None

        ' Login
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(245, 246, 245)
        Me.Controls.Add(Me.pnlContainer)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = True
        Me.MinimizeBox = True
        Me.Name = "Login"
        Me.Text = "Login"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.pnlContainer.ResumeLayout(False)
        Me.pnlContainer.PerformLayout()
        Me.ResumeLayout(False)
    End Sub

    Friend WithEvents lblTitle As Label
    Friend WithEvents lblUsername As Label
    Friend WithEvents usernameTextBox As TextBox
    Friend WithEvents lblPassword As Label
    Friend WithEvents passwordTextbox As TextBox
    Friend WithEvents loginBtn As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents lblStatus As Label
    Friend WithEvents pnlContainer As Panel
End Class
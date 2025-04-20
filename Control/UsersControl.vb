Imports System.Data
Imports MySql.Data.MySqlClient
Imports System.Drawing

Public Class UsersControl
    Inherits UserControl

    Private usersDataGrid As DataGridView
    Private txtFullName, txtUsername, txtPassword, txtEmail, txtPhone As TextBox
    Private cmbRole, cmbStatus As ComboBox
    Private panelControls As Panel
    Private currentUserID As Integer = -1
    Private searchBox As TextBox

    Public Sub New()
        InitializeComponent()
        SetupLayout()
        LoadUsers()
    End Sub

    Private Sub SetupLayout()
        ' Set control background
        Me.BackColor = Color.FromArgb(240, 240, 240)

        ' Header Panel with title and search
        Dim headerPanel As New Panel With {
            .Dock = DockStyle.Top,
            .Height = 60,
            .BackColor = Color.FromArgb(41, 128, 185)
        }

        Dim lblTitle As New Label With {
            .Text = "User Management",
            .Font = New Font("Segoe UI", 16, FontStyle.Bold),
            .ForeColor = Color.White,
            .AutoSize = True,
            .Location = New Point(15, 15)
        }

        searchBox = New TextBox With {
            .Width = 250,
            .Height = 30,
            .Font = New Font("Segoe UI", 10),
            .Anchor = AnchorStyles.Right Or AnchorStyles.Top,
            .Location = New Point(headerPanel.Width - 270, 15)
        }

        AddHandler searchBox.TextChanged, AddressOf SearchBox_TextChanged

        Dim searchIcon As New Label With {
            .Text = "🔍",
            .Font = New Font("Segoe UI", 12),
            .ForeColor = Color.Gray,
            .AutoSize = True,
            .Anchor = AnchorStyles.Right Or AnchorStyles.Top,
            .Location = New Point(headerPanel.Width - 290, 18)
        }

        headerPanel.Controls.Add(lblTitle)
        headerPanel.Controls.Add(searchBox)
        headerPanel.Controls.Add(searchIcon)

        ' DataGridView with modern styling
        usersDataGrid = New DataGridView With {
            .Dock = DockStyle.Fill,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            .BorderStyle = BorderStyle.None,
            .BackgroundColor = Color.White,
            .GridColor = Color.FromArgb(224, 224, 224),
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            .AllowUserToAddRows = False,
            .ReadOnly = True,
            .RowHeadersVisible = False
        }

        ' DataGridView Styling
        usersDataGrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(87, 166, 245)
        usersDataGrid.DefaultCellStyle.Font = New Font("Segoe UI", 9)
        usersDataGrid.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        usersDataGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240)
        usersDataGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(70, 70, 70)
        usersDataGrid.ColumnHeadersHeight = 40
        usersDataGrid.RowTemplate.Height = 35

        AddHandler usersDataGrid.CellClick, AddressOf usersDataGrid_CellClick

        ' Input Controls Container Panel
        panelControls = New Panel With {
            .Dock = DockStyle.Bottom,
            .Height = 270,
            .BackColor = Color.White,
            .Padding = New Padding(20)
        }

        ' Add shadow effect (border)
        AddHandler panelControls.Paint, Sub(sender As Object, e As PaintEventArgs)
                                            Dim topBorder As New Pen(Color.FromArgb(200, 200, 200))
                                            e.Graphics.DrawLine(topBorder, 0, 0, panelControls.Width, 0)
                                        End Sub

        ' Input Fields
        txtFullName = CreateStyledTextBox("Full Name")
        txtUsername = CreateStyledTextBox("Username")
        txtPassword = CreateStyledTextBox("Password")
        txtPassword.PasswordChar = "*"c
        txtEmail = CreateStyledTextBox("Email")
        txtPhone = CreateStyledTextBox("Phone Number")

        cmbRole = CreateStyledComboBox()
        cmbRole.Items.AddRange({"Administrator", "Staff", "Accountant/Clerk"})

        cmbStatus = CreateStyledComboBox()
        cmbStatus.Items.AddRange({"Active", "Inactive"})

        ' Button Panel
        Dim buttonPanel As New FlowLayoutPanel With {
            .FlowDirection = FlowDirection.RightToLeft,
            .Dock = DockStyle.Bottom,
            .Height = 50,
            .Padding = New Padding(0, 10, 20, 0)
        }

        Dim btnAdd As New Button With {
            .Text = "Add User",
            .Width = 120,
            .Height = 35,
            .Font = New Font("Segoe UI", 10),
            .BackColor = Color.FromArgb(41, 128, 185),
            .ForeColor = Color.White,
            .FlatStyle = FlatStyle.Flat
        }
        btnAdd.FlatAppearance.BorderSize = 0
        AddHandler btnAdd.Click, AddressOf btnAdd_Click

        Dim btnUpdate As New Button With {
            .Text = "Update",
            .Width = 120,
            .Height = 35,
            .Font = New Font("Segoe UI", 10),
            .BackColor = Color.FromArgb(46, 204, 113),
            .ForeColor = Color.White,
            .FlatStyle = FlatStyle.Flat
        }
        btnUpdate.FlatAppearance.BorderSize = 0
        AddHandler btnUpdate.Click, AddressOf btnUpdate_Click

        Dim btnDelete As New Button With {
            .Text = "Delete",
            .Width = 120,
            .Height = 35,
            .Font = New Font("Segoe UI", 10),
            .BackColor = Color.FromArgb(231, 76, 60),
            .ForeColor = Color.White,
            .FlatStyle = FlatStyle.Flat
        }
        btnDelete.FlatAppearance.BorderSize = 0
        AddHandler btnDelete.Click, AddressOf btnDelete_Click

        Dim btnClear As New Button With {
            .Text = "Clear Fields",
            .Width = 120,
            .Height = 35,
            .Font = New Font("Segoe UI", 10),
            .BackColor = Color.FromArgb(149, 165, 166),
            .ForeColor = Color.White,
            .FlatStyle = FlatStyle.Flat
        }
        btnClear.FlatAppearance.BorderSize = 0
        AddHandler btnClear.Click, AddressOf btnClear_Click

        buttonPanel.Controls.AddRange({btnAdd, btnUpdate, btnDelete, btnClear})

        ' Layout structure
        Dim leftColumn As New TableLayoutPanel With {
            .Width = 400,
            .Height = 180,
            .ColumnCount = 2,
            .RowCount = 3,
            .Location = New Point(20, 20)
        }
        leftColumn.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 30))
        leftColumn.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 70))

        leftColumn.Controls.Add(CreateFieldLabel("Full Name:"), 0, 0)
        leftColumn.Controls.Add(txtFullName, 1, 0)
        leftColumn.Controls.Add(CreateFieldLabel("Username:"), 0, 1)
        leftColumn.Controls.Add(txtUsername, 1, 1)
        leftColumn.Controls.Add(CreateFieldLabel("Password:"), 0, 2)
        leftColumn.Controls.Add(txtPassword, 1, 2)

        Dim rightColumn As New TableLayoutPanel With {
            .Width = 400,
            .Height = 180,
            .ColumnCount = 2,
            .RowCount = 3,
            .Location = New Point(450, 20)
        }
        rightColumn.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 30))
        rightColumn.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 70))

        rightColumn.Controls.Add(CreateFieldLabel("Email:"), 0, 0)
        rightColumn.Controls.Add(txtEmail, 1, 0)
        rightColumn.Controls.Add(CreateFieldLabel("Phone:"), 0, 1)
        rightColumn.Controls.Add(txtPhone, 1, 1)
        rightColumn.Controls.Add(CreateFieldLabel("Role:"), 0, 2)
        rightColumn.Controls.Add(cmbRole, 1, 2)

        ' Status placed at bottom area
        Dim statusLabel As New Label With {
            .Text = "Status:",
            .Font = New Font("Segoe UI", 10),
            .AutoSize = True,
            .Location = New Point(20, 220)
        }
        cmbStatus.Location = New Point(80, 215)

        panelControls.Controls.Add(leftColumn)
        panelControls.Controls.Add(rightColumn)
        panelControls.Controls.Add(statusLabel)
        panelControls.Controls.Add(cmbStatus)

        ' Container for grid
        Dim gridContainer As New Panel With {
            .Dock = DockStyle.Fill,
            .Padding = New Padding(20, 10, 20, 10)
        }
        gridContainer.Controls.Add(usersDataGrid)

        ' Main layout structure
        Me.Controls.Add(gridContainer)
        Me.Controls.Add(headerPanel)
        Me.Controls.Add(panelControls)
        Me.Controls.Add(buttonPanel)
    End Sub

    Private Function CreateStyledTextBox(placeholder As String) As TextBox
        Dim txt As New TextBox With {
            .Width = 250,
            .Height = 30,
            .Font = New Font("Segoe UI", 10),
            .BorderStyle = BorderStyle.FixedSingle
        }

        ' Add placeholder text functionality
        Dim placeholderText As String = placeholder
        txt.Tag = placeholderText
        txt.ForeColor = Color.Gray
        txt.Text = placeholderText

        AddHandler txt.Enter, Sub(sender As Object, e As EventArgs)
                                  If txt.Text = txt.Tag.ToString() Then
                                      txt.Text = ""
                                      txt.ForeColor = Color.Black
                                  End If
                              End Sub

        AddHandler txt.Leave, Sub(sender As Object, e As EventArgs)
                                  If String.IsNullOrWhiteSpace(txt.Text) Then
                                      txt.Text = txt.Tag.ToString()
                                      txt.ForeColor = Color.Gray
                                  End If
                              End Sub

        Return txt
    End Function

    Private Function CreateStyledComboBox() As ComboBox
        Return New ComboBox With {
            .Width = 250,
            .Height = 30,
            .Font = New Font("Segoe UI", 10),
            .DropDownStyle = ComboBoxStyle.DropDownList,
            .FlatStyle = FlatStyle.Flat
        }
    End Function

    Private Function CreateFieldLabel(text As String) As Label
        Return New Label With {
            .text = text,
            .Font = New Font("Segoe UI", 10),
            .AutoSize = True,
            .TextAlign = ContentAlignment.MiddleRight
        }
    End Function

    Private Sub SearchBox_TextChanged(sender As Object, e As EventArgs)
        Try
            If usersDataGrid.DataSource IsNot Nothing AndAlso TypeOf usersDataGrid.DataSource Is DataTable Then
                Dim dt As DataTable = DirectCast(usersDataGrid.DataSource, DataTable)
                Dim dv As New DataView(dt)

                If Not String.IsNullOrWhiteSpace(searchBox.Text) Then
                    dv.RowFilter = String.Format("FullName LIKE '%{0}%' OR Username LIKE '%{0}%' OR Email LIKE '%{0}%'",
                        searchBox.Text.Replace("'", "''"))
                End If

                usersDataGrid.DataSource = dv.ToTable()
            End If
        Catch ex As Exception
            ' Handle filtering errors gracefully
            MessageBox.Show("Error filtering data: " & ex.Message, "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LoadUsers()
        Try
            Dim query As String = "SELECT UserID, FullName, Username, UserLevelName, Email, PhoneNumber, Status FROM user"
            Dim dt As DataTable = GetData(query)

            ' Format the grid after loading data
            usersDataGrid.DataSource = dt

            ' Set column headers and widths
            If usersDataGrid.Columns.Count > 0 Then
                usersDataGrid.Columns("UserID").Visible = False
                usersDataGrid.Columns("UserLevelName").HeaderText = "Role"
                usersDataGrid.Columns("PhoneNumber").HeaderText = "Phone Number"

                ' Conditional formatting for Status column
                AddHandler usersDataGrid.CellFormatting, Sub(sender As Object, e As DataGridViewCellFormattingEventArgs)
                                                             If usersDataGrid.Columns(e.ColumnIndex).Name = "Status" Then
                                                                 If e.Value IsNot Nothing Then
                                                                     If e.Value.ToString() = "Active" Then
                                                                         e.CellStyle.ForeColor = Color.Green
                                                                         e.CellStyle.Font = New Font(usersDataGrid.DefaultCellStyle.Font, FontStyle.Bold)
                                                                     ElseIf e.Value.ToString() = "Inactive" Then
                                                                         e.CellStyle.ForeColor = Color.Red
                                                                     End If
                                                                 End If
                                                             End If
                                                         End Sub
            End If
        Catch ex As Exception
            MessageBox.Show("Failed to load users: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs)
        ' Validate required fields
        If String.IsNullOrWhiteSpace(txtFullName.Text) OrElse txtFullName.Text = txtFullName.Tag.ToString() OrElse
           String.IsNullOrWhiteSpace(txtUsername.Text) OrElse txtUsername.Text = txtUsername.Tag.ToString() OrElse
           String.IsNullOrWhiteSpace(txtPassword.Text) OrElse txtPassword.Text = txtPassword.Tag.ToString() OrElse
           cmbRole.SelectedIndex = -1 OrElse cmbStatus.SelectedIndex = -1 Then

            MessageBox.Show("Please fill all required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Dim hasher As New PasswordHasher(50000, 16, "SHA256")
            Dim passwordResult = hasher.HashPassword(txtPassword.Text)

            Dim query As String = "
            INSERT INTO user 
            (FullName, Username, Password, Salt, UserLevelName, Email, PhoneNumber, Status, CreatedAt) 
            VALUES 
            (@FullName, @Username, @Password, @Salt, @UserLevelName, @Email, @PhoneNumber, @Status, NOW())"

            Dim parameters As New Dictionary(Of String, Object) From {
                {"@FullName", txtFullName.Text},
                {"@Username", txtUsername.Text},
                {"@Password", passwordResult.HashBase64},
                {"@Salt", passwordResult.SaltBase64},
                {"@UserLevelName", cmbRole.Text},
                {"@Email", txtEmail.Text},
                {"@PhoneNumber", txtPhone.Text},
                {"@Status", cmbStatus.Text}
            }

            If ExecuteQuery(query, parameters) Then
                MessageBox.Show("User added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadUsers()
                ClearFields()
            End If
        Catch ex As Exception
            MessageBox.Show("Error adding user: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs)
        If currentUserID = -1 Then
            MessageBox.Show("Please select a user to update.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        ' Validate required fields
        If String.IsNullOrWhiteSpace(txtFullName.Text) OrElse txtFullName.Text = txtFullName.Tag.ToString() OrElse
           String.IsNullOrWhiteSpace(txtUsername.Text) OrElse txtUsername.Text = txtUsername.Tag.ToString() OrElse
           cmbRole.SelectedIndex = -1 OrElse cmbStatus.SelectedIndex = -1 Then

            MessageBox.Show("Please fill all required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Dim query As String = "UPDATE user SET FullName=@FullName, Username=@Username, UserLevelName=@UserLevelName, Email=@Email, PhoneNumber=@PhoneNumber, Status=@Status"
            Dim parameters As New Dictionary(Of String, Object) From {
                {"@UserID", currentUserID},
                {"@FullName", txtFullName.Text},
                {"@Username", txtUsername.Text},
                {"@UserLevelName", cmbRole.Text},
                {"@Email", txtEmail.Text},
                {"@PhoneNumber", txtPhone.Text},
                {"@Status", cmbStatus.Text}
            }

            ' Only update password & salt if a new password is entered
            If Not String.IsNullOrWhiteSpace(txtPassword.Text) AndAlso txtPassword.Text <> txtPassword.Tag.ToString() Then
                Dim hasher As New PasswordHasher(50000, 16, "SHA256")
                Dim passwordResult = hasher.HashPassword(txtPassword.Text)

                query &= ", Password=@Password, Salt=@Salt"
                parameters.Add("@Password", passwordResult.HashBase64)
                parameters.Add("@Salt", passwordResult.SaltBase64)
            End If

            ' Final WHERE clause
            query &= " WHERE UserID=@UserID"

            If ExecuteQuery(query, parameters) Then
                MessageBox.Show("User updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadUsers()
                ClearFields()
                currentUserID = -1
            End If
        Catch ex As Exception
            MessageBox.Show("Error updating user: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs)
        If currentUserID = -1 Then
            MessageBox.Show("Select a user to delete.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Try
            If MessageBox.Show("Are you sure you want to delete this user?", "Confirm Delete",
                              MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then

                Dim query As String = "DELETE FROM user WHERE UserID=@UserID"
                Dim parameters As New Dictionary(Of String, Object) From {
                    {"@UserID", currentUserID}
                }

                If ExecuteQuery(query, parameters) Then
                    MessageBox.Show("User deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    LoadUsers()
                    ClearFields()
                    currentUserID = -1
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Error deleting user: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs)
        ClearFields()
        currentUserID = -1
    End Sub

    Private Sub ClearFields()
        ' Reset textboxes to show placeholders
        For Each ctrl As Control In panelControls.Controls
            If TypeOf ctrl Is TableLayoutPanel Then
                For Each innerCtrl As Control In ctrl.Controls
                    If TypeOf innerCtrl Is TextBox Then
                        Dim txt As TextBox = DirectCast(innerCtrl, TextBox)
                        txt.Text = txt.Tag.ToString()
                        txt.ForeColor = Color.Gray
                    End If
                Next
            ElseIf TypeOf ctrl Is TextBox Then
                Dim txt As TextBox = DirectCast(ctrl, TextBox)
                txt.Text = txt.Tag.ToString()
                txt.ForeColor = Color.Gray
            End If
        Next

        ' Reset comboboxes
        cmbRole.SelectedIndex = -1
        cmbStatus.SelectedIndex = -1

        ' Highlight Add button
        For Each ctrl As Control In Me.Controls
            If TypeOf ctrl Is FlowLayoutPanel Then
                For Each btn As Control In ctrl.Controls
                    If TypeOf btn Is Button AndAlso btn.Text = "Add User" Then
                        btn.BackColor = Color.FromArgb(41, 128, 185)
                    End If
                Next
            End If
        Next
    End Sub

    Private Sub usersDataGrid_CellClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex >= 0 Then
            Dim row As DataGridViewRow = usersDataGrid.Rows(e.RowIndex)

            ' Store the current user ID for update/delete operations
            currentUserID = Convert.ToInt32(row.Cells("UserID").Value)

            ' Update textboxes with real values (not placeholders)
            txtFullName.Text = row.Cells("FullName").Value.ToString()
            txtFullName.ForeColor = Color.Black

            txtUsername.Text = row.Cells("Username").Value.ToString()
            txtUsername.ForeColor = Color.Black

            txtPassword.Text = txtPassword.Tag.ToString()
            txtPassword.ForeColor = Color.Gray

            txtEmail.Text = row.Cells("Email").Value.ToString()
            txtEmail.ForeColor = Color.Black

            txtPhone.Text = row.Cells("PhoneNumber").Value.ToString()
            txtPhone.ForeColor = Color.Black

            ' Use the actual column name from the database, not the display header
            cmbRole.Text = row.Cells("UserLevelName").Value.ToString()
            cmbStatus.Text = row.Cells("Status").Value.ToString()

            ' Highlight Update button
            For Each ctrl As Control In Me.Controls
                If TypeOf ctrl Is FlowLayoutPanel Then
                    For Each btn As Control In ctrl.Controls
                        If TypeOf btn Is Button AndAlso btn.Text = "Update" Then
                            btn.BackColor = Color.FromArgb(46, 204, 113)
                        End If
                    Next
                End If
            Next
        End If
    End Sub
End Class
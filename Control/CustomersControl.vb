Imports System.Data
Imports MySql.Data.MySqlClient
Imports System.Drawing

Public Class CustomersControl
    Inherits UserControl

    Private customersGrid As DataGridView
    Private txtFirstName, txtLastName, txtMiddleName, txtContact, txtEmail, txtAddress, txtCity, txtProvince, txtZip As TextBox
    Private cmbCustomerType, cmbGender, cmbStatus As ComboBox
    Private dtpBirthDate, dtpRegistrationDate As DateTimePicker
    Private searchBox As TextBox
    Private currentCustomerID As Integer = -1

    Public Sub New()
        InitializeComponent()
        SetupLayout()
        LoadCustomers()
    End Sub

    Private Sub SetupLayout()
        ' Set control background
        Me.BackColor = Color.FromArgb(240, 240, 240)

        ' Header Panel with title and search
        Dim headerPanel As New Panel With {
            .Dock = DockStyle.Top,
            .Height = 60,
            .BackColor = Color.FromArgb(156, 39, 176) ' Purple theme
        }

        Dim lblTitle As New Label With {
            .Text = "Customer Management",
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
            .ForeColor = Color.White,
            .AutoSize = True,
            .Anchor = AnchorStyles.Right Or AnchorStyles.Top,
            .Location = New Point(headerPanel.Width - 290, 18)
        }

        headerPanel.Controls.Add(lblTitle)
        headerPanel.Controls.Add(searchBox)
        headerPanel.Controls.Add(searchIcon)

        ' DataGridView with modern styling
        customersGrid = New DataGridView With {
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
        customersGrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(156, 39, 176) ' Purple highlight
        customersGrid.DefaultCellStyle.Font = New Font("Segoe UI", 9)
        customersGrid.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        customersGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245)
        customersGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(70, 70, 70)
        customersGrid.ColumnHeadersHeight = 40
        customersGrid.RowTemplate.Height = 35

        AddHandler customersGrid.CellClick, AddressOf customersGrid_CellClick

        ' Input Controls Container Panels
        ' In the SetupLayout method, change the detailsPanel height
        Dim detailsPanel As New Panel With {
            .Dock = DockStyle.Bottom,
            .Height = 250, ' Reduced from 470
            .BackColor = Color.White,
            .Padding = New Padding(20)
        }

        ' Add shadow effect (border)
        AddHandler detailsPanel.Paint, Sub(sender As Object, e As PaintEventArgs)
                                           Dim topBorder As New Pen(Color.FromArgb(200, 200, 200))
                                           e.Graphics.DrawLine(topBorder, 0, 0, detailsPanel.Width, 0)
                                       End Sub

        ' Input Fields
        txtFirstName = CreateStyledTextBox("First Name")
        txtLastName = CreateStyledTextBox("Last Name")
        txtMiddleName = CreateStyledTextBox("Middle Name")
        txtContact = CreateStyledTextBox("Contact Number")
        txtEmail = CreateStyledTextBox("Email Address")
        txtAddress = CreateStyledTextBox("Address Line")
        txtCity = CreateStyledTextBox("City")
        txtProvince = CreateStyledTextBox("Province")
        txtZip = CreateStyledTextBox("Zip Code")

        cmbGender = CreateStyledComboBox()
        cmbGender.Items.AddRange({"Male", "Female", "Other"})

        cmbCustomerType = CreateStyledComboBox()
        cmbCustomerType.Items.AddRange({"Regular", "Walk-in", "Corporate"})

        cmbStatus = CreateStyledComboBox()
        cmbStatus.Items.AddRange({"Active", "Inactive"})

        dtpBirthDate = CreateStyledDateTimePicker()
        dtpRegistrationDate = CreateStyledDateTimePicker()

        ' Button Panel
        Dim buttonPanel As New FlowLayoutPanel With {
            .FlowDirection = FlowDirection.RightToLeft,
            .Dock = DockStyle.Bottom,
            .Height = 50,
            .Padding = New Padding(0, 10, 20, 0),
            .BackColor = Color.White
        }

        Dim btnAdd As New Button With {
            .Text = "Add Customer",
            .Width = 130,
            .Height = 35,
            .Font = New Font("Segoe UI", 10),
            .BackColor = Color.FromArgb(156, 39, 176), ' Purple
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
            .BackColor = Color.FromArgb(0, 150, 136), ' Teal
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
            .BackColor = Color.FromArgb(239, 83, 80), ' Red
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
            .BackColor = Color.FromArgb(149, 165, 166), ' Gray
            .ForeColor = Color.White,
            .FlatStyle = FlatStyle.Flat
        }
        btnClear.FlatAppearance.BorderSize = 0
        AddHandler btnClear.Click, AddressOf btnClear_Click

        buttonPanel.Controls.AddRange({btnAdd, btnUpdate, btnDelete, btnClear})

        ' Create left and right columns for the form
        Dim leftColumn As New TableLayoutPanel With {
            .Width = 380,
            .Height = 380,
            .Location = New Point(20, 20),
            .ColumnCount = 2,
            .RowCount = 7
        }

        leftColumn.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 30))
        leftColumn.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 70))

        leftColumn.Controls.Add(CreateFieldLabel("First Name:"), 0, 0)
        leftColumn.Controls.Add(txtFirstName, 1, 0)
        leftColumn.Controls.Add(CreateFieldLabel("Last Name:"), 0, 1)
        leftColumn.Controls.Add(txtLastName, 1, 1)
        leftColumn.Controls.Add(CreateFieldLabel("Middle Name:"), 0, 2)
        leftColumn.Controls.Add(txtMiddleName, 1, 2)
        leftColumn.Controls.Add(CreateFieldLabel("Gender:"), 0, 3)
        leftColumn.Controls.Add(cmbGender, 1, 3)
        leftColumn.Controls.Add(CreateFieldLabel("Birth Date:"), 0, 4)
        leftColumn.Controls.Add(dtpBirthDate, 1, 4)
        leftColumn.Controls.Add(CreateFieldLabel("Contact:"), 0, 5)
        leftColumn.Controls.Add(txtContact, 1, 5)
        leftColumn.Controls.Add(CreateFieldLabel("Email:"), 0, 6)
        leftColumn.Controls.Add(txtEmail, 1, 6)

        Dim rightColumn As New TableLayoutPanel With {
            .Width = 380,
            .Height = 380,
            .Location = New Point(420, 20),
            .ColumnCount = 2,
            .RowCount = 7
        }

        rightColumn.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 30))
        rightColumn.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 70))

        rightColumn.Controls.Add(CreateFieldLabel("Address:"), 0, 0)
        rightColumn.Controls.Add(txtAddress, 1, 0)
        rightColumn.Controls.Add(CreateFieldLabel("City:"), 0, 1)
        rightColumn.Controls.Add(txtCity, 1, 1)
        rightColumn.Controls.Add(CreateFieldLabel("Province:"), 0, 2)
        rightColumn.Controls.Add(txtProvince, 1, 2)
        rightColumn.Controls.Add(CreateFieldLabel("Zip Code:"), 0, 3)
        rightColumn.Controls.Add(txtZip, 1, 3)
        rightColumn.Controls.Add(CreateFieldLabel("Customer Type:"), 0, 4)
        rightColumn.Controls.Add(cmbCustomerType, 1, 4)
        rightColumn.Controls.Add(CreateFieldLabel("Registration:"), 0, 5)
        rightColumn.Controls.Add(dtpRegistrationDate, 1, 5)
        rightColumn.Controls.Add(CreateFieldLabel("Status:"), 0, 6)
        rightColumn.Controls.Add(cmbStatus, 1, 6)

        ' Add columns to details panel
        detailsPanel.Controls.Add(leftColumn)
        detailsPanel.Controls.Add(rightColumn)

        ' Create container for grid with padding
        Dim gridContainer As New Panel With {
            .Dock = DockStyle.Fill,
            .Padding = New Padding(20, 10, 20, 10)
        }
        gridContainer.Controls.Add(customersGrid)

        ' Main layout structure
        Me.Controls.Add(gridContainer)
        Me.Controls.Add(headerPanel)
        Me.Controls.Add(detailsPanel)
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

    Private Function CreateStyledDateTimePicker() As DateTimePicker
        Return New DateTimePicker With {
            .Width = 250,
            .Font = New Font("Segoe UI", 10),
            .Format = DateTimePickerFormat.Short
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
            If customersGrid.DataSource IsNot Nothing AndAlso TypeOf customersGrid.DataSource Is DataTable Then
                Dim dt As DataTable = DirectCast(customersGrid.DataSource, DataTable)
                Dim dv As New DataView(dt)

                If Not String.IsNullOrWhiteSpace(searchBox.Text) Then
                    dv.RowFilter = String.Format("FirstName LIKE '%{0}%' OR LastName LIKE '%{0}%' OR Email LIKE '%{0}%' OR ContactNumber LIKE '%{0}%'",
                        searchBox.Text.Replace("'", "''"))
                End If

                customersGrid.DataSource = dv.ToTable()
            End If
        Catch ex As Exception
            MessageBox.Show("Error filtering data: " & ex.Message, "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LoadCustomers()
        Try
            Dim query As String = "
            SELECT c.CustomerID, c.FirstName, c.LastName, c.MiddleName, c.Gender, c.BirthDate,
                   c.ContactNumber, c.Email, c.AddressLine, c.City, c.Province, c.ZipCode,
                   c.CustomerType, c.RegistrationDate, c.Status,
                   COUNT(b.BookingID) AS TotalBookings
            FROM customer c
            LEFT JOIN booking b ON c.CustomerID = b.CustomerID
            GROUP BY c.CustomerID, c.FirstName, c.LastName, c.MiddleName, c.Gender, c.BirthDate,
                     c.ContactNumber, c.Email, c.AddressLine, c.City, c.Province, c.ZipCode,
                     c.CustomerType, c.RegistrationDate, c.Status
            "

            Dim dt As DataTable = GetData(query)
            customersGrid.DataSource = dt

            ' Format the grid after loading data
            If customersGrid.Columns.Count > 0 Then
                customersGrid.Columns("CustomerID").Visible = False

                ' Set up friendly column names
                customersGrid.Columns("FirstName").HeaderText = "First Name"
                customersGrid.Columns("LastName").HeaderText = "Last Name"
                customersGrid.Columns("MiddleName").HeaderText = "Middle Name"
                customersGrid.Columns("ContactNumber").HeaderText = "Contact"
                customersGrid.Columns("AddressLine").HeaderText = "Address"
                customersGrid.Columns("ZipCode").HeaderText = "Zip Code"
                customersGrid.Columns("CustomerType").HeaderText = "Customer Type"
                customersGrid.Columns("RegistrationDate").HeaderText = "Registration Date"
                customersGrid.Columns("TotalBookings").HeaderText = "Total Bookings"

                ' Column display priority - hide less important columns if space is limited
                customersGrid.Columns("MiddleName").DisplayIndex = 3
                customersGrid.Columns("Email").DisplayIndex = 4
                customersGrid.Columns("ContactNumber").DisplayIndex = 5
                customersGrid.Columns("Status").DisplayIndex = 6
                customersGrid.Columns("TotalBookings").DisplayIndex = 7

                ' Optional: Hide some columns to make the grid more compact
                customersGrid.Columns("AddressLine").Visible = False
                customersGrid.Columns("City").Visible = False
                customersGrid.Columns("Province").Visible = False
                customersGrid.Columns("ZipCode").Visible = False
                customersGrid.Columns("BirthDate").Visible = False

                ' Conditional formatting for Status column
                AddHandler customersGrid.CellFormatting, Sub(sender As Object, e As DataGridViewCellFormattingEventArgs)
                                                             If customersGrid.Columns(e.ColumnIndex).Name = "Status" Then
                                                                 If e.Value IsNot Nothing Then
                                                                     If e.Value.ToString() = "Active" Then
                                                                         e.CellStyle.ForeColor = Color.Green
                                                                         e.CellStyle.Font = New Font(customersGrid.DefaultCellStyle.Font, FontStyle.Bold)
                                                                     ElseIf e.Value.ToString() = "Inactive" Then
                                                                         e.CellStyle.ForeColor = Color.Red
                                                                     End If
                                                                 End If
                                                             End If

                                                             If customersGrid.Columns(e.ColumnIndex).Name = "TotalBookings" Then
                                                                 If e.Value IsNot Nothing AndAlso Convert.ToInt32(e.Value) > 0 Then
                                                                     e.CellStyle.ForeColor = Color.FromArgb(156, 39, 176)
                                                                     e.CellStyle.Font = New Font(customersGrid.DefaultCellStyle.Font, FontStyle.Bold)
                                                                 End If
                                                             End If
                                                         End Sub
            End If
        Catch ex As Exception
            MessageBox.Show("Failed to load customers: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs)
        ' Validate required fields
        If IsFieldEmpty(txtFirstName) OrElse IsFieldEmpty(txtLastName) OrElse
           cmbGender.SelectedIndex = -1 OrElse cmbStatus.SelectedIndex = -1 OrElse
           cmbCustomerType.SelectedIndex = -1 Then
            MessageBox.Show("Please fill all required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Dim query As String = "INSERT INTO customer (FirstName, LastName, MiddleName, Gender, BirthDate, ContactNumber, Email, AddressLine, City, Province, ZipCode, CustomerType, RegistrationDate, Status)
                                VALUES (@FirstName, @LastName, @MiddleName, @Gender, @BirthDate, @ContactNumber, @Email, @AddressLine, @City, @Province, @ZipCode, @CustomerType, @RegistrationDate, @Status)"

            Dim parameters As New Dictionary(Of String, Object) From {
                {"@FirstName", GetTextValue(txtFirstName)},
                {"@LastName", GetTextValue(txtLastName)},
                {"@MiddleName", GetTextValue(txtMiddleName)},
                {"@Gender", cmbGender.Text},
                {"@BirthDate", dtpBirthDate.Value.Date},
                {"@ContactNumber", GetTextValue(txtContact)},
                {"@Email", GetTextValue(txtEmail)},
                {"@AddressLine", GetTextValue(txtAddress)},
                {"@City", GetTextValue(txtCity)},
                {"@Province", GetTextValue(txtProvince)},
                {"@ZipCode", GetTextValue(txtZip)},
                {"@CustomerType", cmbCustomerType.Text},
                {"@RegistrationDate", dtpRegistrationDate.Value.Date},
                {"@Status", cmbStatus.Text}
            }

            If ExecuteQuery(query, parameters) Then
                MessageBox.Show("Customer added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadCustomers()
                ClearFields()
            End If
        Catch ex As Exception
            MessageBox.Show("Error adding customer: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs)
        If currentCustomerID = -1 Then
            MessageBox.Show("Please select a customer to update.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        ' Validate required fields
        If IsFieldEmpty(txtFirstName) OrElse IsFieldEmpty(txtLastName) OrElse
           cmbGender.SelectedIndex = -1 OrElse cmbStatus.SelectedIndex = -1 OrElse
           cmbCustomerType.SelectedIndex = -1 Then
            MessageBox.Show("Please fill all required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Dim query As String = "UPDATE customer SET FirstName=@FirstName, LastName=@LastName, MiddleName=@MiddleName, Gender=@Gender, BirthDate=@BirthDate,
                                  ContactNumber=@ContactNumber, Email=@Email, AddressLine=@AddressLine, City=@City, Province=@Province,
                                  ZipCode=@ZipCode, CustomerType=@CustomerType, RegistrationDate=@RegistrationDate, Status=@Status
                                  WHERE CustomerID=@CustomerID"

            Dim parameters As New Dictionary(Of String, Object) From {
                {"@CustomerID", currentCustomerID},
                {"@FirstName", GetTextValue(txtFirstName)},
                {"@LastName", GetTextValue(txtLastName)},
                {"@MiddleName", GetTextValue(txtMiddleName)},
                {"@Gender", cmbGender.Text},
                {"@BirthDate", dtpBirthDate.Value.Date},
                {"@ContactNumber", GetTextValue(txtContact)},
                {"@Email", GetTextValue(txtEmail)},
                {"@AddressLine", GetTextValue(txtAddress)},
                {"@City", GetTextValue(txtCity)},
                {"@Province", GetTextValue(txtProvince)},
                {"@ZipCode", GetTextValue(txtZip)},
                {"@CustomerType", cmbCustomerType.Text},
                {"@RegistrationDate", dtpRegistrationDate.Value.Date},
                {"@Status", cmbStatus.Text}
            }

            If ExecuteQuery(query, parameters) Then
                MessageBox.Show("Customer updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadCustomers()
                ClearFields()
                currentCustomerID = -1
            End If
        Catch ex As Exception
            MessageBox.Show("Error updating customer: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs)
        If currentCustomerID = -1 Then
            MessageBox.Show("Please select a customer to delete.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Try
            If MessageBox.Show("Are you sure you want to delete this customer?", "Confirm Delete",
                              MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then

                Dim query As String = "DELETE FROM customer WHERE CustomerID=@CustomerID"
                Dim parameters As New Dictionary(Of String, Object) From {
                    {"@CustomerID", currentCustomerID}
                }

                If ExecuteQuery(query, parameters) Then
                    MessageBox.Show("Customer deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    LoadCustomers()
                    ClearFields()
                    currentCustomerID = -1
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Error deleting customer: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs)
        ClearFields()
        currentCustomerID = -1
    End Sub

    Private Sub customersGrid_CellClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex >= 0 Then
            Dim row As DataGridViewRow = customersGrid.Rows(e.RowIndex)

            ' Store the current customer ID for update/delete operations
            currentCustomerID = Convert.ToInt32(row.Cells("CustomerID").Value)

            ' Update textboxes with real values (not placeholders)
            SetTextValue(txtFirstName, row.Cells("FirstName").Value.ToString())
            SetTextValue(txtLastName, row.Cells("LastName").Value.ToString())
            SetTextValue(txtMiddleName, row.Cells("MiddleName").Value.ToString())
            SetTextValue(txtContact, row.Cells("ContactNumber").Value.ToString())
            SetTextValue(txtEmail, row.Cells("Email").Value.ToString())
            SetTextValue(txtAddress, row.Cells("AddressLine").Value.ToString())
            SetTextValue(txtCity, row.Cells("City").Value.ToString())
            SetTextValue(txtProvince, row.Cells("Province").Value.ToString())
            SetTextValue(txtZip, row.Cells("ZipCode").Value.ToString())

            cmbGender.Text = row.Cells("Gender").Value.ToString()
            cmbCustomerType.Text = row.Cells("CustomerType").Value.ToString()
            cmbStatus.Text = row.Cells("Status").Value.ToString()

            dtpBirthDate.Value = Convert.ToDateTime(row.Cells("BirthDate").Value)
            dtpRegistrationDate.Value = Convert.ToDateTime(row.Cells("RegistrationDate").Value)

            ' Highlight Update button
            For Each ctrl As Control In Me.Controls
                If TypeOf ctrl Is FlowLayoutPanel Then
                    For Each btn As Control In ctrl.Controls
                        If TypeOf btn Is Button AndAlso btn.Text = "Update" Then
                            btn.BackColor = Color.FromArgb(0, 150, 136) ' Teal
                        End If
                    Next
                End If
            Next
        End If
    End Sub

    Private Sub ClearFields()
        ' Reset textboxes to show placeholders
        For Each ctrl As Control In Me.Controls
            If TypeOf ctrl Is Panel AndAlso ctrl.BackColor = Color.White Then
                For Each innerCtrl As Control In ctrl.Controls
                    If TypeOf innerCtrl Is TableLayoutPanel Then
                        For Each fieldCtrl As Control In innerCtrl.Controls
                            If TypeOf fieldCtrl Is TextBox Then
                                Dim txt As TextBox = DirectCast(fieldCtrl, TextBox)
                                txt.Text = txt.Tag.ToString()
                                txt.ForeColor = Color.Gray
                            End If
                        Next
                    End If
                Next
            End If
        Next

        ' Reset comboboxes
        cmbCustomerType.SelectedIndex = -1
        cmbGender.SelectedIndex = -1
        cmbStatus.SelectedIndex = -1

        ' Reset date pickers
        dtpBirthDate.Value = DateTime.Now
        dtpRegistrationDate.Value = DateTime.Now

        ' Highlight Add button
        For Each ctrl As Control In Me.Controls
            If TypeOf ctrl Is FlowLayoutPanel Then
                For Each btn As Control In ctrl.Controls
                    If TypeOf btn Is Button AndAlso btn.Text = "Add Customer" Then
                        btn.BackColor = Color.FromArgb(156, 39, 176) ' Purple
                    End If
                Next
            End If
        Next
    End Sub

    ' Helper functions for text field handling
    Private Function GetTextValue(textBox As TextBox) As String
        If textBox.Text = textBox.Tag.ToString() Then
            Return ""
        Else
            Return textBox.Text
        End If
    End Function

    Private Sub SetTextValue(textBox As TextBox, value As String)
        If String.IsNullOrWhiteSpace(value) Then
            textBox.Text = textBox.Tag.ToString()
            textBox.ForeColor = Color.Gray
        Else
            textBox.Text = value
            textBox.ForeColor = Color.Black
        End If
    End Sub

    Private Function IsFieldEmpty(textBox As TextBox) As Boolean
        Return String.IsNullOrWhiteSpace(textBox.Text) OrElse textBox.Text = textBox.Tag.ToString()
    End Function
End Class
Imports System.Data
Imports MySql.Data.MySqlClient

Public Class ComboItem
    Public Property Text As String
    Public Property Value As Object

    Public Sub New(text As String, value As Object)
        Me.Text = text
        Me.Value = value
    End Sub

    Public Overrides Function ToString() As String
        Return Text
    End Function
End Class


Public Class BookingsControl
    Inherits UserControl

    ' DataGridViews for Bookings, Events, and Services
    Private bookingsGrid, eventsGrid, servicesGrid As DataGridView
    Private txtAmount, txtRemarks, txtDiscount As TextBox
    Private cmbCustomerID, cmbEventID, cmbServiceID, cmbStatus, cmbPayment, cmbRefund As ComboBox
    Private dtpBookingDate, dtpEventDate, dtpTime As DateTimePicker

    ' Main container for scrolling
    Private mainPanel As Panel

    Public Sub New()
        InitializeComponent()
        SetupLayout()
        LoadBookings()
        LoadEvents()
        LoadServices()
    End Sub

    Private Sub SetupLayout()
        ' Create main scrollable panel
        mainPanel = New Panel With {
            .Dock = DockStyle.Fill,
            .AutoScroll = True,
            .Padding = New Padding(15)
        }
        Me.Controls.Add(mainPanel)

        ' Create a tabbed interface for grids
        Dim tabControl As New TabControl With {
            .Dock = DockStyle.Top,
            .Height = 250,
            .Font = New Font("Segoe UI", 9.75F, FontStyle.Regular)
        }

        ' Setup tabs
        Dim bookingsTab As New TabPage("Bookings")
        Dim eventsTab As New TabPage("Events")
        Dim servicesTab As New TabPage("Services")

        ' Grid for Bookings
        bookingsGrid = New DataGridView With {
            .Dock = DockStyle.Fill,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            .ReadOnly = True,
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            .AllowUserToAddRows = False,
            .BorderStyle = BorderStyle.None,
            .BackgroundColor = Color.White,
            .RowHeadersVisible = False,
            .AlternatingRowsDefaultCellStyle = New DataGridViewCellStyle With {
                .BackColor = Color.AliceBlue
            },
            .ColumnHeadersDefaultCellStyle = New DataGridViewCellStyle With {
                .BackColor = Color.LightSteelBlue,
                .ForeColor = Color.DarkBlue,
                .Font = New Font("Segoe UI", 9.75F, FontStyle.Bold)
            }
        }
        AddHandler bookingsGrid.CellClick, AddressOf bookingsGrid_CellClick
        bookingsTab.Controls.Add(bookingsGrid)

        ' Grid for Events
        eventsGrid = New DataGridView With {
            .Dock = DockStyle.Fill,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            .ReadOnly = True,
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            .AllowUserToAddRows = False,
            .BorderStyle = BorderStyle.None,
            .BackgroundColor = Color.White,
            .RowHeadersVisible = False,
            .AlternatingRowsDefaultCellStyle = New DataGridViewCellStyle With {
                .BackColor = Color.AliceBlue
            },
            .ColumnHeadersDefaultCellStyle = New DataGridViewCellStyle With {
                .BackColor = Color.LightSteelBlue,
                .ForeColor = Color.DarkBlue,
                .Font = New Font("Segoe UI", 9.75F, FontStyle.Bold)
            }
        }
        eventsTab.Controls.Add(eventsGrid)

        ' Grid for Services
        servicesGrid = New DataGridView With {
            .Dock = DockStyle.Fill,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            .ReadOnly = True,
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            .AllowUserToAddRows = False,
            .BorderStyle = BorderStyle.None,
            .BackgroundColor = Color.White,
            .RowHeadersVisible = False,
            .AlternatingRowsDefaultCellStyle = New DataGridViewCellStyle With {
                .BackColor = Color.AliceBlue
            },
            .ColumnHeadersDefaultCellStyle = New DataGridViewCellStyle With {
                .BackColor = Color.LightSteelBlue,
                .ForeColor = Color.DarkBlue,
                .Font = New Font("Segoe UI", 9.75F, FontStyle.Bold)
            }
        }
        servicesTab.Controls.Add(servicesGrid)

        ' Add tabs to control
        tabControl.TabPages.Add(bookingsTab)
        tabControl.TabPages.Add(eventsTab)
        tabControl.TabPages.Add(servicesTab)

        ' Add to main panel
        mainPanel.Controls.Add(tabControl)

        ' Create booking form group
        Dim bookingGroupBox As New GroupBox With {
            .Text = "Booking Details",
            .Font = New Font("Segoe UI", 10.0F, FontStyle.Bold),
            .Location = New Point(10, tabControl.Bottom + 15),
            .Size = New Size(mainPanel.Width - 40, 420),
            .Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        }

        ' Input Controls for Booking using TableLayoutPanel for better organization
        Dim bookingLayout As New TableLayoutPanel With {
            .Dock = DockStyle.Fill,
            .ColumnCount = 4,
            .RowCount = 8,
            .Padding = New Padding(10),
            .CellBorderStyle = TableLayoutPanelCellBorderStyle.None
        }

        ' Configure columns
        For i As Integer = 0 To 3
            If i Mod 2 = 0 Then
                ' Labels column
                bookingLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 20))
            Else
                ' Control column
                bookingLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 30))
            End If
        Next

        ' Setup row height
        For i As Integer = 0 To 7
            bookingLayout.RowStyles.Add(New RowStyle(SizeType.Absolute, 45))
        Next

        ' Create label style
        Dim labelFont As New Font("Segoe UI", 9.0F)

        ' Create input controls with nicer styling
        cmbCustomerID = New ComboBox With {
            .Dock = DockStyle.Fill,
            .DropDownStyle = ComboBoxStyle.DropDownList,
            .Font = New Font("Segoe UI", 9.0F)
        }

        cmbEventID = New ComboBox With {
            .Dock = DockStyle.Fill,
            .DropDownStyle = ComboBoxStyle.DropDownList,
            .Font = New Font("Segoe UI", 9.0F)
        }

        cmbServiceID = New ComboBox With {
            .Dock = DockStyle.Fill,
            .DropDownStyle = ComboBoxStyle.DropDownList,
            .Font = New Font("Segoe UI", 9.0F)
        }

        ' Populate combo boxes
        PopulateCustomerDropdown()
        PopulateEventsDropdown()
        PopulateServicesDropdown()

        txtAmount = New TextBox With {
            .Dock = DockStyle.Fill,
            .Font = New Font("Segoe UI", 9.0F)
        }

        txtRemarks = New TextBox With {
            .Dock = DockStyle.Fill,
            .Multiline = True,
            .Font = New Font("Segoe UI", 9.0F)
        }

        txtDiscount = New TextBox With {
            .Dock = DockStyle.Fill,
            .Font = New Font("Segoe UI", 9.0F)
        }

        cmbStatus = New ComboBox With {
            .Dock = DockStyle.Fill,
            .DropDownStyle = ComboBoxStyle.DropDownList,
            .Font = New Font("Segoe UI", 9.0F)
        }
        cmbStatus.Items.AddRange({"Pending", "Confirmed", "Cancelled"})

        cmbPayment = New ComboBox With {
            .Dock = DockStyle.Fill,
            .DropDownStyle = ComboBoxStyle.DropDownList,
            .Font = New Font("Segoe UI", 9.0F)
        }
        cmbPayment.Items.AddRange({"Paid", "Unpaid", "Partially Paid"})

        cmbRefund = New ComboBox With {
            .Dock = DockStyle.Fill,
            .DropDownStyle = ComboBoxStyle.DropDownList,
            .Font = New Font("Segoe UI", 9.0F)
        }
        cmbRefund.Items.AddRange({"Refunded", "Not Refunded"})

        dtpBookingDate = New DateTimePicker With {
            .Dock = DockStyle.Fill,
            .Format = DateTimePickerFormat.Short,
            .Font = New Font("Segoe UI", 9.0F)
        }

        dtpEventDate = New DateTimePicker With {
            .Dock = DockStyle.Fill,
            .Format = DateTimePickerFormat.Short,
            .Font = New Font("Segoe UI", 9.0F)
        }

        dtpTime = New DateTimePicker With {
            .Dock = DockStyle.Fill,
            .Format = DateTimePickerFormat.Time,
            .ShowUpDown = True,
            .Font = New Font("Segoe UI", 9.0F)
        }

        ' Add controls to layout - first row
        bookingLayout.Controls.Add(New Label With {.Text = "Customer", .TextAlign = ContentAlignment.MiddleLeft, .Font = labelFont}, 0, 0)
        bookingLayout.Controls.Add(cmbCustomerID, 1, 0)
        bookingLayout.Controls.Add(New Label With {.Text = "Event", .TextAlign = ContentAlignment.MiddleLeft, .Font = labelFont}, 2, 0)
        bookingLayout.Controls.Add(cmbEventID, 3, 0)

        ' Second row
        bookingLayout.Controls.Add(New Label With {.Text = "Service", .TextAlign = ContentAlignment.MiddleLeft, .Font = labelFont}, 0, 1)
        bookingLayout.Controls.Add(cmbServiceID, 1, 1)
        bookingLayout.Controls.Add(New Label With {.Text = "Amount", .TextAlign = ContentAlignment.MiddleLeft, .Font = labelFont}, 2, 1)
        bookingLayout.Controls.Add(txtAmount, 3, 1)

        ' Third row
        bookingLayout.Controls.Add(New Label With {.Text = "Booking Date", .TextAlign = ContentAlignment.MiddleLeft, .Font = labelFont}, 0, 2)
        bookingLayout.Controls.Add(dtpBookingDate, 1, 2)
        bookingLayout.Controls.Add(New Label With {.Text = "Event Date", .TextAlign = ContentAlignment.MiddleLeft, .Font = labelFont}, 2, 2)
        bookingLayout.Controls.Add(dtpEventDate, 3, 2)

        ' Fourth row
        bookingLayout.Controls.Add(New Label With {.Text = "Booking Time", .TextAlign = ContentAlignment.MiddleLeft, .Font = labelFont}, 0, 3)
        bookingLayout.Controls.Add(dtpTime, 1, 3)
        bookingLayout.Controls.Add(New Label With {.Text = "Status", .TextAlign = ContentAlignment.MiddleLeft, .Font = labelFont}, 2, 3)
        bookingLayout.Controls.Add(cmbStatus, 3, 3)

        ' Fifth row
        bookingLayout.Controls.Add(New Label With {.Text = "Payment Status", .TextAlign = ContentAlignment.MiddleLeft, .Font = labelFont}, 0, 4)
        bookingLayout.Controls.Add(cmbPayment, 1, 4)
        bookingLayout.Controls.Add(New Label With {.Text = "Refund Status", .TextAlign = ContentAlignment.MiddleLeft, .Font = labelFont}, 2, 4)
        bookingLayout.Controls.Add(cmbRefund, 3, 4)

        ' Sixth row
        bookingLayout.Controls.Add(New Label With {.Text = "Discount", .TextAlign = ContentAlignment.MiddleLeft, .Font = labelFont}, 0, 5)
        bookingLayout.Controls.Add(txtDiscount, 1, 5)

        ' Remarks (spans two columns)
        bookingLayout.Controls.Add(New Label With {.Text = "Remarks", .TextAlign = ContentAlignment.MiddleLeft, .Font = labelFont}, 0, 6)
        bookingLayout.SetColumnSpan(txtRemarks, 3)
        bookingLayout.Controls.Add(txtRemarks, 1, 6)

        ' Add the layout to the group box
        bookingGroupBox.Controls.Add(bookingLayout)

        ' Add group box to panel
        mainPanel.Controls.Add(bookingGroupBox)

        ' Create buttons with a nicer style
        Dim buttonsPanel As New FlowLayoutPanel With {
            .FlowDirection = FlowDirection.LeftToRight,
            .AutoSize = True,
            .Location = New Point(10, bookingGroupBox.Bottom + 15),
            .Anchor = AnchorStyles.Top Or AnchorStyles.Left,
            .Padding = New Padding(5),
            .BackColor = Color.Transparent
        }

        ' Button style function
        Dim CreateStyledButton = Function(text As String, icon As String) As Button
                                     Dim btn As New Button With {
                .text = text,
                .Font = New Font("Segoe UI", 9.75F, FontStyle.Regular),
                .FlatStyle = FlatStyle.Flat,
                .BackColor = Color.FromArgb(25, 118, 210),
                .ForeColor = Color.White,
                .Padding = New Padding(10, 5, 10, 5),
                .Margin = New Padding(10, 5, 10, 5),
                .Cursor = Cursors.Hand,
                .AutoSize = True
            }
                                     btn.FlatAppearance.BorderSize = 0
                                     Return btn
                                 End Function

        ' Create buttons
        Dim btnAdd As Button = CreateStyledButton("Add Booking", "add")
        AddHandler btnAdd.Click, AddressOf btnAdd_Click

        Dim btnUpdate As Button = CreateStyledButton("Update Booking", "update")
        AddHandler btnUpdate.Click, AddressOf btnUpdate_Click

        Dim btnDelete As Button = CreateStyledButton("Delete Booking", "delete")
        AddHandler btnDelete.Click, AddressOf btnDelete_Click

        Dim btnClear As Button = CreateStyledButton("Clear Form", "clear")
        AddHandler btnClear.Click, AddressOf btnClear_Click

        ' Additional management buttons
        Dim btnAddEvent As Button = CreateStyledButton("Create Event", "event")
        AddHandler btnAddEvent.Click, AddressOf btnAddEvent_Click
        btnAddEvent.BackColor = Color.FromArgb(67, 160, 71)

        Dim btnAddService As Button = CreateStyledButton("Create Service", "service")
        AddHandler btnAddService.Click, AddressOf btnAddService_Click
        btnAddService.BackColor = Color.FromArgb(67, 160, 71)

        ' Add buttons to panel
        buttonsPanel.Controls.AddRange({btnAdd, btnUpdate, btnDelete, btnClear, btnAddEvent, btnAddService})

        ' Add buttons panel to main panel
        mainPanel.Controls.Add(buttonsPanel)
    End Sub

    ' Load Bookings Data
    Private Sub LoadBookings()
        Dim query As String = "SELECT * FROM booking"
        Dim dt As DataTable = GetData(query)
        bookingsGrid.DataSource = dt
    End Sub

    ' Load Events Data
    Private Sub LoadEvents()
        Dim query As String = "SELECT * FROM event"
        Dim dt As DataTable = GetData(query)
        eventsGrid.DataSource = dt
    End Sub

    ' Load Services Data
    Private Sub LoadServices()
        Dim query As String = "SELECT * FROM service_availed"
        Dim dt As DataTable = GetData(query)
        servicesGrid.DataSource = dt
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs)
        Dim query As String = "INSERT INTO booking (CustomerID, EventID, ServiceID, BookingDate, BookedBy, BookingStatus, BookingTime, EventDate, TotalAmount, PaymentStatus, Remarks, DiscountApplied, RefundStatus, CreatedAt)
                           VALUES (@CustomerID, @EventID, @ServiceID, @BookingDate, @BookedBy, @BookingStatus, @BookingTime, @EventDate, @TotalAmount, @PaymentStatus, @Remarks, @DiscountApplied, @RefundStatus, NOW())"

        If cmbCustomerID.SelectedItem Is Nothing Or cmbEventID.SelectedItem Is Nothing Or cmbServiceID.SelectedItem Is Nothing Then
            MessageBox.Show("Please select all required fields (Customer, Event, Service).", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim selectedCustomerID As Object = DirectCast(cmbCustomerID.SelectedItem, ComboItem).Value
        Dim selectedEventID As Object = DirectCast(cmbEventID.SelectedItem, ComboItem).Value
        Dim selectedServiceID As Object = DirectCast(cmbServiceID.SelectedItem, ComboItem).Value

        Dim parameters As New Dictionary(Of String, Object) From {
            {"@CustomerID", selectedCustomerID},
            {"@EventID", selectedEventID},
            {"@ServiceID", selectedServiceID},
            {"@BookingDate", dtpBookingDate.Value},
            {"@BookedBy", SessionInfo.LoggedInUserFullName},
            {"@BookingStatus", cmbStatus.Text},
            {"@BookingTime", dtpTime.Value.ToString("HH:mm:ss")},
            {"@EventDate", dtpEventDate.Value},
            {"@TotalAmount", txtAmount.Text},
            {"@PaymentStatus", cmbPayment.Text},
            {"@Remarks", txtRemarks.Text},
            {"@DiscountApplied", txtDiscount.Text},
            {"@RefundStatus", cmbRefund.Text}
        }

        If ExecuteQuery(query, parameters) Then
            MessageBox.Show("Booking added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            LoadBookings()
            ClearFields()
        End If
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs)
        If bookingsGrid.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a booking to update.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim bookingID As Integer = Convert.ToInt32(bookingsGrid.SelectedRows(0).Cells("BookingID").Value)
        Dim query As String = "UPDATE booking SET CustomerID=@CustomerID, EventID=@EventID, ServiceID=@ServiceID, BookingDate=@BookingDate,
                                BookingStatus=@BookingStatus, BookingTime=@BookingTime, EventDate=@EventDate,
                                TotalAmount=@TotalAmount, PaymentStatus=@PaymentStatus, Remarks=@Remarks,
                                DiscountApplied=@DiscountApplied, RefundStatus=@RefundStatus
                                WHERE BookingID=@BookingID"
        Dim parameters As New Dictionary(Of String, Object) From {
            {"@BookingID", bookingID},
            {"@CustomerID", CType(cmbCustomerID.SelectedItem, ComboItem).Value},
            {"@EventID", CType(cmbEventID.SelectedItem, ComboItem).Value},
            {"@ServiceID", CType(cmbServiceID.SelectedItem, ComboItem).Value},
            {"@BookingDate", dtpBookingDate.Value},
            {"@BookingStatus", cmbStatus.Text},
            {"@BookingTime", dtpTime.Value.ToString("HH:mm:ss")},
            {"@EventDate", dtpEventDate.Value},
            {"@TotalAmount", txtAmount.Text},
            {"@PaymentStatus", cmbPayment.Text},
            {"@Remarks", txtRemarks.Text},
            {"@DiscountApplied", txtDiscount.Text},
            {"@RefundStatus", cmbRefund.Text}
        }
        If ExecuteQuery(query, parameters) Then
            MessageBox.Show("Booking updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            LoadBookings()
            ClearFields()
        End If
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs)
        If bookingsGrid.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a booking to delete.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim bookingID As Integer = Convert.ToInt32(bookingsGrid.SelectedRows(0).Cells("BookingID").Value)
        If MessageBox.Show("Are you sure you want to delete this booking?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Dim query As String = "DELETE FROM booking WHERE BookingID=@BookingID"
            Dim parameters As New Dictionary(Of String, Object) From {
                {"@BookingID", bookingID}
            }
            If ExecuteQuery(query, parameters) Then
                MessageBox.Show("Booking deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadBookings()
                ClearFields()
            End If
        End If
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs)
        ClearFields()
    End Sub

    Private Sub bookingsGrid_CellClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex >= 0 Then
            Dim row As DataGridViewRow = bookingsGrid.Rows(e.RowIndex)

            ' Find and select the matching customer, event and service
            cmbCustomerID.SelectedIndex = FindComboItemIndex(cmbCustomerID, GetCustomerNameByID(row.Cells("CustomerID").Value.ToString()))
            cmbEventID.SelectedIndex = FindComboItemIndex(cmbEventID, GetEventNameByID(row.Cells("EventID").Value.ToString()))
            cmbServiceID.SelectedIndex = FindComboItemIndex(cmbServiceID, GetServiceNameByID(row.Cells("ServiceID").Value.ToString()))

            ' Other assignments                        
            dtpTime.Value = DateTime.ParseExact(row.Cells("BookingTime").Value.ToString(), "HH:mm:ss", Nothing)
            txtAmount.Text = row.Cells("TotalAmount").Value.ToString()
            txtRemarks.Text = row.Cells("Remarks").Value.ToString()
            txtDiscount.Text = row.Cells("DiscountApplied").Value.ToString()

            ' Set combo box values safely
            SetComboBoxValue(cmbRefund, row.Cells("RefundStatus").Value.ToString())
            SetComboBoxValue(cmbStatus, row.Cells("BookingStatus").Value.ToString())
            SetComboBoxValue(cmbPayment, row.Cells("PaymentStatus").Value.ToString())

            dtpBookingDate.Value = Convert.ToDateTime(row.Cells("BookingDate").Value)
            dtpEventDate.Value = Convert.ToDateTime(row.Cells("EventDate").Value)
        End If
    End Sub

    ' Helper method to safely set combobox values
    Private Sub SetComboBoxValue(cmb As ComboBox, value As String)
        Dim index As Integer = cmb.FindStringExact(value)
        If index >= 0 Then
            cmb.SelectedIndex = index
        End If
    End Sub

    ' Helper method to find index in combobox by text
    Private Function FindComboItemIndex(cmb As ComboBox, text As String) As Integer
        For i As Integer = 0 To cmb.Items.Count - 1
            If cmb.Items(i).ToString() = text Then
                Return i
            End If
        Next
        Return -1
    End Function

    Private Sub btnAddEvent_Click(sender As Object, e As EventArgs)
        Dim eventForm As New Form With {
            .Text = "New Event",
            .Size = New Size(500, 650),
            .StartPosition = FormStartPosition.CenterParent,
            .FormBorderStyle = FormBorderStyle.FixedDialog,
            .MaximizeBox = False,
            .MinimizeBox = False
        }

        ' Create scrollable panel for content
        Dim scrollPanel As New Panel With {
            .Dock = DockStyle.Fill,
            .AutoScroll = True
        }
        eventForm.Controls.Add(scrollPanel)

        Dim layout As New TableLayoutPanel With {
            .Dock = DockStyle.Top,
            .AutoSize = True,
            .ColumnCount = 2,
            .RowCount = 13,
            .Padding = New Padding(15),
            .CellBorderStyle = TableLayoutPanelCellBorderStyle.Single
        }

        layout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 40))
        layout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 60))

        ' Set row height
        For i As Integer = 0 To 12
            layout.RowStyles.Add(New RowStyle(SizeType.Absolute, 42))
        Next

        ' Set title
        Dim titleLabel As New Label With {
            .Text = "Create New Event",
            .Font = New Font("Segoe UI", 14, FontStyle.Bold),
            .TextAlign = ContentAlignment.MiddleCenter,
            .Dock = DockStyle.Top,
            .Height = 50,
            .ForeColor = Color.FromArgb(25, 118, 210)
        }
        scrollPanel.Controls.Add(titleLabel)

        ' Form fields with improved styling
        Dim txtEventName As New TextBox With {
            .Dock = DockStyle.Fill,
            .Font = New Font("Segoe UI", 9.75F)
        }

        Dim cmbCustomer As New ComboBox() With {
            .DropDownStyle = ComboBoxStyle.DropDownList,
            .Dock = DockStyle.Fill,
            .Font = New Font("Segoe UI", 9.75F)
        }

        Dim cmbType As New ComboBox() With {
            .DropDownStyle = ComboBoxStyle.DropDownList,
            .Dock = DockStyle.Fill,
            .Font = New Font("Segoe UI", 9.75F)
        }
        cmbType.Items.AddRange(New String() {"Private", "Corporate"})
        cmbType.SelectedIndex = 0

        Dim dtpDate As New DateTimePicker() With {
            .Format = DateTimePickerFormat.Short,
            .Dock = DockStyle.Fill,
            .Font = New Font("Segoe UI", 9.75F)
        }

        Dim dtpStart As New DateTimePicker() With {
            .Format = DateTimePickerFormat.Time,
            .ShowUpDown = True,
            .Dock = DockStyle.Fill,
            .Font = New Font("Segoe UI", 9.75F)
        }

        Dim dtpEnd As New DateTimePicker() With {
            .Format = DateTimePickerFormat.Time,
            .ShowUpDown = True,
            .Dock = DockStyle.Fill,
            .Font = New Font("Segoe UI", 9.75F)
        }

        Dim dtpSetup As New DateTimePicker() With {
            .Format = DateTimePickerFormat.Time,
            .ShowUpDown = True,
            .Dock = DockStyle.Fill,
            .Font = New Font("Segoe UI", 9.75F)
        }

        Dim dtpCleanup As New DateTimePicker() With {
            .Format = DateTimePickerFormat.Time,
            .ShowUpDown = True,
            .Dock = DockStyle.Fill,
            .Font = New Font("Segoe UI", 9.75F)
        }

        Dim cmbVenue As New ComboBox() With {
            .DropDownStyle = ComboBoxStyle.DropDownList,
            .Dock = DockStyle.Fill,
            .Font = New Font("Segoe UI", 9.75F)
        }

        cmbVenue.Items.AddRange(New String() {
            "Albay Astrodome",
            "Legazpi Convention Center",
            "Ibalong Centrum for Recreation",
            "Penaranda Park",
            "Cagsawa Ruins Park",
            "Lignon Hill Nature Park",
            "Pacific Mall Event Center",
            "Embarcadero de Legazpi",
            "Avenue Plaza Hotel",
            "CWC (Camsur Watersports Complex)",
            "Villa Caceres Hotel",
            "Biggs Diner Function Hall",
            "Naga City Civic Center",
            "Bicol University Gymnasium",
            "Jardin Real de Naga",
            "Ateneo de Naga University Gym",
            "Sorsogon Capitol Park",
            "Rizal Beach Resort",
            "Misibis Bay Resort",
            "Balay Cena Una",
            "Hotel Venezia",
            "Doña Mercedes Country Lodge"
        })

        cmbVenue.SelectedIndex = 0

        Dim txtGuests As New TextBox With {
            .Dock = DockStyle.Fill,
            .Font = New Font("Segoe UI", 9.75F)
        }

        Dim txtTheme As New TextBox With {
            .Dock = DockStyle.Fill,
            .Font = New Font("Segoe UI", 9.75F)
        }

        Dim txtRequests As New TextBox With {
            .Dock = DockStyle.Fill,
            .Multiline = True,
            .Font = New Font("Segoe UI", 9.75F)
        }

        ' Create label style
        Dim labelFont As New Font("Segoe UI", 9.75F)
        Dim labelStyle = Function(text As String) As Label
                             Return New Label With {
                                 .text = text,
                                 .TextAlign = ContentAlignment.MiddleLeft,
                                 .Font = labelFont,
                                 .Dock = DockStyle.Fill
                             }
                         End Function

        ' Populate customers
        Dim customerQuery As String = "SELECT CustomerID, FirstName, LastName, MiddleName FROM customer"
                             Dim dt As DataTable = GetData(customerQuery)
                             For Each row As DataRow In dt.Rows
                                 Dim fullName As String = $"{row("FirstName")} {row("MiddleName")} {row("LastName")}".Trim()
                                 cmbCustomer.Items.Add(New ComboItem(fullName, row("CustomerID")))
                             Next
                             If cmbCustomer.Items.Count > 0 Then cmbCustomer.SelectedIndex = 0

                             ' Add controls to layout
                             layout.Controls.Add(labelStyle("Event Name"), 0, 0)
                             layout.Controls.Add(txtEventName, 1, 0)
                             layout.Controls.Add(labelStyle("Customer"), 0, 1)
                             layout.Controls.Add(cmbCustomer, 1, 1)
                             layout.Controls.Add(labelStyle("Event Type"), 0, 2)
                             layout.Controls.Add(cmbType, 1, 2)
                             layout.Controls.Add(labelStyle("Date"), 0, 3)
                             layout.Controls.Add(dtpDate, 1, 3)
                             layout.Controls.Add(labelStyle("Start Time"), 0, 4)
                             layout.Controls.Add(dtpStart, 1, 4)
                             layout.Controls.Add(labelStyle("End Time"), 0, 5)
                             layout.Controls.Add(dtpEnd, 1, 5)
                             layout.Controls.Add(labelStyle("Venue"), 0, 6)
                             layout.Controls.Add(cmbVenue, 1, 6)
                             layout.Controls.Add(labelStyle("Guest Count"), 0, 7)
                             layout.Controls.Add(txtGuests, 1, 7)
                             layout.Controls.Add(labelStyle("Theme"), 0, 8)
                             layout.Controls.Add(txtTheme, 1, 8)
                             layout.Controls.Add(labelStyle("Special Requests"), 0, 9)
                             layout.Controls.Add(txtRequests, 1, 9)
                             layout.Controls.Add(labelStyle("Setup Time"), 0, 10)
                             layout.Controls.Add(dtpSetup, 1, 10)
                             layout.Controls.Add(labelStyle("Cleanup Time"), 0, 11)
                             layout.Controls.Add(dtpCleanup, 1, 11)

        ' Submit button with nice styling
        Dim btnSubmit As New Button With {
            .text = "Create Event",
            .Font = New Font("Segoe UI", 10.0F, FontStyle.Bold),
            .FlatStyle = FlatStyle.Flat,
            .BackColor = Color.FromArgb(25, 118, 210),
            .ForeColor = Color.White,
            .Dock = DockStyle.Fill,
            .Cursor = Cursors.Hand
        }
        btnSubmit.FlatAppearance.BorderSize = 0

        layout.Controls.Add(btnSubmit, 1, 12)

                             ' Add handler for submit button
                             AddHandler btnSubmit.Click, Sub()
                                                             If cmbCustomer.SelectedItem Is Nothing Then
                                                                 MessageBox.Show("Please select a customer.", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                                                                 Return
                                                             End If

                                                             Dim selectedCustomer As ComboItem = CType(cmbCustomer.SelectedItem, ComboItem)
                                                             Dim query = "INSERT INTO event (EventName, CustomerID, EventType, EventDate, StartTime, EndTime, VenueLocation, GuestCount, Theme, SpecialRequests, SetupTime, CleanupTime, Status, CreatedDate)
                VALUES (@EventName, @CustomerID, @EventType, @EventDate, @StartTime, @EndTime, @VenueLocation, @GuestCount, @Theme, @SpecialRequests, @SetupTime, @CleanupTime, 'Upcoming', NOW())"

                                                             Dim parameters As New Dictionary(Of String, Object) From {
                {"@EventName", txtEventName.Text},
                {"@CustomerID", selectedCustomer.Value},
                {"@EventType", cmbType.Text},
                {"@EventDate", dtpDate.Value},
                {"@StartTime", dtpStart.Value.ToString("HH:mm:ss")},
                {"@EndTime", dtpEnd.Value.ToString("HH:mm:ss")},
                {"@VenueLocation", cmbVenue.Text},
                {"@GuestCount", txtGuests.Text},
                {"@Theme", txtTheme.Text},
                {"@SpecialRequests", txtRequests.Text},
                {"@SetupTime", dtpSetup.Value.ToString("HH:mm:ss")},
                {"@CleanupTime", dtpCleanup.Value.ToString("HH:mm:ss")}
            }

                                                             If ExecuteQuery(query, parameters) Then
                                                                 MessageBox.Show("Event created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                                                 LoadEvents() ' Refresh events grid
                                                                 PopulateEventsDropdown() ' Refresh events dropdown
                                                                 eventForm.Close()
                                                             Else
                                                                 MessageBox.Show("Failed to create event.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                                             End If
                                                         End Sub

                             ' Add layout to form
                             scrollPanel.Controls.Add(layout)
                             layout.Location = New Point(0, titleLabel.Bottom)

                             ' Show form as dialog
                             eventForm.ShowDialog()
    End Sub

    Private Sub btnAddService_Click(sender As Object, e As EventArgs)
        Dim serviceForm As New Form With {
            .Text = "New Service",
            .Size = New Size(500, 550),
            .StartPosition = FormStartPosition.CenterParent,
            .FormBorderStyle = FormBorderStyle.FixedDialog,
            .MaximizeBox = False,
            .MinimizeBox = False
        }

        ' Create scrollable panel
        Dim scrollPanel As New Panel With {
            .Dock = DockStyle.Fill,
            .AutoScroll = True
        }
        serviceForm.Controls.Add(scrollPanel)

        ' Title label
        Dim titleLabel As New Label With {
            .Text = "Create New Service",
            .Font = New Font("Segoe UI", 14, FontStyle.Bold),
            .TextAlign = ContentAlignment.MiddleCenter,
            .Dock = DockStyle.Top,
            .Height = 50,
            .ForeColor = Color.FromArgb(67, 160, 71)
        }
        scrollPanel.Controls.Add(titleLabel)

        ' Main layout
        Dim layout As New TableLayoutPanel With {
            .AutoSize = True,
            .ColumnCount = 2,
            .RowCount = 11,
            .Padding = New Padding(15),
            .CellBorderStyle = TableLayoutPanelCellBorderStyle.Single
        }
        layout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 40))
        layout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 60))

        ' Set row height
        For i As Integer = 0 To 10
            layout.RowStyles.Add(New RowStyle(SizeType.Absolute, 42))
        Next

        ' Form controls with improved styling
        Dim txtName As New TextBox With {
            .Dock = DockStyle.Fill,
            .Font = New Font("Segoe UI", 9.75F)
        }

        Dim txtDesc As New TextBox With {
            .Dock = DockStyle.Fill,
            .Multiline = True,
            .Font = New Font("Segoe UI", 9.75F)
        }

        Dim cmbCat As New ComboBox() With {
            .DropDownStyle = ComboBoxStyle.DropDownList,
            .Dock = DockStyle.Fill,
            .Font = New Font("Segoe UI", 9.75F)
        }
        cmbCat.Items.AddRange(New String() {"Decoration", "Food", "Audio-Visual", "Entertainment", "Photography", "Videography", "Venue Setup", "Transportation", "Accommodation", "Staffing"})
        cmbCat.SelectedIndex = 0

        Dim txtPrice As New TextBox With {
            .Dock = DockStyle.Fill,
            .Font = New Font("Segoe UI", 9.75F)
        }

        Dim cmbUnit As New ComboBox() With {
            .DropDownStyle = ComboBoxStyle.DropDownList,
            .Dock = DockStyle.Fill,
            .Font = New Font("Segoe UI", 9.75F)
        }
        cmbUnit.Items.AddRange(New String() {"Per Hour", "Per Event", "Per Guest", "Per Day", "Per Item", "Fixed Price"})
        cmbUnit.SelectedIndex = 0

        Dim cmbAvail As New ComboBox() With {
            .DropDownStyle = ComboBoxStyle.DropDownList,
            .Dock = DockStyle.Fill,
            .Font = New Font("Segoe UI", 9.75F)
        }
        cmbAvail.Items.AddRange(New String() {"Available", "Unavailable", "Limited"})
        cmbAvail.SelectedIndex = 0

        Dim txtSetupReq As New TextBox With {
            .Dock = DockStyle.Fill,
            .Font = New Font("Segoe UI", 9.75F)
        }

        Dim dtpDuration As New DateTimePicker() With {
            .Format = DateTimePickerFormat.Time,
            .ShowUpDown = True,
            .Dock = DockStyle.Fill,
            .Font = New Font("Segoe UI", 9.75F)
        }

        Dim txtMinG As New TextBox With {
            .Dock = DockStyle.Fill,
            .Font = New Font("Segoe UI", 9.75F)
        }

        Dim txtMaxG As New TextBox With {
            .Dock = DockStyle.Fill,
            .Font = New Font("Segoe UI", 9.75F)
        }

        ' Create label style
        Dim labelFont As New Font("Segoe UI", 9.75F)
        Dim labelStyle = Function(text As String) As Label
                             Return New Label With {
                .text = text,
                .TextAlign = ContentAlignment.MiddleLeft,
                .Font = labelFont,
                .Dock = DockStyle.Fill
            }
                         End Function

        ' Add controls to layout
        layout.Controls.Add(labelStyle("Service Name"), 0, 0)
        layout.Controls.Add(txtName, 1, 0)
        layout.Controls.Add(labelStyle("Description"), 0, 1)
        layout.Controls.Add(txtDesc, 1, 1)
        layout.Controls.Add(labelStyle("Category"), 0, 2)
        layout.Controls.Add(cmbCat, 1, 2)
        layout.Controls.Add(labelStyle("Price"), 0, 3)
        layout.Controls.Add(txtPrice, 1, 3)
        layout.Controls.Add(labelStyle("Unit"), 0, 4)
        layout.Controls.Add(cmbUnit, 1, 4)
        layout.Controls.Add(labelStyle("Availability"), 0, 5)
        layout.Controls.Add(cmbAvail, 1, 5)
        layout.Controls.Add(labelStyle("Setup Required"), 0, 6)
        layout.Controls.Add(txtSetupReq, 1, 6)
        layout.Controls.Add(labelStyle("Duration Estimate"), 0, 7)
        layout.Controls.Add(dtpDuration, 1, 7)
        layout.Controls.Add(labelStyle("Min Guests"), 0, 8)
        layout.Controls.Add(txtMinG, 1, 8)
        layout.Controls.Add(labelStyle("Max Guests"), 0, 9)
        layout.Controls.Add(txtMaxG, 1, 9)

        ' Create nicely styled submit button
        Dim btnSubmit As New Button With {
            .Text = "Create Service",
            .Font = New Font("Segoe UI", 10.0F, FontStyle.Bold),
            .FlatStyle = FlatStyle.Flat,
            .BackColor = Color.FromArgb(67, 160, 71),
            .ForeColor = Color.White,
            .Dock = DockStyle.Fill,
            .Cursor = Cursors.Hand
        }
        btnSubmit.FlatAppearance.BorderSize = 0
        layout.Controls.Add(btnSubmit, 1, 10)

        ' Add handler for submit button
        AddHandler btnSubmit.Click, Sub()
                                        If String.IsNullOrWhiteSpace(txtName.Text) Then
                                            MessageBox.Show("Please enter a service name.", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                                            Return
                                        End If

                                        Dim query = "INSERT INTO service_availed (ServiceName, Description, Category, Price, Unit, Availability, SetupRequired, DurationEstimate, MinGuest, MaxGuest, CreatedBy, CreatedDate, UpdatedDate, Status)
                VALUES (@ServiceName, @Description, @Category, @Price, @Unit, @Availability, @SetupRequired, @DurationEstimate, @MinGuest, @MaxGuest, @CreatedBy, NOW(), NOW(), 'Active')"

                                        Dim parameters As New Dictionary(Of String, Object) From {
                {"@ServiceName", txtName.Text},
                {"@Description", txtDesc.Text},
                {"@Category", cmbCat.Text},
                {"@Price", txtPrice.Text},
                {"@Unit", cmbUnit.Text},
                {"@Availability", cmbAvail.Text},
                {"@SetupRequired", txtSetupReq.Text},
                {"@DurationEstimate", dtpDuration.Value.ToString("HH:mm:ss")},
                {"@MinGuest", txtMinG.Text},
                {"@MaxGuest", txtMaxG.Text},
                {"@CreatedBy", SessionInfo.LoggedInUserFullName}
            }

                                        If ExecuteQuery(query, parameters) Then
                                            MessageBox.Show("Service created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                            LoadServices() ' Refresh services grid
                                            PopulateServicesDropdown() ' Refresh services dropdown
                                            serviceForm.Close()
                                        Else
                                            MessageBox.Show("Failed to create service.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                        End If
                                    End Sub

        ' Add layout to panel
        scrollPanel.Controls.Add(layout)
        layout.Location = New Point(0, titleLabel.Bottom)

        ' Show dialog
        serviceForm.ShowDialog()
    End Sub

    Private Sub PopulateCustomerDropdown()
        Dim query As String = "SELECT CustomerID, FirstName, LastName, MiddleName FROM customer WHERE Status = 'Active'"
        Dim dt As DataTable = GetData(query)
        cmbCustomerID.Items.Clear()
        For Each row As DataRow In dt.Rows
            Dim fullName As String = $"{row("FirstName")} {row("MiddleName")} {row("LastName")}"
            cmbCustomerID.Items.Add(New ComboItem(fullName.Trim(), row("CustomerID")))
        Next
        If cmbCustomerID.Items.Count > 0 Then
            cmbCustomerID.SelectedIndex = 0
        End If
    End Sub

    Private Sub PopulateEventsDropdown()
        Dim query As String = "SELECT EventID, EventName FROM event WHERE Status = 'Upcoming'"
        Dim dt As DataTable = GetData(query)
        cmbEventID.Items.Clear()
        For Each row As DataRow In dt.Rows
            cmbEventID.Items.Add(New ComboItem(row("EventName").ToString(), row("EventID")))
        Next
        If cmbEventID.Items.Count > 0 Then
            cmbEventID.SelectedIndex = 0
        End If
    End Sub

    Private Sub PopulateServicesDropdown()
        Dim query As String = "SELECT ServiceID, ServiceName FROM service_availed WHERE Status = 'Active'"
        Dim dt As DataTable = GetData(query)
        cmbServiceID.Items.Clear()
        For Each row As DataRow In dt.Rows
            cmbServiceID.Items.Add(New ComboItem(row("ServiceName").ToString(), row("ServiceID")))
        Next
        If cmbServiceID.Items.Count > 0 Then
            cmbServiceID.SelectedIndex = 0
        End If
    End Sub

    Private Function GetCustomerNameByID(CustomerID As String) As String
        For Each item As ComboItem In cmbCustomerID.Items
            If item.Value.ToString() = CustomerID Then
                Return item.Text
            End If
        Next
        Return ""
    End Function

    Private Function GetEventNameByID(eventID As String) As String
        For Each item As ComboItem In cmbEventID.Items
            If item.Value.ToString() = eventID Then
                Return item.Text
            End If
        Next
        Return ""
    End Function

    Private Function GetServiceNameByID(serviceID As String) As String
        For Each item As ComboItem In cmbServiceID.Items
            If item.Value.ToString() = serviceID Then
                Return item.Text
            End If
        Next
        Return ""
    End Function

    Private Sub ClearFields()
        If cmbCustomerID.Items.Count > 0 Then cmbCustomerID.SelectedIndex = 0 Else cmbCustomerID.SelectedIndex = -1
        If cmbEventID.Items.Count > 0 Then cmbEventID.SelectedIndex = 0 Else cmbEventID.SelectedIndex = -1
        If cmbServiceID.Items.Count > 0 Then cmbServiceID.SelectedIndex = 0 Else cmbServiceID.SelectedIndex = -1

        dtpTime.Value = DateTime.Now
        txtAmount.Clear()
        txtRemarks.Clear()
        txtDiscount.Clear()

        If cmbRefund.Items.Count > 0 Then cmbRefund.SelectedIndex = 0 Else cmbRefund.SelectedIndex = -1
        If cmbStatus.Items.Count > 0 Then cmbStatus.SelectedIndex = 0 Else cmbStatus.SelectedIndex = -1
        If cmbPayment.Items.Count > 0 Then cmbPayment.SelectedIndex = 0 Else cmbPayment.SelectedIndex = -1

        dtpBookingDate.Value = DateTime.Now
        dtpEventDate.Value = DateTime.Now
    End Sub
End Class
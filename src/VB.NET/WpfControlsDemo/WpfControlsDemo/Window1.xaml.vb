﻿Imports Microsoft.Win32
Imports Dynamsoft.DotNet.TWAIN
Imports Dynamsoft.DotNet.TWAIN.Barcode

Namespace WpfControlsDemo
    Partial Public Class Window1
        Inherits Window
        Shared Sub New()
            Dim index As Integer
            index = System.Reflection.Assembly.GetExecutingAssembly().Location.IndexOf("Samples")
            If index <> -1 Then
                strCurrentDirectory = System.Reflection.Assembly.GetExecutingAssembly().Location.Substring(0, index)
                imageDirectory = strCurrentDirectory + "Samples\\Bin\\Images\\WpfDemoImages\\"
            Else
                index = System.Reflection.Assembly.GetExecutingAssembly().Location.LastIndexOf("\\")
                If index <> -1 Then
                    strCurrentDirectory = System.Reflection.Assembly.GetExecutingAssembly().Location.Substring(0, index + 1)
                Else
                    strCurrentDirectory = Environment.CurrentDirectory + "\\"
                End If
                imageDirectory = strCurrentDirectory + "Images\\WpfDemoImages\\"
            End If

            icons = New Dictionary(Of String, ImageBrush)()
        End Sub

        Public Sub New()
            InitializeComponent()
            Try
                dpTitle.Background = New ImageBrush(New BitmapImage(New Uri(imageDirectory + "title.png", UriKind.RelativeOrAbsolute)))
                Dim ibd As IconBitmapDecoder : ibd = New IconBitmapDecoder(New Uri(imageDirectory + "dnt_demo_icon.ico", UriKind.RelativeOrAbsolute), BitmapCreateOptions.None, BitmapCacheOption.Default)
                Me.Icon = ibd.Frames(0)
            Catch ex As Exception
            End Try

            dynamicDotNetTwainThum.MouseShape = True
            dynamicDotNetTwainThum.SetViewMode(1, 4)
            dynamicDotNetTwainThum.AllowMultiSelect = True
            Dim dynamicDotNetTwainDirectory As String
            dynamicDotNetTwainDirectory = strCurrentDirectory
            Dim index As Integer
            index = System.Reflection.Assembly.GetExecutingAssembly().Location.IndexOf("Samples")
            If index <> -1 Then
                dynamicDotNetTwainThum.BarcodeDllPath = dynamicDotNetTwainDirectory + "Redistributable\\BarcodeResources\\"
                dynamicDotNetTwainThum.OCRDllPath = dynamicDotNetTwainDirectory + "Redistributable\\OCRResources\\"
                dynamicDotNetTwainThum.OCRTessDataPath = dynamicDotNetTwainDirectory + "Samples\\Bin\\"
            Else
                dynamicDotNetTwainThum.BarcodeDllPath = dynamicDotNetTwainDirectory + "Redistributable\\BarcodeResources\\"
                dynamicDotNetTwainThum.OCRDllPath = dynamicDotNetTwainDirectory + "Redistributable\\OCRResources\\"
                dynamicDotNetTwainThum.OCRTessDataPath = dynamicDotNetTwainDirectory + "Redistributable\\"

            End If
           

            dynamicDotNetTwainThum.ScanInNewThread = True
            dynamicDotNetTwainThum.MaxImagesInBuffer = 32
            dynamicDotNetTwainThum.EnableKeyboardInteractive = False

            dynamicDotNetTwainView.MouseShape = False
            dynamicDotNetTwainView.SetViewMode(-1, -1)
            dynamicDotNetTwainView.MaxImagesInBuffer = 1
            dynamicDotNetTwainView.ScanInNewThread = True
        End Sub

        Public Shared icons As New System.Collections.Generic.Dictionary(Of String, System.Windows.Media.ImageBrush)
        Public Shared ReadOnly imageDirectory As String
        Public Shared ReadOnly strCurrentDirectory As String

        Private Sub Button_MouseEnter(ByVal sender As Object, ByVal e As MouseEventArgs)
            Dim btn As Button : btn = sender

            If btn.Name = "btnHand" And dynamicDotNetTwainView.MouseShape Then
                Return
            ElseIf btn.Name = "btnArrow" And Not dynamicDotNetTwainView.MouseShape Then
                Return
            End If

            Dim key As String : key = "hover/" + btn.Tag
            If (Not icons.ContainsKey(key)) Then
                Try
                    icons.Add(key, New ImageBrush(New BitmapImage(New Uri(imageDirectory + key + ".png", UriKind.RelativeOrAbsolute))))
                Catch ex As Exception
                End Try
            End If
            Try
                btn.Background = icons(key)
            Catch ex As Exception
            End Try
        End Sub

        Private Sub Button_MouseLeave(ByVal sender As Object, ByVal e As MouseEventArgs)
            Dim btn As Button : btn = sender

            If btn.Name = "btnHand" And dynamicDotNetTwainView.MouseShape Then
                Return
            ElseIf btn.Name = "btnArrow" And Not dynamicDotNetTwainView.MouseShape Then
                Return
            End If

            Dim key As String : key = "normal/" + btn.Tag
            If (Not icons.ContainsKey(key)) Then
                Try
                    icons.Add(key, New ImageBrush(New BitmapImage(New Uri(imageDirectory + key + ".png", UriKind.RelativeOrAbsolute))))
                Catch ex As Exception
                End Try
            End If
            Try
                btn.Background = icons(key)
            Catch ex As Exception
            End Try
        End Sub

        Private Sub Button_Loaded(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Button_MouseLeave(sender, Nothing)

            Dim btn As Button : btn = DirectCast(sender, Button)
            If btn.Name = "btnHand" And dynamicDotNetTwainView.MouseShape Then
                Dim args As MouseButtonEventArgs : args = New MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left)
                args.RoutedEvent = Button.PreviewMouseDownEvent
                Button_PreviewMouseDown(sender, args)

                Dim argsA As MouseEventArgs : argsA = New MouseEventArgs(Mouse.PrimaryDevice, 0)
                argsA.RoutedEvent = Button.MouseLeaveEvent
                Button_MouseLeave(btnArrow, argsA)
            ElseIf btn.Name = "btnArrow" And Not dynamicDotNetTwainView.MouseShape Then
                Dim args As MouseButtonEventArgs : args = New MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left)
                args.RoutedEvent = Button.PreviewMouseDownEvent
                Button_PreviewMouseDown(sender, args)

                Dim argsA As MouseEventArgs : argsA = New MouseEventArgs(Mouse.PrimaryDevice, 0)
                argsA.RoutedEvent = Button.MouseLeaveEvent
                Button_MouseLeave(btnHand, argsA)
            End If
        End Sub

        Private Sub Button_PreviewMouseDown(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
            If e.ChangedButton = MouseButton.Left Then
                Dim btn As Button : btn = sender
                Dim key As String : key = "active/" + btn.Tag
                If (Not icons.ContainsKey(key)) Then
                    Try
                        icons.Add(key, New ImageBrush(New BitmapImage(New Uri(imageDirectory + key + ".png", UriKind.RelativeOrAbsolute))))
                    Catch ex As Exception
                    End Try
                End If
                Try
                    btn.Background = icons(key)
                Catch ex As Exception
                End Try
            End If
        End Sub

        Private Sub Button_PreviewMouseUp(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
            Button_MouseLeave(sender, e)
        End Sub

        Private Sub CloseWindow(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Me.Close()
        End Sub

        Private Sub MaxWindow(ByVal sender As Object, ByVal e As RoutedEventArgs)
            If (WindowState = WindowState.Maximized) Then
                WindowState = WindowState.Normal
            ElseIf (WindowState = WindowState.Normal) Then
                WindowState = WindowState.Maximized
            End If
        End Sub

        Private Sub MinWindow(ByVal sender As Object, ByVal e As RoutedEventArgs)
            WindowState = WindowState.Minimized
        End Sub

        Private Sub MoveWindow(ByVal sender As Object, ByVal e As MouseButtonEventArgs) Handles dpTitle.MouseLeftButtonDown
            DragMove()
        End Sub

        Private Sub UpdateImageInfo()
            Clipboard.Clear()
            dynamicDotNetTwainThum.CopyToClipboard(dynamicDotNetTwainThum.CurrentImageIndexInBuffer)
            dynamicDotNetTwainView.RemoveAllImages()
            dynamicDotNetTwainView.LoadDibFromClipboard()
            Clipboard.Clear()

            tbAll.Text = dynamicDotNetTwainThum.HowManyImagesInBuffer.ToString()
            tbCurrent.Text = (dynamicDotNetTwainThum.CurrentImageIndexInBuffer + 1).ToString()
        End Sub

        Private Sub dynamicDotNetTwainThum_OnDNTKeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles dynamicDotNetTwainThum.OnDNTKeyDown
            Select Case (e.Key)
                Case Key.PageDown, Key.PageUp, Key.Home, Key.End, Key.Left, Key.Right, Key.Up, Key.Down
                    UpdateImageInfo()
            End Select
        End Sub

        Private Sub dynamicDotNetTwainThum_OnMouseClick(ByVal sender As Object, ByVal e As OnMouseClickEventArgs) Handles dynamicDotNetTwainThum.OnMouseClick
            UpdateImageInfo()
        End Sub

        Private Sub dynamicDotNetTwainThum_OnPostAllTransfers(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles dynamicDotNetTwainThum.OnPostAllTransfers
            UpdateImageInfo()
        End Sub

        Private Sub Scan_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Dim scanWnd As ScanWindow : scanWnd = New ScanWindow()
            scanWnd.Owner = Me
            scanWnd.TWAIN = dynamicDotNetTwainThum
            scanWnd.WindowStartupLocation = WindowStartupLocation.CenterOwner
            scanWnd.ShowDialog()
        End Sub

        Private Sub Load_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Dim dlg As OpenFileDialog : dlg = New OpenFileDialog()
            dlg.Filter = "All Supported Files(*.jpg;*.jpe;*.jpeg;*.jfif;*.bmp;*.png;*.tif;*.tiff;*.pdf;*.gif)|*.jpg;*.jpe;*.jpeg;*.jfif;*.bmp;*.png;*.tif;*.tiff;*.pdf;*.gif|JPEG(*.jpg;*.jpeg;*.jpe;*.jfif)|*.jpg;*.jpeg;*.jpe;*.jfif|BMP(*.bmp)|*.bmp|PNG(*.png)|*.png|TIFF(*.tif;*.tiff)|*.tif;*.tiff|PDF(*.pdf)|*.pdf|GIF(*.gif)|*.gif"
            dlg.Multiselect = True
            If (dlg.ShowDialog().Value) Then
                For Each strFileName In dlg.FileNames
                    dynamicDotNetTwainThum.LoadImage(strFileName)
                Next
                UpdateImageInfo()
            End If
        End Sub

        Private Sub Barcode_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            If (dynamicDotNetTwainThum.HowManyImagesInBuffer > 0) Then
                dynamicDotNetTwainThum.IfShowCancelDialogWhenBarcodeOrOCR = True
                Dim results As Result() : results = dynamicDotNetTwainThum.ReadBarcode(dynamicDotNetTwainThum.CurrentImageIndexInBuffer, Dynamsoft.DotNet.TWAIN.Enums.Barcode.BarcodeFormat.All)
                Dim strResult As String : strResult = results.Length.ToString() + " total barcode found." + Chr(10) & Chr(13)
                Dim i As Integer
                For i = 0 To results.Length - 1
                    strResult += "Result " + (i + 1).ToString() + Chr(10) & Chr(13) + " Barcode Format:" + results(i).BarcodeFormat.ToString() + "    Barcode Text:" + results(i).Text + Chr(10) & Chr(13)
                Next i
                MessageBox.Show(strResult, "Barcodes Results")
            End If
        End Sub

        Private Sub OCR_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            If (dynamicDotNetTwainThum.CurrentSelectedImageIndicesInBuffer.Count > 1) Or (dynamicDotNetTwainThum.CurrentSelectedImageIndicesInBuffer.Count = 1 And dynamicDotNetTwainThum.CurrentSelectedImageIndicesInBuffer(0) >= 0) Then
                dynamicDotNetTwainThum.IfShowCancelDialogWhenBarcodeOrOCR = True
                dynamicDotNetTwainThum.OCRResultFormat = Dynamsoft.DotNet.TWAIN.OCR.ResultFormat.Text
                dynamicDotNetTwainThum.OCRLanguage = "eng"
                Dim bytes As Byte() : bytes = dynamicDotNetTwainThum.OCR(dynamicDotNetTwainThum.CurrentSelectedImageIndicesInBuffer)
                If bytes IsNot Nothing Then
                    Dim dlg As SaveFileDialog : dlg = New SaveFileDialog()
                    dlg.Filter = "Text File(*.txt)|*.txt"
                    If (dlg.ShowDialog().Value) Then
                        System.IO.File.WriteAllBytes(dlg.FileName, bytes)
                    End If
                End If
            End If
        End Sub

        Private Sub Hand_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            dynamicDotNetTwainView.MouseShape = True

            Dim args As MouseButtonEventArgs : args = New MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left)
            args.RoutedEvent = Button.PreviewMouseDownEvent
            Button_PreviewMouseDown(sender, args)

            Dim argsA As MouseEventArgs : argsA = New MouseEventArgs(Mouse.PrimaryDevice, 0)
            argsA.RoutedEvent = Button.MouseLeaveEvent
            Button_MouseLeave(btnArrow, argsA)
        End Sub

        Private Sub Arrow_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            dynamicDotNetTwainView.MouseShape = False

            Dim args As MouseButtonEventArgs : args = New MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left)
            args.RoutedEvent = Button.PreviewMouseDownEvent
            Button_PreviewMouseDown(sender, args)

            Dim argsA As MouseEventArgs : argsA = New MouseEventArgs(Mouse.PrimaryDevice, 0)
            argsA.RoutedEvent = Button.MouseLeaveEvent
            Button_MouseLeave(btnHand, argsA)
        End Sub

        Private Sub Zoom_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Dim zoomWnd As ZoomWindow : zoomWnd = New ZoomWindow(dynamicDotNetTwainView.Zoom)
            zoomWnd.Owner = Me
            If (zoomWnd.ShowDialog().Value) Then
                dynamicDotNetTwainView.Zoom = zoomWnd.ZoomRatio
            End If
        End Sub

        Private Sub Switch_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            If (Not dynamicDotNetTwainThum.CurrentSelectedImageIndicesInBuffer.Count = 2) Then
                MessageBox.Show("Please select two images before switching. You can select multiple images by pressing CTRL key and clicking mouse.", "switch warning", MessageBoxButton.OK, MessageBoxImage.Warning)
            Else
                dynamicDotNetTwainThum.SwitchImage(CShort(dynamicDotNetTwainThum.CurrentSelectedImageIndicesInBuffer(0)), CShort(dynamicDotNetTwainThum.CurrentSelectedImageIndicesInBuffer(1)))
            End If

            UpdateImageInfo()
        End Sub

        Private Sub RotateRight_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            If (dynamicDotNetTwainThum.CurrentImageIndexInBuffer >= 0) Then
                dynamicDotNetTwainThum.RotateRight(dynamicDotNetTwainThum.CurrentImageIndexInBuffer)
                UpdateImageInfo()
            End If
        End Sub

        Private Sub RotateLeft_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            If (dynamicDotNetTwainThum.CurrentImageIndexInBuffer >= 0) Then
                dynamicDotNetTwainThum.RotateLeft(dynamicDotNetTwainThum.CurrentImageIndexInBuffer)
                UpdateImageInfo()
            End If
        End Sub

        Private Sub Mirror_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            If (dynamicDotNetTwainThum.CurrentImageIndexInBuffer >= 0) Then
                dynamicDotNetTwainThum.Mirror(dynamicDotNetTwainThum.CurrentImageIndexInBuffer)
                UpdateImageInfo()
            End If
        End Sub

        Private Sub Flip_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            If (dynamicDotNetTwainThum.CurrentImageIndexInBuffer >= 0) Then
                dynamicDotNetTwainThum.Flip(dynamicDotNetTwainThum.CurrentImageIndexInBuffer)
                UpdateImageInfo()
            End If
        End Sub

        Private Sub FitWindow_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            dynamicDotNetTwainView.IfFitWindow = True
        End Sub

        Private Sub OriginalSize_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            dynamicDotNetTwainView.Zoom = 1
            dynamicDotNetTwainView.IfFitWindow = False
        End Sub

        Private Sub Cut_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Dim index As Short : index = dynamicDotNetTwainThum.CurrentImageIndexInBuffer
            If (index >= 0) Then
                Dim rect As Rect : rect = dynamicDotNetTwainView.GetSelectionRect(dynamicDotNetTwainView.CurrentImageIndexInBuffer)
                dynamicDotNetTwainThum.CutFrameToClipboard(index, CInt(rect.Left), CInt(rect.Top), CInt(rect.Right), CInt(rect.Bottom))
                UpdateImageInfo()
            End If
        End Sub

        Private Sub Crop_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Dim index As Short : index = dynamicDotNetTwainThum.CurrentImageIndexInBuffer
            If (index >= 0) Then
                Dim rect As Rect : rect = dynamicDotNetTwainView.GetSelectionRect(dynamicDotNetTwainView.CurrentImageIndexInBuffer)
                dynamicDotNetTwainThum.Crop(index, CInt(rect.Left), CInt(rect.Top), CInt(rect.Right), CInt(rect.Bottom))
                UpdateImageInfo()
            End If
        End Sub

        Private Sub Delete_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Dim index As Short : index = dynamicDotNetTwainThum.CurrentImageIndexInBuffer
            If (index >= 0) Then
                dynamicDotNetTwainThum.RemoveImage(index)
                dynamicDotNetTwainView.RemoveImage(dynamicDotNetTwainView.CurrentImageIndexInBuffer)
                UpdateImageInfo()
            End If
        End Sub

        Private Sub DeleteAll_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            If (dynamicDotNetTwainThum.HowManyImagesInBuffer > 0) Then
                dynamicDotNetTwainThum.RemoveAllImages()
            End If
            If (dynamicDotNetTwainView.HowManyImagesInBuffer > 0) Then
                dynamicDotNetTwainView.RemoveAllImages()
            End If
            UpdateImageInfo()
        End Sub

        Private Sub First_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            If (dynamicDotNetTwainThum.HowManyImagesInBuffer >= 0) Then
                dynamicDotNetTwainThum.CurrentImageIndexInBuffer = CShort(0)
                UpdateImageInfo()
            End If
        End Sub

        Private Sub Previous_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            If (dynamicDotNetTwainThum.CurrentImageIndexInBuffer > 0) Then
                dynamicDotNetTwainThum.CurrentImageIndexInBuffer = CShort(dynamicDotNetTwainThum.CurrentImageIndexInBuffer - 1)
                UpdateImageInfo()
            End If
        End Sub

        Private Sub Next_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            If (dynamicDotNetTwainThum.CurrentImageIndexInBuffer < dynamicDotNetTwainThum.HowManyImagesInBuffer - 1) Then
                dynamicDotNetTwainThum.CurrentImageIndexInBuffer = CShort(dynamicDotNetTwainThum.CurrentImageIndexInBuffer + 1)
                UpdateImageInfo()
            End If
        End Sub

        Private Sub Last_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            If (dynamicDotNetTwainThum.HowManyImagesInBuffer > 0) Then
                dynamicDotNetTwainThum.CurrentImageIndexInBuffer = CShort(dynamicDotNetTwainThum.HowManyImagesInBuffer - 1)
                UpdateImageInfo()
            End If
        End Sub

        Private Sub Save_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Dim saveWnd As SaveWindow : saveWnd = New SaveWindow()
            saveWnd.Owner = Me
            saveWnd.TWAIN = dynamicDotNetTwainThum
            saveWnd.WindowStartupLocation = WindowStartupLocation.CenterOwner
            saveWnd.ShowDialog()
        End Sub
    End Class
End Namespace


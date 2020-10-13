using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.IO;
using Microsoft.Win32;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GraphicEditor
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		enum DrawTool
		{
			NotSelected, Pencil, Line, Ellipse
		}
		DrawTool drawTool;
		SolidColorBrush brush = new SolidColorBrush(Color.FromRgb(0, 0, 0));
		Point currentPoint = new Point();
		Line currentLine = null;
		Ellipse currentEllipse = null;
		bool isPaint = false;

		private void button_pencil_Click(object sender, RoutedEventArgs e)
		{
			drawTool = DrawTool.Pencil;
		}
		private void button_line_Click(object sender, RoutedEventArgs e)
		{
			drawTool = DrawTool.Line;
		}
		private void button_ellipse_Click(object sender, RoutedEventArgs e)
		{
			drawTool = DrawTool.Ellipse;
		}

		private void Canvas_Main_MouseMove(object sender, MouseEventArgs e)
		{
			if (!isPaint) return;
			var point = Mouse.GetPosition(Canvas_Main);
			switch (drawTool)
			{
				case DrawTool.Pencil:
					currentLine = new Line
					{
						Stroke = brush,
						X1 = currentPoint.X,
						Y1 = currentPoint.Y,
						X2 = point.X,
						Y2 = point.Y,
						StrokeStartLineCap = PenLineCap.Round,
						StrokeEndLineCap = PenLineCap.Round
					};
					currentPoint = point;
					Canvas_Main.Children.Add(currentLine);
					break;
				case DrawTool.Line:
					currentLine.X2 = point.X;
					currentLine.Y2 = point.Y;
					break;
				case DrawTool.Ellipse:
					currentEllipse.Width = Math.Abs(point.X - Canvas.GetLeft(currentEllipse));
					currentEllipse.Height = Math.Abs(point.Y - Canvas.GetTop(currentEllipse));
					break;
			}
		}

		private void Canvas_Main_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (isPaint) return;
			isPaint = true;
			currentPoint = Mouse.GetPosition(Canvas_Main);

			if (drawTool == DrawTool.Line)
			{
				currentLine = new Line();
				currentLine.X1 = currentPoint.X;
				currentLine.Y1 = currentPoint.Y;

				currentLine.StrokeStartLineCap = PenLineCap.Round;
				currentLine.StrokeEndLineCap = PenLineCap.Round;
				currentLine.Stroke = brush;
				try
				{
					Canvas_Main.Children.Add(currentLine);
				}
				catch (Exception) { }
			}
			if (drawTool == DrawTool.Ellipse)
			{
				currentEllipse = new Ellipse();
				currentEllipse.Stroke = brush;

				Canvas.SetLeft(currentEllipse, currentPoint.X);
				Canvas.SetTop(currentEllipse, currentPoint.Y);

				Canvas_Main.Children.Add(currentEllipse);
			}
		}

		private void Canvas_Main_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			isPaint = false;
			currentLine = null;
			//currentEllipse = null;
		}

		public void MenuItem_Click(object sender, RoutedEventArgs e)
		{
			var saveImageDialog = new SaveFileDialog
			{
				DefaultExt = ".PNG",
				Filter = "Image (.PNG)|*.PNG"
			};

			if (saveImageDialog.ShowDialog() == true)
			{
				int actualWidth = (int)Canvas_Main.ActualWidth;
				int actualHeight = (int)Canvas_Main.ActualHeight;

				RenderTargetBitmap bitmap = new RenderTargetBitmap(
					actualWidth, actualHeight, 96d, 96d, PixelFormats.Pbgra32);

				bitmap.Render(Canvas_Main);
				PngBitmapEncoder encoder = new PngBitmapEncoder();
				encoder.Frames.Add(BitmapFrame.Create(bitmap));

				using FileStream file = File.Create(saveImageDialog.FileName);
				encoder.Save(file);
			}
		}

		private void MenuItem_Click_1(object sender, RoutedEventArgs e)
		{
			var openFileDialog = new OpenFileDialog
			{
				DefaultExt = ".PNG",
				Filter = "Image (.PNG)|*.PNG"
			};

			if (openFileDialog.ShowDialog() == false)
				return;

			var img = new Image
			{
				Source = new BitmapImage(new Uri(openFileDialog.FileName))
			};

			Canvas_Main.Children.Add(img);
		}

		private void colorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
		{
			brush = new SolidColorBrush((Color)colorPicker.SelectedColor);
		}

		private void button_fill_Click(object sender, RoutedEventArgs e)
		{
			if (currentEllipse != null)
			{
				currentEllipse.Fill = brush;
			}
		}
	}
}
